using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UdReportSummay
{
    class Program
    {
        static void Main(string[] args)
        {


            Log.Init(@"c:\temp\LyrReportsummaryLog_");
            //Log.Init(@"c:\lm\tclweb\htdocs\logs\log");

            string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=urbandaddy;User ID=urbandaddy;" +
                "Password=whosyourdaddy;Connect Timeout=600";

            string exportDirectoryPath = @"z:\public\";

            string lyrReportSummaryData = "lyrReportSummaryData_";
            string lyrUnSummaryData = "lyrUnSummaryData_";
 
            string sftpServerName = "reports.udadmin.com";
            string sftpUsername = "sparklist";
            string sftpPassword = "g3586eh7";


            //DateTime today = DateTime.Now.AddDays(0);
            DateTime today = Convert.ToDateTime("2011-04-01");

            //DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime yesterday = Convert.ToDateTime("2011-03-31");

            string todayDate = today.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");

            lyrReportSummaryData = string.Concat(lyrReportSummaryData, todayDate, ".csv");


            


            Console.WriteLine("Building ReportSummaryData files at " + DateTime.Today);
            Log.WriteLine("");
            GenerateCSV.ExportReportSummary(connString, string.Concat(exportDirectoryPath, lyrReportSummaryData), yesterdayDate);
            Log.WriteLine("");

            Console.WriteLine("Building UnSummarized Data files at " + DateTime.Today);
            Log.WriteLine("");
            GenerateCSV.ExportUnSummary(connString, string.Concat(exportDirectoryPath, lyrUnSummaryData), yesterdayDate);
            Log.WriteLine("");


            Log.WriteLine("Program Finished");



        }
    }
}