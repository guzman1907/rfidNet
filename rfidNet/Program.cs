using Newtonsoft.Json;
using rfidNet.TcpClientSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rfidNet.Core;

namespace rfidNet
{
    class Program
    {

        static void Main(string[] args)
        {
        connection:

            try
            {

                Driver dri = new Driver();
                SettingIpPort settingIp = new SettingIpPort();
                SqlLite sqlLite = new SqlLite();
                sqlLite.CreateDatabaseAndTable();

                var setting = settingIp.LoadJson();


                if (dri.openConexionTcp(setting.driverHost, setting.driverPort))
                {
                    Console.WriteLine("Conexion Establecida");

                    dri.IsoMultiTagIdentify(sqlLite);


                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Error en la Conexion");
                    goto connection;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la Conexion");
                goto connection;

            }


        }

       

    }
}
