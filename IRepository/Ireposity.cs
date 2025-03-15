using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IRepository
{
    public interface Ireposity<T> where T : class,new() //约束实现类型必须是类，且有无参构造函数
    {
        //增删改查
        Task<bool> Add(T t);
        Task<bool> Delete(T t);
        Task<bool> Update(T t);
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Query(int id);
        /// <summary>
        /// 自定义条件查询，根据表示式树查询
        /// </summary>
        /// <returns></returns>
        Task<List<T>> QueryAsync(Expression<Func<T,bool>> func);
        /// <summary>
        /// 自定义条件查询，分页查询
        Task<List<T>> QueryAsync(Expression<Func<T, bool>> func,int page ,int size);
        /// <summary>
        ///   分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>

        Task<List<T>> QueryAsync(int page,int size);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<T>> QueryAsyncAll();


    }
}
