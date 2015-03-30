using System;
using System.IO;
using System.Data;

namespace MoneyMediaCSVParser
{
    class Log
    {
        private static TextWriter logwriter_;
        private static TextWriter Errwriter;

        private static bool logOpen_;


        public static void Init(string basefilename)
        {
            DateTime dateTime = DateTime.Now;
            logwriter_ = new StreamWriter(String.Concat(basefilename, dateTime.ToString("yyyy-MM-dd-ss"), ".log"));
            logwriter_.WriteLine(String.Concat("\r\n\r\nMoneyMedia Started at ", dateTime.ToString("F")));
            logOpen_ = true;
 
        }

        public static void final()
        {
            DateTime dateTime = DateTime.Now;

            logwriter_.WriteLine(String.Concat("\r\n\r\nMoneyMedia Ended at ", dateTime.ToString("F")));
            logwriter_.Flush();

        }

        public static void WriteLine(string line)
        {
            DateTime dateTime = DateTime.Now;
            logwriter_.Write(String.Concat(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), " - ", line, "\r\n"));
            logwriter_.Flush();
        }

        public static void ReportFileStats(DataTable IN_filelog)
        {

            foreach (DataRow row in IN_filelog.Rows)
            {
                Log.WriteLine((string)row["dt_filename"] + " " + (Int32)row["dt_filesize"] + " " + (Int32)row["dt_recs"] + " " +
                 " Start=> " + (DateTime)row["dt_starttime"] + " End=> " + (DateTime)row["dt_endtime"]);

                Console.WriteLine((string)row["dt_filename"] + " " + (Int32)row["dt_filesize"] + " " + (Int32)row["dt_recs"] + " " +
                 " Start=> " + (DateTime)row["dt_starttime"] + " End=> " + (DateTime)row["dt_endtime"]);

            }
            logwriter_.Flush();

        }
        public static void ErrorInit(string errdata)
        {
            
            DateTime dateTime = DateTime.Now;
            Errwriter = new StreamWriter(String.Concat(errdata, "error_data_" +  dateTime.ToString("yyyy-MM-dd-ss"), ".log"));

        }

        public static void WriteErrData(string line)
        {
       
            Errwriter.WriteLine(line);
            Errwriter.Flush();
        }




        public static void Close()
        {
            if (logOpen_ == false)
            {
                try
                {
                    logwriter_.Close();
                }
                catch (Exception e)
                {
                    Log.WriteLine("Log couldn't close for the following reason " + e.Message);
                }
                logOpen_ = false;
            }
        }
    }
}