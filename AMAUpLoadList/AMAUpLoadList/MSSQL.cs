using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

namespace AMAUpLoadList
{
    class MSSQL
    {
        public static string  _amawire_list    = "";
        public static string  _amawire_start   = "";
        public static string  _ama_alerts_list   = "";
        public static string  _ama_alerts_start   = "";
        public static string  _amnews_online_list   = "";
        public static string  _amnews_online_start   = "";
        public static string  _amnews_online2_list   = "";
        public static string  _amnews_online2_start   = "";
        public static string  _health_information_technology_list   = "";
        public static string  _health_information_technology_start   = "";
        public static string  _ama_bookstore_list   = "";
        public static string  _ama_bookstore_start   = "";
        public static string  _health_system_reform_insight_list   = "";
        public static string  _health_system_reform_insight_start   = "";
        public static string  _healthy_lifestyles_eletter_list   = "";
        public static string  _healthy_lifestyles_eletter_start   = "";
        public static string  _physician_health_eletter_list   = "";
        public static string  _physician_health_eletter_start   = "";
        public static string  _ama_disparities_eletter_list   = "";
        public static string  _ama_disparities_eletter_start   = "";
        public static string  _virtual_mentor_toc_list   = "";
        public static string  _virtual_mentor_toc_start   = "";
        public static string  _ama_grad_med_educ_list   = "";
        public static string  _ama_grad_med_educ_start   = "";
        public static string  _ama_health_profs_educ_list   = "";
        public static string  _ama_health_profs_educ_start   = "";
        public static string  _cmecppd_list   = "";
        public static string  _theraputic_insights_new_nwsltr_list   = "";
        public static string  _theraputic_insights_new_nwsltr_start   = "";
        public static string  _cmecppd_start = "";

        public static XDocument getRcordQueryDataResults = new XDocument();
        public static string connString = @"Server=DJEFFERSON-T61\LYRIS;Initial Catalog=AMA; Trusted_Connection=Yes;" +   "Connect Timeout=600";
        public static string[] DemographicValues = new string[5];
        
        public static SqlConnection mssql_conn = new SqlConnection();
        public static SqlConnection mssql_conn1 = new SqlConnection();
        public static SqlConnection mssql_conn2 = new SqlConnection();

        public static void RetrieveDemoFilds()
        {
            int site_id = 2010001995;
            int master_mlid = 187096;
            string apiPassword = "am3r1can";

            mssql_conn.ConnectionString = connString;
            mssql_conn1.ConnectionString = connString;
            mssql_conn2.ConnectionString = connString;
             
            try
            {
                mssql_conn.Open();
                mssql_conn1.Open();
                
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following database connection error was returned: ", e.Message));
            }


            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", site_id, "</SITE_ID>",
                "<MLID>", master_mlid, "</MLID>",
                "<DATA type=\"extra\" id=\"password\">", apiPassword, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = System.Web.HttpUtility.UrlEncodeUnicode(datasetInput);

            Log.WriteLine(encodedInput);

            string recordQueryData = String.Concat("https://www.elabs10.com/API/",
                "mailing_list.html?",
                "type=demographic&",
                "activity=Query-all",
                "&input=", encodedInput);

            // Console.WriteLine(recordQueryData);
            Log.WriteLine(recordQueryData);

            // load API Xml into a XDocument

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
                Log.WriteLine(string.Concat("API call for siteid: ", site_id, " mlid: ", master_mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordQueryData);
                Log.WriteLine("The failure reason was: " + getRcordQueryDataResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", site_id, " mlid: ", master_mlid,
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
                    XAttribute name = el.Attribute("type");

                    // this is always the first xml element, so reset the count
                    if ((string)el.Attribute("type") == "name")
                    {
                        DemographicValues[0] = el.Value.ToString();

                    }
                    else
                        if ((string)el.Attribute("type") == "id")
                        {
                            DemographicValues[1] = el.Value.ToString();

                        }
                        else
                            if ((string)el.Attribute("type") == "type")
                            {
                                DemographicValues[2] = el.Value.ToString();

                            }
                    if ((string)el.Attribute("type") == "group")
                    {
                        DemographicValues[3] = el.Value.ToString();

                    }
                    if ((string)el.Attribute("type") == "state")
                    {
                        DemographicValues[4] = el.Value.ToString();

                        InsertDemoValue();
                    }


                }
               
            }

            InsertDynColums();
        }

