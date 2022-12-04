﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Windows.Media.Imaging; //to test BitmapImage class

namespace TestApp
{
    public partial class SellMenu : Form
    {
        private int account;
        struct TBs
        {
            public TextBox cost;
            public TextBox address;
        }

        public SellMenu(int aID)
        {
            this.account = aID;
            InitializeComponent();


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

            List<PictureBox> pictureBoxes = new List<PictureBox>(); // Where our created picture boxes go


            List<TBs> textBoxes = new List<TBs>(); // Where text boxes for each picture go


            foreach (DataRow row in dt.Rows)
             {
                pictureBoxes.Add(new PictureBox()); // create new PictureBox per each row retrieved from query
                textBoxes.Add(new TBs()); 
             }

            // Honestly, to understand picture handling better, read this:
            // https://stackoverflow.com/questions/10853301/save-and-load-image-sqlite-c-sharp

             IDataReader rdr;
             SQLiteCommand imgCmd;
             string imgQuery;

            TextBox tb; // = new TextBox();
            TBs tbs;

            int i = 1;
            //for (int i = 1; i < pictureBoxes.Count+1; i++) // for each picture/picturebox
            foreach (DataRow row in dt.Rows)
            {
                 imgQuery = "SELECT image FROM Listing WHERE listingID = " + row.ItemArray[0].ToString() + " ;"; // grab picture from db
                 imgCmd = new SQLiteCommand(imgQuery, con);
                 imgCmd.Connection = con;
                 rdr = imgCmd.ExecuteReader(); // reads byte data returned from query

                 try
                 {
                     while(rdr.Read()) // while there is still data to read
                     {
                         byte[] a = (System.Byte[])rdr[0]; // obtain image as byte array

                        // transform byte array to image object
                        // then assign images to picture boxes in order of most recent to oldest 
                        //pictureBoxes[(pictureBoxes.Count - 1) - (i-1)].Image = ByteToImage(a); 
                        pictureBoxes[i - 1].Image = ByteToImage(a);
                    }
                 }
                 catch (Exception exc) { MessageBox.Show(exc.Message); }


                //MessageBox.Show("num attributes: " + dr.ItemArray.Length.ToString());
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    switch (j)
                    {
                        case 2: // Cost 
                            tb = new TextBox();
                            tb.Multiline = true;
                            tb.ReadOnly = true;
                            tb.AutoSize = false;
                            tb.BorderStyle = BorderStyle.None;
                            tb.Text = $"Cost of house: {row.ItemArray[j]}$";
                            tb.Size = new System.Drawing.Size(200, 15);
                            tbs = textBoxes[i-1];
                            tbs.cost = tb;
                            textBoxes[i-1] = tbs;
                            break;
                        case 3:
                            tb = new TextBox();
                            tb.Multiline = true;
                            tb.ReadOnly = true;
                            tb.AutoSize = false;
                            tb.BorderStyle = BorderStyle.None;
                            tb.Text = $"Address of house: {row.ItemArray[j]}";
                            tb.Size = new System.Drawing.Size(200, 15);
                            tbs = textBoxes[i-1];
                            tbs.address = tb;
                            textBoxes[i-1] = tbs;
                            break;
                        default:
                            // do nothing (for now...)
                            break;
                    }
                }

                i++;
            }



             // NOTE: Form must have AutoScroll == true so that contents become scrollable after pictures take up entire page, i.e. :
             this.AutoScroll = true;

             int count = 0;
             foreach(DataRow row in dt.Rows) 
             {
                DisplayImage(textBoxes[(pictureBoxes.Count - 1) - count] ,pictureBoxes[(pictureBoxes.Count-1) - count],count);
                count++;
             }


        }

        private void button1_Click(object sender, EventArgs e) //Sell button should lead the user to the create listing form for a listing to be created.
        {
            this.Hide();
            new CreateListing(account).Show();
        }

        private void button2_Click(object sender, EventArgs e) // Logout button should return the user to the login form
        {
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;
            //MessageBox.Show(account.ToString());
            cmnd.CommandText = @"BEGIN TRANSACTION; INSERT INTO ACCESS (accountID, time, type) VALUES (" + account.ToString() + ", '" + DateTime.Now.ToString() + "', 'logout'); COMMIT;";
            cmnd.ExecuteNonQuery();

            this.Hide();
            new Login().Show();
        }
        
        // helper func to turn byte[] into image obj in constructor
        public Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }

        // helper method for displaying image
        private void DisplayImage(TBs tbs, PictureBox pb, int count, int perRow=3)
        {
            pb.Width = 200;
            pb.Height = 200;
            int y = count / perRow;
            int x = (count + perRow) % perRow;
            pb.Location = new Point(50 + x*300, 100 + 300*y);
            pb.SizeMode = PictureBoxSizeMode.Zoom; // so they whole picture fits in box
            Controls.Add(pb); // Actually displays the damn picturebox


            tbs.address.Location = new Point(50 + x * 300, 300 + 300 * y); // same as picturebox but a little down
            tbs.cost.Location = new Point(50 + x * 300, 330 + 300 * y); // just below cost text

            Controls.Add(tbs.cost);
            Controls.Add(tbs.address);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


    }
}