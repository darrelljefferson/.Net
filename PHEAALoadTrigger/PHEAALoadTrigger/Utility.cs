using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace PHEAALoadTrigger
{
    class Utility
    {

        public static DataTable dt_PHEAA_Incomming = new DataTable("PHEAA");
       
        public static DataTable dt_PHEAA_dups = new DataTable("DUPS");
        public static int count = 0;


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

            Log.WriteLine("File Uploaded! " + remoteFile);

            return success;

        }

        public static void InitDataTables() {
            
                dt_PHEAA_Incomming.Columns.Add("ARC", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("CMOD_UID", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("ACCOUNT", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("LETTER_ID", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("DOC_TYPE", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("DUE_DATE", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("JOIN_DATE", typeof(string));
                dt_PHEAA_Incomming.Columns.Add("EMAIL", typeof(string));
                dt_PHEAA_Incomming.PrimaryKey = new DataColumn[] { dt_PHEAA_Incomming.Columns["EMAIL"] };

                dt_PHEAA_dups.Columns.Add("ARC", typeof(string));
                dt_PHEAA_dups.Columns.Add("CMOD_UID", typeof(string));
                dt_PHEAA_dups.Columns.Add("ACCOUNT", typeof(string));
                dt_PHEAA_dups.Columns.Add("LETTER_ID", typeof(string));
                dt_PHEAA_dups.Columns.Add("DOC_TYPE", typeof(string));
                dt_PHEAA_dups.Columns.Add("DUE_DATE", typeof(string));
                dt_PHEAA_dups.Columns.Add("JOIN_DATE", typeof(string));
                dt_PHEAA_dups.Columns.Add("EMAIL", typeof(string));
        
        }


        public static void ParseFileAndSendMessage( string ftpInternalHostName, string ftpUserName, string ftpPassword, string ftpDirectory,
            string siteid, string mlid, string triggerid, string password, string file, string notify_email)
        {

            
            string tmp1_file = SeperateNoneDupEmails(file);
            string localtemppath = @"c:\temp\";

            string localpathandfile = String.Concat(localtemppath, tmp1_file);

            string annonymousFtpString = string.Concat("ftp://", "anonymous:a@", ftpInternalHostName, "/", ftpDirectory, "/", tmp1_file);

            Log.WriteLine("Uploading  " + file + "=> " + tmp1_file);
            SimpleFtpUplaod(localpathandfile, tmp1_file, ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory);

            Log.WriteLine("API update for " + tmp1_file);
            UploadFile(siteid, mlid, notify_email, annonymousFtpString, password);

            Log.WriteLine("API trigger for " + tmp1_file);
            UploadAndTrigger(siteid, mlid, notify_email, triggerid, annonymousFtpString, password);

            File.Delete(localpathandfile); // delete tmp file no longer needed

            Log.WriteLine("Number of Emails in DataTable is " + dt_PHEAA_Incomming.Rows.Count + " => " + file);

            dt_PHEAA_Incomming.Clear();

            /* -------------------------------------------- THIS LOGIC HANDLES DUPLICATE EMAILS ---------------------------------------------*/

            string tmp2_file = SeperateDupEmails();
            // this section of logic processes single emailaddr one at a time
             string tmpfileanddir = @"c:\temp\" + tmp2_file;
             StreamReader reader = new StreamReader(tmpfileanddir);
 
            // ingnore the header line
            string csvhdr = reader.ReadLine();

            // iterate through the file
            while (!reader.EndOfStream)
            {

                string[] line = reader.ReadLine().Split('|');

                // email is the 8th element
                string email = line[7];


                Log.WriteLine("API Update & Trigger Processing for Dups Email: " + email  );
                
                UpdateSingeRecipientEmails(siteid, mlid, email,  line, password);
                SendSingeRecipientEmails(siteid, mlid, triggerid, email,  password);

            }
            reader.Close();
            File.Delete(tmpfileanddir);
        
        }

        public static string ReturnHTMLEnocodedString(string input)
        {

            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            // Any code begins the 30-day trial.
            crypt.UnlockComponent("LYRISCCrypt_4NataCGcVVkW ");
            crypt.CryptAlgorithm = "none";
            crypt.EncodingMode = "url";

            return crypt.EncryptStringENC(input);


        }

        public static string  SeperateNoneDupEmails(string file)
        {
            string fileName =  Guid.NewGuid().ToString() + ".txt";
            string localfilepath = @"c:\temp\" + fileName;
            Int32 rec_ct = 0;
            Int32 dups_rec_ct = 0;

            FileStream fs = new FileStream(localfilepath,  FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            StreamReader reader = new StreamReader(file);
 
            // insert the header line back into the dup datatable for writing out when processing dups emails
            string[] csvheader = reader.ReadLine().Split('|');

            dt_PHEAA_dups.Rows.Add(csvheader[0], csvheader[1], csvheader[2], csvheader[3], csvheader[4], csvheader[5], csvheader[6], csvheader[7]);

            sw.WriteLine(String.Concat(csvheader[0] , "|", csvheader[1], "|", csvheader[2], "|", csvheader[3], "|", csvheader[4], "|", 
                csvheader[5], "|", csvheader[6], "|", csvheader[7]));

            // iterate through the file
            while (!reader.EndOfStream)
            {

                string[] line = reader.ReadLine().Split('|');

                if (line.Length == 8)
                {
                    // email is the 8th element
                    string email = line[7];
                    // FileStream fs = new FileStream(localpathandfile, FileMode.Create, FileAccess.Write);

                    try
                    {
                        dt_PHEAA_Incomming.Rows.Add(line[0], line[1], line[2], line[3], line[4], line[5], line[6], line[7]);

                        string csvline = String.Concat(line[0], "|", line[1], "|", line[2], "|", line[3], "|", line[4], "|", line[5], "|", line[6], "|", line[7]);
                        sw.WriteLine(csvline);
                        rec_ct++;
                    }
                    catch (ConstraintException e)
                    {
                        Log.WriteLine("Found Dup Email: " + line[7]);
                        dt_PHEAA_dups.Rows.Add(line[0], line[1], line[2], line[3], line[4], line[5], line[6], line[7]);
                        dups_rec_ct++;
                    }


                }
            }
            sw.Close();
            fs.Close();

            Log.WriteLine("Inserted " + rec_ct + " emails into " + fileName);
            Log.WriteLine("Inserted into Dups DataTable " + dups_rec_ct);
            dups_rec_ct = 0;
            rec_ct = 0;
            return fileName;
        }

        public static string SeperateDupEmails()
        {
            string fileName = Guid.NewGuid().ToString() + ".txt";
            string localfilepath = @"c:\temp\" + fileName;
            FileStream fs2 = new FileStream(localfilepath, FileMode.Create, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(fs2);
            Int32 rec_ct = 0;

            foreach (DataRow row in dt_PHEAA_dups.Rows)

            {
                Log.WriteLine("Write Dup Email: " + row["EMAIL"].ToString()  );

               string csvline = String.Concat(row["ARC"].ToString(), "|", row["CMOD_UID"].ToString(), "|", row["ACCOUNT"].ToString(), "|", row["LETTER_ID"], "|", row["DOC_TYPE"].ToString(),
                    "|", row["DUE_DATE"].ToString(), "|", row["JOIN_DATE"].ToString(), "|", row["EMAIL"].ToString());

                sw2.WriteLine(csvline);
                rec_ct++;
            }
            Log.WriteLine("Insert " + rec_ct + "  emails into File "  + fileName);

            sw2.Close();
            fs2.Close();
            dt_PHEAA_dups.Clear();
            return fileName;

        }



        public static void UploadFile(string siteid, string mlid, string notify_email, string uploadfile, string password)
        {
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"email\">", notify_email, "</DATA>",
                "<DATA type=\"extra\" id=\"file\">", uploadfile, "</DATA>",
                "<DATA type=\"extra\" id=\"validate\">", "on", "</DATA>",
                "<DATA type=\"extra\" id=\"type\">", "active", "</DATA>",
                "<DATA type=\"extra\" id=\"update\">", "on", "</DATA>",
                // "<DATA type=\"extra\" id=\"trigger\">", "yes", "</DATA>",
                //"<DATA type=\"extra\" id=\"clear_trigger_history\">", "all", "</DATA>",
                "<DATA type=\"extra\" id=\"Delimiter\">", "|", "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");



            //Log.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            //Log.WriteLine(encodedInput);

            string recordUpload = String.Concat("https://www.elabs7.com/API/",            // chg to 7 
                "mailing_list.html?",
                "type=record&",
                "activity=upload",
                "&input=", encodedInput);


            //Log.WriteLine(recordUpload);


            // load API Xml into a XDocument
            XDocument uploadFile = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                uploadFile = XDocument.Load(recordUpload);
                
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // Loop on upload status call til status is done
            EmalLabs.CallStatusCheck(uploadfile, siteid, mlid, password);

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = uploadFile.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)uploadFile.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordUpload);
                Log.WriteLine("The failure reason was: " + uploadFile.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)uploadFile.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordUpload);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                // log the id, just in case we need it later
                Log.WriteLine(uploadFile.Element(dataset).Element(data).Value);


            }


        }


        public static void UpdateSingeRecipientEmails(string siteid, string mlid,  string email, string[] line, string apiPassword)
        {
            // build the api call

            string demostr = "";

            for (int i=0; i < line.Count() -1 ; i++) 
            {

              switch (i)
              {

                  case 0:
                        demostr = String.Concat("<DATA type=\"demographic\"", " id=\"" , 52534 , "\">", line[i],  "</DATA>" ); // ARC
                        break;
                  case 1:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 60475, "\">", line[i], "</DATA>"); // CMOD_UID
                        break;
                  case 2:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 58687, "\">", line[i], "</DATA>"); // Account#
                        break;
                  case 3:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 44293, "\">", line[i], "</DATA>"); // Letter_id
                        break;
                  case 4:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 60476, "\">", line[i], "</DATA>"); // Doc_type
                        break;
                  case 5:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 43302, "\">", line[i], "</DATA>"); // Due_date
                        break;
                  case 6:
                        demostr += String.Concat("<DATA type=\"demographic\"", " id=\"", 33630, "\">", line[i], "</DATA>"); // join_date
                        break;
              }

                    
            }
           
            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>", mlid, "</MLID>",
               "<DATA type=\"email\" >", email, "</DATA>",
               demostr,
               "<DATA type=\"extra\" id=\"state\">", "active", "</DATA>",
               "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
               "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            //Log.WriteLine(encodedInput);

            string Firetrigger = String.Concat("https://www.elabs7.com/API/",        // chg to 7 
                "mailing_list.html?",
                "type=record&",
                "activity=update",
                "&input=", encodedInput);


           // Log.WriteLine(Firetrigger);

            // load API Xml into a XDocument
            XDocument fireEmail = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                fireEmail = XDocument.Load(Firetrigger);
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
                Log.WriteLine("The following api call that failed: \n" + Firetrigger);
                Log.WriteLine("The failure reason was: " + fireEmail.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)fireEmail.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " has no records"));
                Log.WriteLine("The following api call has no records: \n" + Firetrigger);
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

        public static void SendSingeRecipientEmails(string siteid, string mlid, string triggerid, string email, string apiPassword)
        {
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>", mlid, "</MLID>",
                "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
                "<DATA type=\"extra\" id=\"recipients\">", email, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
               "</DATASET>");

            //Log.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            // Log.WriteLine(encodedInput);

            string Firetrigger = String.Concat("https://www.elabs7.com/API/",        // chg to 7 
                "mailing_list.html?",
                "type=triggers&",
                "activity=fire-trigger",
                "&input=", encodedInput);


           // Log.WriteLine(Firetrigger);

            // load API Xml into a XDocument
            XDocument fireEmail = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                fireEmail = XDocument.Load(Firetrigger);
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
                Log.WriteLine("The following api call that failed: \n" + Firetrigger);
                Log.WriteLine("The failure reason was: " + fireEmail.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)fireEmail.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " has no records"));
                Log.WriteLine("The following api call has no records: \n" + Firetrigger);
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

        public static void UploadAndTrigger(string siteid, string mlid, string notify_email, string triggerid, string uploaddatafile, string password)
        {
            // build the api call
            int count = 0;
            DateTime datetime =  DateTime.Now;

            string multiemails = "";
            int i = 1;
            foreach (DataRow row in dt_PHEAA_Incomming.Rows)
            {
                if (i != dt_PHEAA_Incomming.Rows.Count)
                {
                    multiemails += String.Concat(row["EMAIL"], ",");
                    i++;
                }
                else
                {
                    multiemails += row["EMAIL"];
                }

            }

                string[] recipientsArr = multiemails.Split(',');
                int chunkSize = 100;
                List<List<string>> chunks = Split(new List<string>(recipientsArr), chunkSize);

                foreach (List<string> chunk in chunks)
                {
                    string currentRecipients = string.Join(",", chunk.ToArray());
                    count = chunk.Count;

                    // currentRecipients.Replace(" ", "");
                    // Log.WriteLine(currentRecipients);
                    string encodedInput = ReturnHTMLEnocodedString(currentRecipients);

                    Regex r = new Regex(@"\++");
                    encodedInput = r.Replace(encodedInput, @"");

                    string datasetInput = String.Concat("<DATASET>",
                        "<SITE_ID>", siteid, "</SITE_ID>",
                        "<MLID>", mlid, "</MLID>",
                        "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
                        "<DATA type=\"extra\" id=\"recipients\">", encodedInput, "</DATA>",
                        "<DATA type=\"extra\" id=\"clickthru\">", "on", "</DATA>",
                        "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                        "</DATASET>");

                   // Log.WriteLine(datasetInput);
                  

                    
                   // Log.WriteLine(encodedInput);

                    string recordUpload = String.Concat("https://www.elabs7.com/API/",            // chg to 7 
                        "mailing_list.html?",
                        "type=triggers&",
                        "activity=fire-trigger",
                        "&input=", datasetInput);


                    // Log.WriteLine(recordUpload);


                    // load API Xml into a XDocument
                    XDocument uploadFile = new XDocument();
                    // load the xml from the api call and check for errors
                    try
                    {
                        uploadFile = XDocument.Load(recordUpload);

                    }
                    catch (Exception e)
                    {
                        Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
                    }

                    Log.WriteLine("Processing mass emails " + count);

                    // since the api returned back for errors can be either upper or lower case, we have to check which one it is
                    // assume upper case and adjust if we find lower case
                    string dataset = "DATASET";
                    string type = "TYPE";
                    string data = "DATA";

                    // load the xml as a string
                    string checkXML = uploadFile.ToString();

                    // now check to see case and re-populate the variables if they are lower case
                    if (checkXML.Contains("dataset"))
                    {
                        dataset = dataset.ToLower();
                        type = type.ToLower();
                        data = data.ToLower();
                    }
                   /* 
                    // now check and log if we encouter an error or find no records returned
                    if ((string)uploadFile.Element(dataset).Element(type).Value == "error")
                    {
                        Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                            " failed"));
                        Log.WriteLine("The following api call that failed: \n" + recordUpload);
                        Log.WriteLine("The failure reason was: " + uploadFile.Element(dataset).Element(data).Value);
                        Log.WriteLine("");
                    }
                    else if ((string)uploadFile.Element(dataset).Element(type).Value == "norecords")
                    {
                        Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                            " has no records"));
                        Log.WriteLine("The following api call has no records: \n" + recordUpload);
                        Log.WriteLine("");
                    }
                    // if not errors, then we just log the data results
                    else
                    {
                        // log the id, just in case we need it later
                        Log.WriteLine(uploadFile.Element(dataset).Element(data).Value);


                    }
                     */
                } // End-FOREACH

        }
        private static List<List<string>> Split(List<string> source, int chunckSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunckSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        } 
            }

        }
  

