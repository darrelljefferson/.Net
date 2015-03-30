using System;
using System.Collections.Generic;
using System.Text;

namespace NbcorlandoDailyMemberDump
{
    class Program
    {
        static int Main(string[] args)
        {
            Log.Init(@"c:\lm_custom\Members_log");
            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcorlando;Trusted_Connection=Yes;" +
            //    "Connect Timeout=600";
            
            string connString = String.Concat("Data Source=db18.hosting.lyris.net;",
                "Initial Catalog=nbcorlando;",
                "User ID=nbcorlando;",
                "Password=orlandobloom88;",
                "Connect Timeout=600");

            
            if (args.Length < 2)
            {
                Console.WriteLine("Please enter 2 arguments");
                return 1;
            }
            else
            {
                Console.WriteLine("The start date is: " + args[0]);
                Console.WriteLine("The end date is: " + args[1]);
            }
            
            string exportPath = @"c:\lm_custom\exportMembers\";

            //DateTime now = DateTime.Now.AddDays(0);
            //DateTime now = Convert.ToDateTime("2010-12-06");
            DateTime now = Convert.ToDateTime(args[1]);
            //DateTime now = DateTime.Now.AddDays(-1);
            
            string todayUOMain = String.Concat("export-", "uo_main-", now.ToString("yyyy-MM-dd"), ".csv");
            string todayUOAp = String.Concat("export-", "uo_ap-", now.ToString("yyyy-MM-dd"), ".csv");
            string todayUOCitywalk = String.Concat("export-", "uo_citywalk-", now.ToString("yyyy-MM-dd"), ".csv");

            //DateTime yesterday = DateTime.Now.AddDays(-1);
            //DateTime yesterday = Convert.ToDateTime("2010-12-05");
            DateTime yesterday = Convert.ToDateTime(args[0]);
            //DateTime yesterday = DateTime.Now.AddDays(-2);
            
            string yesterdayUOMain = String.Concat("export-", "uo_main-", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayUOAp = String.Concat("export-", "uo_ap-", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayUOCitywalk = String.Concat("export-", "uo_citywalk-", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string todayDate = now.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");

            string ftpUsername = "support";
            string ftpPassword = "ftpP0w3r!";

            string ftpServer = "ftp.corp.lyris.com";
            string ftpPath = "/private/nbcorlando/olddata";
            string ftpServerPath = "ftp://"+ftpServer + ftpPath;


            
            //FtpConnect.RemoveOldFiles("ftp.lyris.com", ftpUsername, ftpPassword, todayUOMain, todayUOAp,
            //    todayUOCitywalk, yesterdayUOMain, yesterdayUOAp, yesterdayUOCitywalk);
            GenerateFlatFile.ExportToCSVFile(connString, exportPath + todayUOMain, "uo_main", todayDate, yesterdayDate);
            GenerateFlatFile.ExportToCSVFile(connString, exportPath + todayUOAp, "uo_ap", todayDate, yesterdayDate);
            GenerateFlatFile.ExportToCSVFile(connString, exportPath + todayUOCitywalk, "uo_citywalk", todayDate, yesterdayDate);
            
            
            Upload upload = new Upload();
            
            upload.uploadFile(ftpServerPath, exportPath + todayUOMain, ftpUsername, ftpPassword);

            upload.uploadFile(ftpServerPath, exportPath + todayUOAp, ftpUsername, ftpPassword);

            upload.uploadFile(ftpServerPath, exportPath + todayUOCitywalk, ftpUsername, ftpPassword);
            
            /*
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayUOMain, ftpUsername, ftpPassword);
            
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayUOAp, ftpUsername, ftpPassword);
            
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayUOCitywalk, ftpUsername, ftpPassword);
            */

            Log.WriteLine("Program Finished");

            return 3;
        }
    }
}
