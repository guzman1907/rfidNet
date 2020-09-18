using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rfidNet
{
    class SettingIpPort
    {
        public int driverPort;
        public string driverHost;
        public int socketPort;
        public string socketHost;

        public SettingIpPort()
        {
            this.driverPort = 100;
            this.driverHost = "192.168.1.200";
            this.socketPort = 1302;
            this.socketHost = "127.0.0.1";
        }

        public SettingIpPort LoadJson()
        {
            using (StreamReader r = new StreamReader("C:\\FileSetting\\SetingsJson.json"))
            {
                string json = r.ReadToEnd();
                SettingIpPort setting = JsonConvert.DeserializeObject<SettingIpPort>(json);
                return setting;
            }
        }
    }
}
