using Gee.DefaultServices;
using Gee.Core.DependancyInjection;
using TenantApi.Service.Infrastructure;
using Gee.Core.BaseInfrastructure.DataProviders;
using TenantApi.Service.Infrastructure.ModelPrepareFactory;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//Configurare App setting and ConnectionStrings
builder.ConfigureDefaultApplicationServices<TenantDbContext>();
builder.AddDefaultSwaggerGen();
builder.Services.RunDefaultDatabaseServices<TenantDbContext>();

var certificate = DataSettingsManager.LoadSettings().CertificateDetail;
builder.AddDefaultAuthentication(certificate?.CertificatePath, certificate?.CertificatePassword);
//services
builder.Services.AddScoped(typeof(TenantModelFactory));
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
// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDefaultSwagger();
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
