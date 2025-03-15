using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model;
using System.Collections.Generic;

namespace WebApplication2.Model
{
    public class SqlContext : DbContext
    {

        public SqlContext(DbContextOptions<SqlContext> options)
        : base(options)
        { }

        public DbSet<User> user { get; set; }
    }

    public class Servicemy :IServicemy
    {
       
        private readonly IOptionsSnapshot<DbSettings> optDbSettings;
        private readonly IOptionsSnapshot<SmtpSettings> optSmtpSettings;

        public Servicemy(IOptionsSnapshot<DbSettings> optDbSettings, IOptionsSnapshot<SmtpSettings> optSmtpSettings)
        {
            this.optDbSettings = optDbSettings;
            this.optSmtpSettings = optSmtpSettings;
        }
        public void Test()
        {
            var db = optDbSettings.Value;
            Console.WriteLine($"数据库:{db.DbType},{db.ConnectionString}");
            var smtp = optSmtpSettings.Value;
            Console.WriteLine($"Stmp: {smtp.Server},{smtp.UserName},{smtp.Password}");
        }
    }
    public interface IServicemy
    {
        public void Test();
    }
}
