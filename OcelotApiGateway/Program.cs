using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });
builder.Services.AddSwaggerForOcelot(builder.Configuration, o =>
{
    o.GenerateDocsDocsForGatewayItSelf(opt =>
      {

          //opt.GatewayDocsTitle = "Gee Gateway";
          //opt.GatewayDocsOpenApiInfo = new()
          //{
          //    Title = "Gee Gateway",
          //    Version = "v1",
          //};
      }
      );
});
builder.Configuration.AddJsonFile("ocelot.json");
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSwaggerForOcelotUI(option =>
{
    option.PathToSwaggerGenerator = "/swagger/docs";
});
app.MapControllers();
await app.UseOcelot();
app.Run();