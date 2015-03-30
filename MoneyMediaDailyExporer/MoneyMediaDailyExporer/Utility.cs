using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyMediaDailyExporer
{
    class Utility
    {

        public static Boolean SimpleFtpUplaod(string localFile, string remoteFile,
          string ftpHostName, string ftpUserName,
          string ftpPassword, string directory)
        {
            Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

            bool success;

            //  Any string unlocks the component for the 1st 30-days.
            success = ftp.UnlockComponent("LYRISCFTP_kJUAJWNU8Rnh");
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return success;
            }

            ftp.Hostname = ftpHostName;
            ftp.Username = ftpUserName;
            ftp.Password = ftpPassword;

            //  The default data transfer mode is "Active" as opposed to "Passive".

            //  Connect and login to the FTP server.
            success = ftp.Connect();
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return success;
            }

            //  Change to the remote directory where the file will be uploaded.
            success = ftp.ChangeRemoteDir(directory);
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return success;
            }

            // first delete the file, if it already exists
            //success = ftp.DeleteRemoteFile(remoteFile);
            //if (success != true)
            //{

            //    // log it, just in case
            //    Log.WriteLine(ftp.LastErrorText);
            //}

            //  Upload a file
            // local file is the local path
            // remote file is the name of the on the ftp server
            success = ftp.PutFile(localFile, remoteFile);
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return success;
            }

            ftp.Disconnect();

            Log.WriteLine("File Uploaded! " + remoteFile);

            return success;

        }
    }
}