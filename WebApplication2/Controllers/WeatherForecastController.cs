using IRepository;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using WebApplication2.Model;


namespace WebApplication2.Controllers;

[ApiController]
[Route("User/[action]")]
public class WeatherForecastController : ControllerBase
{
    private SqlContext _context;
    private IServicemy iser;
   


    private Ireposity<article> ireposity;
    public WeatherForecastController(
        SqlContext context,
        Ireposity<article> ireposity,
        IServicemy iser)
    {
        
        _context = context;
        this.ireposity = ireposity;
        this.iser= iser;
       
    }

 
    [HttpGet(Name ="register")]
    public void register(String name,String password)
    {
        Console.WriteLine("register");
        User user = new User(name, password);
        _context.user.Add(user);
        _context.SaveChanges();
    }
  
    [HttpGet(Name = "OpenPort")]
    public void test(int id)
    {
        var serial = new PortLine();
        serial.OpenPort("COM13");

        // ���Ͳ������ݣ����緢��"Hello"��

        //byte[] testData = System.Text.Encoding.ASCII.GetBytes($"{id}");
        //byte[] newData = new byte[] { 1 };
        //serial.SendData(newData);

        // ���ֳ��������Խ�������
        Console.WriteLine("��������˳�...");
        Console.ReadKey();
        NumDelImp<int[]> numMethod=new NumDelImp<int[]>();
        numMethod.printArr();
        //int[] arr = NumDelImp<int[]>.arr;
        //for (int i = 0; i < arr.Length; i++)
        //{
        //    Console.WriteLine(arr[i]);
        //}


        //   serial.ClosePort();

    }
}
