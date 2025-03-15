using Microsoft.Extensions.Options;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository2
{
    class Servicemy
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
            Console.WriteLine($"数据库:{ db.DbType},{db.ConnectionString}");
            var smtp = optSmtpSettings.Value;
            Console.WriteLine($"Stmp: {smtp.Server},{smtp.UserName},{smtp.Password}");
        }
    }
}
