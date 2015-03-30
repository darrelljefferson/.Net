using System;
using System.Collections.Generic;
using System.Text;

namespace PheaaUploadAndFireTriggerParser
{
    class SFTP
    {
        public static void GetFlatFile(string hostname, string username, string password,
            string localFilePath, string remoteFilePath)
        {
            

            //  Important: It is helpful to send the contents of the
            //  sftp.LastErrorText property when requesting support.

            Chilkat.SFtp sftp = new Chilkat.SFtp();

            //  Any string automatically begins a fully-functional 30-day trial.
            bool success;
            success = sftp.UnlockComponent("LYRISCSSH_bYK5kMZA7Vna ");
            if (success != true)
            {
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Set some timeouts, in milliseconds:
            sftp.ConnectTimeoutMs = 5000;
            sftp.IdleTimeoutMs = 10000;

            //  Connect to the SSH server.
            //  The standard SSH port = 22
            //  The hostname may be a hostname or IP address.
            int port;
            port = 10022;
            success = sftp.Connect(hostname, port);
            if (success != true)
            {
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Authenticate with the SSH server.  Chilkat SFTP supports
            //  both password-based authenication as well as public-key
            //  authentication.  This example uses password authenication.
            success = sftp.AuthenticatePw(username, password);
            if (success != true)
            {
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  After authenticating, the SFTP subsystem must be initialized:
            success = sftp.InitializeSftp();
            if (success != true)
            {
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Download the file:
            //string remoteFilePath;
            //string localFilePath;
            
            //  Note: The remote filepath may be an absolute filepath,
            //  a relative filepath, or simply a filename.
            //  Relative filepaths are always relative to the home directory
            //  of the SFTP/SSH user account.  There is no such thing
            //  as "current remote directory" in the SFTP protocol.
            //  A filename with no path implies that the file is located
            //  in the SFTP user account's home directory.
            
            success = sftp.DownloadFileByName(remoteFilePath, localFilePath);
            if (success != true)
            {
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            Log.WriteLine("Successful Download.");
        }
    }
}
