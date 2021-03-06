﻿using System;
using System.IO;

namespace ArrowElectronics
{
    class Log
    {
        private static TextWriter logwriter_;

        private static bool logOpen_;


        public static void Init(string basefilename)
        {
            DateTime dateTime = DateTime.Now;
            logwriter_ = new StreamWriter(String.Concat(basefilename, dateTime.ToString("yyyy-MM-dd"), ".txt"));
            logwriter_.WriteLine(String.Concat("\r\n\r\nLog Started at ", dateTime.ToString("F")));
            logOpen_ = true;
        }

        public static void WriteLine(string line)
        {
            DateTime dateTime = DateTime.Now;
            logwriter_.Write(String.Concat(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), " - ", line, "\r\n"));
            logwriter_.Flush();
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