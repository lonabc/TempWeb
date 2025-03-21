using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Filter
{
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<MyExceptionFilter> logger;

        private readonly IHostEnvironment env;

        public MyExceptionFilter(ILogger<MyExceptionFilter> logger, IHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            Console.WriteLine("全局异常处理开始");
            Exception exception = context.Exception; //获取异常
            logger.LogError(exception, "UnhandledException occured"); //将异常写入日志
            string message;
            if (env.IsDevelopment()) //判断环境
            {
                message = exception.ToString();
            }
            else
            {
                message = "程序中出现未处理异常"; //避免生产环境中写入异常堆栈
            }
            ObjectResult result = new ObjectResult(new { code = 500, message }); //设置响应报文
            result.StatusCode = 500;
            context.Result = result;
            context.ExceptionHandled = true; //禁止使用默认的异常响应逻辑
            return Task.CompletedTask; //Task.CompletedTask 表示返回无返回值的Task
        }
    }
}
