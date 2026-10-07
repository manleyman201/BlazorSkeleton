using System.Reflection;
using BlazorSkeleton.Data.Services;
using BlazorSkeleton.Data.Services.Interfaces;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Swagger / OpenAPI: generates the API document from the controllers and their XML doc comments.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BlazorSkeleton API", Version = "v1" });

    var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    options.IncludeXmlComments(xmlFile);
});

// Services (business logic - one per feature)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

// Swagger UI at /swagger. Not exposed in PROD.
if (!app.Environment.IsEnvironment("PROD"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorSkeleton API v1"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
