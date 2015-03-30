using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RailEuropeFileParser
{
    class ReceiptsManager
    {
        private static List<Receipt> receipts;
        private const string DATAFILE = @"c:\custom\import\RE\receipts.dat";

        public static void Init()
        {
            receipts = new List<Receipt>();
            if (File.Exists(DATAFILE))
            {
                DeserializeData();
            }
        }

        public static void SaveReceipts()
        {
            var recips = from r in receipts
                         where r.DateSent >= DateTime.Today.AddDays(-15)
                         select r;

            SerializeData(recips.ToList());
        }
        public static void AddReceipt(string email)
        {
            Receipt newReceipt = new Receipt();
            newReceipt.Email = email;
            newReceipt.DateSent = DateTime.Now;
            receipts.Add(newReceipt);
        }
        public static IEnumerable<string> GetNotEmailable()
        {
            var rec = from r in receipts
                      group r by r.Email into grouped
                      where grouped.Count() >= 2
                      select grouped.Key;

            return rec;
        }

        private static void SerializeData(List<Receipt> data)
        {
            Stream str = File.OpenWrite(DATAFILE);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(str, data);
            str.Flush();
            str.Close();
        }
        private static void DeserializeData()
        {
            Stream str = File.OpenRead(DATAFILE);
            BinaryFormatter formatter = new BinaryFormatter();
            receipts = (List<Receipt>)(formatter.Deserialize(str));
            str.Flush();
            str.Close();
        }
    }

    [Serializable]
    class Receipt
    {
        string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        DateTime dateSent;
        public DateTime DateSent
        {
            get { return dateSent; }
            set { dateSent = value; }
        }
    }
}
