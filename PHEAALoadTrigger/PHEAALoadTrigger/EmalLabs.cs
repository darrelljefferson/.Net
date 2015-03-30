using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using System.Threading;

namespace PHEAALoadTrigger
{
    class EmalLabs
    {
        public static bool mystatus = false;
        public static bool bHasNoErrors = true;


        public static void CallStatusCheck(string file, string siteid, string mlid, string password)
        {
            string dataset = "DATASET";
            string dataset_1 = "DATASET_1";
            string type = "TYPE";
            string data = "DATA";
            string file_1 = "FILE";
            string status_1 = "STATUS";
            string apistatus = "pending";


            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"extra\" id=\"file\">", file, "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");



            Log.WriteLine(datasetInput);

            string encodedInput = Utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string recordStatus = String.Concat("https://www.elabs7.com/API/",          // chg back to elabs 7
                "mailing_list.html?",
                "type=record&",
                "activity=upload-status&",
                "input=", encodedInput);


            Log.WriteLine(recordStatus);
            Log.WriteLine(" Start Looping in Emaillab wait state: "  + " " + file);
            do
            {

                // load API Xml into a XDocument
                XDocument uploadStatus = new XDocument();
                // load the xml from the api call and check for errors
                try
                {
                    uploadStatus = XDocument.Load(recordStatus);

                }
                catch (Exception e)
                {
                    Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
                    bHasNoErrors = false;
                }

                string CheckStatus = uploadStatus.ToString();

                if (CheckStatus.Contains("dataset"))
                // now check to see case and re-populate the variables if they are lower case
                {
                    dataset = dataset.ToLower();
                    type = type.ToLower();
                    data = data.ToLower();
                    file_1 = file_1.ToLower();
                    status_1 = status_1.ToLower();
                }

                if ((string)uploadStatus.Element(dataset).Element(type).Value == "error")
                {
                    Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                        " failed"));
                    Log.WriteLine("The following api call that failed: \n" + recordStatus);
                    Log.WriteLine("The failure reason was: " + uploadStatus.Element(dataset).Element(data).Value);
                    Log.WriteLine("");
                    bHasNoErrors = false;
                }
                else if ((string)uploadStatus.Element(dataset).Element(type).Value == "norecords")
                {
                    Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                        " has no records"));
                    Log.WriteLine("The following api call has no records: \n" + recordStatus);
                    Log.WriteLine("");
                    bHasNoErrors = false;
                }
                // if not errors, then we just log the data results
                else
                {

                    apistatus = uploadStatus.Element(dataset).Element(dataset_1).Element(status_1).ToString();
                    
                    // Console.WriteLine("Looping in Emaillab wait state: " + apistatus + " " + file);

                }
            } while (!apistatus.Contains("done") && bHasNoErrors);
            Log.WriteLine("End Looping in Emaillab wait state: " + apistatus + " " + file);

        }

    }
}
