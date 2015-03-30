using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;

namespace ScanPorts
{
    class Program
    {
        static void Main(string[] args)
        {
            var instances = new ManagementClass("Win32_SerialPort").GetInstances();   
            foreach ( ManagementObject port in instances )        {  
                Console.WriteLine("{0}: {1}", port["deviceid"], port["name"]);
                Console.ReadKey();
            }  
        }
    }
}
