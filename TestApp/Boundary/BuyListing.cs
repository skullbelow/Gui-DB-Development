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
using System.IO;
using TestApp.Entity;
using TestApp.Control;

namespace TestApp
{
    public partial class BuyListing : Form
    {
        string listingID = "-1";
        Account account;
        public BuyListing(TextBox cost, TextBox address, TextBox rooms, TextBox bathrooms, PictureBox pb, string lID, Account acc)
        {
            InitializeComponent();

            account = acc;

            listingID = lID;

            pictureBox1.Image = pb.Image;

            // Address TextBox
            textBox5.Text = address.Text;
            textBox5.Multiline = true;
            textBox5.ReadOnly = true;
            textBox5.AutoSize = false;
            textBox5.BorderStyle = BorderStyle.None;

            // Cost TextBox
            textBox6.Text = cost.Text;
            textBox6.Multiline = true;
            textBox6.ReadOnly = true;
            textBox6.AutoSize = false;
            textBox6.BorderStyle = BorderStyle.None;

            // Rooms TextBox
            textBox7.Text = rooms.Text;
            textBox7.Multiline = true;
            textBox7.ReadOnly = true;
            textBox7.AutoSize = false;
            textBox7.BorderStyle = BorderStyle.None;

            // Bathooms TextBox
            textBox8.Text = bathrooms.Text;
            textBox8.Multiline = true;
            textBox8.ReadOnly = true;
            textBox8.AutoSize = false;
            textBox8.BorderStyle = BorderStyle.None;
        }



        private void button1_Click(object sender, EventArgs e) // Submit button should remove a listing from the menu (and database?) and return the user to the menu
        {


            string validity = BuyListingControl.IsValid(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
            if (!validity.Equals("All Good!"))
            {
                MessageBox.Show(validity);
                return; // terminiate early
            }


            // after input validation we need to save the input as a new entry in the Purchase table
            // and remove corresponding listing from listing table (i.e. no longer an option to buy)
            BuyListingControl.InsertPurchaseRemoveListing(listingID, account, textBox1.Text.GetHashCode().ToString(), textBox2.Text.GetHashCode().ToString(), textBox3.Text.GetHashCode().ToString(), textBox4.Text.GetHashCode().ToString());


            this.Hide();
            new BuyMenu(account).Show();

        }

        private void button2_Click(object sender, EventArgs e)//Back Button: returns to buy menu, cancels buy listing form
        {
            this.Hide();
            new BuyMenu(account).Show();
        }
    }
}
