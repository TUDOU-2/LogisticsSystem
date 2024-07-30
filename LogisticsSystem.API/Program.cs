using LogisticsSystem.API.Context;
using Microsoft.EntityFrameworkCore;
using LogisticsSystem.Api;
using LogisticsSystem.API.Context.Repository;
using AutoMapper;
using LogisticsSystem.API.Extensions;
using LogisticsSystem.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LogisticsSystemContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");
    option.UseSqlite(connectionString);
}).AddUnitOfWork<LogisticsSystemContext>()
.AddCustomRepository<Users, UsersRepository>()
.AddCustomRepository<Customer, CustomerRepository>()
.AddCustomRepository<AirTransport, AirTransportRepository>()
.AddCustomRepository<AirTransportDetail, AirTransportDetailRepository>()
.AddCustomRepository<SeaTransport,SeaTransportRepository>()
.AddCustomRepository<SeaTransportDetail,SeaTransportDetailRepository>()
.AddCustomRepository<ExpressTransport,ExpressTransportRepository>()
.AddCustomRepository<ExpressTransportDetail,ExpressTransportDetailRepository>();


// 注册服务
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IAirTransportService, AirTransportService>();
builder.Services.AddTransient<IAirTransportDetailService, AirTransportDetailService>();
builder.Services.AddTransient<ISeaTransportService, SeaTransportService>();
builder.Services.AddTransient<ISeaTransportDetailService, SeaTransportDetailService>();
builder.Services.AddTransient<IExpressTransportService, ExpressTransportService>();
builder.Services.AddTransient<IExpressTransportDetailService, ExpressTransportDetailService>();


// 创建AutoMapper配置
var autoMapperConfig = new MapperConfiguration(config =>
{
    // 添加映射关系配置文件
    config.AddProfile(new AutoMapperProfile());
});
// 注册AutoMapper
builder.Services.AddSingleton(autoMapperConfig.CreateMapper());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
