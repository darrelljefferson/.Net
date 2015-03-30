using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class ftpitclass
    {
        public void ftpit(string ftppath, string ftpuser, string ftppass, string reportfilename)
        {
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);
        }

        static private void uploadFile(string FTPAddress, string filePath, string username, string password)
        {

            //Create FTP request
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FTPAddress + "/" + Path.GetFileName(filePath));

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            //Load the file
            FileStream stream = File.OpenRead(filePath);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            //Upload file

            Stream requestStream = request.GetRequestStream();
            //stream.Write(bs, 0, (int)bs.Length);
            int bufferSize = 8192;
            byte[] bytes = new byte[bufferSize];
            Stream s = new MemoryStream(buffer);
            int read = 0;
            while ((read = s.Read(bytes, 0, bytes.Length)) != 0)
            {
                requestStream.Write(bytes, 0, read);
            }
            requestStream.Flush();
            requestStream.Close();
            Console.WriteLine("FTP Upload Success");
        }
    }
}
