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
using System.Text.RegularExpressions;

using System.IO; // Added for image to byte conversion (i.e. MemoryStream object in ImageToByte() )

namespace TestApp
{
    public partial class CreateListing : Form
    {
        private int accountID;

        public CreateListing(int aID)
        {
            this.accountID = aID;
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // Upload Button: this is used to intiate the upload of a listing image
        {
            // Create image.
            //Image newImage = Image.FromFile("C:\\Users\\genev\\OneDrive\\Pictures\\Houses\\635.jpg");

            String imgLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg|png files(*.png)|*.png";

                if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imgLocation = dialog.FileName;
                    pictureBox1.ImageLocation = imgLocation;
                }
            }
            catch
            {
                MessageBox.Show("Wrong filetype. Try again.");
                return;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e) // address input box
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) //cost input box
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) //number of rooms
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) //number of bathrooms
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)// image display box
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //Submit Button: should validate the form, update the database, and return the seller to the seller menu
        {
            //input validation block
            Regex r = new Regex("[a-zA-Z0-9,. ]+"); //address needs to accept commas, spaces, and periods as well
            if (!r.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Address can only be alphanumeric. Try again.");
                return;// terminate early
            }
            r = new Regex("^[0-9]+$"); //cost needs to accept only numbers
            if (!r.IsMatch(textBox2.Text))
            {
                MessageBox.Show("Cost can only be numeric. Try again.");
                return;// terminate early
            }
            r = new Regex("^[a-zA-Z0-9,.' ]+"); //Description needs to accept commas, spaces, apostrophes, periods. 
            if (!r.IsMatch(textBox1.Text))
            {
                MessageBox.Show("Descriptions can only be alphanumeric. Try again.");
                return;// terminate early
            }
            if (!(Decimal.ToInt32(numericUpDown1.Value) < 11) && !(Decimal.ToInt32(numericUpDown2.Value) < 11))
            {
                MessageBox.Show("There can be no more than 10 rooms or 10 bathrooms. Try again.");
                return;// terminate early
            }


            Image photo = new Bitmap(pictureBox1.ImageLocation);
            byte[] pic = ImageToByte(photo, System.Drawing.Imaging.ImageFormat.Jpeg);



            //after input validation we need to save the input as a new entry in the Listing table
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;

            cmnd.CommandText = String.Format("BEGIN TRANSACTION; INSERT INTO Listing ( aID, cost, address, image, rooms, bathrooms) VALUES (" + accountID.ToString() + @", " + textBox2.Text + @", '" + textBox1.Text + @"', @0," + numericUpDown1.Value.ToString() + @", " + numericUpDown2.Value.ToString() + @"); COMMIT;"); 
            SQLiteParameter param = new SQLiteParameter("@0", System.Data.DbType.Binary);
            param.Value = pic;
            cmnd.Parameters.Add(param);
            cmnd.ExecuteNonQuery();

            //for debugging purposes only
            /*string nq = ("SELECT * FROM Listing");

            SQLiteCommand cmd = new SQLiteCommand(nq, con);
            //adapter
            //datatable
            DataTable dt = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(dt);

             foreach (DataRow row in dt.Rows) 
             {
                 foreach (DataColumn column in dt.Columns)
                 {
                     MessageBox.Show(row[column].ToString());
                 }
             }*/

            this.Hide();
            new SellMenu(accountID).Show();
        }



        public byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }







    }
}
