using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApp.Entity;

namespace TestApp.Control
{
    class CreateListingControl
    {

        public static string IsValid(string address, string cost, int rooms, int bathrooms)
        {
            // user input validation block
            Regex r = new Regex("^[a-zA-Z0-9,. ]+$"); //address needs to accept commas, spaces, and periods as well
            if (!r.IsMatch(address))
            {
                return "Address can only be alphanumeric. Try again.";
            }
            r = new Regex("^[0-9]+$"); //cost needs to accept only numbers
            if (!r.IsMatch(cost))
            {
                return "Cost can only be numeric. Try again.";
            }
            if (!(rooms < 21) || !(bathrooms < 21))
            {
                return "There can be no more than 20 rooms or 20 bathrooms. Try again.";
            }

            return "All Good!";

        }


        public static bool InsertListing(Image photo, Account account, string cost, string address, string rooms, string bathrooms)
        {
            byte[] pic = ImageToByte(photo, System.Drawing.Imaging.ImageFormat.Jpeg);
            return DBConnector.InsertListing(pic,account,cost,address,rooms,bathrooms);
        }

        public static byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
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
