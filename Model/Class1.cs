using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Model
{
    public class ModleContext: DbContext
    {
       public ModleContext(DbContextOptions<ModleContext> options): base(options)
        { 
        }
        public DbSet<User> user { get; set; }
        public DbSet<article> article { get; set; }
    }

    public class DbSettings
    {
        public string DbType { get; set; }
        public string ConnectionString { get; set; }

    }

    public class SmtpSettings
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
