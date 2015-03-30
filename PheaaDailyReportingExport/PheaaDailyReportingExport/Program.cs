using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PheaaDailyReportingExport
{
    class Program
    {
        static void Main(string[] args)
        {

            Log.Init(@"c:\custom\PHEAA_Exporter_Log_");

            string sftpServer = "cmfiles.pheaa.org";
            string sftpUsername = "lyrinssh";
            string sftpPassword = "#487EaTs4";
            
            // place holders, in case we need the name of lists later
            string mlidName66885 = "FLS_FAA Intro";
            string mlidName69061 = "FLS Survey";
            string mlidName71779 = "FLS_New_Report";
            string mlidName76394 = "AES_Citizens";
            string mlidName79580 = "AES_Ally_Bank";



            // build up the string arrays that contain the
            // demographic names and id's
            string[] nameArray66885 = {"JOIN_DATE","Segment", "LETTER_ID"};
            string[] idArray66886 = {"33630","45208", "44293"};
            string[] nameArray69061 = {"First Name","Last Name","JOIN_DATE","Segment", "LETTER_ID"};
            string[] idArray69061 = {"1","2","33630","45208", "44293" };
            string[] nameArray71195 = {"First Name","Last Name","UNQID","ARC","JOIN_DATE","LETTER_ID"};
            string[] idArray71195 = { "1", "2", "52534", "33630","53418", "44293"};
            string[] nameArray79580 = {"First Name","Last Name","UNQID","ARC","JOIN_DATE", "LETTER_ID"};
            string[] idArray79580 = { "53418", "1", "2", "52534", "33630","44293" };
            string[] triggerIDArray79580 = { "7091", "7092",
                                               "7093", "7114" };
            string[] triggerIDArray71195 = { "6305","6306","6307","6307","6308","6309",
                                               "6316","6317","6319","6320","6321","6322",
                                               "6323","6324","6328","6329","6330","6331" };
            string[] triggerIDArray71779 = { "8125" };

            //DateTime start = Convert.ToDateTime("2011-03-14");
            DateTime start = DateTime.Now.AddDays(-2);
            //DateTime end = Convert.ToDateTime("2011-03-15");
            DateTime end = DateTime.Now.AddDays(-1);
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
                {"AES_Ally_Bank","79580"},
                {"AES_BoA","71195"},
                {"FLS_New_Repmt","71779"}
            };
            string[] firstNamePair = { "first name", "" };
            string[] lastNamePair = { "last name", "" };
            string[] uniqueIDPari = { "UNQUID", "" };
            string[] arcPair = { "ARC", "" };
            string[] joinDatePair = { "JOIN_DATE", "" };
            string[] letteridPair = { "LETTER_ID", "" };


            Dictionary<string, string[]> demographicIDHash = new Dictionary<string, string[]>()
            {
                {"1",firstNamePair},
                {"2",lastNamePair},
                {"53418",uniqueIDPari},
                {"52534",arcPair},
                {"33630",joinDatePair},
                {"44293",letteridPair}
            };

            foreach (KeyValuePair<string, string> item in mlidName)
            {
                Log.WriteLine("working on: " + item.Key + " mlid: " + item.Value);
                
                string sentFile = string.Concat(item.Key, "_sent", "_test_",todayDate, ".csv");

                string openFile = string.Concat(item.Key, "_open", "_test_",todayDate, ".csv");

                string clickFile = string.Concat(item.Key, "_click", "_test_",todayDate, ".csv");

                string unsubFile = string.Concat(item.Key, "_unsub", "_test_",todayDate, ".csv");

                string bounceFile = string.Concat(item.Key, "_bounce", "_test_",todayDate, ".csv");

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

                if (item.Value == "71195")
                {
                    foreach (string triggerid in triggerIDArray71195)
                    {
                        Console.WriteLine(triggerid);
                        ReportBuilder.ParseTriggerDetailedData(siteid,
                            item.Value, triggerid, startDate,
                            endDate, apiPassword, sentEmails, openEmails,
                            clickEmails, bounceEmails, unsubEmails,
                            emailArray);
                    }
                }
                else if (item.Value == "79580")
                {
                    foreach (string triggerid in triggerIDArray79580)
                    {
                        ReportBuilder.ParseTriggerDetailedData(siteid,
                            item.Value, triggerid, startDate,
                            endDate, apiPassword, sentEmails, openEmails,
                            clickEmails, bounceEmails, unsubEmails,
                            emailArray);
                    }
                }
                else if (item.Value == "71779")
                {
                    foreach (string triggerid in triggerIDArray71779)
                    {
                        ReportBuilder.ParseTriggerDetailedData(siteid,
                            item.Value, triggerid, startDate,
                            endDate, apiPassword, sentEmails, openEmails,
                            clickEmails, bounceEmails, unsubEmails,
                            emailArray);
                    }
                }

                //used to store the email address and their demographic vlaues
                Dictionary<string, string[]> emailDemographicLookup = new Dictionary<string, string[]>();

                //build the email/demographic lookup table
                emailDemographicLookup = ReportBuilder.BuildEmailDemographicLookup(
                    siteid, item.Value, apiPassword, idArray71195, emailArray, demographicIDHash);



                // generate the files
                ReportBuilder.CreateCsvFile(sentFilePath, startDate, item.Key,
                    sentEmails, emailDemographicLookup, nameArray71195, "sent");
                ReportBuilder.CreateCsvFile(openFilePath, startDate, item.Key,
                    openEmails, emailDemographicLookup, nameArray71195, "open");
                ReportBuilder.CreateCsvFile(clickFilePath, startDate, item.Key,
                    clickEmails, emailDemographicLookup, nameArray71195, "click");
                ReportBuilder.CreateCsvFile(bounceFilePath, startDate, item.Key,
                    bounceEmails, emailDemographicLookup, nameArray71195, "bounce");
                ReportBuilder.CreateCsvFile(unsubFilePath, startDate, item.Key,
                    unsubEmails, emailDemographicLookup, nameArray71195, "usnub");
            

                
                // upload the files
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, sentFilePath, sentFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, openFilePath, openFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, clickFilePath, clickFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, bounceFilePath, bounceFile);
                SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, unsubFilePath, unsubFile);
                    
                
            };
            
            /*
            string sentFile = string.Concat(mlidName79580, "_sent_", todayDate, ".csv");

            string openFile = string.Concat(mlidName79580, "_open_", todayDate, ".csv");

            string clickFile = string.Concat(mlidName79580, "_click_", todayDate, ".csv");

            string unsubFile = string.Concat(mlidName79580, "_unsub_", todayDate, ".csv");

            string bounceFile = string.Concat(mlidName79580, "_bounce_", todayDate, ".csv");

            string sentFilePath = string.Concat(exportPath,sentFile);
            string openFilePath = string.Concat(exportPath,openFile);
            string clickFilePath = string.Concat(exportPath,clickFile);
            string unsubFilePath = string.Concat(exportPath,unsubFile);
            string bounceFilePath = string.Concat(exportPath,bounceFile);

            


            // Dictionary to be populated
            Dictionary<string, string[]> sentEmails = new Dictionary<string, string[]> { };
            Dictionary<string, string[]> openEmails = new Dictionary<string, string[]> { };
            Dictionary<string, string[]> clickEmails = new Dictionary<string, string[]> { };
            Dictionary<string, string[]> bounceEmails =  new Dictionary<string,string[]> { };
            Dictionary<string, string[]> unsubEmails =  new Dictionary<string,string[]> { };
            
            // Master List to be used for all demogrpahic lookups
            List<string> emailArray = new List<string>();

            ReportBuilder.ParseTriggerDetailedData(siteid,
                mlid, triggerIDArray79580[0], startDate,
                endDate, apiPassword, sentEmails, openEmails,
                clickEmails, bounceEmails, unsubEmails,
                emailArray);

            //used to store the email address and their demographic vlaues
            Dictionary<string, string[]> emailDemographicLookup = new Dictionary<string, string[]>();
            //Dictionary<string, string[]> emailDemographicLookup = ReportBuilder.ParseDemographicValues(
            //    siteid, mlid, apiPassword, "dimundsngun1@hotmail.com", idArray69061);

            //build the email/demographic lookup table
            emailDemographicLookup = ReportBuilder.BuildEmailDemographicLookup(
                siteid, mlid, apiPassword, idArray69061, emailArray);

            //create the csv file
            /*
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_sent.csv", sentCsv, emailDemographicLookup,
                nameArray71779, "sent", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_bounce.csv", bounceCsv,
                emailDemographicLookup, nameArray71779, "bounce", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_open.csv", opensCsv,
                emailDemographicLookup, nameArray71779, "open", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_click.csv", clicksCsv,
                            emailDemographicLookup, nameArray71779, "click", "FLS_New_Report");
            */
            /*
            ReportBuilder.CreateCsvFile(importPath + mlidName79580 + "_sent.csv", sentEmails,
                emailDemographicLookup, nameArray71779, "7091", "sent", "AES_Ally_Bank",startDate);
            ReportBuilder.CreateCsvFile(importPath + mlidName79580 + "_open.csv", openEmails,
                emailDemographicLookup, nameArray71779, "7091", "open", "AES_Ally_Bank", startDate);
            ReportBuilder.CreateCsvFile(importPath + mlidName79580 + "_click.csv", clickEmails,
                emailDemographicLookup, nameArray71779, "7091", "click", "AES_Ally_Bank", startDate);
            ReportBuilder.CreateCsvFile(importPath + mlidName79580 + "_bounce.csv", bounceEmails,
                emailDemographicLookup, nameArray71779, "7091", "bounce", "AES_Ally_Bank", startDate);
            ReportBuilder.CreateCsvFile(importPath + mlidName79580 + "_unsub.csv", unsubEmails,
                emailDemographicLookup, nameArray71779, "7091", "unsub", "AES_Ally_Bank", startDate);
            */
            /*
            // generate the files
            ReportBuilder.CreateCsvFile(sentFilePath, startDate, "AES_Ally_Bank", 
                sentEmails, emailDemographicLookup, nameArray71779, "sent");
            ReportBuilder.CreateCsvFile(openFilePath, startDate, "AES_Ally_Bank", 
                openEmails, emailDemographicLookup, nameArray71779, "open");
            ReportBuilder.CreateCsvFile(clickFilePath, startDate, "AES_Ally_Bank", 
                clickEmails, emailDemographicLookup, nameArray71779, "click");
            ReportBuilder.CreateCsvFile(bounceFilePath, startDate, "AES_Ally_Bank",
                bounceEmails, emailDemographicLookup, nameArray71779, "bounce");
            ReportBuilder.CreateCsvFile(unsubFilePath, startDate, "AES_Ally_Bank", 
                unsubEmails, emailDemographicLookup, nameArray71779, "usnub");

            
            // upload the files
            SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, sentFilePath, sentFile);
            SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, openFilePath, openFile);
            SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, clickFilePath, clickFile);
            SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, bounceFilePath, bounceFile);
            SFTP.UploadFile(sftpServer, sftpUsername, sftpPassword, unsubFilePath, unsubFile);
            
            /*
            
            // build up the messageid's
            Dictionary<string,DateTime> messageIdLookup = ReportBuilder.ParseMessageListData(siteid,mlid,startDate,endDate,apiPassword);

            
            
            // now pull in the csv links for the various reports
            string csvlink = "";

            csvlink = ReportBuilder.GenerateCSVLink(siteid, mlid, "972048", "sent", startDate, endDate, apiPassword, messageIdLookup);

            Console.WriteLine(csvlink);
            utility.DownloadCsv(csvlink, importPath + mlidName71779 + "_sent.csv");

            csvlink = ReportBuilder.GenerateCSVLink(siteid, mlid, "972048", "bounce", startDate, endDate, apiPassword, messageIdLookup);

            utility.DownloadCsv(csvlink, importPath + mlidName71779 + "_bounce.csv");

            csvlink = ReportBuilder.GenerateCSVLink(siteid, mlid, "972048", "open", startDate, endDate, apiPassword, messageIdLookup);

            utility.DownloadCsv(csvlink, importPath + mlidName71779 + "_opens.csv");

            csvlink = ReportBuilder.GenerateCSVLink(siteid, mlid, "972048", "click", startDate, endDate, apiPassword, messageIdLookup);

            utility.DownloadCsv(csvlink, importPath + mlidName71779 + "_clicks.csv");
            
            
            
            
            // read in all the files into a streamreader object for each type

            StreamReader sentCsv = new StreamReader(importPath + mlidName71779+"_sent.csv");
            StreamReader bounceCsv = new StreamReader(importPath + mlidName71779+"_bounce.csv");
            StreamReader opensCsv = new StreamReader(importPath + mlidName71779 + "_opens.csv");
            StreamReader clicksCsv = new StreamReader(importPath + mlidName71779 + "_clicks.csv");
            
            // build up the email addresses we need to get the demographic information for

            //put them into a List object
            List<string> emailArray = new List<string>();

            //add in the unique emails from the different files
            emailArray = utility.ParseCsvAddEmail(sentCsv, emailArray);
            Console.WriteLine("the email count: " + emailArray.Count);
            emailArray = utility.ParseCsvAddEmail(bounceCsv, emailArray);
            Console.WriteLine("the email count: " + emailArray.Count);
            emailArray = utility.ParseCsvAddEmail(opensCsv, emailArray);
            Console.WriteLine("the email count: " + emailArray.Count);
            emailArray = utility.ParseCsvAddEmail(clicksCsv, emailArray);
            Console.WriteLine("the email count: " + emailArray.Count);

            //used to store the email address and their demographic vlaues
            Dictionary<string, string[]> emailDemographicLookup = new Dictionary<string, string[]>();
            //Dictionary<string, string[]> emailDemographicLookup = ReportBuilder.ParseDemographicValues(
            //    siteid, mlid, apiPassword, "dimundsngun1@hotmail.com", idArray69061);

            //build the email/demographic lookup table
            emailDemographicLookup = ReportBuilder.BuildEmailDemographicLookup(
                siteid, mlid, apiPassword, idArray69061, emailArray);

            //StreamReader newBounceCsv = new StreamReader(importPath + "bounce.csv");

            //reinitate the stream to re-read the file
            sentCsv = new StreamReader(importPath + mlidName71779 + "_sent.csv");
            bounceCsv = new StreamReader(importPath + mlidName71779 + "_bounce.csv");
            opensCsv = new StreamReader(importPath + mlidName71779 + "_opens.csv");
            clicksCsv = new StreamReader(importPath + mlidName71779 + "_clicks.csv");

            
            //create the csv file
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_sent.csv", sentCsv, emailDemographicLookup,
                nameArray71779, "sent", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_bounce.csv", bounceCsv,
                emailDemographicLookup, nameArray71779, "bounce", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_open.csv",opensCsv,
                emailDemographicLookup, nameArray71779, "open", "FLS_New_Report");
            ReportBuilder.CreateCsvFile(importPath + mlidName71779 + "_click.csv", clicksCsv,
                            emailDemographicLookup, nameArray71779, "click", "FLS_New_Report");
            */
            
            

        }
    }
}
