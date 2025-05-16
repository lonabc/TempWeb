using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public  class SocketServer
    {
        private const int Port = 5000;
      
        private TcpListener _Listener;

        public async Task StartAsync()
        {
            _Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Port); //IPAddress.Any 监听主机所有网络地址和接口,相当于socket()和bind()函数
                                                              // _Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), Port);// 监听指定端口和ip地址
            _Listener.Start();  //listen函数调用
            Console.WriteLine("Socket 服务已启动,开始监听");
            while(true)
            {
                TcpClient client = await _Listener.AcceptTcpClientAsync();
                Console.WriteLine("会话连接");
                _ = HandleRead(client);
                _ = HandleSend(client,"tes");
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    //接受数据
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    //string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //Console.WriteLine("Received:"+data);
                    string testdata=  await HandleRead(client);
                    Console.WriteLine(testdata);
                    // 发送数据给客户端
                    //string response = "Hello from server!";
                    //byte[] responseData = Encoding.UTF8.GetBytes(response);
                    //await stream.WriteAsync(responseData, 0, responseData.Length);
                    //Console.WriteLine("Sent to client: " + response);
                    await HandleSend(client,testdata);
                }
            }
            Console.WriteLine("Client disconnected");
        }

        private async Task<String> HandleRead(TcpClient client)
        {
            String data="函数分离测试";
            using (NetworkStream stream=client.GetStream())
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int byteRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (byteRead==0) break; //客户端断开连接
                    data = Encoding.UTF8.GetString(buffer, 0, byteRead);
                    Console.WriteLine("Received:" + data);
                }
              
            }
            Console.WriteLine("Client disconnected");
            return data;
            
        }

        private async Task HandleSend(TcpClient client,String data)
        {
           
            using (NetworkStream stream = client.GetStream())
            {
                while (true)
                {
                    string response = "Hello from server!";
                    // byte[] responseData = Encoding.UTF8.GetBytes(response); //将字符串转换为字节
                 //   int[] tempArr = NumDelImp<int[]>.arr;
                    int newTemp = NumDelImp<int[]>.newTemp;

                   // byte[] byteArray = new byte[tempArr.Length * sizeof(int)]; // 计算字节流长度
                    byte[] byteNewTemp = BitConverter.GetBytes(newTemp);


                    //Buffer.BlockCopy(tempArr, 0, byteArray, 0, byteArray.Length); //将数组转换为字节流

                    // await stream.WriteAsync(byteArray, 0, byteArray.Length);
                    await stream.WriteAsync(byteNewTemp, 0, byteNewTemp.Length);
                    Console.WriteLine("Sent to client: " + response);
                    await  Task.Delay(1000); 
                }
            }
        }
    }
}
