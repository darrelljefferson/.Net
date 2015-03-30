using System;
using System.Collections.Generic;
using System.Text;

namespace NbcOlypmicsDailyReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\lm_custom\Daily_Reporting_log");

            string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcolympics;User ID=nbcolympics;" +
                "Password=g0fortheg0ld;Connect Timeout=600";

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcolympics;Trusted_Connection=Yes;" +
            //    "Connect Timeout=600";

            string exportDirectoryPath = @"c:\lm_custom\export\";
            string reportName = "NBC_Olympics_Daily_Reporting_";
            string ftpServerName = "ftp2.universalstudios.com";
            string ftpDirectory = "";
            //string ftpDirectoryPath = "ftp://ftp.lyris.com/private/nbcorlando";
            string ftpDirectoryPath = string.Concat("ftp://", ftpServerName, ftpDirectory);
            string ftpUsername = "OlyEmailAlerts";
            string ftpPassword = "olymp1cs";

            DateTime today = DateTime.Now.AddDays(-2);

            DateTime yesterday = DateTime.Now.AddDays(-3);

            string todayDate = today.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");
   

            string currentReportName = String.Concat(reportName, todayDate, ".csv");

            //string lastReportName = String.Concat(reportName, lastWeekDate, ".csv");



            //FtpConnect.RemoveOldFiles(ftpServerName, ftpUsername, ftpPassword, currentReportName,
            //    lastReportName, ftpDirectory);
            //FtpConnect.RemoveCurrentFiles("ftp.lyris.com", "support", "support1994", todaySuccess, todayTmpRejection,
            //    todayPermRejection, todayOpens, todayClicks);

            DBStatements.DeleteDailyReportingData(connString);
            DBStatements.InsertListData(connString);
            DBStatements.CheckForPreviousInserts(connString, "unsub", yesterdayDate, todayDate);
            DBStatements.CheckForPreviousInserts(connString, "sub", yesterdayDate, todayDate);
            DBStatements.CheckForPreviousInserts(connString, "total", yesterdayDate, todayDate);
            DBStatements.CheckForPreviousInserts(connString, "sent", yesterdayDate, todayDate);
            DBStatements.UpdateUnsubReportingData(connString, todayDate);
            DBStatements.UpdateSubscribeReportingData(connString, todayDate);
            DBStatements.UpdateListTotalsReportingData(connString, todayDate);
            DBStatements.UpdateListSentReportingData(connString, todayDate);            
            
            GenerateFlatFile.ExportReportToCSVFile(connString, exportDirectoryPath + currentReportName);

            Upload upload = new Upload();
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + currentReportName, ftpUsername, ftpPassword);

            Log.WriteLine("Program Finished");
        }
    }
}
