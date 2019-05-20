using System.Net;
using System.Net.Sockets;

namespace Talk.FLCConnector
{
    public class SocketHelper
    {
        public Socket socket;
        private byte[] plcHead1 = new byte[22]
        {
            0x03,0x00,0x00,0x16,0x11,0xE0,0x00,0x00,0x00,0x01,0x00,0xC0,0x01,0x0A,0xC1,0x02,
            0x01,0x02,0xC2,0x02,0x01,0x00
        };
        private byte[] plcHead2 = new byte[25]
        {
            0x03,0x00,0x00,0x19,0x02,0xF0,0x80,0x32,0x01,0x00,0x00,0x04,0x00,0x00,0x08,0x00,
            0x00,0xF0,0x00,0x00,0x01,0x00,0x01,0x01,0xE0
        };
        public void init()
        {
            //if (socket == null)
            //{
            //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //}

            //socket.BeginSend(state.Buffer, state.AlreadyDealLength, state.DataLength - state.AlreadyDealLength,
            //     SocketFlags.None, new AsyncCallback(SendCallBack), state);


            TcpClient tcp = new TcpClient();
            tcp.Connect(IPAddress.Parse("127.0.0.1"), 102);
        }
    }
}
