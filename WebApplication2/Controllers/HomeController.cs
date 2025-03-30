using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Expressions;
using Model;
using Repository;
using Repository.JwtConfig;
using System.Runtime;
using System.Security.Claims;
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
                e.SlidingExpiration = TimeSpan.FromSeconds(2);//设置滑动过期时间
                Console.WriteLine("从数据库中获取数据");
                return await cacheMy.testRead();
            });

            Console.WriteLine("数据返回");
            return DateTime.Now;
        }
      

        [HttpPost, Route("Login")]
        public async Task<ActionResult<string>> Login([FromBody] User user)
        {
            if (user == null) return "参数为空";
            if (user.Id != 1 || user.Password != "258913")
            {
                return "用户密码不正确";
            }
            Object jwtCache;
            memCache.TryGetValue("JwtToken", out jwtCache);
            if (jwtCache != null)
            {
                return jwtCache.ToString();
            }


            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json",optional:false,reloadOnChange:false);

            IConfigurationRoot configRoot = configBuilder.Build();
            string key = configRoot["JWT:SecKey"]; // 直接访问
            int expireSeconds = configRoot.GetValue<int>("JWT:ExpireSeconds"); // 强类型转换
      

            string userid = user.Id.ToString();
            DateTime expire = DateTime.Now.AddSeconds(expireSeconds);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,userid));
            claims.Add(new Claim(ClaimTypes.Name,user.Password));
            claims.Add(new Claim(ClaimTypes.Role, "Auth")); //添加角色权限
            string jwtToke = JwtTool.BuildToken(claims, key, expire);

            if (jwtToke != null)
            { 
               memCache.Set("JwtToken", jwtToke, TimeSpan.FromSeconds(expireSeconds));
            }
            Console.WriteLine(jwtToke);
            return jwtToke;
        }


    }
}
