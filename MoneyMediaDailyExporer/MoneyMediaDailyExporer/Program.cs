using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyMediaDailyExporer
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("ATTN: Begin processing at " + DateTime.Now);

            Console.WriteLine(" ");
            Console.WriteLine("Machine is : " + Environment.MachineName);
            Console.WriteLine(" ");

            string connString = "";
            string opensReportName = "";
            string clicksReportName = "";
            string bouncesReportName = "";
            string unsubsReportName = "";
 
            

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
             //   "Password=m0ney$$$!!11;Connect Timeout=600";



            if (Environment.MachineName == "MM2")
            {
                 Log.Init(@"c:\lm_custom\MoneyMedia2DailyExport_log");

                 connString = "Data Source=10.11.4.18;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
                                                         "Password=m0ney$$$!!11;Connect Timeout=600";
                  opensReportName = "mm2_opens";
                  clicksReportName = "mm2_clicks";
                  bouncesReportName = "mm2_bounces";
                  unsubsReportName = "mm2_unsubscribes";
            }
            else
            {
                  Log.Init(@"c:\lm_custom\MoneyMediaDailyExport_log");

                  connString = "Data Source=db13.hosting.lyris.net;Initial Catalog=moneymedia;User ID=moneymedia;" +
                                             "Password=money060607;Connect Timeout=600";

                   opensReportName = "mm_opens";
                   clicksReportName = "mm_clicks";
                   bouncesReportName = "mm_bounces";
                   unsubsReportName = "mm_unsubscribes";

            }


            Console.WriteLine(connString);

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=abcPSTEST;Trusted_Connection=Yes;" +
            //    "Connect Timeout=600";

            string exportDirectoryPath = @"c:\lm_custom\export\";

            string ftpInternalServerName = "ftp.corp.lyris.com";
            string ftpExternalServerName = "ftp.lyris.com";
            string ftpDirectory = "/private/moneymedia";
            
            string ftpDirectoryPath = string.Concat("ftp://", ftpExternalServerName, ftpDirectory);
            string ftpUsername = "support";
            string ftpPassword = "ftpP0w3r!";




            DateTime today = DateTime.Now.AddDays(-3);
            //DateTime today = Convert.ToDateTime("2010-03-30");

            DateTime yesterday = DateTime.Now.AddDays(-4);
            //DateTime lastWeek = Convert.ToDateTime("2010-03-23");

            string todayDate = today.ToString("yyyy-MM-dd");
            string yestrdayDate = yesterday.ToString("yyyy-MM-dd");
   

            opensReportName = String.Concat(opensReportName, todayDate, ".csv");
            clicksReportName = String.Concat(clicksReportName,todayDate,".csv");
            bouncesReportName = String.Concat(bouncesReportName, todayDate, ".csv");
            unsubsReportName = String.Concat(unsubsReportName, todayDate, ".csv");

            //FtpConnect.RemoveOldFiles(ftpServerName, ftpUsername, ftpPassword, currentReportName,
            //    lastReportName, ftpDirectory);
            //FtpConnect.RemoveCurrentFiles("ftp.lyris.com", "support", "support1994", todaySuccess, todayTmpRejection,
            //    todayPermRejection, todayOpens, todayClicks);


            //GenerateFlatFile.ExportReportToCSVFile(connString, exportDirectoryPath + currentReportName, todayDate, fromDate);
            GenerateFlatFile.ExportOpensToCSVFile(connString, exportDirectoryPath + opensReportName, todayDate, yestrdayDate);
            GenerateFlatFile.ExportClicksToCSVFile(connString, exportDirectoryPath + clicksReportName, todayDate, yestrdayDate);
            GenerateFlatFile.ExportBouncesToCSVFile(connString, exportDirectoryPath + bouncesReportName, todayDate, yestrdayDate);
            GenerateFlatFile.ExportUnsubsToCSVFile(connString, exportDirectoryPath + unsubsReportName, todayDate, yestrdayDate);
  /*          
            Upload upload = new Upload();
            Log.WriteLine("Uploading: " + opensReportName);
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + opensReportName, ftpUsername, ftpPassword);

            Log.WriteLine("Uploading: " + clicksReportName);
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + clicksReportName, ftpUsername, ftpPassword);

            Log.WriteLine("Uploading: " + bouncesReportName);
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + bouncesReportName, ftpUsername, ftpPassword);

            Log.WriteLine("Uploading: " + unsubsReportName);
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + unsubsReportName, ftpUsername, ftpPassword);
*/


            Log.WriteLine("Uploading: " + opensReportName);
            Utility.SimpleFtpUplaod(exportDirectoryPath + opensReportName, ftpDirectoryPath + @"/" + opensReportName, ftpExternalServerName, ftpUsername, ftpPassword, ftpDirectory);

            Log.WriteLine("Uploading: " + clicksReportName);
            Utility.SimpleFtpUplaod(exportDirectoryPath + clicksReportName, ftpDirectoryPath + @"/" + clicksReportName, ftpExternalServerName, ftpUsername, ftpPassword, ftpDirectory);

            Log.WriteLine("Uploading: " + bouncesReportName);
            Utility.SimpleFtpUplaod(exportDirectoryPath + bouncesReportName, ftpDirectoryPath + @"/" + bouncesReportName, ftpExternalServerName, ftpUsername, ftpPassword, ftpDirectory);
         
            Log.WriteLine("Uploading: " + unsubsReportName);
            Utility.SimpleFtpUplaod(exportDirectoryPath + unsubsReportName, ftpDirectoryPath + @"/" + unsubsReportName, ftpExternalServerName, ftpUsername, ftpPassword, ftpDirectory);
          
            Console.WriteLine("ATTN: END processing at " + DateTime.Now);
            Log.WriteLine("Program Finished");
        }
    }
}
