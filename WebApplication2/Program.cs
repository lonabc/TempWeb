using Microsoft.EntityFrameworkCore;
using WebApplication2.Model;
using Repository;
using IRepository;
using Model;
using MySqlConnector;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 配置数据库连接
//string? sqlConnection = builder.Configuration.GetConnectionString("Connection");
//if (sqlConnection != null)
//{
//    builder.Services.AddDbContext<SqlContext>(options =>
//    {
//        var serverVersion = ServerVersion.AutoDetect(sqlConnection);  //mysql版本: {8.2.0-mysql}
//        options.UseMySql(sqlConnection, serverVersion);
//    });
//    builder.Services.AddDbContext<ModleContext>(options =>
//    {
//        var serverVersion = ServerVersion.AutoDetect(sqlConnection);  //mysql版本: {8.2.0-mysql}
//        options.UseMySql(sqlConnection, serverVersion);
//    });
//}
#endregion

#region 注册服务配置旧版
// 2. 读取配置文件
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot config = configBuilder.Build();
// 绑定 DbSettings 配置
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("Db"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smt"));

#endregion


#region 注册服务配置新版
builder.Services.AddScoped(typeof(INumDel<>), typeof(NumDelImp<>)); //指定了泛型类型
builder.Services.AddScoped<TestClass>();
#endregion


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
