using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Repository;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
   
    [ApiController]
    [Route("HomeController/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly IMemoryCache memCache;
        private CacheMy cacheMy;
        public HomeController(IMemoryCache memCache,CacheMy cache)
        {
            this.memCache = memCache;
            cacheMy = cache;
        }
        [HttpGet(Name = "TestCache")]
        [ResponseCache(Duration =5)]
        public async Task<DateTime> testCache()
        {
            Console.WriteLine("缓存方法测试");
            var items = await memCache.GetOrCreateAsync("AllBooks", async(e) =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10); //设置绝对过期时间为10秒
                Console.WriteLine("从数据库中获取数据");
                return await cacheMy.testRead();
            });

            Console.WriteLine("数据返回");
            return DateTime.Now;
        }

    }
}