        public static void InsertDemoValue()
        {

             SqlCommand sqlcmd = new SqlCommand();
             string ws_state = "";

            //string altercmd = String.Concat("alter table demographic_data add column ", DemographicValues[1].Replace(' ', '_'), "varchar(50)");

             if (DemographicValues[4] == "active") {
                  ws_state = "'Y'";}
             else {
                 ws_state = "'F'";
             }

            string insertcmd = String.Concat("insert into demographics_data values ", @"(", 
                                         DemographicValues[1], 
                                         ",'", DemographicValues[0].Replace(' ', '_'), "',",
                                            "'", DemographicValues[2], "',", "'", DemographicValues[3], "'", ",", ws_state, ")");



            try
            {
                sqlcmd.Parameters.Add(new SqlParameter(@"demo_id", DemographicValues[1]));
                sqlcmd.Parameters.Add(new SqlParameter(@"column_name", DemographicValues[0]));
                sqlcmd.Parameters.Add(new SqlParameter(@"data_type", DemographicValues[2]));
                sqlcmd.Parameters.Add(new SqlParameter(@"grouped", DemographicValues[3]));
                sqlcmd.Parameters.Add(new SqlParameter(@"state", ws_state));

                sqlcmd.Connection = mssql_conn;
                sqlcmd.CommandText = insertcmd;
                //sqlcmd.CommandType = CommandType.Text;
                sqlcmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void InsertDynColums()
        {

            
            try
            {
                SqlCommand sqlcmd3 = new SqlCommand();
                sqlcmd3.Connection = mssql_conn1;

                using (SqlCommand sqlcmd2 = new SqlCommand("SELECT * FROM demographics_data", mssql_conn))
                {
                    //
                    // Invoke ExecuteReader method.
                    //
                    SqlDataReader reader = sqlcmd2.ExecuteReader();
                    while (reader.Read())
                    {
                        int demp_id = reader.GetInt32(0);    // demo_id
                        string column_name = reader.GetString(1);  // Name column_name
                        Console.WriteLine("Made it here");
                        //
                        // Write the values read from the database to the screen.
                        //


                        column_name = column_name.Replace('0', 'z');
                        string altercmd = String.Concat("alter table master_list add  ", column_name.Replace('/','_') , " varchar(50)");
                        
                               
                        Console.WriteLine(altercmd);

                        sqlcmd3.CommandText = altercmd;
                        
                        sqlcmd3.ExecuteNonQuery();
                          
                    }
                }


                
            }
            catch (SqlException e) 
            {
                Console.WriteLine(e.ToString());
            }


        }  // END OF PROC -- InsertDynColums


        public static void BuildMSQLRecord(string[] data_values, string IN_FILE, string IN_STATE)
        {

            if (!DetermineIfPresent(data_values[0]))
            {
                PerformInsert(data_values, IN_FILE, IN_STATE);
            }
            else
            {
                PerformUpdate(data_values, IN_FILE, IN_STATE);
            }




        } // END PROC -- BuildMSQLRecord


        public static bool DetermineIfPresent(string IN_EMAIL)
        {
            
            int rows = 0;
            string selectstr = "select email_address from master_list where email_address = " +  "'" + IN_EMAIL + "'";

            mssql_conn2.ConnectionString = connString;



            try
            {

                mssql_conn2.Open();

            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following database connection error was returned: ", e.Message));
            }
            SqlCommand sqlcmd3 = new SqlCommand(selectstr, mssql_conn2);

            sqlcmd3.Parameters.Add(@"email_address", IN_EMAIL);
           
            rows = sqlcmd3.ExecuteNonQuery();

            if (rows > 0)
                return true;
            else
                return false;

        }  // END PROC -- DetermineIfPresent


        public static void PerformUpdate(string[] IN_DATA_VALUES, string IN_FILE, string IN_STATE)
        {

            string updtlist = "";
            string updatestr = String.Concat("UPDATE master_list SET ",
                                                
                                                "ME = ", IN_DATA_VALUES[1], ",",
                                                "Source = ", IN_DATA_VALUES[2], ",",
                                                "SecurID = ", IN_DATA_VALUES[3], ",",
                                                "GLBT = ", IN_DATA_VALUES[4], ",",
                                                "Comment_ = ", IN_DATA_VALUES[5], ",",
                                                "ZMSS = ", IN_DATA_VALUES[6], ",",
                                                "IMG = ", IN_DATA_VALUES[7], ",",
                                                "MAC = ", IN_DATA_VALUES[8], ",",
                                                "SMS = ", IN_DATA_VALUES[9], ",",
                                                "WPC = ", IN_DATA_VALUES[10], ",",
                                                "status = ", IN_DATA_VALUES[11], ",",
                                                "YPS = ", IN_DATA_VALUES[12], ",",
                                                "MSS = ", IN_DATA_VALUES[13], ",",
                                                "JoinForm = ", IN_DATA_VALUES[14], ",",
                                                "NonPhysician = ", IN_DATA_VALUES[15], ",",
                                                "State = ", IN_DATA_VALUES[16], ",",
                                                "MedEdu = ", IN_DATA_VALUES[17], ",",
                                                "Last_Name_ = ", IN_DATA_VALUES[18], ",",
                                                "title_ = ", IN_DATA_VALUES[19], ",",
                                                "SPG = ", IN_DATA_VALUES[20], ",",
                                                "Segment = ", IN_DATA_VALUES[21], ",",
                                                "SpamComplaint = ", IN_DATA_VALUES[22], ",",
                                                "First_Name_ = ", IN_DATA_VALUES[23], ",",
                                                "ProfLifeCycle = ", IN_DATA_VALUES[24], ",",
                                                "CareerSetting = ", IN_DATA_VALUES[25], ",",
                                                "Segment2 = ", IN_DATA_VALUES[26], ",",
                                                "fullname_ ", IN_DATA_VALUES[27]);
                                       

            switch (IN_FILE)
            {
                case "amawire":
                      updtlist =  String.Concat( " ,_amawire_list = " + IN_STATE, ",  _amawire_start = " ,IN_DATA_VALUES[14]);
                      break;
                
                case   "ama_alerts":
                        updtlist =  String.Concat( ", _ama_alerts_list = " + IN_STATE, ",  _ama_alerts_start = " ,IN_DATA_VALUES[14]);
                        break;
                case  "amnews_online":
                        updtlist =  String.Concat( ", _amnews_online_list = " + IN_STATE, ",  _amnews_online_start = " ,IN_DATA_VALUES[14]);
                        break;
                case  "amnews_online2":
                        updtlist =  String.Concat( ", _amnews_online2_list = " + IN_STATE, ",  _amnews_online2_start = " ,IN_DATA_VALUES[14]);
                        break;
                case  "health_information_technology":
                        updtlist =  String.Concat( " _health_information_technology_list = " + IN_STATE, ",  _health_information_technology_start = " ,IN_DATA_VALUES[14]);
                        break;
                case  "ama_bookstore":
                        updtlist =  String.Concat( " ,_ama_bookstore_list = " + IN_STATE, ",  _ama_bookstore_start = " ,IN_DATA_VALUES[14]);
                        break;                                                
                case  "health_system_reform_insight":
                        updtlist =  String.Concat( ",_health_system_reform_insight_list = " + IN_STATE, ",  _health_system_reform_insight_start = " ,IN_DATA_VALUES[14]);
                        break; 
                case  "healthy_lifestyles_eletter":
                        updtlist =  String.Concat( ",_healthy_lifestyles_eletter_list = " + IN_STATE, ",  _healthy_lifestyles_eletter_start = " ,IN_DATA_VALUES[14]);
                        break;                                                
                case  "physician_health_eletter":
                        updtlist =  String.Concat( ",_physician_health_eletter_list = " + IN_STATE, ",  _physician_health_eletter_start = " ,IN_DATA_VALUES[14]);
                        break;                                                 
                                                
                case  "ama_disparities_eletter":
                        updtlist =  String.Concat( ",_ama_disparities_eletter_list = " + IN_STATE, ",  _ama_disparities_eletter_start = " ,IN_DATA_VALUES[14]);
                        break;                                               
                case  "virtual_mentor_toc_list":
                        updtlist =  String.Concat( ",_virtual_mentor_toc_list = " + IN_STATE, ",  _virtual_mentor_toc_start = " ,IN_DATA_VALUES[14]);
                        break;                                                
                 case  "ama_grad_med_educ":
                        updtlist =  String.Concat( ",_ama_grad_med_educ_list = " + IN_STATE, ",  _ama_grad_med_educ_start = " ,IN_DATA_VALUES[14]);
                        break;                                               
                 case  "ama_health_profs_educ":
                        updtlist =  String.Concat( ",_ama_health_profs_educ_list = " + IN_STATE, ",  _ama_health_profs_educ_start = " ,IN_DATA_VALUES[14]);
                        break;                                                 
                 case  "cmecppd_list":
                        updtlist =  String.Concat( ",_cmecppd_list = " + IN_STATE, ",  _cmecppd_list_start = " ,IN_DATA_VALUES[14]);
                        break;
                 case "theraputic_insights_new_nwsltr":
                        updtlist = String.Concat(",_theraputic_insights_new_nwsltr_list = " + IN_STATE, ",  _theraputic_insights_new_nwsltr_start = ", IN_DATA_VALUES[14]);
                        break;     
            } 
                   updatestr +=  updtlist;
        

                  updatestr += String.Concat(" where EMAIL = ", IN_DATA_VALUES[0] ) ;


            Console.WriteLine(updatestr);
            SqlCommand sqlupdatecmd = new SqlCommand(updatestr, mssql_conn2);
            sqlupdatecmd.ExecuteNonQuery();




        } // END OF PROC PerformUpdate



        public static void PerformInsert(string[] IN_DATA_VALUES, string IN_FILE, string IN_STATE)
        {
            string updtlist = "";
            string updatestr = String.Concat("INSERT INTO master_list Values (" , "'", IN_DATA_VALUES[1], "'",  ",",
                                             "'",    IN_DATA_VALUES[2],  "'", ",",
                                             "'", IN_DATA_VALUES[3], "'", ",",
                                            "'", IN_DATA_VALUES[4], "'", ",",
                                            "'", IN_DATA_VALUES[5], "'", ",",
                                            "'", IN_DATA_VALUES[6], "'", ",",
                                            "'", IN_DATA_VALUES[7], "'", ",",
                                            "'", IN_DATA_VALUES[8], "'", ",",
                                            "'", IN_DATA_VALUES[9], "'", ",",
                                            "'", IN_DATA_VALUES[10], "'", ",",
                                            "'", IN_DATA_VALUES[11], "'", ",",
                                            "'", IN_DATA_VALUES[12], "'", ",",
                                            "'", IN_DATA_VALUES[13], "'", ",",
                                            "'", IN_DATA_VALUES[14], "'", ",",
                                            "'", IN_DATA_VALUES[15], "'", ",",
                                            "'", IN_DATA_VALUES[16], "'", ",",
                                            "'", IN_DATA_VALUES[17], "'", ",",
                                            "'", IN_DATA_VALUES[18], "'", ",",
                                            "'", IN_DATA_VALUES[19], "'", ",",
                                            "'", IN_DATA_VALUES[20], "'", ",",
                                            "'", IN_DATA_VALUES[21], "'", ",",
                                            "'", IN_DATA_VALUES[22], "'", ",",
                                            "'", IN_DATA_VALUES[23], "'", ",",
                                            "'", IN_DATA_VALUES[24], "'", ",",
                                            "'", IN_DATA_VALUES[25], "'", ",",
                                            "'", IN_DATA_VALUES[26], "'", ",",
                                            "'", IN_DATA_VALUES[27] , "'");
            Console.WriteLine(updatestr);
            SqlCommand sqlupdatecmd = new SqlCommand(updatestr, mssql_conn2);
            sqlupdatecmd.ExecuteNonQuery();


            updtlist = String.Concat("UPDATE master_list SET ");

            switch (IN_FILE)
            {
                case "amawire":
                    updtlist = String.Concat(" _amawire_list = " + IN_STATE, ",  _amawire_start = ", IN_DATA_VALUES[14]);
                    break;

                case "ama_alerts":
                    updtlist = String.Concat(" _ama_alerts_list = " + IN_STATE, ",  _ama_alerts_start = ", IN_DATA_VALUES[14]);
                    break;
                case "amnews_online":
                    updtlist = String.Concat(" _amnews_online_list = " + IN_STATE, ",  _amnews_online_start = ", IN_DATA_VALUES[14]);
                    break;
                case "amnews_online2":
                    updtlist = String.Concat(" _amnews_online2_list = " + IN_STATE, ",  _amnews_online2_start = ", IN_DATA_VALUES[14]);
                    break;
                case "health_information_technology":
                    updtlist = String.Concat(" _health_information_technology_list = " + IN_STATE, ",  _health_information_technology_start = ", IN_DATA_VALUES[14]);
                    break;
                case "ama_bookstore":
                    updtlist = String.Concat(" _ama_bookstore_list = " + IN_STATE, ",  _ama_bookstore_start = ", IN_DATA_VALUES[14]);
                    break;
                case "health_system_reform_insight":
                    updtlist = String.Concat("_health_system_reform_insight_list = " + IN_STATE, ",  _health_system_reform_insight_start = ", IN_DATA_VALUES[14]);
                    break;
                case "healthy_lifestyles_eletter":
                    updtlist = String.Concat("_healthy_lifestyles_eletter_list = " + IN_STATE, ",  _healthy_lifestyles_eletter_start = ", IN_DATA_VALUES[14]);
                    break;
                case "physician_health_eletter":
                    updtlist = String.Concat("_physician_health_eletter_list = " + IN_STATE, ",  _physician_health_eletter_start = ", IN_DATA_VALUES[14]);
                    break;

                case "ama_disparities_eletter":
                    updtlist = String.Concat("_ama_disparities_eletter_list = " + IN_STATE, ",  _ama_disparities_eletter_start = ", IN_DATA_VALUES[14]);
                    break;
                case "virtual_mentor_toc_list":
                    updtlist = String.Concat("_virtual_mentor_toc_list = " + IN_STATE, ",  _virtual_mentor_toc_start = ", IN_DATA_VALUES[14]);
                    break;
                case "ama_grad_med_educ":
                    updtlist = String.Concat("_ama_grad_med_educ_list = " + IN_STATE, ",  _ama_grad_med_educ_start = ", IN_DATA_VALUES[14]);
                    break;
                case "ama_health_profs_educ":
                    updtlist = String.Concat("_ama_health_profs_educ_list = " + IN_STATE, ",  _ama_health_profs_educ_start = ", IN_DATA_VALUES[14]);
                    break;
                case "cmecppd_list":
                    updtlist = String.Concat("_cmecppd_list = " + IN_STATE, ",  _cmecppd_list_start = ", IN_DATA_VALUES[14]);
                    break;
                case "theraputic_insights_new_nwsltr":
                    updtlist = String.Concat("_theraputic_insights_new_nwsltr_list = " + IN_STATE, ",  _theraputic_insights_new_nwsltr_start = ", IN_DATA_VALUES[14]);
                    break;
            }
            updatestr += updtlist;


            updatestr += String.Concat(" where EMAIL = ", IN_DATA_VALUES[0]);

            Console.WriteLine(updatestr);

            SqlCommand sqlupdatecmd2 = new SqlCommand(updatestr, mssql_conn2);
            sqlupdatecmd2.ExecuteNonQuery();


        } // END OF PROC PerformUpdate

    }
}

