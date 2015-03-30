using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Web;

namespace AmaCreateDownLoads
{
    class Program
    {



        public static string[] type_array = new string[3];

        public static string siteid = "2010001995";
        public static int elabs5 = 10;
        public static string apiPassword = "am3r1can";
        public static string master_list_name = "AMA_Grand_Daddy_List";
        public static DateTime today = Convert.ToDateTime("2011-05-27");
        public static int rec_ct = 0;

        static void Main(string[] args)
        {
            //Log.Init(@"c:\temp\AMA_");
            //DateTime today = DateTime.Now.AddDays(0);

            string todayDate = today.ToString("yyyyMMdd");




            // Dictionary to hold  list and mlid info
            Dictionary<string, Int32> listMlidMap = new Dictionary<string, Int32>()
            {
               
                {"amawire" ,	119722},
                {"ama_alerts",	119692},
                {"amnews_online",	119724 },
                {"amnews_online2" , 119723},
                {"health_information_technology", 119744},
                {"ama_bookstore",	119693},
                {"health_system_reform_insight",	119747},
                {"healthy_lifestyles_eletter",	119748},
                {"physician_health_eletter",	119772},
                {"ama_disparities_eletter",	119697},
                {"virtual_mentor_toc_list",	119785},
                {"ama_grad_med_educ",	119699},
                {"ama_health_profs_educ",	119701},
                {"cmecppd",	119727},
                {"theraputic_insights_new_nwsltr",	119783}
            };


            foreach (KeyValuePair<string, Int32> item in listMlidMap)
            {
                Console.WriteLine("Processing: " + " " + item.Key);
                // iterate through the files and send the messages
                if (item.Key == "amawire")
                {
                    RequestDownLoad(item.Value);

                }
                else if (item.Key == "ama.alerts")
                {

                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "amnews.online")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "health_information_technology")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "ama_bookstore")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "physician_health_eletter")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "ama_disparities_eletter")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "virtual_mentor_toc_list")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "ama_grad_med_educ")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "cmecppd")
                {
                    RequestDownLoad(item.Value);
                }
                else if (item.Key == "theraputic_insights_new_nwsltr")
                {
                    RequestDownLoad(item.Value);
                }
            } // End-Foreach

            Console.WriteLine("Total downloads requested is " + rec_ct);


        }



        public static void RequestDownLoad(Int32 IN_MLID)
        {


            string[] download_types = new string[] { "active", "unsubscribed", "bounced", "trashed" };

            for (int i = 0; i < 4; i++)
            {

                string datasetInput = String.Concat("<DATASET>",
                        "<SITE_ID>", siteid, "</SITE_ID>",
                        "<MLID>", IN_MLID, "</MLID>",
                        "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                        "<DATA type=\"extra\" id=\"email_notify\">", "djefferson@lyris.com", "</DATA>",
                        "<DATA type=\"extra\" id=\"type\">", download_types[i], "</DATA>",
                        "</DATASET>");




                //Log.WriteLine(datasetInput);
                string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);
                //string encodedInput = ReturnHTMLEnocodedString(datasetInput);

                //Log.WriteLine(encodedInput);



                string recordUpload = String.Concat("https://www.elabs10.com/API/",
                    "mailing_list.html?",
                    "type=record&",
                    "activity=download&",
                    "&input=", encodedInput);


                Console.WriteLine(recordUpload);


                // load API Xml into a XDocument
                XDocument mlidcontext = new XDocument();
                // load the xml from the api call and check for errors

                try
                {
                    mlidcontext = XDocument.Load(recordUpload);
                    Console.WriteLine("Finished to Call XDocument for: " + IN_MLID);

                }
                catch (Exception e)
                {
                    Console.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
                }
            } // End-Fo Loop

        }
    }

}