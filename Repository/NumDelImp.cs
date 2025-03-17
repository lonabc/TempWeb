using System;
using IRepository;

namespace Repository
{
    public class NumDelImp<T> : INumDel<T> where T : class //车市
    {
        public static int[] arr = new int[3];
        public static int newTemp = 0;

        private static int currentIndex=0;
        public void numMove(int[] arr)
        {
            for (int i = 1; i < arr.Length; i++)
            {
                arr[i - 1] = arr[i];
            }
        }

        public void numMoveTask(int num)
        {
            if (currentIndex < arr.Length)
            {
                arr[currentIndex] = num; //将数字放入当前索引位置
                currentIndex++; //索引加1
            }
            else
            {
                numMove(arr); //将数组中的数字向前移动
                currentIndex--; //索引减1
            }
        }

        public async Task numDely(int num)
        {
            numMoveTask(num);
            await Task.Delay(500);         
        }


        public void printArr()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
            }
        }
    }
}
