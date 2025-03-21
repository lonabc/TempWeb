using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Filter
{
    public class MyActionFilter1 : IAsyncActionFilter
    {
        private readonly IMemoryCache memoryCache;

        public MyActionFilter1(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }



        //context 表示Action执行的上下文对象，可以从中获取请求路径、参数值等信息
        //next 表示下一个要执行的筛选器，如果是最后一个就代表Controller层的接口方法
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("过滤器测试");
            string removeIP = context.HttpContext.Connection.RemoteIpAddress.ToString(); //获取context的请求ip
            string cacheKey = $"LastVisitTick_{removeIP}";
            long? lastTick = memoryCache.Get<long?>(cacheKey); //获取缓存
            if (lastTick == null || Environment.TickCount64 - lastTick > 2000)//Environment.TickCount64 获取当前时间戳，这里判断是否大于1秒
            {
                memoryCache.Set(cacheKey, Environment.TickCount64, TimeSpan.FromSeconds(10));
                await next();
                return;
            }
            else
            { 
                context.Result = new ContentResult { StatusCode = 429 };
                return ; // 正确写法（无逗号）
            }
        }
    }
}
