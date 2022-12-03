using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions; // added for input validation (stopping SQL injections)

namespace TestApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        /*        private void Login_Load(object sender, EventArgs e)
                {
                    SQLiteConnection conn = new SQLiteConnection(@"data source = nAccountDb.db");
                    try
                    {
                        conn.Open();
                    }
                    catch
                    {
                        Console.WriteLine("DB conn open erorr");
                    }

                    SQLiteDataReader dataReader;
                    SQLiteCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT * FROM Account";

                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Console.WriteLine(dataReader.GetInt32(0));
                    }
                }
        */

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
                con.Open();
                //command object

                // Regex to validate user input as alphanumeric only (PREVENTING SQL INJECTIONS)
                 Regex r = new Regex("^[a-zA-Z0-9]+$");
                 if (!r.IsMatch(textBox1.Text) || !r.IsMatch(textBox2.Text))
                 {
                     MessageBox.Show("Credentials can only be alphanumeric. Try again.");
                     return;// terminate early
                 }
                

                string query = ("SELECT * FROM ACCOUNT WHERE name='" + textBox1.Text.GetHashCode().ToString() + "' AND password='" + textBox2.Text.GetHashCode().ToString() + "'");
                //string query = "SELECT * from Account";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                //adapter
                //datatable
                DataTable dt = new DataTable();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
                DataRow newRow; // result row based on query pull

                try 
                {
                    newRow = dt.Rows[0]; // setting the inital row (should be only one result row because of the query syntax)
                }
                catch 
                {
                    MessageBox.Show("Incorrect credentials, try again");

                    return; // terminating early
                }

                //MessageBox.Show(newRow[2].ToString()); // FOR REFERENCE

                //MessageBox.Show(dt.ToString());
                //MessageBox.Show(adapter.ToString());

                /*foreach (DataRow row in dt.Tables[0].Rows)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = row["accountID"].ToString();
                    lvi.SubItems.Add(row["name"].ToString());
                    lvi.SubItems.Add(row["password"].ToString());
                    lvi.SubItems.Add(row["type"].ToString());
                    //listView1.Items.Add(lvi);

                }*/


                // FOR REFERENCE
                /*foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        MessageBox.Show(row[column].ToString());
                    }
                }*/

                if (newRow[1].ToString() == textBox1.Text.GetHashCode().ToString() && newRow[2].ToString() == textBox2.Text.GetHashCode().ToString()) //if user and password match then...
                {
                    if (newRow[3].ToString() == "buyer") //...take them to buyer menu
                    {
                        this.Hide();
                        new BuyMenu().Show();
                    }
                    if (newRow[3].ToString() == "seller") //...take them to seller menu
                    {
                        this.Hide();
                        int account = Int32.Parse(newRow[0].ToString());
                        new SellMenu(account).Show();//no longer throws login error
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
    }
}
