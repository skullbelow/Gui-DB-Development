using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApp.Entity;

namespace TestApp.Control
{
    class BuyListingControl
    {


        public static string IsValid(string cardNum, string exp, string cvv, string cardName)
        {
            //input validation
            Regex r = new Regex("^[0-9 ]+$");
            if (!r.IsMatch(cardNum)) // Card number is 16 integers long and only numeric
            {
                return "Card number only requires numbers. Try again.";
            }
            if (cardNum.Length != 16)
            {
                return "The card number consists of a sixteen number length string Ex: 1111222233334444. Try again.";
            }
            r = new Regex("^[0-9/]+"); //card expiration should only be numbers but for our purposes allowing '/' may also work
            if (!r.IsMatch(exp))
            {
                return "Card expiration should be in the form MM/YY. Try again.";
            }
            if (exp.Length != 5)
            {
                return "The expiration date is in MM/YY format. Try again.";
            }
            r = new Regex("^[0-9]+"); //CVV is only three integers and only numeric 
            if (!r.IsMatch(cvv))
            {
                return "The CVV is only numbers. Try again.";
            }
            if (cvv.Length != 3)
            {
                return "The CVV should only be a length of three numbers.";
            }
            r = new Regex("^[a-zA-Z]+"); //Cardholder names need only accept the alphabet. 
            if (!r.IsMatch(cardName))
            {
                return "Cardholder name should only be in letters of the alphabet, no special characters. Try again.";
            }
            if (cardName.Length > 50)
            {
                return"Character Limit is 50 letters.";
            }


            return "All Good!";

        }


        public static void InsertPurchaseRemoveListing(string listingID, Account account, string cardNumHash, string expHash, string cvvHash, string cardNameHash)
        {
            DBConnector.InsertPurchaseRemoveListing(listingID, account, cardNumHash, expHash, cvvHash, cardNameHash);
        }





    }
}
