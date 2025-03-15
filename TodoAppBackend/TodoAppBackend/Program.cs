using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoAppBackend.Data;
using TodoAppBackend.Repositories;
using TodoAppBackend.Services;
using TodoAppBackend.UnitOfWork;
var builder = WebApplication.CreateBuilder(args);

// Thêm Swagger vào dịch vụ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' không tồn tại trong appsettings.json.");
}

//   DbContext sử dụng SQL Server
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString));

//Repository
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ISubTaskRepository, SubTaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Service
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ISubTaskService, SubTaskService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
