using System;

namespace NbcorlandoDailyMemberDump
{
    class FtpConnect
    {

        public static void RemoveOldFiles(string server, string username, string password, string main, string ap, string citywalk, string oldMain, string oldAP, string oldCitywalk)
        {
            FTP fTP = new FTP();
            try
            {
                fTP.Connect(server, username, password);
                fTP.ChangeDir("private/nbcorlando");
                //fTP.ChangeDir("private/brian");
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("FTP connect failed with the following message: ", e.Message));
            }
            fTP.RemoveFile(main);
            fTP.RemoveFile(ap);
            fTP.RemoveFile(citywalk);
            fTP.RemoveFile(oldMain);
            fTP.RemoveFile(oldAP);
            fTP.RemoveFile(oldCitywalk);
            Log.WriteLine("Old files removed from FTP server");
        }
    }
}
