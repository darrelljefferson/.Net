using System;
using System.Collections.Generic;
using System.Text;

namespace PheaaDailyReportingExport
{
    class SFTP
    {
        public static void UploadFile(string server, string username, string password, string filePath, string fileName)
        {
            //  Important: It is helpful to send the contents of the
            //  sftp.LastErrorText property when requesting support.

            Chilkat.SFtp sftp = new Chilkat.SFtp();

            //  Any string automatically begins a fully-functional 30-day trial.
            bool success;
            success = sftp.UnlockComponent("LYRISCSSH_bYK5kMZA7Vna ");
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Set some timeouts, in milliseconds:
            sftp.ConnectTimeoutMs = 10000;
            sftp.IdleTimeoutMs = 20000;

            //  Connect to the SSH server.
            //  The standard SSH port = 22
            //  The hostname may be a hostname or IP address.
            int port;
            //string hostname;
            //hostname = "reports.urbandaddy.com";
            port = 10022;
            success = sftp.Connect(server, port);
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Authenticate with the SSH server.  Chilkat SFTP supports
            //  both password-based authenication as well as public-key
            //  authentication.  This example uses password authenication.
            success = sftp.AuthenticatePw(username, password);
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  After authenticating, the SFTP subsystem must be initialized:
            success = sftp.InitializeSftp();
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //string foo = sftp.OpenDir("/");
            //Console.WriteLine(foo);

            //  Open a file for writing on the SSH server.
            //  If the file already exists, it is overwritten.
            //  (Specify "createNew" instead of "createTruncate" to
            //  prevent overwriting existing files.)
            string handle;
            handle = sftp.OpenFile(fileName, "writeOnly", "createTruncate");
            if (handle == null)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Upload from the local file to the SSH server.
            success = sftp.UploadFile(handle, filePath);
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            //  Close the file.
            success = sftp.CloseHandle(handle);
            if (success != true)
            {
                //Console.WriteLine(sftp.LastErrorText);
                Log.WriteLine(sftp.LastErrorText);
                return;
            }

            Console.WriteLine(fileName + " Success.");
            Log.WriteLine("The following file was successfully uploaded: " + fileName);

        }
    }
}
