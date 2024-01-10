using System.Reflection;
using FeatureAPI.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args); //專案build
var configuration = builder.Configuration; //設定
if (builder.Environment.IsProduction())//讀取vscode參數
{
    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}else{
    configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
}
configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRouter();
builder.Services.AddAPIService();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.2", new OpenApiInfo
    {
        Version = "v1.2",
        Title = "Account API",
        Description = "Account API"
    });
    c.EnableAnnotations();
    c.ExampleFilters();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // pick comments from classes, include controller comments: another tip from StackOverflow
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBodyLengthLimit = int.MaxValue;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSwagger(options =>
{
    options.PreSerializeFilters.Add((swagger, httpReq) =>
    {
        if (builder.Environment.EnvironmentName.Equals("SIT"))
        {
            var sitServerUrl  = configuration.GetSection("SITServerUrl").Value;
            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = sitServerUrl } };
        }
    });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./swagger/v1.2/swagger.json", "Account API v1.2");
    c.DefaultModelsExpandDepth(-1);
    c.RoutePrefix = string.Empty;
    // c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
    // {
    //     ["activated"] = false
    // };
});
app.UseRouting();

app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Test}");
});
app.Run();
