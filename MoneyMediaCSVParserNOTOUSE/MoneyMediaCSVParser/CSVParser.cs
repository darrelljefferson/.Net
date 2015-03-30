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

            string[] elements = new string[14];
            string errormsg = "";
            DataTable filestats = new DataTable();

            filestats.Columns.Add("filename", typeof(string));
            filestats.Columns.Add("filesize", typeof(Int32));
            filestats.Columns.Add("recs", typeof(Int32));
            filestats.Columns.Add("filename", typeof(string));
           
            foreach (string path in Directory.GetFiles(gbl_dir_path, "*." + gbl_extension))
            {

                SQLInserter.CreateTempSortTbl();
                fs = new FileStream(path, FileMode.Open);

                Console.WriteLine("Processing: " + path + " Size is " + fs.Length);
                Log.WriteLine("Processing: " + path + " Size is " + fs.Length);
                try
                {
                    sr = new StreamReader(fs);
                    string inputstr;
                    Boolean b_first_time = true;
                    while ((inputstr = sr.ReadLine()) != null)
                    {
                        if (!b_first_time)
                        {
                            elements = inputstr.Split(new Char [] {'|', ','} ,14,  StringSplitOptions.None);
                     

                                SQLInserter.LoadTmpTbl(elements);
                                

                            
                        } // end first-time if-stmt
                        else
                        {
                            b_first_time = false;
                        }
                        

                    } // end of while 
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
            
                
            }

        }

    }

