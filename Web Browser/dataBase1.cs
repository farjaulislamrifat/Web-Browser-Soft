using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace Web_Browser
{

    internal class dataBase1
    {
        public void addData(string link, string title)
        {
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            command.CommandText = "insert into bookmark(link , title ) values ('" + link + "',' " + title + "' )";
            command.ExecuteNonQuery();
            con.Close();
        } 
        public void HaddData(string link, string title)
        {
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            try
            {
            command.CommandText = "insert into history( link , title ) values ('" + link + "',' " + title + "' )";
            command.ExecuteNonQuery();

            }catch(Exception e)
            {

            }
            con.Close();
        }
        public void deleteData(int title)
        {
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            List<List<string>> data = new List<List<string>>();
            data = getData();
            for (int i = 0; i < data[2].Count; i++)
            { 
                command.CommandText = "UPDATE bookmark SET id = '" + (i + 1) + "' WHERE id = '" + data[2][i] + "'";
                try
                {
                command.ExecuteNonQuery();
                }catch(Exception v)
                {
                }
            }
            command.CommandText = "DELETE FROM bookmark WHERE id = '"+title+"' ";
            command.ExecuteNonQuery();
            con.Close();
        } public void HdeleteData(int id)
        {
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            List<List<string>> data = new List<List<string>>();
            data = HgetData();
            for (int i = 0; i < data[2].Count; i++)
            {
            command.CommandText = "UPDATE history SET id = '"+(i+1)+"' WHERE id = '" + data[2][i] +"'";
                try
                {

            command.ExecuteNonQuery();
                }catch(Exception g)
                {
                 
                }
            }
            command.CommandText = "DELETE FROM history WHERE id = '" + id  + "' ";
            command.ExecuteNonQuery();
            con.Close();
        }
        public void hdeleteallhistory()
        {
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
           
            command.CommandText = "DELETE FROM history  ";
            command.ExecuteNonQuery();
            con.Close();
        }
        public List<List<string>> getData()
        {
            SQLiteDataReader d;
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            command.CommandText = "Select* From bookmark";
            d = command.ExecuteReader();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            while (d.Read())
            {
                list3.Add(d.GetInt32(0).ToString());
                list1.Add(d.GetString(1));
                list2.Add(d.GetString(2)); 
            }
            List<List<string>> list4 = new List<List<string>>();
            list4.Add(list1);
            list4.Add(list2);
            list4.Add(list3);
            return list4;
        } 
        public List<List<string>> HgetData()
        {
            SQLiteDataReader d;
            SQLiteConnection con = new SQLiteConnection("data.db");
            con = new SQLiteConnection("Data Source=data.db;Version=3;");
            con.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = con;
            command.CommandText = "Select* From history";
            d = command.ExecuteReader();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            while (d.Read())
            {
                list3.Add(d.GetInt32(0).ToString());
                list1.Add(d.GetString(1));
                list2.Add(d.GetString(2));             
            }

            List<List<string>> list4 = new List<List<string>>();
            list4.Add(list1);
            list4.Add(list2);
            list4.Add(list3);
            return list4;
        }
    }
}
