using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Web;

namespace AMAUpLoadList
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

        Chilkat.Csv csv = new Chilkat.Csv();

        public static FileStream fs = new FileStream(@"e:\Projects\AMA\AMAupLoad.csv", FileMode.Create, FileAccess.Write );
        public static StreamWriter sw = new StreamWriter(fs);


        // create the array to be used to store the "extra" values
        public static string[] IdentityValues = new string[300];

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

           // MSSQL.RetrieveDemoFilds();


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


        public static void ProcessFile(string IN_file, string IN_state, Int32 IN_mlid)
        {


            Chilkat.Csv csv = new Chilkat.Csv();

            string localpath = @"e:\projects\AMA\unzip\";
            string fullpath = String.Concat(localpath, IN_file, ".csv");
            string[,] tbl_col_name = new string[137, 2];
            string[] data_values = new string[138];

            csv.HasColumnNames = true;
            bool success = csv.LoadFile(fullpath);
            int col = 0;
            int row = 0;
            if (success == true)
            {
                //  Display the column names:
                int i;
                string colName;
                int idx;
                for (i = 0; i <= csv.NumColumns - 1; i++)
                {
                    tbl_col_name[i, 0] = csv.GetColumnName(i);
                }


                //  The following line demonstrates to to get the column
                //  index given a column name:
                //idx = csv.GetIndex(colName);


                Console.WriteLine("Records on this files : " + csv.NumRows);

                for (row = 1; col <= csv.NumRows - 1; row++)
                {

                    for (col = 0; col <= csv.NumColumns - 1; col++)
                    {
                      //  tbl_col_name[0, col] = csv.GetCell(row, col) + "\r\n";
                        data_values[col] = csv.GetCell(row, col);

                    } // end for-loop
                            MSSQL.BuildMSQLRecord(data_values, IN_file, IN_state);
                 }  // end for-loop
             }  // end-if




                }
            }
        }  // END OF PROC ---- ProcessFile


     



