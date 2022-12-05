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
using TestApp.Entity;
using TestApp.Control;

namespace TestApp
{
    public partial class CreateListing : Form
    {
        private Account account;

        public CreateListing(Account aID)
        {
            this.account = aID;
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e) // Upload Button: this is used to intiate the upload of a listing image
        {

            String imgLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                //dialog.Filter = "jpg files(*.jpg)|*.jpg|png files(*.png)|*.png";
                dialog.Filter = "jpg files(*.jpg)|*.jpg"; // only allow  jpg's due to System.Drawing.Imaging.ImageFormat.Jpeg in button1_Click()

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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



        private void button1_Click(object sender, EventArgs e) //Submit Button: should validate the form, update the database, and return the seller to the seller menu
        {

            string validity = CreateListingControl.IsValid(textBox1.Text, textBox2.Text, Decimal.ToInt32(numericUpDown1.Value), Decimal.ToInt32(numericUpDown2.Value));
            if (!validity.Equals("All Good!"))
            {
                MessageBox.Show(validity);
                return; // terminiate early
            }


            try
            {
                pictureBox1.ImageLocation.Equals(""); // this will throw exception if picture is not selected...
            }
            catch //... but that is okay because we handle the exception accordingly here
            {
                MessageBox.Show("You must upload a picture to create listing.");
                return;// terminate early
            }

            Image photo = new Bitmap(pictureBox1.ImageLocation);

            if( CreateListingControl.InsertListing(photo,account,textBox2.Text,textBox1.Text,numericUpDown1.Value.ToString(),numericUpDown2.Value.ToString())) // In the event the address was already taken
            {
                MessageBox.Show("This address is already being listed.");
                return;// terminate early
            }


            this.Hide();
            new SellMenu(account).Show();
        }

        private void button3_Click(object sender, EventArgs e) //Back Button: returns to sell menu, cancels create listing form
        {
            this.Hide();
            new SellMenu(account).Show();
        }
    }
}
