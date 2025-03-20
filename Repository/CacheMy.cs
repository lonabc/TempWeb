using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public  class CacheMy
    {
        public async Task<string> testRead()
        {
            Thread.Sleep(2);
            return "Test";
        }

    }
}
