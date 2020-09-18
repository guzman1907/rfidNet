using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rfidNet.Core
{
    public class SqlLite
    {
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;
        public void CreateDatabaseAndTable()
        {
            if (!File.Exists("TagsTfid.sqlite"))
            {
                SQLiteConnection.CreateFile("TagsTfid.sqlite");

                //string sql = @"CREATE TABLE Tag(
                //               ID INTEGER PRIMARY KEY AUTOINCREMENT ,
                //               TagId           TEXT      NOT NULL,
                //            );";
                string sql = "CREATE TABLE [Tag] ([id] INTEGER PRIMARY KEY NOT NULL , [TagId] TEXT)";

                con = new SQLiteConnection("Data Source=TagsTfid.sqlite;Version=3;");
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
                con.Close();

            }
            else
            {
                con = new SQLiteConnection("Data Source=TagsTfid.sqlite;Version=3;");
            }
        }

        public void AddData(string TagId)
        {
            try
            {
                cmd = new SQLiteCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into Tag(TagId) values ('" + TagId + "')";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        public void SelectDataByTagId(string TagId)
        {
            int counter = 0;
            cmd = new SQLiteCommand($"Select * From Tag where Tag ={TagId}", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                counter++;
                Console.WriteLine(dr[0] + " : " + dr[1] + " " + dr[2]);

            }
            con.Close();

        }
        public List<string> SelectData()
        {
            List<string> tags = new List<string>();
            cmd = new SQLiteCommand("Select * From Tag", con);
            con.Open();
            dr = cmd.ExecuteReader();
          
            while (dr.Read())
            {

                var tagId = dr[1].ToString();
                tags.Add(tagId);
            }
            con.Close();
            return tags;
        }
        public void RemoveData(string TagId)
        {
            int counter = 0;
            cmd = new SQLiteCommand($"DELETE FROM Tag WHERE TagId = '{TagId}' and id > 0; ", con);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                counter++;
                Console.WriteLine(dr[0] + " : " + dr[1] + " " + dr[2]);

            }
            con.Close();

        }
    }
}
