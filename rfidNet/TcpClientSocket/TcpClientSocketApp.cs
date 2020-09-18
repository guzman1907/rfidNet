using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace rfidNet.TcpClientSocket
{
    public class TcpClientSocketApp
    {
        public void TcpClientConnect(string messageToSend)
        {
        connection:
            try
            {
                SettingIpPort settingIp = new SettingIpPort();

                var setting = settingIp.LoadJson();

                TcpClient client = new TcpClient(setting.socketHost, setting.socketPort);
                // messageToSend = "My name is Neo";
                messageToSend = $"{messageToSend}";
                int byteCount = Encoding.ASCII.GetByteCount(messageToSend + 1);
                byte[] sendData = Encoding.ASCII.GetBytes(messageToSend);

                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                Console.WriteLine("sending data to server...");

                //StreamReader sr = new StreamReader(stream);
                //string response = sr.ReadLine();
                //Console.WriteLine(response);

                //stream.Close();
                client.Close();
                //Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to connect...");
                goto connection;
            }
        }
    }
}
