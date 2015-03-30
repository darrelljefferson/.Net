﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace RailEuropeFileParser
{
    class SendEmail
    {
        public static void SendSingeRecipientEmails(string siteid, string mlid,
            string triggerid, string email, string[] values, string apiPassword)
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

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string firetrigger = String.Concat("https://www.elabs3.com/API/",
                "mailing_list.html?",
                "type=triggers&",
                "activity=fire-trigger&",
                "input=", encodedInput);


            Log.WriteLine(firetrigger);

            string url = "https://www.elabs3.com/API/mailing_list.html?";
            string payload = String.Concat("type=triggers&",
                "activity=fire-trigger&",
                "input=", encodedInput);


            // load API Xml into a XDocument
            XDocument fireEmail = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                fireEmail = MakeRequest(url, payload);
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

        public static void SendMultipleRecipientsEmail(string siteid, string mlid,
            string triggerid, string email, string [] values, string ftpFileLocation, string apiPassword)
        {
            // build the api call

            SendEmail.CreateEmailMember(siteid, mlid, email, values, apiPassword);

            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>", mlid, "</MLID>",
               "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients\">", email, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients_data\">", ftpFileLocation, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
               "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string firetrigger = String.Concat("https://www.elabs3.com/API/",
                "mailing_list.html?",
                "type=triggers&",
                "activity=fire-trigger&",
                "&input=", encodedInput);


            Log.WriteLine(firetrigger);

            string url = "https://www.elabs3.com/API/mailing_list.html?";
            string payload = String.Concat("type=triggers&",
                "activity=fire-trigger&",
                "input=", encodedInput);

            // load API Xml into a XDocument
            XDocument fireEmail = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                fireEmail = MakeRequest(url, payload);
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
                        Log.WriteLine("The following emails were sent: " + el.Value.ToString());
                    }
                    // log it the email
                    else if ((string)el.Attribute("type") == "not sent" && el.Value != "")
                    {
                        Log.WriteLine("The following emails were not sent: " +
                            el.Value.ToString());
                    }
                }
            }
        }

        public static void SendEmailsPostRequest(string siteid, string mlid,  string triggerid, string emails, string ftpPath,
            string apiPassword)
        {
            //WebClient wc = new WebClient();
            //wc.Proxy = null;

            //var data = new System.Collections.Specialized.NameValueCollection();
            //data.Add("type", "triggers");
            //data.Add("activity", "fire-trigger");
            string[] emaillist = emails.Split(',');

            //foreach (string email in emaillist ) {
            //      CreateEmailMember(siteid, mlid, email, values, apiPassword);
            //}


            string datasetInput = String.Concat("<DATASET>",
            "<SITE_ID>", siteid, "</SITE_ID>",
            "<MLID>", mlid, "</MLID>",
            "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
            "<DATA type=\"extra\" id=\"recipients\">", emails, "</DATA>",
            "<DATA type=\"extra\" id=\"recipients_data\">", ftpPath, "</DATA>",
            "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
            "</DATASET>");

            //data.Add("input", datasetInput);

            //Log.WriteLine(datasetInput);

            string encodedInput = SendEmail.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string url = "https://www.elabs3.com/API/mailing_list.html";
            string payload = String.Concat("type=triggers&",
                "activity=fire-trigger&",
                "input=", datasetInput);

            string result = MakeRequestString(url, payload);
            Log.WriteLine(result);

            //byte[] result = wc.UploadValues("https://www.elabs3.com/API/mailing_list.html",
            //    "POST", data);
            //System.IO.File.WriteAllBytes("sendManyRecipients.xml", result);
            System.IO.File.WriteAllText("sendManyRecipients.xml", result);
            System.Diagnostics.Process.Start("sendManyRecipients.xml");
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

        public static void CreateEmailMember(string siteid, string mlid, string email, string [] values, string apiPassword)
        {


            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>", mlid, "</MLID>",
               "<DATA type=\"email\" >", email, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
               "</DATASET>");



            Console.WriteLine(datasetInput);

            string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            Console.WriteLine(encodedInput);

            string recordUpload = String.Concat("https://www.elabs3.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=add",
                "&input=", encodedInput);


            Console.WriteLine(recordUpload);


            // load API Xml into a XDocument
            XDocument uploadFile = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                uploadFile = XDocument.Load(recordUpload);

            }
            catch (Exception e)
            {
                Console.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }



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

        #region Helpers
        private static XDocument MakeRequest(string url, string messageData)
        {
            string resultStr;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.UserAgent = "IE6.0";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ContentLength = messageData.Length;

            using (Stream myStream = myReq.GetRequestStream())
            {
                myStream.Write(System.Text.Encoding.ASCII.GetBytes(messageData), 0, messageData.Length);
                myStream.Close();
            }
            myReq.Timeout = 60000;

            // Stream in the HTTP Response

            StreamReader myStreamReader = new StreamReader(myReq.GetResponse().GetResponseStream());
            resultStr = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return XDocument.Parse(resultStr);

        }
        private static string MakeRequestString(string url, string messageData)
        {
            string resultStr;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            myReq.Method = "POST";
            myReq.UserAgent = "IE6.0";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ContentLength = messageData.Length;

            using (Stream myStream = myReq.GetRequestStream())
            {
                myStream.Write(System.Text.Encoding.ASCII.GetBytes(messageData), 0, messageData.Length);
                myStream.Close();
            }
            myReq.Timeout = 60000;

            // Stream in the HTTP Response

            StreamReader myStreamReader = new StreamReader(myReq.GetResponse().GetResponseStream());
            resultStr = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return resultStr;

        }
        #endregion
    }
}
