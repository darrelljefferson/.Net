using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace ConsoleApplication1
{
    class CommandUtilities
    {
        public static void ZipFile(string zipFileName, string sourceFileName)
        {
            Console.WriteLine(sourceFileName);
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\zip.exe";
            process.StartInfo.Arguments = String.Concat(zipFileName, " ", sourceFileName);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }

    }
}
