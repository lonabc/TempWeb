

using IRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using Model;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class Repository<T> : Ireposity<T> where T : class, new()
    {

        private ModleContext _context;
        private Boolean result { get; set; }

        public Repository(ModleContext context)
        {
            _context = context;
        }
      


        async public Task<bool> Add(T t)//添加
        {
            try
            {
                Console.WriteLine("添加方法执行");
                 _context.Set<T>().Add(t);
                int result =await _context.SaveChangesAsync();

                if(result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } catch (DbUpdateException e)
            { 
                Console.WriteLine("数据库更新异常"+e);
                return false;
            }
         
           

        }
        async public Task<bool> Delete(T t) //删除
        {
            result = false;
            if (t != null)
            {
                _context.Set<T>().Remove(t);
                _context.SaveChanges();
                return !result;
            }
            return result;
           
        }
         async public Task<T> Query(int id) //查询单个
        {
           T data= _context.Set<T>().Find(id);
            return data;
            
        }
         async public Task<List<T>> QueryAsync(Expression<Func<T, bool>> func) //自定义条件查询
        {
            List<T> data= await _context.Set<T>().ToListAsync();
            return data;
        }
        async public Task<List<T>> QueryAsync(Expression<Func<T, bool>> func, int page, int size) //自定义条件查询，分页查询
        {
            List<T>data= await _context.Set<T>().Where(func).Skip(page).Take(size).ToListAsync();
            return data;
        }
        public async Task<List<T>> QueryAsync(int page, int size) //分页查询
        {
            List<T> data =await _context.Set<T>().Skip(page).Take(size).ToListAsync();
            return data;
        }
        public async Task<List<T>> QueryAsyncAll()//查询所有
        {
            List<T>data =  _context.Set<T>().ToList();
            return data;
        }
        public async Task<bool> Update(T t)//更新单条数据
        {
           _context.Set<T>().Update(t);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
   
}
