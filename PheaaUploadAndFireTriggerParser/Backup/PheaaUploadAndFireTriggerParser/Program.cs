using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PheaaUploadAndFireTriggerParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\custom\EcorrUploadFireTrigger_log_");

            // sftp information
            string sftpServer = "cmfiles.pheaa.org";
            string sftpUsername = "lyrisssh";
            string sftpPassword = "#487EaTs4";

            // farm info for the upload call
            string siteid = "23394";
            string mlid = "79580";
            string apiPassword = "87h3jps";

            //DateTime today = DateTime.Now.AddDays(0);
            DateTime today = Convert.ToDateTime("2011-05-10");
            string todayDate = today.ToString("yyyyMMdd");

            string aescorrTriggerID = "8285";
            string aesinstallTriggerID = "8286";
            string aesintTriggerID = "8287";
            string aesintnotTriggerID = "8289";
            string aesrepayTriggerID = "8290";
            string fdcorrTriggerID = "8280";
            string fdinstallTriggerID = "8281";
            string fdintTriggerID = "8282";
            string fdintnotTriggerID = "8283";
            string fdrepayTriggerID = "8284";



            // Dictionary to hold  list and mlid info
            Dictionary<string, string> listMlidMap = new Dictionary<string, string>()
            {
               
                {"ECORR_AESCORR","99505"},
                {"ECORR_AESINSTALL","99510"},
                {"ECORR_AESINT","99513"},
                {"ECORR_AESINTNOT","99514"},
                {"ECORR_AESREPAY","99515"},
                {"ECORR_FLSCORR","99496"},
                {"ECORR_FLSINSTALL","99497"},
                {"ECORR_FLSINT","99498"},
                {"ECORR_FLSINTNOT","99499"},
                {"ECORR_FLSREPAY","99500"}

                
            };

            string localPath = @"c:\custom\pheaa_import\";
            //string localFileName = string.Concat("AES_Ally_Bank_",
            //    todayDate,".csv");


            
            foreach (KeyValuePair<string, string> item in listMlidMap)
            {


                string localFileName = string.Concat(item.Key,
                    "_", todayDate, ".txt");

                string localFilePath = string.Concat(localPath, localFileName);

                string ftpExternalHostName = "ftp.lyris.com";
                string ftpInternalHostName = "ftp.corp.lyris.com";
                string ftpUserName = "support";
                string ftpPassword = "ftpP0w3r!";
                string ftpDirectory = "private/pheaa";
                string annonymousFtpString = string.Concat(
                    "ftp://", "anonymous:a@", ftpExternalHostName, "/", ftpDirectory,
                    "/", localFileName);
                string notifyEmail = "bmueller@lyris.com";

                Console.WriteLine(annonymousFtpString);


                SFTP.GetFlatFile(sftpServer, sftpUsername, sftpPassword, localFilePath,
                    localFileName);


                Boolean successUplaod = Utility.SimpleFtpUplaod(localFilePath, localFileName,
                    ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory);


                // only run the list upload call if the upload was successful
                if (successUplaod)
                {
                    UploadFile(siteid, item.Value, notifyEmail, annonymousFtpString, apiPassword);
                }

            }
            

            // now let's send the messages
            foreach (KeyValuePair<string, string> item in listMlidMap)
            {
                string localFileName = string.Concat(item.Key,
                    "_", todayDate, ".txt");

                string localFilePath = string.Concat(localPath, localFileName);

                // iterate through the files and send the messages
                if (item.Key == "ECORR_AESCORR")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, aescorrTriggerID,
                        apiPassword,localFilePath);
                }
                else if (item.Key == "ECORR_AESINSTALL")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, aesinstallTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_AESINT")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, aesintTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_AESINTNOT")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, aesintnotTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_AESREPAY")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, aesrepayTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_FLSCORR")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, fdcorrTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_FLSINSTALL")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, fdinstallTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_FLSINT")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, fdintTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_FLSINTNOT")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, fdintnotTriggerID,
                        apiPassword, localFilePath);
                }
                else if (item.Key == "ECORR_FLSREPAY")
                {
                    Utility.ParseFileAndSendMessage(siteid, item.Value, fdrepayTriggerID,
                        apiPassword, localFilePath);
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
                "<DATA type=\"extra\" id=\"file\">", file, "</DATA>",
                "<DATA type=\"extra\" id=\"validate\">", "on", "</DATA>",
                "<DATA type=\"extra\" id=\"type\">", "active", "</DATA>",
                "<DATA type=\"extra\" id=\"update\">", "on", "</DATA>",
                //"<DATA type=\"extra\" id=\"trigger\">", "yes", "</DATA>",
                //"<DATA type=\"extra\" id=\"clear_trigger_history\">", "all", "</DATA>",
                "<DATA type=\"extra\" id=\"Delimiter\">", "|", "</DATA>",
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
