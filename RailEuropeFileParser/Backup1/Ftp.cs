using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RailEuropeFileParser
{
    class Ftp
    {
        public static void DownloadFile(string hostname, string ftpDirecotry,
            string remotefile, string localfile, string username, string password)
        {
            Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

            bool success;

            // Any string unlocks the component for the 1st 30-days.
            success = ftp.UnlockComponent("LYRISCFTP_kJUAJWNU8Rnh ");
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return;
            }

            ftp.Hostname = hostname;
            ftp.Username = username;
            ftp.Password = password;

            // The default data transfer mode is "Active" as opposed to "Passive".

            // Connect and login to the FTP server.
            success = ftp.Connect();
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return;
            }

            // Change to the remote directory where the file is located.
            // This step is only necessary if the file is not in the root directory
            // for the FTP account.


            if (ftpDirecotry != "/")
            {

                success = ftp.ChangeRemoteDir(ftpDirecotry);
                if (success != true)
                {
                    Log.WriteLine(ftp.LastErrorText);
                    return;
                }

            }

            // Download a file.

            success = ftp.GetFile(remotefile, localfile);
            if (success != true)
            {
                if (ftp.LastErrorText.Contains("FtpResponse21: 550"))
                {
                    Log.WriteLine("The following file wasn't found: " + remotefile);
                }
                Log.WriteLine(ftp.LastErrorText);
                return;
            }

            ftp.Disconnect();

            Log.WriteLine("File Downloaded!");



        }

        public static Boolean SimpleFtpUplaod(string localFile, string remoteFile,
            string ftpHostName, string ftpUserName, string ftpPassword, string directory)
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
            success = ftp.DeleteRemoteFile(remoteFile);
            if (success != true)
            {

                // log it, just in case
                Log.WriteLine(ftp.LastErrorText);
            }

            //  Upload a file
            // remote file is the local path
            // remote file is the name of the on the ftp server
            success = ftp.PutFile(localFile, remoteFile);
            if (success != true)
            {
                Log.WriteLine(ftp.LastErrorText);
                return success;
            }

            ftp.Disconnect();

            Log.WriteLine("File Uploaded!");

            return success;

        }
    }
}
