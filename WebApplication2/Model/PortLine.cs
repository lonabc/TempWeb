using IRepository;
using Repository;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Model
{
    public class PortLine
    {
        private INumDel<int[]> _numDel;
        private SerialPort _serialPort;
        private int[] temps = new int[30]; // 用于存储温度数据

        public PortLine(INumDel<int[]> numDel)
        {
            _numDel = numDel;
        }

        public PortLine()
        {
        }

        public void OpenPort(string portName, int baudRate = 115200)
        {
            _serialPort = new SerialPort(portName)
            {
                BaudRate = baudRate,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None
            };

            // 数据接收事件
            //  _serialPort.DataReceived += HandleDataReceived;
            // _serialPort.DataReceived += HandleTextDataReceived;
            // _serialPort.DataReceived += HandleNumberReceived;
            _serialPort.DataReceived += HandleFloatReceived;    
            try
            {
                _serialPort.Open();
                Console.WriteLine($"串口 {portName} 打开成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打开串口失败: {ex.Message}");
            }
        }

        public void ClosePort()
        {
            if (_serialPort?.IsOpen == true)
            {
                _serialPort.DataReceived -= HandleDataReceived;
                _serialPort.Close();
                Console.WriteLine("串口已关闭");
            }
        }

        // 发送hex数据（同步方式）
        public void SendData(byte[] data)
        {
            int count = 0;
            if (_serialPort?.IsOpen == true)
            {
                _serialPort.Write(data,0,data.Length); //二进制格式发送数据
                System.Threading.Thread.Sleep(1);
                //                    _serialPort.BaseStream.Flush(); // 刷新缓冲
                count++;
                Console.WriteLine($"数据发送次数：{count}");
                Console.WriteLine($"发送数据十六进制: {BitConverter.ToString(data)}");
                Console.WriteLine($"发送数据（实际内容）: {Encoding.ASCII.GetString(data)}");

            }
        }

        // 异步接收hex数据处理
        private void HandleDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_serialPort.BytesToRead];
            int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);
            Console.WriteLine($"接收到数据: {BitConverter.ToString(buffer, 0, bytesRead)}");
            // 将字节数据转换为字符串（假设是ASCII编码）
            string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"接收到数据（实际内容）: {receivedData}");
        }

        //接收整数数字数据转换为10进制    
        private async void  HandleNumberReceived(object sender, SerialDataReceivedEventArgs e)
        {
            NumDelImp<int[]> numMethod = new NumDelImp<int[]>();
            byte[] buffer = new byte[_serialPort.BytesToRead];
            int bytesRead = _serialPort.Read(buffer, 0, buffer.Length);
            // 打印每个字节的十进制值
            Console.WriteLine("接收到数据（十进制）:");
            foreach (byte b in buffer)
            {
                numMethod.numDely(b);
                NumDelImp<int[]>.newTemp=b;
                Console.WriteLine(b); // 打印字节的十进制值
            }
    
        }

        private async void HandleFloatReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string receivedString = _serialPort.ReadExisting();

            // 去除可能的换行符和空白字符
            receivedString = receivedString.Trim();

            // 明确指定使用点号作为小数点的格式
            if (float.TryParse(receivedString,
                              System.Globalization.NumberStyles.Float,
                              System.Globalization.CultureInfo.InvariantCulture,
                              out float receivedFloat))
            {
                Console.WriteLine($"接收到数据: {receivedFloat}");
            }
            else
            {
                Console.WriteLine($"无效数据: '{receivedString}'");
            }
        }
    }
}
