using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestApp_API.Repositories.Implementations;
using GestApp_API.Configuration;
using Microsoft.Extensions.Configuration;
using GestApp_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do contexto do Entity Framework Core para PostgreSQL
builder.Services.AddDbContext<DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped(typeof(IGenericRepository), typeof(GenericRepository));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
