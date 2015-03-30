using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UrbanDaddyDailyDataDump
{
    class Program
    {
        static void Main(string[] args)
        {


            Log.Init(@"c:\lm_custom\UrbandDaddy_Log_");
            //Log.Init(@"c:\lm\tclweb\htdocs\logs\log");

            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=urbandaddy;User ID=urbandaddy;" +
              //  "Password=whosyourdaddy;Connect Timeout=600";

            string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=urbandaddy;Trusted_Connection=Yes;" +
                "Connect Timeout=600";

    

            string exportDirectoryPath = @"c:\lm_custom\export\";
            //string exportDirectoryPath = @"c:\ud_export\";

            /*
            for (int i = -21; i <= 0; i++)
            {
                int currentDate = i + 1;

                string sentReportName = "sent_";
                string messageReportName = "message_";
                string bounceReportName = "bounce_";
                string opensReportName = "opens_";
                string clicksReportName = "clicks_";

                DateTime today = DateTime.Now.AddDays(currentDate);
                DateTime yesterday = DateTime.Now.AddDays(i);

                string todayDate = today.ToString("yyyy-MM-dd");
                string yesterdayDate = yesterday.ToString("yyyy-MM-dd");

                if (today > Convert.ToDateTime("2007-10-04"))
                {
                    opensReportName = string.Concat(opensReportName, todayDate, ".csv");
                    Log.WriteLine("");
                    GenerateFlatFile.ExportOpensToCSVFile(connString, string.Concat(exportDirectoryPath, opensReportName), yesterdayDate);
                }

                if (today > Convert.ToDateTime("2007-10-04"))
                {
                    clicksReportName = string.Concat(clicksReportName, todayDate, ".csv");
                    Log.WriteLine("");
                    //GenerateFlatFile.ExportClicksToCSVFile(connString, string.Concat(exportDirectoryPath, clicksReportName), yesterdayDate);
                    GenerateFlatFile.NewExportClicksToCSVFile(connString, string.Concat(exportDirectoryPath, clicksReportName), yesterdayDate);
                }

                if (today > Convert.ToDateTime("2008-11-18"))
                {
                    sentReportName = string.Concat(sentReportName, todayDate, ".csv");
                    messageReportName = string.Concat(messageReportName, todayDate, ".csv");
                    Log.WriteLine("");
                    GenerateFlatFile.ExportSentToCSVFile(connString, string.Concat(exportDirectoryPath, sentReportName), yesterdayDate);
                    Log.WriteLine("");
                    GenerateFlatFile.ExportMessageToCSVFile(connString, string.Concat(exportDirectoryPath, messageReportName), yesterdayDate);
                }

                if (today > Convert.ToDateTime("2010-02-05"))
                {
                    bounceReportName = string.Concat(bounceReportName, todayDate, ".csv");
                    Log.WriteLine("");
                    GenerateFlatFile.ExportBounceMessagesToCsv(connString, string.Concat(exportDirectoryPath, bounceReportName), yesterdayDate);
                }
                
                Log.WriteLine("");
            }
            */

            
            string sentReportName = "sent_";
            string messageReportName = "message_";
            string bounceReportName = "bounce_";
            string opensReportName = "opens_";
            string clicksReportName = "clicks_";
            string memberReportName = "members_";


            string sftpServerName = "reports.udadmin.com";
            string sftpUsername = "sparklist";
            string sftpPassword = "g3586eh7";

            
            //DateTime today = DateTime.Now.AddDays(0);
            DateTime today = Convert.ToDateTime("2011-04-01");

            //DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime yesterday = Convert.ToDateTime("2011-03-31");

            string todayDate = today.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");

            sentReportName = string.Concat(sentReportName, todayDate, ".csv");
            messageReportName = string.Concat(messageReportName, todayDate, ".csv");
            bounceReportName = string.Concat(bounceReportName, todayDate, ".csv");
            opensReportName = string.Concat(opensReportName, todayDate, ".csv");
            clicksReportName = string.Concat(clicksReportName, todayDate, ".csv");
            memberReportName = string.Concat(memberReportName, todayDate, ".csv");
            
            
            
            Log.WriteLine("");
            GenerateFlatFile.ExportSentToCSVFile(connString, string.Concat(exportDirectoryPath, sentReportName), yesterdayDate);
            
            Log.WriteLine("");
            GenerateFlatFile.ExportMessageToCSVFile(connString, string.Concat(exportDirectoryPath, messageReportName), yesterdayDate);
            
            Log.WriteLine("");
            GenerateFlatFile.ExportBounceMessagesToCsv(connString, string.Concat(exportDirectoryPath, bounceReportName), yesterdayDate);
            
            Log.WriteLine("");
            GenerateFlatFile.ExportOpensToCSVFile(connString, string.Concat(exportDirectoryPath, opensReportName), yesterdayDate);
            
            Log.WriteLine("");
            GenerateFlatFile.ExportClicksToCSVFile(connString, string.Concat(exportDirectoryPath, clicksReportName), yesterdayDate);
            Log.WriteLine("");
            
            //GenerateFlatFile.ExportMembersToCSVFile(connString, string.Concat(exportDirectoryPath, memberReportName), yesterdayDate);
            //Log.WriteLine("");
            
            /*
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, sentReportName), sentReportName);
            
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, messageReportName), messageReportName);
            
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, bounceReportName), bounceReportName);
            
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, opensReportName), opensReportName);
            
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, clicksReportName), clicksReportName);
            
            Log.WriteLine("");
            SFTP.UploadFile(sftpServerName, sftpUsername, sftpPassword, string.Concat(exportDirectoryPath, memberReportName), memberReportName);
            
            Log.WriteLine("");
            */
            
            Log.WriteLine("Program Finished");
            
            

        }
    }
}