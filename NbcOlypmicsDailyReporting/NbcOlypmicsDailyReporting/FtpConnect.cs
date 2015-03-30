using System;

namespace NbcOlypmicsDailyReporting
{
    class FtpConnect
    {

        public static void RemoveOldFiles(string server, string username, string password,
            string report, string oldReport, string ftpPath)
        {
            FTP fTP = new FTP();
            try
            {
                fTP.Connect(server, username, password);

                fTP.ChangeDir(ftpPath);
                //fTP.ChangeDir("private/brian");
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("FTP connect failed with the following message: ", e.Message));
            }

            fTP.RemoveFile(report);
            fTP.RemoveFile(oldReport);

            Log.WriteLine("Old files removed from FTP server");
        }

        public static void RemoveCurrentFiles(string server, string username, string password, string success, string tmpFailures, string permFailures, string opens, string click)
        {
            FTP fTP = new FTP();
            try
            {
                fTP.Connect(server, username, password);

                fTP.ChangeDir("private/nbcorlando/");
                //fTP.ChangeDir("private/brian");
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("FTP connect failed with the following message: ", e.Message));
            }

            fTP.RemoveFile(success);
            fTP.RemoveFile(tmpFailures);
            fTP.RemoveFile(permFailures);
            fTP.RemoveFile(opens);
            fTP.RemoveFile(click);

            Log.WriteLine("Old files removed from FTP server");
        }
    }
}
