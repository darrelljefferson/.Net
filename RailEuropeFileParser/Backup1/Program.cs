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
            string uploadFtpDirectory = "private/RailEurope";

            string ftpFilePath = string.Concat(uploadFtpExternalHost,
                "/", uploadFtpDirectory);

            string scenario1TriggerId = "128379";
            string scenario2TriggerId = "128380";
            string scenario3TriggerId = "128381";
            string scenario4TriggerId = "128382";
            string scenario5TriggerId = "128383";
            string scenario6TriggerId = "128384";

            //DateTime today = DateTime.Now.AddDays(-1);
            DateTime today = Convert.ToDateTime("2011-05-09");
            string todayDate = today.ToString("yyyy-MM-dd");

            string scenario1FileName = string.Concat("RetargetAbandoners",
                todayDate, ".csv");
            string scenario2FileName = string.Concat("NoConvertWeclome",
                todayDate, ".csv");
            string scenario3FileName = string.Concat("RetargetFaresandSchedules",
                todayDate, ".csv");
            string scenario4FileName = string.Concat("UnconvertedShoppers",
                todayDate, ".csv");
            string scenario5FileName = string.Concat("PostBooking",
                todayDate, ".csv");
            string scenario6FileName = string.Concat("PostTravel",
                todayDate, ".csv");

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

            string fileDirectory = @"c:\custom\import\";

            
            // get the files
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario1FileName,
                fileDirectory + scenario1FileName, ftpUsername, ftpPassword);
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario2FileName,
                fileDirectory + scenario2FileName, ftpUsername, ftpPassword);
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario3FileName,
                fileDirectory + scenario3FileName, ftpUsername, ftpPassword);
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario4FileName,
                fileDirectory + scenario4FileName, ftpUsername, ftpPassword);
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario5FileName,
                fileDirectory + scenario5FileName, ftpUsername, ftpPassword);
            Ftp.DownloadFile(ftpServer, "/lyris/", scenario6FileName,
                fileDirectory + scenario6FileName, ftpUsername, ftpPassword);
            


            
            Dictionary<string, string[]> emailHash = new Dictionary<string, string[]> { };

            Log.WriteLine("parsing and creating the file");
            Log.WriteLine("");
            
            
            // parse and create the new files to be used for uploading
            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario1FileName, 
                emailHash, fileDirectory+newScenario1FileName, scenario1TriggerId, false);

            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario2FileName,
                emailHash, fileDirectory + newScenario2FileName, scenario2TriggerId, false);

            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario3FileName,
                emailHash, fileDirectory + newScenario3FileName, scenario3TriggerId, false);

            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario4FileName,
                emailHash, fileDirectory + newScenario4FileName, scenario4TriggerId, false);


            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario5FileName,
                emailHash, fileDirectory + newScenario5FileName, scenario5TriggerId, true);

            CsvParser.ParseAndCreateNewCsvFile(fileDirectory + scenario6FileName,
                emailHash, fileDirectory + newScenario6FileName, scenario6TriggerId, false);
            
            
            Log.WriteLine("uploading the files");
            Log.WriteLine("");

            // upload the files
            Ftp.SimpleFtpUplaod(fileDirectory + newScenario1FileName, newScenario1FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Ftp.SimpleFtpUplaod(fileDirectory + newScenario2FileName, newScenario2FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Ftp.SimpleFtpUplaod(fileDirectory + newScenario3FileName, newScenario3FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Ftp.SimpleFtpUplaod(fileDirectory + newScenario4FileName, newScenario4FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Ftp.SimpleFtpUplaod(fileDirectory + newScenario5FileName, newScenario5FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);

            Ftp.SimpleFtpUplaod(fileDirectory + newScenario6FileName, newScenario6FileName,
                uploadFtpExternalHost, uploadFtpUsername, uploadFtpPassword, uploadFtpDirectory);
            
            Log.WriteLine("sending the files");
            Log.WriteLine("");
            
            
            // send the messages
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario1FileName, siteid, mlid,
                scenario1TriggerId, apiPassword, false, ftpFilePath + newScenario1FileName);
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario2FileName, siteid, mlid,
                scenario2TriggerId, apiPassword, false, ftpFilePath + newScenario2FileName);
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario3FileName, siteid, mlid,
                scenario3TriggerId, apiPassword, false, ftpFilePath + newScenario3FileName);
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario4FileName, siteid, mlid,
                scenario4TriggerId, apiPassword, false, ftpFilePath + newScenario4FileName);
            
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario5FileName, siteid, mlid,
                scenario5TriggerId, apiPassword, true, ftpFilePath + newScenario5FileName);
            
            CsvParser.ParseAndSendMessages(fileDirectory + newScenario6FileName, siteid, mlid,
                scenario6TriggerId, apiPassword, false, ftpFilePath + newScenario6FileName);
            
            Log.WriteLine("Program Finished");

        }


    }


}
