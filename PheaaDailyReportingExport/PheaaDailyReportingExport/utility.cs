using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PheaaDailyReportingExport
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

        public static Dictionary<string,string>SegmentIDTable()
        {

            // build up the Dictionary of segmentid's
            Dictionary<string, string> segmentPairs = new Dictionary<string, string>()
            {
                {"42606","A: Bill Phillips"},
                {"42607","A: Carol Ritberger, Ph.D. News"},
                {"42570","A: Caroline Myss Newsletter"},
                {"42513","B: Master:Virtue"},
                {"40330","B: Master:Northrup"},
                {"39710"," B: Master:Dyer"},
                {"42564","A: Gary Renard Enews"},
                {"40335","B: HYL:Get Healthy Newsletter"},
                {"43235","Affiliates"},
                {"55266","B: Hay House Australia Newsletter"},
                {"47574","BL: Hay House Book Launch"},
                {"39703","B: Master: HH"},
                {"40329","B: Master:HHR"},
                {"40645"," B: HYL:HealYourLifeNewsletter"},
                {"49567"," B: ICANDOIT.net"},
                {"42572","A: Immaculee Ilibagiza: LTT"},
                {"40336","B: HYL:Inspiration Newsletter"},
                {"40333","B: HYL:Inspiration Newsletter"},
                {"42573"," A: Joan Borysenko,Ph.D. E-news"},
                {"42566","A: John Holland E-newsletter"},
                {"40328","B: Master:LH"},
                {"49246","B: Oh My God Movie Newsletter"},
                {"40332","B: Master:LH"},
                {"42568","A: PlayGround Pump Enews"},
                {"42567","A: Sonia Choquette Newsletter"},
                {"40334","B: HYL:SuccessAbundanceNewsletter"},
                {"42605","A: SuzeOrman Updates[DontSend]"},
                {"53528","Wisdom Community"},
                {"43279","Northrup:WWC only"},
                {"49221","B: You Can Heal Your Life Movie"},
                {"42565","A: ColetteBaronReid Newsletter"},
                {"49566","B: The Shift Movie"} 
            };

            return segmentPairs;
        }


        public static Boolean DownloadCsv(string link, string filepath)
        {

            Chilkat.Http http = new Chilkat.Http();

            bool success;

            //  Any string unlocks the component for the 1st 30-days.
            success = http.UnlockComponent("LYRISCHttp_zUmhzUf4XV1i ");
            if (success != true)
            {
                Log.WriteLine(http.LastErrorText);
                return success;
            }

            //  Download the Python language install.
            //  Note: This URL may have changed since this example was created.
            success = http.Download(link, filepath);
            if (success != true)
            {
                Log.WriteLine(http.LastErrorText);
            }
            else
            {
                Console.WriteLine("CSV Download Complete!");
            }

            return success;

        }

        public static List<string> ParseCsvAddEmail(StreamReader fileStream, List<string> emailArray)
        {
            // email should be the 2nd element of each file, but do a safety check anyways
            string line = fileStream.ReadLine();
            // put the headers into an array
            string[] value = line.Split(',');

            int emailPosition = 0;

            // iterate through the array to find the value of the email address
            for (int i = 0; i < value.Length; i++)
            {
                // break once the position is found
                if (value[i].ToLower() == "email")
                {
                    emailPosition = i;
                    break;
                }
            }

            // iterate through the csv file and add email address into the List
            while (!fileStream.EndOfStream)
            {
                // put the csv row into a array structure
                value = fileStream.ReadLine().Split(',');

                // add the eamil to the list array if it doesn't exist
                if (!emailArray.Contains(value[emailPosition]))
                {
                    emailArray.Add(value[emailPosition]);
                    //Console.WriteLine("Email: " + value[emailPosition] + " added");
                }
            }

            fileStream.Close();

            return emailArray;
        }



    }
}
