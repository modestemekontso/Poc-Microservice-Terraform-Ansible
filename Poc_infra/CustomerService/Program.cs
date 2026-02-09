using Microsoft.EntityFrameworkCore;
using CustomerService.Models.Data;
using CustomerService.Contrats;
using CustomerService.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/health", () => "OK");

app.Run();
