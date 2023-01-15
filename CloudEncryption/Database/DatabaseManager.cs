using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudEncryption.Encryption;

namespace CloudEncryption.Database
{
    public class DatabaseManager
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // modifiable : SQLiteConnection constructor arguments
            sqlite_conn = new SQLiteConnection("Data Source=database.db; " +
                "Version = 3; " +
                "New = True; " +
                "Compress = True; ");
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                //
            }
            return sqlite_conn;
        }
        public static bool CreateTable(SQLiteConnection conn)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "CREATE TABLE USERS(username varchar(20), email varchar(30), password varchar(256));";
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool InsertData(SQLiteConnection conn, string Query)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = Query;
                sqlite_cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool CheckUser(SQLiteConnection conn, string userToCheck)
        {
            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM USERS WHERE username='" + userToCheck + "';";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if (sqlite_datareader.GetString(0) == userToCheck)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex) { return false; }
            return true;
        }
        public static bool CheckPassword(SQLiteConnection conn,string username, string passToCheck)
        {
            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT password FROM USERS WHERE username='" + username + "';";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    if(EncryptionHandler.AES_Encrypt(passToCheck) == sqlite_datareader.GetString(0))
                    {
                        break;
                    }
                    return false;
                }
            }
            catch (Exception ex) { return false; }
            return true;
        }
        public static string GetEmail(SQLiteConnection conn, string userToCheck)
        {
            if(CheckUser(CreateConnection(), userToCheck))
            {
                try
                {
                    SQLiteDataReader sqlite_datareader;
                    SQLiteCommand sqlite_cmd;
                    sqlite_cmd = conn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT email FROM USERS where username='" + userToCheck + "';";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    while (sqlite_datareader.Read())
                    {
                        if(sqlite_datareader.GetString(0) != null)
                        {
                            conn.Close();
                            return sqlite_datareader.GetString(0);
                        }    
                    }
                    return "false";
                } catch { }
            }
            return "false";
        }
        public static bool Register(SQLiteConnection conn, string username, string email, string password)
        {
            password = EncryptionHandler.AES_Encrypt(password);
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO USERS VALUES('"+username+"','"+email+"','"+password+"');";
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public static bool Login(SQLiteConnection conn, string username, string password)
        {
            password = EncryptionHandler.AES_Encrypt(password);
            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT PASSWORD FROM USERS WHERE USERNAME='" + username + "';";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                string db_return = "";
                while (sqlite_datareader.Read())
                {
                    db_return += sqlite_datareader.GetString(0);
                }
                conn.Close();
                if (password == db_return)
                {
                    return true;
                }
            } catch (Exception ex) { }
            return false;
        }
    }
}
