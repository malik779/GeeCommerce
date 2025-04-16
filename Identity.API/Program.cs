using Duende.IdentityServer.EntityFramework.DbContexts;
using Gee.Core.BaseInfrastructure.DataProviders;
using Gee.Core.DependancyInjection;
using Gee.Core.Extensions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllersWithViews();
//Configurare App setting and ConnectionStrings and Default services
builder.ConfigureDefaultApplicationServices<ApplicationDbContext>();

// Apply database migration automatically. Note that this approach is not
// recommended for production scenarios. Consider generating SQL scripts from
// migrations instead.
builder.Services.RunDefaultDatabaseServices<ApplicationDbContext>(false);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.WithOrigins("https://localhost:60002").AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddMigration<ApplicationDbContext, UsersSeed>();
builder.Services.AddMigration<ConfigurationDbContext, UsersSeed>();
builder.Services.AddMigration<PersistedGrantDbContext>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
builder.Services.AddTransient<IRedirectService, RedirectService>();

var dataSetting = DataSettingsManager.LoadSettings();



var certificate = dataSetting.CertificateDetail;
var cert = new X509Certificate2(certificate?.CertificatePath, certificate?.CertificatePassword);

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var identityServer=builder.Services.AddIdentityServer(options =>
{
    options.IssuerUri = configuration["PATH_BASE"] ?? string.Empty; ;
    options.Authentication.CookieLifetime = TimeSpan.FromHours(24);

    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    // TODO: Remove this line in production.
    options.KeyManagement.Enabled = false;
})
.AddAspNetIdentity<ApplicationUser>()
 .AddConfigurationStore(options =>
 {
     options.ConfigureDbContext = builder =>
         builder.UseSqlServer(dataSetting.ConnectionString,
         sql => sql.MigrationsAssembly(migrationsAssembly));
 })
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = builder =>
        builder.UseSqlServer(dataSetting.ConnectionString,
        sql => sql.MigrationsAssembly(migrationsAssembly));
}).AddSigningCredential(cert);


var app = builder.Build();

//app.MapDefaultEndpoints();

app.UseStaticFiles();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

// This cookie policy fixes login issues with Chrome 80+ using HTTP
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
app.UseIdentityServer();
//app.Use(async (context, next) =>
//{
//    var tenantId = context.Request.Headers["Tenant-ID"]; // Get tenant ID from headers
//                                                         // var tenantSettings = tenantService.GetTenantSettings(tenantId);

//    if (1 == 1)
//    {
//        app.UseIdentityServer();
//    }
//    else
//    {
//        app.UseAuthentication();
//    }

//    await next.Invoke();
//});
app.UseCors("AllowAll");
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute("default","{controller=Home}/action==Index/{id?}");
//});
app.MapDefaultControllerRoute();

app.Run();
