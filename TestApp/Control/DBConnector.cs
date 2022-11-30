using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TestApp.Entity;

namespace TestApp.Control
{
    public static class DBConnector
    {
        public static void InitializeDB()
        {
            SQLiteConnection.CreateFile("nAccountDb.db");
            using (SQLiteConnection conn = new SQLiteConnection(@"data source = nAccountDb.db"))
            {
                using (SQLiteCommand cmnd = new SQLiteCommand())
                {
                    conn.Open();
                    cmnd.Connection = conn;
                    string strSql = @"BEGIN TRANSACTION; 
                    DROP TABLE IF EXISTS ACCOUNT;
                    DROP TABLE IF EXISTS Listing;
                    DROP TABLE IF EXISTS Property;
                    DROP TABLE IF EXISTS Purchase;
                    COMMIT;";
                    cmnd.CommandText = strSql;
                    cmnd.ExecuteNonQuery();
                    string table = @"CREATE TABLE [ACCOUNT]
                                  ([accountID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                                , [name] TEXT NOT NULL UNIQUE        
                                , [password] TEXT NOT NULL
                                , [type] TEXT NOT NULL
                                );"; // name and password added here
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [Listing]
                            ([listingID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                            , [aID] INTEGER NOT NULL        
                            , [cost] REAL NOT NULL
                            , [address] TEXT NOT NULL UNIQUE
                            , [image] BLOB
                            , FOREIGN KEY([aID]) REFERENCES [ACCOUNT]([accountID])
                            );";
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [Logout]
                            ([logID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                            , [aID] INTEGER NOT NULL
                            , [time] TEXT NOT NULL
                            , [date] TEXT NOT NULL
                            , FOREIGN KEY([aID]) REFERENCES [ACCOUNT]([accountID])
                            );";
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [PURCHASE]
                           ([purchaseID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                           , [listingID] INTEGER NOT NULL
                           , [accountID] INTEGER NOT NULL
                           , FOREIGN KEY([accountID]) REFERENCES [ACCOUNT]([accountID])
                           , FOREIGN KEY([listingID]) REFERENCES [LISTING]([listingID])
                           );";
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    strSql = @"BEGIN TRANSACTION; 
                    INSERT INTO ACCOUNT (accountID, name, password, type) VALUES (1, 'buyer1', 'pass1', 'buyer');
                    COMMIT;";
                    cmnd.CommandText = strSql; // added by pat
                    cmnd.ExecuteNonQuery(); // added by eve
                    //cmnd.CommandText = strSql;
                    //string usrname1 = "cus";

                    /*cmnd.CommandText = strSql;
                    string buyer = "BuyThisProperty";
                    string pwd1 = "HousingMarketIsExpesinve";
                    string seller = "emp";
                    string pwd2 = "2wsx";
                    int x = buyer.GetHashCode();
                    int y = pwd1.GetHashCode();
                    int x1 = seller.GetHashCode();
                    int y1 = pwd2.GetHashCode();
                    cmnd.Parameters.AddWithValue("$hashusr1", x);
                    cmnd.Parameters.AddWithValue("$hashpwd1", y);
                    cmnd.Parameters.AddWithValue("$hashusr2", x1);
                    cmnd.Parameters.AddWithValue("$hashpwd2", y1); */
                    //cmnd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
        public static Account GetUser(string usr, string pwd)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"data source = nAccountDb.db"))
            {
                conn.Open();
                int x = usr.GetHashCode();
                int y = pwd.GetHashCode();
                string stm = @"SELECT[Id]
                        ,[username]
                        ,[password]
                        ,[type]
                        FROM[ACCOUNT]
                        WHERE[username] == ($name)
                        AND[password] == ($pd);";
                using (SQLiteCommand cmnd = new SQLiteCommand(stm, conn))
                {
                    cmnd.Parameters.AddWithValue("$name", x);
                    cmnd.Parameters.AddWithValue("$pd", y);
                    using (SQLiteDataReader rdr = cmnd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Account acct = new Account(rdr.GetInt32(0), usr, rdr.GetString(3));
                            return acct;
                        }
                        Account act = new Account(0, null, null);
                        return act;
                    }
                }
            }
        }
    }
}
