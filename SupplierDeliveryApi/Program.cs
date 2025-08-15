using EnterpriseLibrary.Service.DomainNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SupplierDeliveryAPI.Application.Contracts;
using SupplierDeliveryAPI.Application.Services;
using SupplierDeliveryAPI.Infrastructure;
using SupplierDeliveryAPI.Infrastructure.Repository;
using Utils.DomainNotification;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SupplierDeliveryAPI", Version = "v1" });

    // Adicionar suporte ao Bearer Token
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<INotifier, Notifier>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<SupplierDeliveryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? ""))
        };
    });

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SupplierDeliveryApi v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle = "Supplier Delivery API Documentation";
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();