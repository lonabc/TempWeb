using Dynamic.Json;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Filter
{
    public  class CheckAndParsingMiddleware
    {

        private readonly RequestDelegate next;
        public CheckAndParsingMiddleware(RequestDelegate next) // 
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string pwd = context.Request.Query["password"];
            if (pwd == "123")
            {
                //context.Request.HasJsonContentType()
                if (context.Request.ContentType != null && (context.Request.ContentType.Contains("application/json") || context.Request.ContentType.Contains("text/json"))) //判读当前请求是不是json请求
                {
                    var reqStream = context.Request.Body;
                    dynamic? jsonObj = DJson.Parse(reqStream); //将json数据转换为dynamic类型，无需用对应的类进行数据接收方便，快捷。
                    context.Items["BodyJson"] = jsonObj; //将解析后的 JSON 对象（jsonObj）存储到当前 HTTP 请求的共享数据字典（context.Items）中，并给它起一个名字叫 "BodyJson"。
                }
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }
    }
}
