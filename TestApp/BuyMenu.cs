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

namespace TestApp
{
    public partial class BuyMenu : Form
    {
        public BuyMenu()
        {
            InitializeComponent();
            //after input validation we need to save the input as a new entry in the Listing table
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();



            //need to populate the page with listing images and information
            string query = "SELECT image FROM Listing ;";
            SQLiteCommand cmd = new SQLiteCommand(query, con);
            cmd.Connection = con;
            //adapter
            //datatable
            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(dt);

            List<PictureBox> pictureBoxes = new List<PictureBox>();

            List<PictureBox> pictureBoxesGUI = new List<PictureBox>();
            pictureBoxesGUI.Add(pictureBox1);
            pictureBoxesGUI.Add(pictureBox2);
            pictureBoxesGUI.Add(pictureBox3);
            /*pictureBoxesGUI.Add(pictureBox4);
            pictureBoxesGUI.Add(pictureBox5);
            pictureBoxesGUI.Add(pictureBox6);
            pictureBoxesGUI.Add(pictureBox7);
            pictureBoxesGUI.Add(pictureBox8);*/


            foreach (DataRow row in dt.Rows)
            {
                pictureBoxes.Add(new PictureBox());
            }

            IDataReader rdr;
            SQLiteCommand imgCmd;
            string imgQuery;

            for (int i = 1; i < pictureBoxes.Count + 1; i++)
            {
                imgQuery = "SELECT image FROM Listing WHERE listingID = " + i.ToString() + " ;";
                imgCmd = new SQLiteCommand(imgQuery, con);
                imgCmd.Connection = con;
                rdr = imgCmd.ExecuteReader();

                try
                {
                    while (rdr.Read())
                    {
                        byte[] a = (System.Byte[])rdr[0];
                        pictureBoxes[(pictureBoxes.Count - 1) - (i - 1)].Image = ByteToImage(a);
                    }
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); }
            }

            int count = 0;
            foreach (PictureBox pb in pictureBoxes)
            {
                pictureBoxesGUI[count].SizeMode = PictureBoxSizeMode.Zoom;
                pictureBoxesGUI[count].Image = pb.Image;
                count++;
            }
        }

        private void button1_Click(object sender, EventArgs e)// buy button should take a buyer to the buy form of a selected listing
        {
            this.Hide();
            new BuyListing().Show();
        }

        private void button2_Click(object sender, EventArgs e)//logout button should return the user to the home page
        {
            this.Hide();
            new Login().Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        //public Image Base64ToImage(string base64String)
        public Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }
    }
}
