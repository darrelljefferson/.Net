using System;
using System.Collections.Generic;
using System.Text;

namespace NbcOlympicsWeeklyReporting
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\lm_custom\Weekly_Reporting_log");

            string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcolympics;User ID=nbcolympics;" +
                "Password=g0fortheg0ld;Connect Timeout=600";

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcolympics;Trusted_Connection=Yes;" +
            //    "Connect Timeout=600";

            string exportDirectoryPath = @"c:\lm_custom\export\";
            string reportName = "NBC_Olympics_Weekly_Reporting_";
            string ftpServerName = "ftp2.universalstudios.com";
            string ftpDirectory = "";
            //string ftpDirectoryPath = "ftp://ftp.lyris.com/private/nbcorlando";
            string ftpDirectoryPath = string.Concat("ftp://", ftpServerName, ftpDirectory);
            string ftpUsername = "OlyEmailAlerts";
            string ftpPassword = "olymp1cs";

            DateTime today = DateTime.Now.AddDays(-1);
            DateTime yesterday = DateTime.Now.AddDays(-2);
            DateTime lastWeek = DateTime.Now.AddDays(-8);

            string todayDate = today.ToString("yyyy-MM-dd");
            string lastWeekDate = lastWeek.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");


            string currentReportName = String.Concat(reportName, todayDate, ".csv");


            DBStatements.UpdateWeeklyReporting(connString, lastWeekDate, yesterdayDate);

            GenerateFlatFile.ExportReportToCSVFile(connString, exportDirectoryPath + currentReportName);



            Upload upload = new Upload();
            upload.uploadFile(ftpDirectoryPath, exportDirectoryPath + currentReportName, ftpUsername, ftpPassword);

            Log.WriteLine("Program Finished");

        }
    }
}
