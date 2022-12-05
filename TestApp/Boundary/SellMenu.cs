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
using TestApp.Entity;
using TestApp.Control;

namespace TestApp
{
    public partial class SellMenu : Form
    {
        private Account account;


        public SellMenu(Account acc)
        {
            
            InitializeComponent();

            // NOTE: Form should have AutoScroll == true so that contents become scrollable after pictures take up entire page, i.e. :
            this.AutoScroll = true;

            this.account = acc;

            List<Listing> listings = SellMenuControl.FormatListings();



            int count = 0;
            foreach (Listing listing in listings)
            {
                DisplayImage(listing.adressTextBox, listing.costTextBox, listing.pictureBox, count);
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
            SellMenuControl.AddLogoutToDB(account);
            this.Hide();
            new Login().Show();
        }


        // helper method for displaying image
        private void DisplayImage(TextBox address, TextBox cost, PictureBox pb, int count, int perRow = 3)
        {
            pb.Width = 200;
            pb.Height = 200;
            int y = count / perRow;
            int x = (count + perRow) % perRow;
            pb.Location = new Point(50 + x * 300, 100 + 300 * y);
            pb.SizeMode = PictureBoxSizeMode.Zoom; // so they whole picture fits in box
            this.Controls.Add(pb); 

            address.Location = new Point(50 + x * 300, 300 + 300 * y); // same as picturebox but a little down
            cost.Location = new Point(50 + x * 300, 330 + 300 * y); // just below cost text

            this.Controls.Add(cost);
            this.Controls.Add(address);
        }





    }
}
