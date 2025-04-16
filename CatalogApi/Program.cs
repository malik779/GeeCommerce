using CatalogApi.CoreServices.ModelFactories;
using CatalogApi.CoreServices.Services;
using CatalogApi.CoreServices.Services.Interfaces;
using CatalogApi.InfraStructure;
using Gee.Core.BaseInfrastructure.DataProviders;
using Gee.Core.DependancyInjection;
using Gee.DefaultServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
//Configurare App setting and ConnectionStrings
builder.ConfigureDefaultApplicationServices<CatalogDbContext>();
builder.AddDefaultSwaggerGen();

builder.Services.RunDefaultDatabaseServices<CatalogDbContext>();

var certificate = DataSettingsManager.LoadSettings().CertificateDetail;
builder.AddDefaultAuthentication(certificate?.CertificatePath, certificate?.CertificatePassword);

builder.Services.AddScoped(typeof(CategoryModelFactory));
builder.Services.AddScoped(typeof(ICategoryService),typeof(CategoryService));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
var app = builder.Build();
app.UseDefaultSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseDeveloperExceptionPage();
    
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseSharedPolicies();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
