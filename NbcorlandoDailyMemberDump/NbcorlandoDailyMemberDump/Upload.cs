using System;
using System.IO;
using System.Net;

namespace NbcorlandoDailyMemberDump
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
            Stream stream = ftpWebRequest.GetRequestStream();
            stream.Write(bs, 0, (int)bs.Length);
            stream.Close();
            Log.WriteLine(String.Concat("The following file was uploaded: ", filePath));
        }
    }
}