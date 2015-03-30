using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PheaaDailyImporterUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\custom\PHEAAImporter_log_");
            
            // sftp information
            string sftpServer = "cmfiles.pheaa.org";
            string sftpUsername = "lyrisssh";
            string sftpPassword = "#487EaTs4";

            // farm info for the upload call
            string siteid = "23394";
            string mlid = "79580";
            string apiPassword = "87h3jps";

            //DateTime today = DateTime.Now.AddDays(0);
            DateTime today = Convert.ToDateTime("2011-05-23");
            string todayDate = today.ToString("yyyy-MM-dd");
            //string todayDate = "20110511";


            // Dictionary to hold  list and mlid info
            Dictionary<string, string> listMlidMap = new Dictionary<string, string>()
            {
               
                {"ECORR_AESCORR","99505"},
                {"ECORR_AESINSTALL","99510"},
                {"ECORR_AESINT","99513"},
                {"ECORR_AESINTNOT","99514"},
                {"ECORR_AESREPAY","99515"}

                
            };

            string localPath = @"c:\custom\pheaa_export\";
            //string localFileName = string.Concat("AES_Ally_Bank_",
            //    todayDate,".csv");
            
            

            foreach (KeyValuePair<string, string> item in listMlidMap)
            {
               
                
                string localFileName = string.Concat(item.Key,
                    "_",todayDate, ".csv"); 

                string localFilePath = string.Concat(localPath, localFileName);

                string ftpExternalHostName = "ftp.lyris.com";
                string ftpInternalHostName = "ftp.corp.lyris.com";
                string ftpUserName = "support";
                string ftpPassword = "ftpP0w3r!";
                string ftpDirectory = "private/pheaa";
                //string annonymousFtpString = string.Concat(
                //    "ftp://", "anonymous:a@", ftpExternalHostName, "/", ftpDirectory,
                //    "/", localFileName);


                string notifyEmail = "kbrown1@aessuccess.org";

                string annonymousFtpString = localFilePath;
                Console.WriteLine(annonymousFtpString);


                SFTP.GetFlatFile(sftpServer, sftpUsername, sftpPassword, localFilePath,
                    localFileName);


                //Boolean successUplaod = Utility.SimpleFtpUplaod(localFilePath, localFileName,
                //    ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory);


                // only run the list upload call if the upload was successful
                Boolean successUpload = true;
                if (successUpload)
                {
                    UploadFile(siteid, item.Value, notifyEmail, annonymousFtpString, apiPassword);
                }

            }

        
            


            

            


        }

        public static void UploadFile(string siteid, string mlid, string email,
            string file, string password)
        {
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"email\">", email, "</DATA>",
                "<DATA type=\"extra\" id=\"file\">",file, "</DATA>",
                "<DATA type=\"extra\" id=\"validate\">", "on", "</DATA>",
                "<DATA type=\"extra\" id=\"type\">", "active", "</DATA>",
                "<DATA type=\"extra\" id=\"update\">", "on", "</DATA>",
                "<DATA type=\"extra\" id=\"trigger\">", "yes", "</DATA>",
                //"<DATA type=\"extra\" id=\"clear_trigger_history\">", "all", "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string recordUpload = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=upload&",
                "&input=", encodedInput);

            Console.WriteLine(recordUpload);
            Log.WriteLine(recordUpload);


            // load API Xml into a XDocument
            XDocument uploadFile = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                uploadFile = XDocument.Load(recordUpload);
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = uploadFile.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)uploadFile.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordUpload);
                Log.WriteLine("The failure reason was: " + uploadFile.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)uploadFile.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordUpload);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                // log the id, just in case we need it later
                Log.WriteLine(uploadFile.Element(dataset).Element(data).Value);
                
            }

            
        }

        public static string ReturnHTMLEnocodedString(string input)
        {

            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            // Any code begins the 30-day trial.
            crypt.UnlockComponent("LYRISCCrypt_4NataCGcVVkW ");
            crypt.CryptAlgorithm = "none";
            crypt.EncodingMode = "url";

            return crypt.EncryptStringENC(input);


        }
    }
}
