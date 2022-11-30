using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Entity
{
    public class Account
    {
        private int accountID;
        private string accountName;
        private string accountType;

        public Account(int AccountID, string AccountName, string AccountType)
        {
            setAccountID(AccountID);
            setAccountName(AccountName);
            setAccountType(AccountType);
        }
        public int getAccountID()
        {
            return accountID;
        }
        public void setAccountID(int naccountID)
        {
            accountID = naccountID;
        }

        public string getAccountName()
        {
            return accountName;
        }
        public void setAccountName(string naccountName)
        {
            accountName = naccountName;
        }
        public string getAccountType()
        {
            return accountType;
        }
        public void setAccountType(string naccountType)
        {
            accountType = naccountType;
        }
    }
}
