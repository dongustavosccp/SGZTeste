using AuthAPI.Application.Contracts;
using AuthAPI.Application.Services;
using AuthAPI.Infrastructure;
using AuthAPI.Infrastructure.Repositories;
using EnterpriseLibrary.Service.DomainNotification;
using Microsoft.EntityFrameworkCore;
using Utils.DomainNotification;
using Utils.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddSwaggerGen();

// Configure services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<INotifier, Notifier>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? ""));

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();