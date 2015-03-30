using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyMediaDailyExporer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            /*
            // moneymedia config
            Log.Init(@"c:\lm_custom\MoneyMediaDailyExport_log");

            string connString = "Data Source=db13.hosting.lyris.net;Initial Catalog=moneymedia;User ID=moneymedia;" +
                "Password=money060607;Connect Timeout=600";

            string opensReportName = "mm_opens";
            string clicksReportName = "mm_clicks";
            */
            
            // moneymedia 2 config
            Log.Init(@"c:\lm_custom\MoneyMedia2DailyExport_log");
            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
            //    "Password=m0ney$$$!!11;Connect Timeout=600";
            string connString = "Data Source=10.11.4.18;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
                "Password=m0ney$$$!!11;Connect Timeout=600";

            string opensReportName = "mm2_opens";
            string clicksReportName = "mm2_clicks";
            

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=abcPSTEST;Trusted_Connection=Yes;" +
            //    "Connect Timeout=600";

            string exportDirectoryPath = @"c:\lm_custom\export\";

            string ftpServerName = "ftp.corp.lyris.com";
            string ftpDirectory = "/private/moneymedia";
            //string ftpDirectoryPath = "ftp://ftp.lyris.com/private/nbcorlando";
            string ftpDirectoryPath = string.Concat("ftp://", ftpServerName, ftpDirectory);
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

            //FtpConnect.RemoveOldFiles(ftpServerName, ftpUsername, ftpPassword, currentReportName,
            //    lastReportName, ftpDirectory);
            //FtpConnect.RemoveCurrentFiles("ftp.lyris.com", "support", "support1994", todaySuccess, todayTmpRejection,
            //    todayPermRejection, todayOpens, todayClicks);



            //GenerateFlatFile.ExportReportToCSVFile(connString, exportDirectoryPath + currentReportName, todayDate, fromDate);
            GenerateFlatFile.ExportOpensToCSVFile(connString, exportDirectoryPath + opensReportName, todayDate, yestrdayDate);
            GenerateFlatFile.ExportClicksToCSVFile(connString, exportDirectoryPath + clicksReportName, todayDate, yestrdayDate);


            
            Upload upload = new Upload();
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + opensReportName, ftpUsername, ftpPassword);
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + clicksReportName, ftpUsername, ftpPassword);
            



            Log.WriteLine("Program Finished");
        }
    }
}
