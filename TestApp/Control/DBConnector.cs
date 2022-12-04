﻿using System;
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
                    COMMIT;"; //Drop existing tables
                    cmnd.CommandText = strSql;
                    cmnd.ExecuteNonQuery();
                    string table = @"CREATE TABLE [ACCOUNT]
                                  ([accountID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                                , [name] TEXT NOT NULL UNIQUE        
                                , [password] TEXT NOT NULL
                                , [type] TEXT NOT NULL
                                );"; // Account table created 
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [Listing]
                            ([listingID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                            , [aID] INTEGER NOT NULL        
                            , [cost] REAL NOT NULL
                            , [address] TEXT NOT NULL UNIQUE
                            , [image] BLOB NOT NULL
                            , [rooms] INTEGER NOT NULL
                            , [bathrooms] INTEGER NOT NULL
                            , FOREIGN KEY([aID]) REFERENCES [ACCOUNT]([accountID])
                            );"; //Listing table created
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [PURCHASE]
                           ([purchaseID] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE
                           , [listingID] INTEGER NOT NULL 
                           , [accountID] INTEGER NOT NULL
                           , [cardNum] INTEGER NOT NULL
                           , [expiration] TEXT NOT NULL
                           , [cvv] INTEGER NOT NULL
                           , [cardHolder] TEXT NOT NULL
                           , FOREIGN KEY([accountID]) REFERENCES [ACCOUNT]([accountID])
                           , FOREIGN KEY([listingID]) REFERENCES [LISTING]([listingID]) ON DELETE NO ACTION
                           );"; // Purchase table created, added card information to the table FIX CCV TO CVV
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    table = @"CREATE TABLE [ACCESS]
                            ([accountID] INTEGER NOT NULL    
                            , [time] TEXT NOT NULL
                            , [type] TEXT NOT NULL
                            , FOREIGN KEY([accountID]) REFERENCES [ACCOUNT]([accountID])
                            , PRIMARY KEY (accountID, time)
                            );"; // Access table created for logins and logouts
                    cmnd.CommandText = table;
                    cmnd.ExecuteNonQuery();
                    strSql = @"BEGIN TRANSACTION; 
                    INSERT INTO ACCOUNT (accountID, name, password, type) VALUES (1, $hashusr1, $hashpwd1, 'buyer');
                    INSERT INTO ACCOUNT (accountID, name, password, type) VALUES (2, $hashusr2, $hashpwd2, 'seller');
                    COMMIT;";

                    cmnd.CommandText = strSql;
                    string buyer = "buyer1";
                    string pwd1 = "pass1";
                    string seller = "seller2";
                    string pwd2 = "pass2";
                    int x = buyer.GetHashCode();
                    int y = pwd1.GetHashCode();
                    int x1 = seller.GetHashCode();
                    int y1 = pwd2.GetHashCode();
                    cmnd.Parameters.AddWithValue("$hashusr1", x);
                    cmnd.Parameters.AddWithValue("$hashpwd1", y);
                    cmnd.Parameters.AddWithValue("$hashusr2", x1);
                    cmnd.Parameters.AddWithValue("$hashpwd2", y1);
                    cmnd.ExecuteNonQuery();
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
