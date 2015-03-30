using System;
using System.IO;
using System.Net;

namespace NbcOlympicsWeeklyReporting
{
    class Upload
    {

        public void uploadFile(string FTPAddress, string filePath, string username, string password)
        {
            FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(String.Concat(FTPAddress, "/", Path.GetFileName(filePath)));
            ftpWebRequest.Method = "STOR";
            ftpWebRequest.Credentials = new NetworkCredential(username, password);
            ftpWebRequest.UsePassive = true;
            ftpWebRequest.UseBinary = true;
            ftpWebRequest.KeepAlive = false;
            ftpWebRequest.Timeout = 600000;
            Console.WriteLine(filePath);
            FileStream fileStream = File.OpenRead(filePath);
            byte[] bs = new byte[fileStream.Length];
            fileStream.Read(bs, 0, (int)bs.Length);
            fileStream.Close();
            try
            {
                Stream requestStream = ftpWebRequest.GetRequestStream();
                //stream.Write(bs, 0, (int)bs.Length);
                int bufferSize = 8192;    // default to 4K, may want to use 8K instead          
                byte[] bytes = new byte[bufferSize];
                Stream s = new MemoryStream(bs);
                int read = 0;
                while ((read = s.Read(bytes, 0, bytes.Length)) != 0)
                {
                    requestStream.Write(bytes, 0, read);
                }
                requestStream.Flush();
                requestStream.Close();
                Log.WriteLine(String.Concat("The following file was uploaded: ", filePath));
            }
            catch (Exception e)
            {
                string errorText = "FTP upload failed with the following error: " + e;
                Log.WriteLine(errorText);
                Log.WriteLine("file name: " + filePath);
                Email.CreateErrorMessageBody(errorText);
                
            }


        }
    }
}
