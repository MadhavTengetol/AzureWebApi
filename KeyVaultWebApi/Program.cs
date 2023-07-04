using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using KeyVaultWebApi.Data;
using KeyVaultWebApi.Helper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var keyVaultManager = new KeyVaultManager(new SecretClient(new Uri(builder.Configuration["AzureKeyVault:url"]),new DefaultAzureCredential()));

builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(keyVaultManager.GetSecret("connectionString").Result));

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
