using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace RailEuropeFileParser
{
    class old_SendEmail
    {
        public static void SendSingeRecipientEmails(string siteid, string mlid,
            string triggerid, string email, string apiPassword)
        {
            // build the api call
            
            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>",siteid,"</SITE_ID>",
               "<MLID>",mlid,"</MLID>",
               "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients\">",email,"</DATA>",
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
                        Log.WriteLine("The following email was sent: "+el.Value.ToString());
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
            string triggerid, string email, string ftpFileLocation,string apiPassword)
        {
            // build the api call

            

            string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>",mlid,"</MLID>",
               "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients\">", email, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients_data\">", ftpFileLocation, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">",apiPassword,"</DATA>",
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
                        Log.WriteLine("The following emails were sent: "+el.Value.ToString());
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

        public static void SendEmailsPostRequest(string siteid, string mlid, 
            string triggerid, string emails, string ftpPath,
            string apiPassword)
        {
            WebClient wc = new WebClient();
            wc.Proxy = null;

            var data = new System.Collections.Specialized.NameValueCollection();
            data.Add("type", "triggers");
            data.Add("activity", "fire-trigger");


               string datasetInput = String.Concat("<DATASET>",
               "<SITE_ID>", siteid, "</SITE_ID>",
               "<MLID>",mlid,"</MLID>",
               "<DATA type=\"extra\" id=\"trigger_id\">", triggerid, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients\">", emails, "</DATA>",
               "<DATA type=\"extra\" id=\"recipients_data\">", ftpPath, "</DATA>",
               "<DATA type=\"extra\" id=\"password\">",apiPassword,"</DATA>",
               "</DATASET>");

            data.Add("input", datasetInput);

            //Log.WriteLine(datasetInput);

            string encodedInput = old_SendEmail.ReturnHTMLEnocodedString(datasetInput);

            //Log.WriteLine(encodedInput);


            byte[] result = wc.UploadValues("https://www.elabs3.com/API/mailing_list.html",
                "POST", data);
            System.IO.File.WriteAllBytes("sendManyRecipients.xml", result);
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
    }
}
