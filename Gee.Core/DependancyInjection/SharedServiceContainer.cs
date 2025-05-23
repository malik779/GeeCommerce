﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Gee.Core.Middlewares;
using Gee.Core.BaseInfrastructure.Config;
using Gee.Core.BaseInfrastructure.Helpers;
using System.Net;
using Gee.Core.BaseInfrastructure.FileProvider;
using Gee.Core.BaseInfrastructure;
using Gee.Core.BaseInfrastructure.DataProviders;
using Gee.Core.Extensions;
using Gee.Core.Logs;
using Gee.Core.Caching;
using AutoMapper;
using Gee.Core.Interfaces;
using Gee.Core.BaseInfrastructure.DataProviders.Certificate;
using Gee.Core.Enum;
using Microsoft.Extensions.Hosting;
using Gee.Core.MultiTenancy.Registeration;
using Gee.Core.BaseModels;
using Gee.Core.MultiTenancy.Services;
namespace Gee.Core.DependancyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceProvider  ConfigureApplicationServices<TContext>(this IServiceCollection services, IConfigurationManager config) where TContext : DbContext
        {
            services.AddMultiTenancy<TenantBaseModel>().WithHostResolutionStrategy()
                .WithTenantLookupService<TenantLookupService<TenantBaseModel>>();
            ServiceProvider serviceProvider = ApplicationServices<TContext>(services);
            return serviceProvider;
        }

        private static ServiceProvider ApplicationServices<TContext>(IServiceCollection services) where TContext : DbContext
        {
            var dataSetting = DataSettingsManager.LoadSettings();

            services.AddDbContext<TContext>(option => option.UseSqlServer(dataSetting.ConnectionString, sqlserverOption => sqlserverOption.EnableRetryOnFailure()));
            var serviceProvider = services.AddSingleton<ICertificateProvider, CertificateProvider>()
                                        .AddSingleton<ICacheKeyManager, CacheKeyManager>()
                                        .AddSingleton<IStaticCacheManager, MemoryCacheManager>()
                                        .AddSingleton<IConfigurationManager, ConfigurationManager>()
                                        .AddScoped(typeof(IBaseRepository<>), typeof(EntityRepository<>))
                                        .AddScoped<IGenericFileProvider, GenericFileProvider>()
                                        .AddScoped<IShortTermCacheManager, PerRequestCacheManager>()
                                        .AddScoped<ICacheKeyService, MemoryCacheManager>()
                                        .AddTransient<IDataProviderManager<TContext>, DataProviderManager<TContext>>()
                                        .AddTransient(serviceProvider =>
                                            serviceProvider.GetRequiredService<IDataProviderManager<TContext>>().GetDataProvider(serviceProvider.GetRequiredService<TContext>()))
                                        .AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>))
                                        .BuildServiceProvider();




            var certificateDetail = serviceProvider.GetRequiredService<ICertificateProvider>().GetCertificateDetail();
            DataSettingsManager.SaveCertificateDetail(certificateDetail);
            //services.AddScoped<IBaseAdminModelFactory, BaseAdminModelFactory>();
            //Cache 
            services.AddMemoryCache();

            var appSettings = Singleton<AppSettingConfig>.Instance;
            var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();
            if (distributedCacheConfig.Enabled)
            {
                services.AddDistributedCache();
            }



            //register mapper configurations
            AddAutoMapper();
            return serviceProvider;
        }

        public  static void RunDefaultDatabaseServices<TContext>(this IServiceCollection services,bool IsDBMigrator=true) where TContext : DbContext   
        { 
            var dataSetting = DataSettingsManager.LoadSettings();
           // Create the Default Database From connection provided in the app setting if not exist 
            var dataProvider = DataProviderManager<TContext>.GetDataProvider(Enum.DataProviderType.SqlServer,null);
            if (string.IsNullOrEmpty(dataSetting.ConnectionString))
            {
                LogExceptions.LogException(new Exception("Connection is not valid"));
                throw new Exception("Connection is not valid");
            }

            if (!dataProvider.DatabaseExistsAsync().Result)
            {
                try
                {
                    dataProvider.CreateDatabase(null);
                }
                catch (Exception ex)
                {
                    LogExceptions.LogException(ex);
                    throw new Exception(string.Format("Database Creation Error", ex.Message));
                }
            }
            else
            {
                //check whether database exists
                if (!dataProvider.DatabaseExistsAsync().Result)
                    throw new Exception("Database Doest Not Exist");
            }
 
           if(IsDBMigrator)
                services.AddMigration<TContext>();
            
           
        }
        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            //register middleware to block all outside api call
            //app.UseMiddleware<ListenToOnlyApiGateway>();
            return app;
        }


        public static IServiceProvider ConfigureDefaultApplicationServices<TContext>(this IHostApplicationBuilder builder) where TContext : DbContext
        {
            var services = builder.Services;
            var Configuration = builder.Configuration;

            CommonHelper.DefaultFileProvider = new GenericFileProvider(builder.Environment);
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
           
            //create default file provider
           

            //register type finder
            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();

            foreach (var config in configurations)
            {
                Configuration.GetSection(config!=null?config.Name:"")?.Bind(config,option=>option.BindNonPublicProperties=true);
            }
                

            var appSettings = AppSettingHelper.SaveAppSetting(configurations, CommonHelper.DefaultFileProvider, false);
            services.AddSingleton(appSettings);

            //configure project services
           var serviceProvider= services.ConfigureApplicationServices<TContext>(builder.Configuration);
           return serviceProvider;
        }
        public static void AddDistributedCache(this IServiceCollection services)
        {
            var appSettings = Singleton<AppSettingConfig>.Instance;
            var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

            if (!distributedCacheConfig.Enabled)
                return;

            switch (distributedCacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Memory:
                    services.AddDistributedMemoryCache();
                    break;

                case DistributedCacheType.SqlServer:
                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = distributedCacheConfig.ConnectionString;
                        options.SchemaName = distributedCacheConfig.SchemaName;
                        options.TableName = distributedCacheConfig.TableName;
                    });
                    break;

                case DistributedCacheType.Redis:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;

                case DistributedCacheType.RedisSynchronizedMemory:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                        options.InstanceName = distributedCacheConfig.InstanceName ?? string.Empty;
                    });
                    break;
            }
        }
        public static void AddAutoMapper()
        {
            //find mapper configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .Where(mapperConfiguration => mapperConfiguration != null)
                .OrderBy(mapperConfiguration => mapperConfiguration?.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                    cfg.AddProfile(instance?.GetType());
            });

            //register
            AutoMapperConfiguration.Init(config);
        }
        //public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        //{
        //    //file provider
        //    services.AddScoped<INopFileProvider, NopFileProvider>();

        //    //web helper
        //    services.AddScoped<IWebHelper, WebHelper>();

        //    //user agent helper
        //    services.AddScoped<IUserAgentHelper, UserAgentHelper>();

        //    //plugins
        //    services.AddScoped<IPluginService, PluginService>();
        //    services.AddScoped<OfficialFeedManager>();

        //    //static cache manager
        //    var appSettings = Singleton<AppSettings>.Instance;
        //    var distributedCacheConfig = appSettings.Get<DistributedCacheConfig>();

        //    services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

        //    services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
        //    services.AddScoped<IShortTermCacheManager, PerRequestCacheManager>();

        //    if (distributedCacheConfig.Enabled)
        //    {
        //        switch (distributedCacheConfig.DistributedCacheType)
        //        {
        //            case DistributedCacheType.Memory:
        //                services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
        //                services.AddScoped<ICacheKeyService, MemoryDistributedCacheManager>();
        //                break;
        //            case DistributedCacheType.SqlServer:
        //                services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
        //                services.AddScoped<ICacheKeyService, MsSqlServerCacheManager>();
        //                break;
        //            case DistributedCacheType.Redis:
        //                services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
        //                services.AddScoped<IStaticCacheManager, RedisCacheManager>();
        //                services.AddScoped<ICacheKeyService, RedisCacheManager>();
        //                break;
        //            case DistributedCacheType.RedisSynchronizedMemory:
        //                services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
        //                services.AddSingleton<ISynchronizedMemoryCache, RedisSynchronizedMemoryCache>();
        //                services.AddSingleton<IStaticCacheManager, SynchronizedMemoryCacheManager>();
        //                services.AddScoped<ICacheKeyService, SynchronizedMemoryCacheManager>();
        //                break;
        //        }

        //        services.AddSingleton<ILocker, DistributedCacheLocker>();
        //    }
        //    else
        //    {
        //        services.AddSingleton<ILocker, MemoryCacheLocker>();
        //        services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        //        services.AddScoped<ICacheKeyService, MemoryCacheManager>();
        //    }

        //    //work context
        //    services.AddScoped<IWorkContext, WebWorkContext>();

        //    //store context
        //    services.AddScoped<IStoreContext, WebStoreContext>();

        //    //services
        //    //services.AddScoped<IBackInStockSubscriptionService, BackInStockSubscriptionService>();
        //    //services.AddScoped<ICategoryService, CategoryService>();
        //    //services.AddScoped<ICompareProductsService, CompareProductsService>();
        //    //services.AddScoped<IRecentlyViewedProductsService, RecentlyViewedProductsService>();
        //    //services.AddScoped<IManufacturerService, ManufacturerService>();
        //    //services.AddScoped<IPriceFormatter, PriceFormatter>();
        //    //services.AddScoped<IProductAttributeFormatter, ProductAttributeFormatter>();
        //    //services.AddScoped<IProductAttributeParser, ProductAttributeParser>();
        //    //services.AddScoped<IProductAttributeService, ProductAttributeService>();
        //    //services.AddScoped<IProductService, ProductService>();
        //    //services.AddScoped<ICopyProductService, CopyProductService>();
        //    //services.AddScoped<ISpecificationAttributeService, SpecificationAttributeService>();
        //    //services.AddScoped<IProductTemplateService, ProductTemplateService>();
        //    //services.AddScoped<ICategoryTemplateService, CategoryTemplateService>();
        //    //services.AddScoped<IManufacturerTemplateService, ManufacturerTemplateService>();
        //    //services.AddScoped<ITopicTemplateService, TopicTemplateService>();
        //    //services.AddScoped<IProductTagService, ProductTagService>();
        //    //services.AddScoped<IAddressService, AddressService>();
        //    //services.AddScoped<IAffiliateService, AffiliateService>();
        //    //services.AddScoped<IVendorService, VendorService>();
        //    //services.AddScoped<ISearchTermService, SearchTermService>();
        //    //services.AddScoped<IGenericAttributeService, GenericAttributeService>();
        //    //services.AddScoped<IMaintenanceService, MaintenanceService>();
        //    //services.AddScoped<ICustomerService, CustomerService>();
        //    //services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();
        //    //services.AddScoped<ICustomerReportService, CustomerReportService>();
        //    //services.AddScoped<IPermissionService, PermissionService>();
        //    //services.AddScoped<IAclService, AclService>();
        //    //services.AddScoped<IPriceCalculationService, PriceCalculationService>();
        //    //services.AddScoped<IGeoLookupService, GeoLookupService>();
        //    //services.AddScoped<ICountryService, CountryService>();
        //    //services.AddScoped<ICurrencyService, CurrencyService>();
        //    //services.AddScoped<IMeasureService, MeasureService>();
        //    //services.AddScoped<IStateProvinceService, StateProvinceService>();
        //    //services.AddScoped<IStoreService, StoreService>();
        //    //services.AddScoped<IStoreMappingService, StoreMappingService>();
        //    //services.AddScoped<IDiscountService, DiscountService>();
        //    //services.AddScoped<ILocalizationService, LocalizationService>();
        //    //services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
        //    //services.AddScoped<ILanguageService, LanguageService>();
        //    //services.AddScoped<IDownloadService, DownloadService>();
        //    //services.AddScoped<IMessageTemplateService, MessageTemplateService>();
        //    //services.AddScoped<IQueuedEmailService, QueuedEmailService>();
        //    //services.AddScoped<INewsLetterSubscriptionService, NewsLetterSubscriptionService>();
        //    //services.AddScoped<INotificationService, NotificationService>();
        //    //services.AddScoped<ICampaignService, CampaignService>();
        //    //services.AddScoped<IEmailAccountService, EmailAccountService>();
        //    //services.AddScoped<IWorkflowMessageService, WorkflowMessageService>();
        //    //services.AddScoped<IMessageTokenProvider, MessageTokenProvider>();
        //    //services.AddScoped<ITokenizer, Tokenizer>();
        //    //services.AddScoped<ISmtpBuilder, SmtpBuilder>();
        //    //services.AddScoped<IEmailSender, EmailSender>();
        //    //services.AddScoped<ICheckoutAttributeFormatter, CheckoutAttributeFormatter>();
        //    //services.AddScoped<IGiftCardService, GiftCardService>();
        //    //services.AddScoped<IOrderService, OrderService>();
        //    //services.AddScoped<IOrderReportService, OrderReportService>();
        //    //services.AddScoped<IOrderProcessingService, OrderProcessingService>();
        //    //services.AddScoped<IOrderTotalCalculationService, OrderTotalCalculationService>();
        //    //services.AddScoped<IReturnRequestService, ReturnRequestService>();
        //    //services.AddScoped<IRewardPointService, RewardPointService>();
        //    //services.AddScoped<IShoppingCartService, ShoppingCartService>();
        //    //services.AddScoped<ICustomNumberFormatter, CustomNumberFormatter>();
        //    //services.AddScoped<IPaymentService, PaymentService>();
        //    //services.AddScoped<IEncryptionService, EncryptionService>();
        //    //services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
        //    //services.AddScoped<IUrlRecordService, UrlRecordService>();
        //    //services.AddScoped<IShipmentService, ShipmentService>();
        //    //services.AddScoped<IShippingService, ShippingService>();
        //    //services.AddScoped<IDateRangeService, DateRangeService>();
        //    //services.AddScoped<ITaxCategoryService, TaxCategoryService>();
        //    //services.AddScoped<ICheckVatService, CheckVatService>();
        //    //services.AddScoped<ITaxService, TaxService>();
        //    //services.AddScoped<ILogger, DefaultLogger>();
        //    //services.AddScoped<ICustomerActivityService, CustomerActivityService>();
        //    //services.AddScoped<IForumService, ForumService>();
        //    //services.AddScoped<IGdprService, GdprService>();
        //    //services.AddScoped<IPollService, PollService>();
        //    //services.AddScoped<IBlogService, BlogService>();
        //    //services.AddScoped<ITopicService, TopicService>();
        //    //services.AddScoped<INewsService, NewsService>();
        //    //services.AddScoped<IDateTimeHelper, DateTimeHelper>();
        //    //services.AddScoped<INopHtmlHelper, NopHtmlHelper>();
        //    //services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
        //    //services.AddScoped<IExportManager, ExportManager>();
        //    //services.AddScoped<IImportManager, ImportManager>();
        //    //services.AddScoped<IPdfService, PdfService>();
        //    //services.AddScoped<IUploadService, UploadService>();
        //    //services.AddScoped<IThemeProvider, ThemeProvider>();
        //    //services.AddScoped<IThemeContext, ThemeContext>();
        //    //services.AddScoped<IExternalAuthenticationService, ExternalAuthenticationService>();
        //    //services.AddSingleton<IRoutePublisher, RoutePublisher>();
        //    //services.AddScoped<IReviewTypeService, ReviewTypeService>();
        //    //services.AddSingleton<IEventPublisher, EventPublisher>();
        //    //services.AddScoped<ISettingService, SettingService>();
        //    //services.AddScoped<IBBCodeHelper, BBCodeHelper>();
        //    //services.AddScoped<IHtmlFormatter, HtmlFormatter>();
        //    //services.AddScoped<IVideoService, VideoService>();
        //    //services.AddScoped<INopUrlHelper, NopUrlHelper>();
        //    //services.AddScoped<IWidgetModelFactory, WidgetModelFactory>();

        //    //attribute services
        //    services.AddScoped(typeof(IAttributeService<,>), typeof(AttributeService<,>));

        //    //attribute parsers
        //    services.AddScoped(typeof(IAttributeParser<,>), typeof(Services.Attributes.AttributeParser<,>));

        //    //attribute formatter
        //    services.AddScoped(typeof(IAttributeFormatter<,>), typeof(AttributeFormatter<,>));

        //    //plugin managers
        //    services.AddScoped(typeof(IPluginManager<>), typeof(PluginManager<>));
        //    services.AddScoped<IAuthenticationPluginManager, AuthenticationPluginManager>();
        //    services.AddScoped<IMultiFactorAuthenticationPluginManager, MultiFactorAuthenticationPluginManager>();
        //    services.AddScoped<IWidgetPluginManager, WidgetPluginManager>();
        //    services.AddScoped<IExchangeRatePluginManager, ExchangeRatePluginManager>();
        //    services.AddScoped<IDiscountPluginManager, DiscountPluginManager>();
        //    services.AddScoped<IPaymentPluginManager, PaymentPluginManager>();
        //    services.AddScoped<IPickupPluginManager, PickupPluginManager>();
        //    services.AddScoped<IShippingPluginManager, ShippingPluginManager>();
        //    services.AddScoped<ITaxPluginManager, TaxPluginManager>();
        //    services.AddScoped<ISearchPluginManager, SearchPluginManager>();

        //    services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //    //register all settings
        //    var typeFinder = Singleton<ITypeFinder>.Instance;

        //    var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
        //    foreach (var setting in settings)
        //    {
        //        services.AddScoped(setting, serviceProvider =>
        //        {
        //            var storeId = DataSettingsManager.IsDatabaseInstalled()
        //                ? serviceProvider.GetRequiredService<IStoreContext>().GetCurrentStore()?.Id ?? 0
        //                : 0;

        //            return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, storeId).Result;
        //        });
        //    }

        //    //picture service
        //    if (appSettings.Get<AzureBlobConfig>().Enabled)
        //        services.AddScoped<IPictureService, AzurePictureService>();
        //    else
        //        services.AddScoped<IPictureService, PictureService>();

        //    //roxy file manager
        //    services.AddScoped<IRoxyFilemanService, RoxyFilemanService>();
        //    services.AddScoped<IRoxyFilemanFileProvider, RoxyFilemanFileProvider>();

        //    //installation service
        //    services.AddScoped<IInstallationService, InstallationService>();

        //    //slug route transformer
        //    if (DataSettingsManager.IsDatabaseInstalled())
        //        services.AddScoped<SlugRouteTransformer>();

        //    //schedule tasks
        //    services.AddSingleton<ITaskScheduler, TaskScheduler>();
        //    services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();

        //    //event consumers
        //    var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
        //    foreach (var consumer in consumers)
        //        foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
        //        {
        //            var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
        //            return isMatch;
        //        }, typeof(IConsumer<>)))
        //            services.AddScoped(findInterface, consumer);

        //    //XML sitemap
        //    services.AddScoped<IXmlSiteMap, XmlSiteMap>();

        //    //register the Lazy resolver for .Net IoC
        //    var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;
        //    if (!useAutofac)
        //        services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));
        //}
    }
}
