﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Web;

namespace AMAListConsolidate
{
    class Program
    {
       
        public static DataTable dt_mlid = new DataTable();
        public static DataTable dt_mlid_demo = new DataTable();

        public static string[] type_array = new string[4];
        public static string[,] demo_array = new string[300, 2];

        public static DataTable email_stats = new DataTable();

        public static string siteid = "2010001995";  
        public static int elabs5 = 10;  
        public static string apiPassword = "am3r1can";  
        public static string master_list_name = "AMA_Grand_Daddy_List";
        public static DateTime today = Convert.ToDateTime("2011-05-27");

        static void Main(string[] args)
        {
            Log.Init(@"c:\temp\AMA_");
            //DateTime today = DateTime.Now.AddDays(0);
            
            string todayDate = today.ToString("yyyyMMdd");
            

            dt_mlid.Columns.Add("MLID", typeof(Int32));
            dt_mlid.Columns.Add("EMAIL", typeof(string));
            dt_mlid.Columns.Add("USERID", typeof(string));
            dt_mlid.Columns.Add("TRASHED", typeof(string));
            dt_mlid.Columns.Add("STATEDATE", typeof(DateTime));

            dt_mlid_demo.Columns.Add("MLID", typeof(Int32));
            dt_mlid_demo.Columns.Add("EMAIL", typeof(string));
            dt_mlid_demo.Columns.Add("USERID", typeof(string));
            dt_mlid_demo.Columns.Add("DEMOGAPHIC_ID", typeof(int));
            dt_mlid_demo.Columns.Add("VALUE", typeof(string));


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
                    GetMlidData(item.Value);
                   
                }
                else if (item.Key == "ama.alerts")
                {

                    GetMlidData(item.Value);
                }
                else if (item.Key == "amnews.online")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "health_information_technology")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "ama_bookstore")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "physician_health_eletter")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "ama_disparities_eletter")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "virtual_mentor_toc_list")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "ama_grad_med_educ")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "cmecppd")
                {
                    GetMlidData(item.Value);
                }
                else if (item.Key == "theraputic_insights_new_nwsltr")
                {
                    GetMlidData(item.Value);
                }
            } // End-Foreach

            Console.WriteLine("Build Medged File");
            MergeAllMLID();

        }

        public static void MergeAllMLID()
        {
            StreamWriter sr = new StreamWriter(@"c:\temp\master.csv");
          
                
                foreach (DataRow row in dt_mlid.Rows)
                {

                    sr.WriteLine(master_list_name + "," + row["EMAIL"] + "," + row["USERID"] + " ," + row["TRASHED"] + "," + row["STATEDATE"]);

                }
                sr.Close();
                Console.WriteLine("total number of emails: " + dt_mlid.Rows.Count);
        }

        public static void GetMlidData(Int32 IN_MLID)
        {
 
            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", IN_MLID, "</MLID>",
                "<DATA type=\"extra\" id=\"type\">", "active", "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);
            string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);
            //string encodedInput = ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string recordUpload = String.Concat("https://www.elabs10.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=query-listdata&",
                "&input=", encodedInput);


            Log.WriteLine(recordUpload);

            Console.WriteLine("About to Call XDocument");
            // load API Xml into a XDocument
            XDocument mlidcontext = new XDocument();
            // load the xml from the api call and check for errors
            
            try
            {
                mlidcontext = XDocument.Load(recordUpload);
                Console.WriteLine("Finished to Call XDocument");
                
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";
            string record = "RECORD";

            // load the xml as a string
            string checkXML = mlidcontext.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
                record = record.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)mlidcontext.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_MLID,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordUpload);
                Log.WriteLine("The failure reason was: " + mlidcontext.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)mlidcontext.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_MLID,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordUpload);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                // log the id, just in case we need it later
                Log.WriteLine(mlidcontext.Element(dataset).Element(record).Element(data).Value);

            
                // load it into a Enumerable interface element
                IEnumerable<XElement> elList =
                    from el in mlidcontext.Descendants("DATA")
                    select el;


                // iterate through the elements and then check to see if the demographic is populated

                // declare the email string
                string email = "";
                int demo_row = 0;
                int demo_col = 0;

                foreach (XElement el in elList)
                {
                    

                    // save the email
                    if ((string)el.Attribute("type") == "email")
                    {
                        // set the Email Value
                        
                        type_array[0]   = el.Value.ToString();
                        Console.WriteLine(type_array[0]);
                        
                    }
 
                    else if ((string)el.Attribute("type") == "extra"
                        && (string)el.Attribute("id") == "trashed")
                    {
                        // save the trashed flag
                        type_array[1] = el.Value.ToString();
                        Console.WriteLine(type_array[1]);
                    }
                     
                    else if ((string)el.Attribute("type") == "extra"
                        && (string)el.Attribute("id") == "statedate")
                    {
                        // save the statedate  
                        type_array[2] = el.Value.ToString();
                        Console.WriteLine(type_array[2]);

                    }
                    // add to the unsub list
                    else if ((string)el.Attribute("type") == "extra"
                        && (string)el.Attribute("id") == "uid")
                    {

                        type_array[3] = el.Value.ToString();
                        Console.WriteLine(type_array[3]);
                       
                    }
                    // add to the bounce list and then reset email value
                    // this ia always the last of each xml element
                    else if ((string)el.Attribute("type") == "demographic")
                        
                    {
                        // declare the array to be used in the dictionary
                        
                        //demo_array[demo_row, demo_col] = el.Value.ToString();   // Demo id value
                       // demo_array[demo_row++, ++demo_col] = el.Value.ToString(); // Demo value
                        
                    }
                    BuildTableData(IN_MLID);
                }  // End -FOREACH

                

            }  // End -IF



        }

        public static void BuildTableData(Int32 IN_MLID)
        {
            dt_mlid.Rows.Add(IN_MLID, type_array[0], type_array[3], type_array[1], today);
            Console.WriteLine("adding to datatabe: " + type_array[0] + " " + type_array[3]);
            //type_array = null;
           // type_array = null;
            //for (int i = 0; i == type_array.Length; i++)
            //{
            //    for (int col = 0; col < 2; col++)
            //    {

            //        dt_mlid_demo.Rows.Add(IN_MLID, type_array[0], type_array[3]);
            //    }
   

            //}


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
