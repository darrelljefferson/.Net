using System;

namespace nbcorlandoDailyDataDump
{
    class Program
    {

        private static int Main(string[] args)
        {
            Log.Init(@"c:\lm_custom\NbcOrlandoDaily_log");
            
            string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcorlando;User ID=nbcorlando;"+
                "Password=orlandobloom88;Connect Timeout=600";
            
            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=nbcorlando;Trusted_Connection=Yes;" +
              //  "Connect Timeout=600";


            if (args.Length < 2)
            {
                System.Console.WriteLine("Please enter 2 arguments.");
                return 1;
            }
            else
            {
                Console.WriteLine("The start date is: " + args[0]);
                Console.WriteLine("the end date is: " + args[1]);
            }

            string exportPath = @"c:\lm_custom\export\";

            //DateTime now = DateTime.Now;
            //DateTime now = Convert.ToDateTime("2010-12-13");
            DateTime now = Convert.ToDateTime(args[1]);
            //DateTime now = DateTime.Now.AddDays(-1);
            
            string todaySuccess = String.Concat("success_", now.ToString("yyyy-MM-dd"), ".csv");
            string todayTmpRejection = String.Concat("temporary_rejection_", now.ToString("yyyy-MM-dd"), ".csv");
            string todayPermRejection = String.Concat("permanent_rejection_", now.ToString("yyyy-MM-dd"), ".csv");
            string todayOpens = String.Concat("opens_", now.ToString("yyyy-MM-dd"), ".csv");
            string todayClicks = String.Concat("clickthroughs_", now.ToString("yyyy-MM-dd"), ".csv");

            //DateTime yesterday = DateTime.Now.AddDays(-1.0);
            //DateTime yesterday = Convert.ToDateTime("2010-12-12");
            DateTime yesterday = Convert.ToDateTime(args[0]);
            //DateTime yesterday = DateTime.Now.AddDays(-2);
            
            string yesterdaySuccess = String.Concat("success_", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayTmpRejection = String.Concat("temporary_rejection_", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayPermRejection = String.Concat("permanent_rejection_", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayOpens = String.Concat("opens_", yesterday.ToString("yyyy-MM-dd"), ".csv");
            string yesterdayClicks = String.Concat("clickthroughs_", yesterday.ToString("yyyy-MM-dd"), ".csv");

            DateTime tomorrow = DateTime.Now.AddDays(1);
            
            string todayDate = now.ToString("yyyy-MM-dd");
            string yesterdayDate = yesterday.ToString("yyyy-MM-dd");
            string tomorrowDate = tomorrow.ToString("yyyy-MM-dd");


            string ftpUserName = "support";
            string ftpPassword = "ftpP0w3r!";

            
            //FtpConnect.RemoveOldFiles("ftp.lyris.com", ftpUserName, ftpPassword, todaySuccess, todayTmpRejection,
            //    todayPermRejection, todayOpens, todayClicks, yesterdaySuccess, yesterdayTmpRejection,
            //    yesterdayPermRejection, yesterdayOpens, yesterdayClicks);
            //FtpConnect.RemoveCurrentFiles("ftp.lyris.com", "support", "5UP3RU53R", todaySuccess, todayTmpRejection,
            //    todayPermRejection, todayOpens, todayClicks);


            

            
            GenerateFlatFile.ExportSuccessToCSVFile(connString, exportPath + todaySuccess, todayDate, yesterdayDate);
            GenerateFlatFile.ExportTemporaryRejectionToCSVFile(connString, exportPath + todayTmpRejection, todayDate, tomorrowDate);
            GenerateFlatFile.ExportPermanentRejectionToCSVFile(connString, exportPath + todayPermRejection, todayDate, yesterdayDate);
            GenerateFlatFile.ExportOpensToCSVFile(connString, exportPath + todayOpens, todayDate, yesterdayDate);
            GenerateFlatFile.ExportClickthroughsToCSVFile(connString, exportPath + todayClicks, todayDate, yesterdayDate);
            
            
            Upload upload = new Upload();

            /*
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando/olddata", exportPath + todaySuccess, ftpUserName, ftpPassword);
            //upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando/olddata", exportPath + todayTmpRejection, "support", "5UP3RU53R");
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando/olddata", exportPath + todayPermRejection, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando/olddata", exportPath + todayOpens, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando/olddata", exportPath + todayClicks, ftpUserName, ftpPassword);
            */
            /*
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todaySuccess, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayTmpRejection, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayPermRejection, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayOpens, ftpUserName, ftpPassword);
            upload.uploadFile("ftp://ftp.lyris.com/private/nbcorlando", exportPath + todayClicks, ftpUserName, ftpPassword);
            */

            Log.WriteLine("Program Finished");

            return 2;
        }
    }
}
