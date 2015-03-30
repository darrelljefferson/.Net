using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Web;

namespace AMAMergeList
{
    class Program
    {

        public static DataTable dt_mlid = new DataTable("List");
        public static DataTable dt_mlid_demo = new DataTable("Demo");
        public static DataSet myDataSet = new DataSet("AMA");



        public static string[] type_array = new string[4];
        public static string[,] demo_array = new string[300, 2];

        public static DataTable email_stats = new DataTable();

        public static string siteid = "2010001995";
        public static int elabs5 = 10;
        public static string apiPassword = "am3r1can";
        public static string master_list_name = "AMA_Grand_Daddy_List";
        public static DateTime today = Convert.ToDateTime("2011-05-27");

        // create the array to be used to store the "extra" values
        public static string[] IdentityValues = new string[7];

        static void Main(string[] args)
        {
            Log.Init(@"c:\temp\AMA_");
            //DateTime today = DateTime.Now.AddDays(0);

            string todayDate = today.ToString("yyyyMMdd");


            dt_mlid.Columns.Add("MLID", typeof(Int32));
            dt_mlid.Columns.Add("EMAIL", typeof(string));
            dt_mlid.Columns.Add("LIST", typeof(string));
            dt_mlid.Columns.Add("STATE", typeof(string));
            dt_mlid.Columns.Add("STATEDATE", typeof(string));

            dt_mlid_demo.Columns.Add("MLID", typeof(Int32));
            dt_mlid_demo.Columns.Add("EMAIL", typeof(string));
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
                Console.WriteLine("Processing:  " + item.Key);
                // iterate through the files and send the messages
                if (item.Key == "amawire")
                {
                    GetMlidData(item.Key, item.Value);

                }
                else if (item.Key == "ama.alerts")
                {

                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "amnews.online")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "health_information_technology")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "ama_bookstore")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "physician_health_eletter")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "ama_disparities_eletter")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "virtual_mentor_toc_list")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "ama_grad_med_educ")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "cmecppd")
                {
                    GetMlidData(item.Key, item.Value);
                }
                else if (item.Key == "theraputic_insights_new_nwsltr")
                {
                    GetMlidData(item.Key, item.Value);
                }
            } // End-Foreach

            Console.WriteLine("Completed Merge AMA List");
           

        }


        /*---------------------------------------------------------------------------------------------------------
         * 
         *  Get Membber email Information
         * 
         * 
         *--------------------------------------------------------------------------------------------------------*/


        public static void GetMlidData(string IN_file, Int32 IN_mlid)
        {
            string UC_File = IN_file.ToUpper();


            for (int i = 0; i < 4; i++)

            {
                switch (i)
                {

                    case 0:

                        Unzip.UnzipFile(UC_File + "-ACTIVE");
                        ProcessFile(UC_File + "-ACTIVE", "active", IN_mlid);
                        
                        break;
                    case 1:

                        Unzip.UnzipFile(UC_File + "-TRASHED");
                        ProcessFile(UC_File + "-TRASHED", "trashed", IN_mlid);
                        
                        break;

                    case 2:

                        Unzip.UnzipFile(UC_File + "-UNSUBSCRIBED");
                        ProcessFile(UC_File + "-UNSUBSCRIBED", "unscribed", IN_mlid);
                        
                        break;
                    case 3:

                        Unzip.UnzipFile(UC_File + "-BOUNCED");
                        ProcessFile(UC_File + "-BOUNCED", "bounced", IN_mlid);
                        
                        break;
                        
                }
            }

         }

        /*---------------------------------------------------------------------------------------------------------
           * 
           * 
           * 
           * 
           *--------------------------------------------------------------------------------------------------------*/


        public static void ProcessFile(string IN_file, string IN_type, Int32 IN_mlid)
        {
            

            Chilkat.Csv csv = new Chilkat.Csv();
  
            string localpath = @"e:\projects\AMA\unzip\";
            string fullpath = String.Concat(localpath, IN_file, ".csv");


            csv.HasColumnNames = true;
            bool success = csv.LoadFile(fullpath);

            if (success != true )
            {
                // do something
            }


            for (int i = 0; i < csv.NumRows - 1; i++)
            {

                string emailaddr = csv.GetCell(i, 0);


            
                // build the api call

                string datasetInput = String.Concat("<DATASET>",
                    "<SITE_ID>", siteid, "</SITE_ID>",
                    "<MLID>", IN_mlid, "</MLID>",
                    "<DATA TYPE=\"EMAIL\" ID=\"START_DATE\">",emailaddr, "</DATA>",
                    "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                    "</DATASET>");

                Log.WriteLine(datasetInput);
            
                string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);

                Log.WriteLine(encodedInput);
            
                string recordQueryData = String.Concat("https://www.elabs10.com/API/",
                    "mailing_list.html?",
                    "type=record&",
                    "activity=Query-Data",
                    "&input=",encodedInput);

               // Console.WriteLine(recordQueryData);
                Log.WriteLine(recordQueryData);

                // load API Xml into a XDocument
                XDocument getRcordQueryDataResults = new XDocument();         
                // load the xml from the api call and check for errors
                try
                {
                    getRcordQueryDataResults = XDocument.Load(recordQueryData);
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

                // load the xml as a string
                string checkXML = getRcordQueryDataResults.ToString();

                // now check to see case and re-populate the variables if they are lower case
                if (checkXML.Contains("dataset"))
                {
                    dataset = dataset.ToLower();
                    type = type.ToLower();
                    data = data.ToLower();
                }

                // now check and log if we encouter an error or find no records returned
                if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "error")
                {
                    Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_mlid,
                        " failed"));
                    Log.WriteLine("The following api call that failed: \n" + recordQueryData);
                    Log.WriteLine("The failure reason was: " + getRcordQueryDataResults.Element(dataset).Element(data).Value);
                    Log.WriteLine("");
                }
                else if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "norecords")
                {
                    Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_mlid,
                        " has no records"));
                    Log.WriteLine("The following api call has no records: \n" + recordQueryData);
                    Log.WriteLine("");
                }
                // if no errors, then continue with the call
                // declare the int array to use to keep track of the counts

                else
                {
         
                    // load it into a Enumerable interface element
                    IEnumerable<XElement> elList =
                        from el in getRcordQueryDataResults.Descendants("DATA")
                        select el;


                    // iterate through the elements and then check to see if the demographic is populated

                    foreach (XElement el in elList)
                    {
                        XAttribute name = el.Attribute("id");
                    
                        // this is always the first xml element, so reset the count
                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "state")
                        {
                            IdentityValues[0] = el.Value.ToString();

                        }
                        else
                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "statedate")
                        {
                            IdentityValues[1] = el.Value.ToString();

                        }

                        else
                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "uid")
                        {
                            IdentityValues[2] = el.Value.ToString();

                        }
                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "joindate")
                        {
                            IdentityValues[3] = el.Value.ToString();

                        }

                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "trashed")
                        {
                            IdentityValues[4] = el.Value.ToString();

                        }
                        if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "email")
                        {
                            IdentityValues[5] = el.Value.ToString();

                        }

                        else if ((string)el.Attribute("type") == "demographic")
                             

                        dt_mlid_demo.Rows.Add(IN_mlid, emailaddr, (string)el.Attribute("id").Value.ToString(), el.Value.ToString());
                        
                        }

                        

                dt_mlid.Rows.Add(IN_mlid, emailaddr, IdentityValues[0] , IdentityValues[1] , IdentityValues[2] );
                CallAPIInserttMembers(IN_mlid, IN_file);
                }


            }
        }


        public static bool DetermineIfMemberExist(string emailaddr) {

            string datasetInput = String.Concat("<DATASET>",
                        "<SITE_ID>", siteid, "</SITE_ID>",
                        "<MLID>", IN_mlid, "</MLID>",
                        "<DATA TYPE=\"EMAIL\" ID=\"START_DATE\">", emailaddr, "</DATA>",
                        "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                        "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);

            Log.WriteLine(encodedInput);

            string recordQueryData = String.Concat("https://www.elabs10.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=Query-Data",
                "&input=", encodedInput);

            // Console.WriteLine(recordQueryData);
            Log.WriteLine(recordQueryData);

            // load API Xml into a XDocument
            XDocument getRcordQueryDataResults = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                getRcordQueryDataResults = XDocument.Load(recordQueryData);
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

            // load the xml as a string
            string checkXML = getRcordQueryDataResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordQueryData);
                Log.WriteLine("The failure reason was: " + getRcordQueryDataResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", IN_mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordQueryData);
                Log.WriteLine("");
            }
            // if no errors, then continue




        }
         
        public static void CallAPIInserttMembers(Int32 IN_mlid, string IN_file)
        {

            
            string dynamicataset = ConstructDataSet( IN_mlid, IN_file);

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", 187096, "</MLID>",
                 "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                dynamicataset,
                "</DATASET>");

            Log.WriteLine(datasetInput);
            string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);
            

            Log.WriteLine(encodedInput);

            string recordUpload = String.Concat("https://www.elabs10.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=add",
                "&input=", encodedInput);


            Log.WriteLine(recordUpload);

            // Console.WriteLine("About to Call XDocument");
            // load API Xml into a XDocument
            XDocument mlidcontext = new XDocument();
            // load the xml from the api call and check for errors

            try
            {
                mlidcontext = XDocument.Load(recordUpload);
                //Console.WriteLine("Finished to Call XDocument");

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
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", "186520",
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordUpload);
                Log.WriteLine("The failure reason was: " + mlidcontext.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)mlidcontext.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", "186520",
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordUpload);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                // log the id, just in case we need it later
               // Log.WriteLine(mlidcontext.Element(dataset).Element(record).Element(data).Value);


                // load it into a Enumerable interface element
                IEnumerable<XElement> elList =
                    from el in mlidcontext.Descendants("DATA")
                    select el;


                // iterate through the elements and then check to see if the demographic is populated


                foreach (XElement el in elList)
                {


                    // save the email
                    if ((string)el.Attribute("type") == "email")
                    {
                        // set the Email Value

                        type_array[0] = el.Value.ToString();
                        //Console.WriteLine(type_array[0]);

                    }



                }  // End -IF



            }

        }

        public static string ConstructDataSet(Int32 IN_mild, string IN_file) {

            

           string state = "";
           string datademo = "";

           // assign state  
            if (IdentityValues[0] == "active") {
                state = "subscribed";
            }
            else {

                state = "unsubscribed";
            }

            Int32 lookup_list_demo = DemographicLookupList(IN_file);

            string datademo1 = String.Concat("<DATA type=\"demographic\"", " id=\"", 51448, "\">", state, "</DATA>");


           // assign join date
           Int32 lookup_list_dt = DemographicLookupDate(IN_file);
           string datademo2 = String.Concat("<DATA type=\"demographic\"", " id=\"", 51449, "\">", IdentityValues[3], "</DATA>");



            foreach (DataRow row in dt_mlid_demo.Rows)
            {
                datademo += String.Concat("<DATA type=\"demographic\" ",  " id=\"", row["DEMOGAPHIC_ID"],  "\">", row["VALUE"], "</DATA>");

            }

            string datasetInput = String.Concat("<DATA type=\"email\" > ",  dt_mlid.Rows[0]["EMAIL"], " </DATA>",
                    datademo,
                    datademo1,
                    datademo2);


             dt_mlid_demo.Clear();
             dt_mlid.Clear();

            return (datasetInput);
        }

        public static Int32 DemographicLookupList(string IN_file) {

            int demo_List;

            Dictionary <string, int> DictListNameDemo = new Dictionary<string, int>()
            {
                {"amawire" ,	51448},
                {"ama_alerts",	51451},
                {"amnews_online",51453	 },
                {"amnews_online2" , 51457},
                {"health_information_technology",51458 },
                {"ama_bookstore", 51460	},
                {"health_system_reform_insight",51462	},
                {"healthy_lifestyles_eletter",	51464},
                {"physician_health_eletter",51466	},
                {"ama_disparities_eletter",	51468},
                {"virtual_mentor_toc_list",51470},
                {"ama_grad_med_educ",51472	},
                {"ama_health_profs_educ",	51474},
                {"cmecppd",	51476},
                {"theraputic_insights_new_nwsltr",	51477}
            };



            if (DictListNameDemo.TryGetValue(IN_file, out demo_List))
            {
                return (demo_List);
            }
            else
            {
                return (0);
            }


        }

        public static int DemographicLookupDate(string IN_file)
        {

            int demo_Date;

            Dictionary<string, int> DictListDateDemo = new Dictionary<string, int>()
            {
                {"amawire" ,51449	},
                {"ama_alerts",	51452},
                {"amnews_online",	51454 },
                {"amnews_online2" ,51457 },
                {"health_information_technology",51459 },
                {"ama_bookstore", 51461	},
                {"health_system_reform_insight",51463	},
                {"healthy_lifestyles_eletter",51465},
                {"physician_health_eletter",51467	},
                {"ama_disparities_eletter",	51469},
                {"virtual_mentor_toc_list",51471},
                {"ama_grad_med_educ",51473	},
                {"ama_health_profs_educ",51475	},
                {"cmecppd",	51482},
                {"theraputic_insights_new_nwsltr",51478	}
            };



            if (DictListDateDemo.TryGetValue(IN_file, out demo_Date))
            {
                return (demo_Date);
            }
            else
            {
                return (0);
            }



        }




    }


  }
