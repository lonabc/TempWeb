﻿using System;
using System.Collections.Generic;
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
                _ = HandleClientAsync(client);//异步处理客户端，忽略其返回值
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
                    int bytesRead = await stream.ReadAsync(buffer,0,buffer.Length);
                    if (bytesRead == 0) break;
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received:"+data);


                    // 发送数据给客户端
                    string response = "Hello from server!";
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseData, 0, responseData.Length);
                    Console.WriteLine("Sent to client: " + response);
                }
            }
            Console.WriteLine("Client disconnected");
        }
    }
}
