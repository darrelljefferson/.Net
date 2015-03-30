using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PheaaDailyExport
{
    class Program
    {
        static void Main(string[] args)
        {
            string logFile = @"C:\lm_custom\PheaaDailyExportLogFile_";
            try
            {
                Log.Init(logFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Concat("Log open failed: ", e.ToString()));
                return;
            }




        }
    }
}
