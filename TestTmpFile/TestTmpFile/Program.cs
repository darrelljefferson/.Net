using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestTmpFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = Guid.NewGuid().ToString() + ".txt";
            Console.WriteLine(fileName);
        }
    }
}
