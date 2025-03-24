
using Microsoft.EntityFrameworkCore;
using WebApplication2.Model;
using Repository;
using IRepository;
using Model;
using MySqlConnector;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Repository.Filter;
using Microsoft.AspNetCore.Builder;
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
//注册异常操作器
builder.Services.Configure<MvcOptions>(options => {
    options.Filters.Add<MyExceptionFilter>();
});
//注册操作筛选器
builder.Services.Configure<MvcOptions>(Options => {
    Options.Filters.Add<MyActionFilter1>();
 //   Options.Filters.Add<MyActionFilter2>();
});
#endregion


#region 注册服务配置新版
builder.Services.AddScoped(typeof(INumDel<>), typeof(NumDelImp<>)); //指定了泛型类型
builder.Services.AddScoped<TestClass>();
builder.Services.AddScoped<CacheMy>();
#endregion



//将内存缓存相关服务注册到依赖注入容器里
builder.Services.AddMemoryCache();

var app = builder.Build();

#region  中间件服务测试
app.Map("/HomeController", async appbuilder => //设置中间件map
{
    appbuilder.UseMiddleware<CheckAndParsingMiddleware>();
    appbuilder.Run(async ctx => {
        ctx.Response.ContentType = "text/html";
        ctx.Response.StatusCode = 200;
        dynamic jsonObj = ctx.Items["BodyJson"];
        int i = jsonObj.i;
        int j = jsonObj.j;
        await ctx.Response.WriteAsync($"{i}+{j}={i + j}");
    });
});
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseResponseCaching(); //针对每个

app.MapControllers();

app.Run();
