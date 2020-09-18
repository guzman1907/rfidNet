using Newtonsoft.Json;
using RfidApiLib;
using rfidNet.Core;
using rfidNet.TcpClientSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rfidNet
{
    class Driver
    {
        enum ResVal : ushort { SUCCESS = 2001, STOP = 2009 };

        MR6100Api.MR6100Api Api = new MR6100Api.MR6100Api();
        RfidApi rfidApi = new RfidApi();
        //SqlLite sqlLite = new SqlLite();
        List<string> Tags = new List<string>();
        /*
         Conexion mediante Tcp
             */
        public Boolean openConexionTcp(String ip, int port)
        {

            if (Api.TcpConnectReader(ip, port).Equals((int)ResVal.SUCCESS))
                return true;
            else
                return false;
        }

        public Boolean closeConexionTcp()
        {
            if (Api.TcpCloseConnect().Equals((int)ResVal.SUCCESS))
                return true;
            else
                return false;
        }

        public String GetFirmwareVersion()
        {

            int readerAddr = Convert.ToInt32("0XFF", 16);
            byte v1 = 0,
                 v2 = 0;
            string s;

            if (Api.GetFirmwareVersion(readerAddr, ref v1, ref v2).Equals((int)ResVal.SUCCESS))
            {

                s = string.Format("Fireware Version : V{0:d2}.{1:d2}", v1, v2);
                return s.Equals("") ? "No Version" : s;

            }
            else
            {
                return "Error en la version";
            }
        }

        /*
         Conexion mediante commond
             */
        public Boolean OpenComPort(String strProt, int nBound)
        {
            if (Api.OpenCommPort(strProt, nBound).Equals((int)ResVal.SUCCESS))
                return true;
            else
                return false;
        }

        public void CloseComPort()
        {
            Api.CloseCommPort();
        }

        public List<String> IsoReadWithID(String IsoAddr, String IsoCnt)
        {

            List<String> tags = new List<string>();
            int addr;
            int len;
            int i = 0;
            int status = 0;
            byte byAntenna = 0;
            byte[] TagID = new byte[16];
            byte[] value = new byte[32];
            string s = "The data is:";
            string s1 = "";

            try
            {

                addr = int.Parse(IsoAddr);
                len = int.Parse(IsoCnt);

            }
            catch
            {

            }

            return null;

        }

        public void IsoMultiTagIdentify(SqlLite sqlLite)
        {

            List<String> tags = new List<string>();
            int status;
            int i, j;
            byte[,] tag_buf = new byte[100, 14];
            int tag_cnt = 0;
            byte tag_flag = 0;
            int getCount = 0;
            int readerAddr = Convert.ToInt32("0XFF", 16);
            string s = "";
            string s1 = "";

            try
            {

                do
                {
                    status = Api.EpcMultiTagIdentify(readerAddr, ref tag_buf, ref tag_cnt, ref tag_flag);
                    if (status.Equals(2009))
                    {
                        throw new Exception();
                    }

                    //if (tag_flag == 1)
                    //this.BackColor = Color.MediumBlue;
                    //else
                    //this.BackColor = Color.MidnightBlue;

                    if (tag_cnt >= 100)
                    {
                        throw new Exception();
                    }



                    if (tag_cnt > 0)
                    {
                        try
                        {
                            Tags = sqlLite.SelectData();

                            for (i = 0; i < tag_cnt; i++)
                            {
                                // s1 = string.Format("NO.{0:D}: ", tag_cnt);
                                //s1 += string.Format("[ANT{0:D}]", tag_buf[i, 1] + 1);
                                for (j = 2; j < 14; j++)
                                {
                                    s = string.Format("{0:X2}", tag_buf[i, j]);
                                    s1 += s;

                                }
                            }

                            if (s1 == "000000000000000000000000")
                            {
                                //libInfo.Items.Add("000");
                                continue;
                            }

                            if (!Tags.Contains(s1))
                            {
                                Tags.Add(s1);
                                sqlLite.AddData(s1);
                                TcpClientSocketApp tcp = new TcpClientSocketApp();
                                tcp.TcpClientConnect(s1);

                                new Task(() =>
                                {
                                    Thread.Sleep(30000);
                                    sqlLite.RemoveData(s1);
                                }).Start();
                            }


                        }
                        catch
                        {

                        }

                    }



                } while (tag_cnt == 0);
                IsoMultiTagIdentify(sqlLite);
            }

            catch
            {
                throw new Exception();

            }
        }

    }
}
