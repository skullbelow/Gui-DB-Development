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
using TestApp.Entity;
using TestApp.Control;

namespace TestApp
{
    public partial class BuyMenu : Form
    {

        Account account;
        List<Listing> listings;


        public BuyMenu(Account acc)
        {
            InitializeComponent();

            // NOTE: Form should have AutoScroll == true so that contents become scrollable after pictures take up entire page, i.e. :
            this.AutoScroll = true;

            this.account = acc;


            listings = BuyMenuControl.FormatListings();


            for(int i = 0; i < listings.Count; i++)
            {
                // We display in reverse order so the most recent listings come first
                DisplayImage(listings[(listings.Count-1)-i].radioButton, listings[(listings.Count - 1) - i].adressTextBox, listings[(listings.Count - 1) - i].costTextBox, listings[(listings.Count - 1) - i].rooms , listings[(listings.Count - 1) - i].bathRooms, listings[(listings.Count - 1) - i].pictureBox, i);
            }


        }


        private void button1_Click(object sender, EventArgs e)// buy button should take a buyer to the buy form of a selected listing
        {
            

            bool found = false;
            int index = 0;
            for(int i = 0; i < listings.Count; i++)
            {
                if(listings[i].radioButton.Checked)
                {
                    found = true;
                    index = i;
                }
            }

            if (found)
            {
                this.Hide();
                new BuyListing(listings[index].costTextBox, listings[index].adressTextBox, listings[index].rooms, listings[index].bathRooms, listings[index].pictureBox,listings[index].getId().ToString(),account).Show();
            }
            else
            {
                MessageBox.Show("You must select a house.");
                return;
            }
            
            

        }

        private void button2_Click(object sender, EventArgs e)//logout button should return the user to the home page
        {
            BuyMenuControl.AddLogoutToDB(account);
            this.Hide();
            new Login().Show();
        }


        private void DisplayImage(RadioButton rb, TextBox address, TextBox cost, TextBox rooms, TextBox bathrooms, PictureBox pb, int count, int perRow = 3)
        {
            pb.Width = 200;
            pb.Height = 200;
            int y = count / perRow;
            int x = (count + perRow) % perRow;
            pb.Location = new Point(50 + x * 300, 100 + 300 * y);
            pb.SizeMode = PictureBoxSizeMode.Zoom; // so they whole picture fits in box
            Controls.Add(pb); // Actually displays the damn picturebox


            address.Location = new Point(50 + x * 300, 300 + 300 * y); // same as picturebox but a little down
            cost.Location = new Point(50 + x * 300, 315 + 300 * y); // just below cost text
            rooms.Location = new Point(50 + x * 300, 330 + 300 * y); // same as picturebox but a little down
            bathrooms.Location = new Point(50 + x * 300, 345 + 300 * y); // just below cost text

            Controls.Add(cost);
            Controls.Add(address);
            Controls.Add(rooms);
            Controls.Add(bathrooms);


            rb.Location = new Point(50 + x * 300, 360 + 300 * y);
            Controls.Add(rb);

        }

    }
}
