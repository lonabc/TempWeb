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

#endregion


#region ע����������°�
builder.Services.AddScoped(typeof(INumDel<>), typeof(NumDelImp<>)); //ָ���˷�������
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
