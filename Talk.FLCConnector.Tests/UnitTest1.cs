using System.Net;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace Talk.FLCConnector.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            TcpClient tcp = new TcpClient();
            tcp.Connect(IPAddress.Parse("127.0.0.1"), 102);
            var BufferSize = 999;
            if (tcp.Connected)
            {
                var msg = "123123";
                NetworkStream streamToServer = tcp.GetStream();
                byte[] buffer = Encoding.Unicode.GetBytes(msg); //msgΪ���͵��ַ���    
                lock (streamToServer)
                {
                    streamToServer.Write(buffer, 0, buffer.Length);     // ����������
                }
                //�����ַ��� 
                buffer = new byte[BufferSize];
                lock (streamToServer)
                {
                    var bytesRead = streamToServer.Read(buffer, 0, BufferSize);
                }
            }
        }
    }
}
