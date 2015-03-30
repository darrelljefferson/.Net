using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ProcessRunning
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
                { Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id); }


            IPHostEntry host; 
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) 
            { if (ip.AddressFamily.ToString() == "InterNetwork") { localIP = ip.ToString(); } 
            } 
            return localIP;
        }
    }
}
