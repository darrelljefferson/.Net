﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PheaaUploadAndFireTriggerParser
{
    class Utility
    {
        public static Boolean SimpleFtpUplaod(string localFile,string remoteFile, 
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

        public static void ParseFileAndSendMessage(string siteid,string mlid,
            string triggerid, string password, string file)
        {
            StreamReader reader = new StreamReader(file);

            // ingnore the header line
            reader.ReadLine();

            // iterate through the file
            while (!reader.EndOfStream)
            {
                string[] line = reader.ReadLine().Split('|');

                // email is the 8th element
                string email = line[7];

                Log.WriteLine("email found is: " + email);

                SendSingeRecipientEmails(siteid, mlid, triggerid,
                    email, password);


            }
        }

        public static void SendSingeRecipientEmails(string siteid, string mlid,
            string triggerid, string email, string apiPassword)
        {
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>", mlid, "</MLID>",
               "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients\">", email, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
               "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = Program.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string firetrigger = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=triggers&",
                "activity=fire-trigger&",
                "&input=", encodedInput);


            Log.WriteLine(firetrigger);

            // load API Xml into a XDocument
            XDocument fireEmail = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                fireEmail = XDocument.Load(firetrigger);
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the record_Query-ListData API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string record = "RECORD";
            string data = "DATA";

            // load the xml as a string
            string checkXML = fireEmail.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                record = record.ToLower();
                data = data.ToLower();
            }


            // now check and log if we encouter an error or find no records returned
            if ((string)fireEmail.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " failed"));
                Log.WriteLine("The following api call that failed: \n" + firetrigger);
                Log.WriteLine("The failure reason was: " + fireEmail.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)fireEmail.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " has no records"));
                Log.WriteLine("The following api call has no records: \n" + firetrigger);
                Log.WriteLine("");
            }
            // no failures, so parse the xml now
            else
            {
                IEnumerable<XElement> elSent =
                    from el in fireEmail.Descendants("DATA")
                    select el;

                foreach (XElement el in elSent)
                {
                    if ((string)el.Attribute("type") == "sent" && el.Value != "")
                    {
                        Log.WriteLine("The following email was sent: " + el.Value.ToString());
                    }
                    // log it the email
                    else if ((string)el.Attribute("type") == "not sent" && el.Value != "")
                    {
                        Log.WriteLine("The following email was not sent: " +
                            el.Value.ToString());
                    }
                }


            }
        }




    }
}
