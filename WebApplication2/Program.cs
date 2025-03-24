
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

#region �������ݿ�����
//string? sqlConnection = builder.Configuration.GetConnectionString("Connection");
//if (sqlConnection != null)
//{
//    builder.Services.AddDbContext<SqlContext>(options =>
//    {
//        var serverVersion = ServerVersion.AutoDetect(sqlConnection);  //mysql�汾: {8.2.0-mysql}
//        options.UseMySql(sqlConnection, serverVersion);
//    });
//    builder.Services.AddDbContext<ModleContext>(options =>
//    {
//        var serverVersion = ServerVersion.AutoDetect(sqlConnection);  //mysql�汾: {8.2.0-mysql}
//        options.UseMySql(sqlConnection, serverVersion);
//    });
//}
#endregion

#region ע��������þɰ�
// 2. ��ȡ�����ļ�
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
IConfigurationRoot config = configBuilder.Build();
// �� DbSettings ����
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("Db"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smt"));
//ע���쳣������
builder.Services.Configure<MvcOptions>(options => {
    options.Filters.Add<MyExceptionFilter>();
});
//ע�����ɸѡ��
builder.Services.Configure<MvcOptions>(Options => {
    Options.Filters.Add<MyActionFilter1>();
 //   Options.Filters.Add<MyActionFilter2>();
});
#endregion


#region ע����������°�
builder.Services.AddScoped(typeof(INumDel<>), typeof(NumDelImp<>)); //ָ���˷�������
builder.Services.AddScoped<TestClass>();
builder.Services.AddScoped<CacheMy>();
#endregion



//���ڴ滺����ط���ע�ᵽ����ע��������
builder.Services.AddMemoryCache();

var app = builder.Build();

#region  �м���������
app.Map("/HomeController", async appbuilder => //�����м��map
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

app.UseResponseCaching(); //���ÿ��

app.MapControllers();

app.Run();
