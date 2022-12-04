using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestApp.Entity;

namespace TestApp.Control
{
    class LoginControl
    {
        public static bool IsValid(String user, String pass)
        {

            // Regex to validate user input as alphanumeric only (PREVENTING SQL INJECTIONS)
            Regex r = new Regex("^[a-zA-Z0-9]+$");
            if (!r.IsMatch(user) || !r.IsMatch(pass))
            {
                return false;
            }
            else
                return true;
        }


        public static Account ObtainUser(string usr, string pwd)
        {
            return DBConnector.GetUser(usr, pwd);
        }

        public static void AddLoginToDB(Account acc)
        {
            DBConnector.AddLoginToDB(acc);
        }




    }
}
