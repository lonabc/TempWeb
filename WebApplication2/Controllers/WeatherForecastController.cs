using IRepository;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using System.Threading.Tasks;
using WebApplication2.Model;


namespace WebApplication2.Controllers;

[ApiController]
[Route("User/[action]")]
public class WeatherForecastController : ControllerBase
{

    
    public WeatherForecastController()
    {
       
    }
  
    [HttpGet(Name = "OpenPort")]
    public void test(int id)
    {
        var serial = new PortLine();
        serial.OpenPort($"COM{id}");

        // 发送测试数据（例如发送"Hello"）

        //byte[] testData = System.Text.Encoding.ASCII.GetBytes($"{id}");
        //byte[] newData = new byte[] { 1 };
        //serial.SendData(newData);

        // 保持程序运行以接收数据
        Console.WriteLine("按任意键退出...");
        Console.ReadKey();
        NumDelImp<int[]> numMethod=new NumDelImp<int[]>();
        // numMethod.printArr();
        int[] arr = NumDelImp<int[]>.arr;
        for (int i = 0; i < arr.Length; i++)
        {
            Console.WriteLine(arr[i]);
        }


        //   serial.ClosePort();

    }
    [HttpGet(Name = "Socket")]
    public async Task SocketTest()
    {
        SocketServer socketServer = new SocketServer();
        await socketServer.StartAsync();
     
    }
}
