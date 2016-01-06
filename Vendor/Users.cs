using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor
{
    public class Users
    {
        public struct UsersData
        {
            public string userName;
            public List<string> lastPayment;
            public UserCertificate certificate;
            public UserCommitment commitment;
        }

        public struct UserCommitment
        {
            public string vendorName;
            public string basicCoin;
            public string date;
            public string legthOfPayword;
        }

        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            //klucz publiczny usera??????????????????????????????????????????????????????????????????????
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
        }
    }

    
}
