using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data;
using System.Text.RegularExpressions;



namespace MoneyMediaCSVParser
{
    class CSVParser
    {

        public static FileStream fs;
        public static StreamReader sr;

        public static string gbl_dir_path;
        public static string gbl_extension;
        public static string gbl_err_path;
        public static string gbl_log_path;
        public static string gbl_dups_path;

        public static string fullpathfile;

    /*-----------------------------------------------------------------------------------------------------------------
     * 
     * 
     * 
     * -----------------------------------------------------------------------------------------------------------------
     */ 
        public static void Init(string dirpath, string logpath, string errpath, string dupspath, string extension)
        {
            
            gbl_dir_path = dirpath;
            gbl_log_path = logpath;
            gbl_dups_path = dupspath;
            gbl_err_path = errpath;
            gbl_extension = extension;
    
         }
        /*---------------------------------------------------------------- -------------------------------------------------
         * 
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------------
         */ 
        public static void RetreiveCSVFiles() {
            Console.WriteLine("Begin RetreiveCSVFiles()");
            Int32 rec_ct = 0;
            string[] elements = new string[14];
            string errormsg = "";
            DataTable filestats = new DataTable("filestats");
            DateTime starttime = new DateTime();
            DateTime endtime = new DateTime();
            filestats.Columns.Add("dt_filename", typeof(string));
            filestats.Columns.Add("dt_filesize", typeof(Int32));
            filestats.Columns.Add("dt_recs", typeof(Int32));
            filestats.Columns.Add("dt_starttime", typeof(DateTime));
            filestats.Columns.Add("dt_endtime", typeof(DateTime));

          
            foreach (string path in Directory.GetFiles(gbl_dir_path, "*." + gbl_extension))
            {

                //SQLInserter.CreateTempSortTbl();
                fs = new FileStream(path, FileMode.Open);
                starttime = DateTime.Now;
                Console.WriteLine("Processing: " + path + " Size is " + fs.Length);
                Log.WriteLine("Processing: " + path + " Size is " + fs.Length);
                try
                {
                    SQLInserter.CreateTempSortTbl();           // clear temp table per file for processings
                    sr = new StreamReader(fs);
                    string inputstr;
                    Boolean b_first_time = true;
                    while ((inputstr = sr.ReadLine()) != null)
                    {
                        if (!b_first_time)

                        {
                            inputstr = inputstr.Replace("\"", "");
                            elements = inputstr.Split(new Char [] {'|', ','} ,14,  StringSplitOptions.None);
                            rec_ct++;

                                SQLInserter.LoadTmpTbl(elements);      
                                

                            
                        } // end first-time if-stmt
                        else
                        {
                            b_first_time = false;
                        }
                        

                    } // end of while 
                           endtime = DateTime.Now;
                           filestats.Rows.Add(path, fs.Length, rec_ct, starttime, endtime);
                           SQLInserter.MM_Process_ListMgr();
                } // end of try

                catch (Exception e)
                {
                    Console.WriteLine("Failed to open: " + fullpathfile + e.ToString());
                    Console.WriteLine(errormsg);
                        
                    System.Diagnostics.EventLog Elog = new System.Diagnostics.EventLog();
                    Elog.Source = "MoneyMediaCSVParser";
                    Elog.WriteEntry(errormsg);


                    Log.WriteLine("Connection to CSV File failed ");
                    Environment.Exit(9);
                }
                sr.Close();
                fs.Close();
                GC.Collect();

            }
            Log.ReportFileStats(filestats);
            Console.WriteLine("End RetreiveCSVFiles()"); 
                
            }

        }

    }

