using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public  interface INumDel<T> where T : class //约束实现类型必须是类
    {
        abstract void numMove(int[] arr);
        void  numMoveTask(int num);
        Task numDely(int num);
    }
}
