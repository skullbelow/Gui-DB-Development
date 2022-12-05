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
            //input validation
            Regex r = new Regex("^[0-9 ]+$");
            if (!r.IsMatch(textBox1.Text)) // Card number is 16 integers long and only numeric
            {
                MessageBox.Show("Card number only requires numbers. Try again.");
                return;// terminate early
            }
            if (textBox1.Text.Length != 16)
            {
                MessageBox.Show("The card number consists of a sixteen number length string Ex: 1111222233334444. Try again.");
                return;
            }
            r = new Regex("^[0-9/]+"); //card expiration should only be numbers but for our purposes allowing '/' may also work
            if (!r.IsMatch(textBox2.Text))
            {
                MessageBox.Show("Card expiration should be in the form MM/YY. Try again.");
                return;// terminate early
            }
            if (textBox2.Text.Length != 5)
            {
                MessageBox.Show("The expiration date is in MM/YY format. Try again.");
                return;
            }
            r = new Regex("^[0-9]+"); //CCV is only three integers and only numeric 
            if (!r.IsMatch(textBox3.Text))
            {
                MessageBox.Show("The CVV is only numbers. Try again.");
                return;// terminate early
            }
            if (textBox3.Text.Length != 3)
            {
                MessageBox.Show("The CVV should only be a length of three numbers.");
                return;
            }
            r = new Regex("^[a-zA-Z]+"); //Cardholder names need only accept the alphabet. 
            if (!r.IsMatch(textBox4.Text))
            {
                MessageBox.Show("Cardholder name should only be in letters of the alphabet, no special characters. Try again.");
                return;// terminate early
            }
            if (textBox4.Text.Length > 50)
            {
                MessageBox.Show("Character Limit is 50 letters.");
                return;
            }

            //after input validation we need to save the input as a new entry in the Listing table
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;
            //MessageBox.Show(@"BEGIN TRANSACTION; INSERT INTO PURCHASE ( listingID, accountID, cardNum, expiration, cvv, cardHolder) VALUES (" + listingID + @", " + account.ToString() + @", " + textBox1.Text.GetHashCode().ToString() + @",'" + textBox2.Text.GetHashCode().ToString() + "', " + textBox3.Text.GetHashCode().ToString() + ", '" + textBox4.Text.GetHashCode().ToString() + @"'); COMMIT;");
            cmnd.CommandText = @"BEGIN TRANSACTION; INSERT INTO PURCHASE ( listingID, accountID, cardNum, expiration, cvv, cardHolder) VALUES (" + listingID + @", " + account.getAccountID().ToString() + @", " + textBox1.Text.GetHashCode().ToString() + @",'" + textBox2.Text.GetHashCode().ToString() + "', " + textBox3.Text.GetHashCode().ToString() + ", '" + textBox4.Text.GetHashCode().ToString() + @"'); COMMIT;";
            cmnd.ExecuteNonQuery();


            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandText = @"BEGIN TRANSACTION; DELETE FROM Listing WHERE listingID=" + listingID + "; COMMIT;";
            cmd.ExecuteNonQuery();

            this.Hide();
            new BuyMenu(account).Show();

        }




    }
}
