using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Filter
{
    [AttributeUsage(AttributeTargets.Method)] //限制该属性只能作用于方法
    public  class NotTransactionalAttribute : Attribute // 继承 Attribute，可以当作方法标记使用
    {
    }
}
