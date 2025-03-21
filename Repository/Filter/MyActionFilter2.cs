using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Repository.Filter
{
    public class MyActionFilter2 : IAsyncActionFilter
    {
        //context 表示Action执行的上下文对象，可以从中获取请求路径、参数值等信息
        //next 表示下一个要执行的筛选器，如果是最后一个就代表Controller层的接口方法
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasNotTransactionalAttribute = false;
            if (context.ActionDescriptor is ControllerActionDescriptor)   // 判断当前执行函数是否是控制器动作
            {
                var actionDesc = (ControllerActionDescriptor)context.ActionDescriptor; //获取方法标记
                hasNotTransactionalAttribute = actionDesc.MethodInfo.IsDefined(typeof(NotTransactionalAttribute)); //判断标识是否为NotTransactionalAttribute
            }
            if (hasNotTransactionalAttribute) //如果不是事务性操作直接放行
            {
                await next();
                return;
            }
            using var txScoped = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled); //开启事务
            var result = await next(); //执行操作
            if (result.Exception == null)
            {
                txScoped.Complete(); //事务提交
            }
        }
    }
}
