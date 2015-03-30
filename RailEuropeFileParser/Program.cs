using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RailEuropeFileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"C:\custom\RailEuropeLog");

            HistoryTracker.Init();

            string farm = "elabs3";
            string type = "triggers";
            string activity = "fire-trigger";
            string siteid = "11194";
            string mlid = "254995";
            string apiPassword = "Lyris12345";

            string ftpServer = "ftp.raileurope.com";
            string ftpUsername = "lyris";
            string ftpPassword = "aItc3aEM";

            string uploadFtpExternalHost = "ftp.lyris.com";
            string uplaodFtpInternalHost = "ftp.corp.lyris.com";
            string uploadFtpUsername = "support";
            string uploadFtpPassword = "ftpP0w3r!";
            string uploadFtpDirectory = "private/RailEurope/";

            string ftpFilePath = string.Concat(uploadFtpExternalHost, "/", uploadFtpDirectory);

            string scenario1TriggerId = "128379";
            string scenario2TriggerId = "128380";
            string scenario3TriggerId = "128381";
            string scenario4TriggerId = "128382";
            string scenario5TriggerId = "128383";
            string scenario6TriggerId = "128384";

            DateTime today = DateTime.Now.AddDays(-1);
            //DateTime today = Convert.ToDateTime("2011-05-31");
            string todayDate = today.ToString("yyyy-MM-dd");

            string scenario1FileName = string.Concat("RetargetAbandoners",
                 "TESTFILE", ".csv");
            string scenario2FileName = string.Concat("NoConvertWeclome",
                 "TESTFILE", ".csv");
            string scenario3FileName = string.Concat("RetargetFaresandSchedules",
                "TESTFILE", ".csv");
            string scenario4FileName = string.Concat("UnconvertedShoppers",
                "TESTFILE", ".csv");
            string scenario5FileName = string.Concat("PostBooking",
               "TESTFILE", ".csv");
            string scenario6FileName = string.Concat("PostTravel",
                 "TESTFILE", ".csv");

            string newScenario1FileName = string.Concat("scenario1_",
                todayDate, ".csv");
            string newScenario2FileName = string.Concat("scenario2_",
                todayDate, ".csv");
            string newScenario3FileName = string.Concat("scenario3_",
                todayDate, ".csv");
            string newScenario4FileName = string.Concat("scenario4_",
                todayDate, ".csv");
            string newScenario5FileName = string.Concat("scenario5_",
                todayDate, ".csv");
            string newScenario6FileName = string.Concat("scenario6_",
                todayDate, ".csv");

            string fileDirectory = @"c:\custom\import\RE\";
/*
            Console.WriteLine("Start FTP of Download Scenario files ");
            // get the files
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario1FileName,
                fileDirectory + scenario1FileName, ftpUsername, ftpPassword);

            Console.WriteLine("Start FTP of Download scenario2FileName ");
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario2FileName,
                fileDirectory + scenario2FileName, ftpUsername, ftpPassword);

            Console.WriteLine("Start FTP of Download scenario3FileName ");
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario3FileName,
                fileDirectory + scenario3FileName, ftpUsername, ftpPassword);

            Console.WriteLine("Start FTP of Download scenario4FileName ");
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario4FileName,
                fileDirectory + scenario4FileName, ftpUsername, ftpPassword);

            Console.WriteLine("Start FTP of Download scenario5FileName ");
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario5FileName,
                fileDirectory + scenario5FileName, ftpUsername, ftpPassword);

            Console.WriteLine("Start FTP of Download scenario6FileName ");
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario6FileName,
                fileDirectory + scenario6FileName, ftpUsername, ftpPassword);

            Console.WriteLine("End FTP of Download Scenario files ");
 */

            Log.WriteLine("parsing and creating the file");
            Log.WriteLine("");

            Console.WriteLine("Start CSV Parser of Scenario files ");

            
            // parse and create the new files to be used for uploading
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario1FileName, 
                 fileDirectory+newScenario1FileName, scenario1TriggerId, false);

            Console.WriteLine("Start CSV Parser of Scenario2 files ");

            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario2FileName,
                 fileDirectory + newScenario2FileName, scenario2TriggerId, false);

            Console.WriteLine("Start CSV Parser of Scenario3 files ");
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario3FileName,
                 fileDirectory + newScenario3FileName, scenario3TriggerId, false);

            Console.WriteLine("Start CSV Parser of Scenario4 files ");
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario4FileName,
                 fileDirectory + newScenario4FileName, scenario4TriggerId, false);


            Console.WriteLine("Start CSV Parser of Scenario5 files ");
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario5FileName,
                 fileDirectory + newScenario5FileName, scenario5TriggerId, true);

            Console.WriteLine("Start CSV Parser of Scenario6 files ");
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario6FileName,
                 fileDirectory + newScenario6FileName, scenario6TriggerId, false);
            Console.WriteLine("End CSV Parser of Scenario files ");
            
            Log.WriteLine("uploading the files");
            Log.WriteLine("");

            Console.WriteLine("Start Ftp Upload of Scenario files ");
            // upload the files
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario1FileName, newScenario1FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Console.WriteLine("Start Ftp Upload of Scenario2 files ");
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario2FileName, newScenario2FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Console.WriteLine("Start Ftp Upload of Scenario3 files ");
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario3FileName, newScenario3FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Console.WriteLine("Start Ftp Upload of Scenario4 files ");
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario4FileName, newScenario4FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Console.WriteLine("Start Ftp Upload of Scenario5 files ");
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario5FileName, newScenario5FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Console.WriteLine("Start Ftp Upload of Scenario6 files ");
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario6FileName, newScenario6FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);
            
            Log.WriteLine("sending the files");
            Log.WriteLine("");
            Console.WriteLine("End Ftp Upload of Scenario files ");


            Console.WriteLine("Start Send of messages 1");
            // send the messages
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario1FileName, siteid, mlid,
                scenario1TriggerId, apiPassword, false, ftpFilePath + newScenario1FileName);

            Console.WriteLine("Start Send of messages 2 ");
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario2FileName, siteid, mlid,
                scenario2TriggerId, apiPassword, false, ftpFilePath + newScenario2FileName);

            Console.WriteLine("Start Send of messages 3 ");
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario3FileName, siteid, mlid,
                scenario3TriggerId, apiPassword, false, ftpFilePath + newScenario3FileName);

            Console.WriteLine("Start Send of messages 4 ");
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario4FileName, siteid, mlid,
                scenario4TriggerId, apiPassword, false, ftpFilePath + newScenario4FileName);

            Console.WriteLine("Start Send of messages 5 ");
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario5FileName, siteid, mlid,
                scenario5TriggerId, apiPassword, true, ftpFilePath + newScenario5FileName);

            Console.WriteLine("Start Send of messages 6 ");
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario6FileName, siteid, mlid,
                scenario6TriggerId, apiPassword, false, ftpFilePath + newScenario6FileName);

            HistoryTracker.PurgeOldEmails();

            Log.WriteLine("Program Finished");
            Console.WriteLine("End Send of messages ");

        }


    }


}
