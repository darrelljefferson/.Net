using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PheaaEcoreDailyExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\custom\PHEAA_Ecore_Exporter_Log_");

            string sftpServer = "cmfiles.pheaa.org";
            string sftpUsername = "lyriseob";
            string sftpPassword = "#487EaTs4";

            string[] DemographicNames = {"ARC","JOIN_DATE","CMOD_UID","ACCOUNT#",
                                            "LETTER_ID","DOC_TYPE","DUE_DATE"};
            string[] DemographicIDs = { "53534", "33630", "60475", "58687",
                                          "44293","60476","43302"};

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

            //DateTime start = Convert.ToDateTime("2011-03-14");
            DateTime start = DateTime.Now.AddDays(-1);
            //DateTime end = Convert.ToDateTime("2011-03-15");
            DateTime end = DateTime.Now.AddDays(0);
            DateTime today = DateTime.Now.AddDays(0);


            string startDate = start.ToString("yyyy-MM-dd");
            Console.WriteLine(startDate);
            string endDate = end.ToString("yyyy-MM-dd");
            Console.WriteLine(endDate);
            string todayDate = today.ToString("yyyy-MM-dd");
            Console.WriteLine(todayDate);

            string siteid = "23394";
            //string mlid = "79580";
            string apiPassword = "87h3jps";

            string exportPath = @"c:\custom\export\";


            Dictionary<string, string> mlidName = new Dictionary<string, string>()
            {
                {"AES_ECORR_CORR","99505"},
                {"AES_ECORR_INSTALL","99510"},
                {"AES_ECORR_INT","99513"},
                {"AES_ECORR_INTNOT","99514"},
                {"AES_ECORR_REPAY","99515"},
                {"FLS_ECORR_CORR","99496"},
                {"FLS_ECORR_INSTALL","99497"},
                {"FLS_ECORR_INT","99498"},
                {"FLS_ECORR_INTNOT","99499"},
                {"FLS_ECORR_REPAY","99500"}
            };

            string[] arcPair = { "ARC", "" };
            string[] joinDatePair = { "JOIN_DATE", "" };
            string[] cmod_idPair = { "CMOD_ID", "" };
            string[] accountNumberPair = { "Account#", "" };
            string[] letterIDPair = { "LETTER_ID", "" };
            string[] docTypePair = { "DOC_TYPE", "" };
            string[] dueDatePair = { "DUE_DATE", "" };

            Dictionary<string, string[]> demographicIDHash = new Dictionary<string, string[]>()
            {
                {"53534",arcPair},
                {"33630",joinDatePair},
                {"60475",cmod_idPair},
                {"58687",accountNumberPair},
                {"44293",letterIDPair},
                {"60476",docTypePair},
                {"43302",dueDatePair}
            };

            foreach (KeyValuePair<string, string> item in mlidName)
            {
                Log.WriteLine("working on: " + item.Key + " mlid: " + item.Value);

                string sentFile = string.Concat(item.Key, "_SENT_", todayDate, ".csv");

                string openFile = string.Concat(item.Key, "_OPEN_", todayDate, ".csv");

                string clickFile = string.Concat(item.Key, "_CLICK_", todayDate, ".csv");

                string unsubFile = string.Concat(item.Key, "_UNSUB_", todayDate, ".csv");

                string bounceFile = string.Concat(item.Key, "_BOUNCE_", todayDate, ".csv");

                string sentFilePath = string.Concat(exportPath, sentFile);
                string openFilePath = string.Concat(exportPath, openFile);
                string clickFilePath = string.Concat(exportPath, clickFile);
                string unsubFilePath = string.Concat(exportPath, unsubFile);
                string bounceFilePath = string.Concat(exportPath, bounceFile);

                // Master List to be used for all demogrpahic lookups
                List<string> emailArray = new List<string>();

                // Dictionary to be populated
                List<string[]> sentEmails = new List<string[]> { };
                List<string[]> openEmails = new List<string[]> { };
                List<string[]> clickEmails = new List<string[]> { };
                List<string[]> bounceEmails = new List<string[]> { };
                List<string[]> unsubEmails = new List<string[]> { };

                // iterate through the files and send the messages
                if (item.Key == "AES_ECORR_CORR")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        aescorrTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                
                else if (item.Key == "AES_ECORR_INSTALL")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        aesinstallTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "AES_ECORR_INT")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        aesintTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "AES_ECORR_INTNOT")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        aesintnotTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "AES_ECORR_REPAY")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        aesrepayTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "FLS_ECORR_CORR")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        fdcorrTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "FLS_ECORR_INSTALL")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        fdinstallTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "FLS_ECORR_INT")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        fdintTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "FLS_ECORR_INTNOT")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        fdintnotTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                else if (item.Key == "FLS_ECORR_REPAY")
                {
                    ReportBuilder.ParseTriggerDetailedData(siteid, item.Value,
                        fdrepayTriggerID, startDate, endDate, apiPassword,
                        sentEmails, openEmails, clickEmails, bounceEmails,
                        unsubEmails, emailArray);
                }
                

                //used to store the email address and their demographic vlaues
                Dictionary<string, string[]> emailDemographicLookup = new Dictionary<string, string[]>();

                //build the email/demographic lookup table
                emailDemographicLookup = ReportBuilder.BuildEmailDemographicLookup(
                    siteid, item.Value, apiPassword, DemographicIDs, emailArray, demographicIDHash);



                // generate the files
                ReportBuilder.CreateCsvFile(sentFilePath, startDate, item.Key,
                    sentEmails, emailDemographicLookup, DemographicNames, "sent");
                ReportBuilder.CreateCsvFile(openFilePath, startDate, item.Key,
                    openEmails, emailDemographicLookup, DemographicNames, "open");
                ReportBuilder.CreateCsvFile(clickFilePath, startDate, item.Key,
                    clickEmails, emailDemographicLookup, DemographicNames, "click");
                ReportBuilder.CreateCsvFile(bounceFilePath, startDate, item.Key,
                    bounceEmails, emailDemographicLookup, DemographicNames, "bounce");
                ReportBuilder.CreateCsvFile(unsubFilePath, startDate, item.Key,
                    unsubEmails, emailDemographicLookup, DemographicNames, "usnub");


                
                // upload the files
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, sentFilePath, sentFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, openFilePath, openFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, clickFilePath, clickFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, bounceFilePath, bounceFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, unsubFilePath, unsubFile);
                


            };
            
        }
    }
}
