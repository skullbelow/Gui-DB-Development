using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using TestApp.Entity;
using System.Data;
using System.IO; // You must add this for File I/O when inserting pre-made houses

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
                    INSERT INTO LISTING (listingID, aID, cost, address, image, rooms, bathrooms) VALUES (1, 2, 350000, '123 tester road', (@IM1), 5, 2);
                    INSERT INTO LISTING (listingID, aID, cost, address, image, rooms, bathrooms) VALUES (2, 2, 360000, '132 tester road', (@IM2), 10, 4);
                    INSERT INTO LISTING (listingID, aID, cost, address, image, rooms, bathrooms) VALUES (3, 2, 370000, '321 tester road', (@IM3), 15, 6);
                    COMMIT;";

                    //byte[] imageData1 = File.ReadAllBytes(@"C:\Users\genev\OneDrive\Documents\PlayGround\Gui-DB-Development\Houses\259.jpg");
                    //byte[] imageData2 = File.ReadAllBytes(@"C:\Users\genev\OneDrive\Documents\PlayGround\Gui-DB-Development\Houses\635.jpg");
                    //byte[] imageData3 = File.ReadAllBytes(@"C:\Users\genev\OneDrive\Documents\PlayGround\Gui-DB-Development\Houses\IMG_9333.jpg");
                    byte[] imageData1 = File.ReadAllBytes(@"..\..\..\Houses\259.jpg");
                    byte[] imageData2 = File.ReadAllBytes(@"..\..\..\Houses\635.jpg");
                    byte[] imageData3 = File.ReadAllBytes(@"..\..\..\Houses\IMG_9333.jpg");

                    cmnd.Parameters.AddWithValue("@IM1", imageData1);
                    cmnd.Parameters.AddWithValue("@IM2", imageData2);
                    cmnd.Parameters.AddWithValue("@IM3", imageData3);


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

                string stm = @"SELECT * FROM ACCOUNT WHERE name='" + usr.GetHashCode().ToString() + "' AND password='" + pwd.GetHashCode().ToString() + "'";
                using (SQLiteCommand cmnd = new SQLiteCommand(stm, conn))
                {
                    DataTable dt = new DataTable();
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmnd);
                    adapter.Fill(dt);
                    DataRow newRow; // result row based on query pull

                    try
                    {
                        newRow = dt.Rows[0]; // setting the inital row (should be only one result row because of the query syntax)
                        Account acc = new Account(Int32.Parse(newRow[0].ToString()), newRow[1].ToString(), newRow[3].ToString() );
                        return acc;
                    }
                    catch //user enters wrong credentials
                    {
                        Account acc = new Account(-500, "", ""); // dummy Account that gets flagged from accountID of -500
                        return acc;
                    }

                    

                }
            }
        }


        public static DataTable GetListings()
        {
            //Create SQLite connection and open it
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();



            //query to obtain all rows in Listing table
            string query = "SELECT * FROM Listing ;";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Connection = con; // probably unnecessary 


            DataTable dt = new DataTable(); // where query results will get stored
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd); // Constructs a data adapter using the specified select command (i.e. cmd)
            adapter.Fill(dt); // fill out data table with query results

            return dt;
        }



        public static void AddLoginToDB(Account acc)
        {
            SQLiteConnection conn = new SQLiteConnection(@"data source = nAccountDb.db");
            conn.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = conn;
            //MessageBox.Show(DateTime.Now.ToString());
            cmnd.CommandText = @"BEGIN TRANSACTION; INSERT INTO ACCESS (accountID, time, type) VALUES (" + acc.getAccountID().ToString() + ", '" + DateTime.Now.ToString() + "', 'login'); COMMIT;";
            cmnd.ExecuteNonQuery();
            conn.Close();
        }



        public static void AddLogoutToDB(Account acc)
        {
            SQLiteConnection conn = new SQLiteConnection(@"data source = nAccountDb.db");
            conn.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = conn;
            //MessageBox.Show(DateTime.Now.ToString());
            cmnd.CommandText = @"BEGIN TRANSACTION; INSERT INTO ACCESS (accountID, time, type) VALUES (" + acc.getAccountID().ToString() + ", '" + DateTime.Now.ToString() + "', 'logout'); COMMIT;";
            cmnd.ExecuteNonQuery();
            conn.Close();
        }


        public static IDataReader GetImageReader(DataRow row)
        {
            string imgQuery;
            SQLiteCommand imgCmd;
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            imgQuery = "SELECT image FROM Listing WHERE listingID = " + row.ItemArray[0].ToString() + " ;"; // grab picture from db
            imgCmd = new SQLiteCommand(imgQuery, con);
            imgCmd.Connection = con;
            return imgCmd.ExecuteReader(); // reads byte data returned from query
        }



        public static bool InsertListing(byte[] pic, Account account, string cost, string address, string rooms, string bathrooms)
        {


            DataTable dt = GetListings();

            foreach(DataRow row in dt.Rows)
            {
                if(address.Equals(row.ItemArray[3].ToString())) // if the address is already being listed
                {
                    return true;
                }
            }



            //after input validation we need to save the input as a new entry in the Listing table
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;

            cmnd.CommandText = String.Format("BEGIN TRANSACTION; INSERT INTO Listing ( aID, cost, address, image, rooms, bathrooms) VALUES (" + account.getAccountID().ToString() + @", " + cost + @", '" + address + @"', @0," + rooms + @", " + bathrooms + @"); COMMIT;");
            SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
            param.Value = pic;
            cmnd.Parameters.Add(param);
            cmnd.ExecuteNonQuery();

            return false;
            
        }



        public static void InsertPurchaseRemoveListing(string listingID, Account account, string cardNumHash, string expHash, string cvvHash, string cardNameHash)
        {
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;
            //                 @"BEGIN TRANSACTION; INSERT INTO PURCHASE ( listingID, accountID, cardNum, expiration, cvv, cardHolder) VALUES (" + listingID + @", " + account.ToString() + @", " + textBox1.Text.GetHashCode().ToString() + @",'" + textBox2.Text.GetHashCode().ToString() + "', " + textBox3.Text.GetHashCode().ToString() + ", '" + textBox4.Text.GetHashCode().ToString() + @"'); COMMIT;");
            cmnd.CommandText = @"BEGIN TRANSACTION; INSERT INTO PURCHASE ( listingID, accountID, cardNum, expiration, cvv, cardHolder) VALUES (" + listingID + @", " + account.getAccountID().ToString() + @", " + cardNumHash + @",'" + expHash + "', " + cvvHash + ", '" + cardNameHash + @"'); COMMIT;";
            cmnd.ExecuteNonQuery();


            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandText = @"BEGIN TRANSACTION; DELETE FROM Listing WHERE listingID=" + listingID + "; COMMIT;";
            cmd.ExecuteNonQuery();
        }


    }
}
