using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HayhouseWeeklyReportGenerator
{
    class utility
    {
        public static string ReturnHTMLEnocodedString(string input)
        {
            
            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            // Any code begins the 30-day trial.
            crypt.UnlockComponent("LYRISCCrypt_4NataCGcVVkW ");
            crypt.CryptAlgorithm = "none";
            crypt.EncodingMode = "url";
            
            return crypt.EncryptStringENC(input);


        }

        // DataTable for the reporting
        public static DataTable reportingTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Subject", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Sent_Date", typeof(string));
            table.Columns.Add("From", typeof(string));
            table.Columns.Add("Segment", typeof(string));
            table.Columns.Add("Sent", typeof(string));
            table.Columns.Add("Total_Opened", typeof(string));
            table.Columns.Add("Unique_Opened", typeof(string));
            table.Columns.Add("%Unique_Opened", typeof(string));
            table.Columns.Add("Unique_Clicked", typeof(string));
            table.Columns.Add("%Unique_Clicked", typeof(string));
            table.Columns.Add("Unsubscribed", typeof(string));
            table.Columns.Add("Bounced", typeof(string));
            table.Columns.Add("%Bounced", typeof(string));
            table.Columns.Add("Hard_Bounced", typeof(string));
            table.Columns.Add("%Hard_Bounced", typeof(string));
            table.Columns.Add("Soft_Bounced", typeof(string));
            table.Columns.Add("%Soft_Bounced", typeof(string));
            table.Columns.Add("Delivered", typeof(string));
            table.Columns.Add("%Delivered", typeof(string));
            table.Columns.Add("Spam_Complaints", typeof(string));
            table.Columns.Add("Unique_Referrers", typeof(string));


            return table;


        }
    }
}
