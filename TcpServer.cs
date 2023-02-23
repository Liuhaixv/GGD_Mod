using Il2CppSystem;
using MelonLoader;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GGD_Hack
{ 
public class TCPTestServer
    {
        private TcpListener listener;
        private TcpClient client;
        private int clientsDisconnectedNum = 0;

        public TCPTestServer(int port)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(ipAddress, port);
            MelonLogger.Msg("TCP服务器监听端口为: " + port.ToString());
        }

        public void Start()
        {
            Thread t = new Thread(StartServer);
            t.Start();
        }

        private void StartServer()
        {
            listener.Start();
            MelonLogger.Msg("Server started. Waiting for a connection...");

            while (true)
            {
                //等待新的客户端连接
                TcpClient newClient = listener.AcceptTcpClient();
                MelonLogger.Msg(System.String.Format("[{0}]", clientsDisconnectedNum.ToString()) + "New client connected.");

                //如果当前已经有一个客户端连接，则关闭之前的连接
                if (client != null)
                {
                    client.Close();
                    MelonLogger.Msg("Previous client disconnected.");
                    clientsDisconnectedNum++;
                }

                //处理新客户端连接
                client = newClient;
                HandleClient(client);
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (true)
            {
                //从客户端读取数据
                bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    //读取异常处理
                    break;
                }

                //如果客户端断开连接，bytesRead为0
                if (bytesRead == 0)
                {
                    MelonLogger.Msg("Client disconnected.");
                    break;
                }

                //将收到的数据转换成字符串并输出
                string messageReceived = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MelonLogger.Msg("Received: {0}", messageReceived);

                //发送到CommandHandler
                bool success = CommandHandler.HandleMessage(messageReceived);

                //将收到的数据转换成大写并发送回客户端
                //string dataToSend = messageReceived.ToUpper();
                string responseString = null;
                if (success)
                {
                    responseString = "success";
                }else
                {
                    responseString= "error";
                }

                byte[] data = System.Text.Encoding.UTF8.GetBytes(responseString);
                try
                {                    
                    stream.Write(data, 0, data.Length);                    
                }
                catch
                {
                    //写入异常处理
                    break;
                }
            }

            //客户端连接关闭后清理资源
            client.Close();
            client = null;
        }

        public void Stop()
        {
            listener.Stop();

            //关闭当前客户端连接
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }
    }
}
