using Microsoft.EntityFrameworkCore;
using TaskCBT.Application.Interfaces;
using TaskCBT.Application.Mapping;
using TaskCBT.Application.Services;
using TaskCBT.Domain.Interfaces;
using TaskCBT.Infrastructure.Data;
using TaskCBT.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Add DbContext
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

// Register services
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<INotificationService, NotificationService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
