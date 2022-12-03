using System;
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

            /*List<PictureBox> pictureBoxesGUI = new List<PictureBox>(); // A list of the preplaced picture boxes
            // Adding all preplaced picture boxes
            pictureBoxesGUI.Add(pictureBox1); 
            pictureBoxesGUI.Add(pictureBox2);
            pictureBoxesGUI.Add(pictureBox3);
            pictureBoxesGUI.Add(pictureBox4);
            pictureBoxesGUI.Add(pictureBox5);
            pictureBoxesGUI.Add(pictureBox6);
            pictureBoxesGUI.Add(pictureBox7);
            pictureBoxesGUI.Add(pictureBox8);*/


            foreach (DataRow row in dt.Rows)
             {
                 pictureBoxes.Add(new PictureBox()); // create new picture box per each row retrieved from query
             }

            // Honestly, to under stand below better, read this:
            // https://stackoverflow.com/questions/10853301/save-and-load-image-sqlite-c-sharp

            IDataReader rdr;
             SQLiteCommand imgCmd;
             string imgQuery;

             for(int i = 1; i < pictureBoxes.Count+1; i++) // for each picture/picturebox
             {
                 imgQuery  = "SELECT image FROM Listing WHERE listingID = " + i.ToString() + " ;"; // grab picture from db
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
                        pictureBoxes[(pictureBoxes.Count - 1) - (i-1)].Image = ByteToImage(a); 
                     }
                 }
                 catch (Exception exc) { MessageBox.Show(exc.Message); }
             }


            // NOTE: Form must have AutoScroll == true so that contents become scrollable after pictures take up entire page
            int count = 0;
             foreach(PictureBox pb in pictureBoxes) 
             {
                // Method 1
                // for each existing picture box in GUI, give them the images of the picture boxes filled in above

                // WARNING: THIS WILL CRASH AS CURRENTLY DESIGNED IF NUMBER OF IMAGES IN LISTING TABLE EXCEEDS
                // THE NUMBER OF PICTURE BOXES PRE-PLACED IN DESIGN GUI

                /*pictureBoxesGUI[count].SizeMode = PictureBoxSizeMode.Zoom; // so they whole picture fits in box
                pictureBoxesGUI[count].Image = pb.Image;*/

                // Method 2

                DisplayImage(pb,count);

                count++;
             }

            


            // Code for obtaining info from each row
            /*foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    MessageBox.Show(row[column].ToString());
                }
            }*/


            ///////////     PSUEDO CODING OUT IDEAS

            // run query above and save to "results"

            // create Picturebox list "pList"

            // for each row in "results"
            //      pList.add(new PictureBox())

            // for each PictureBox in pList
            //      PictureBox.path = corresponding row information
            //      PictureBox.click() = {displayStuff();}




        }

        private void button1_Click(object sender, EventArgs e) //Sell button should lead the user to the create listing form for a listing to be created.
        {
            this.Hide();
            new CreateListing(account).Show();
        }

        private void button2_Click(object sender, EventArgs e) // Logout button should return the user to the login form
        {
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
        private void DisplayImage(PictureBox pb, int count, int perRow=3)
        {
            pb.Width = 200;
            pb.Height = 200;
            int y = count / perRow;
            int x = (count + perRow) % perRow;
            pb.Location = new Point(50 + x*300, 100 + 300*y);
            pb.SizeMode = PictureBoxSizeMode.Zoom; // so they whole picture fits in box
            Controls.Add(pb);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
