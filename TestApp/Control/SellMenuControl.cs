using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.Entity;

namespace TestApp.Control
{


    struct TBs
    {
        public TextBox cost;
        public TextBox address;
    }


    class SellMenuControl
    {

        public static List<Listing> FormatListings()
        {
            DataTable dt = DBConnector.GetListings(); // get all listings in DB


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
            TextBox tb;
            TBs tbs;
            int i = 1;

            foreach (DataRow row in dt.Rows)
            {

                rdr = DBConnector.GetImageReader(row);

                try
                {
                    while (rdr.Read()) // while there is still data to read
                    {
                        byte[] a = (System.Byte[])rdr[0]; // obtain image as byte array

                        // transform byte array to image object
                        // then assign images to picture boxes in order of most recent to oldest 
                        pictureBoxes[i - 1].Image = ByteToImage(a);
                    }
                }
                catch (Exception exc) { MessageBox.Show(exc.Message); } // Just in case 


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
                            tbs = textBoxes[i - 1];
                            tbs.cost = tb;
                            textBoxes[i - 1] = tbs;
                            break;
                        case 3: // Address 
                            tb = new TextBox();
                            tb.Multiline = true;
                            tb.ReadOnly = true;
                            tb.AutoSize = false;
                            tb.BorderStyle = BorderStyle.None;
                            tb.Text = $"Address of house: {row.ItemArray[j]}";
                            tb.Size = new System.Drawing.Size(200, 15);
                            tbs = textBoxes[i - 1];
                            tbs.address = tb;
                            textBoxes[i - 1] = tbs;
                            break;
                        default:
                            // do nothing (for now...)
                            break;
                    }
                }

                i++;
            }

            List<Listing> listings = new List<Listing>();

            int count = 0;
            foreach (DataRow row in dt.Rows)
            {
                listings.Add(new Listing(Int32.Parse(row.ItemArray[0].ToString()), Int32.Parse(row.ItemArray[1].ToString()), row.ItemArray[3].ToString(), Int32.Parse(row.ItemArray[2].ToString()), pictureBoxes[(pictureBoxes.Count - 1) - count], textBoxes[(pictureBoxes.Count - 1) - count].address, textBoxes[(pictureBoxes.Count - 1) - count].cost));
                count++;
            }


            return listings;


        }



        public static void AddLogoutToDB(Account acc)
        {
            DBConnector.AddLogoutToDB(acc);
        }



        // helper func to turn byte[] into image obj in constructor
        public static Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }



    }
}
