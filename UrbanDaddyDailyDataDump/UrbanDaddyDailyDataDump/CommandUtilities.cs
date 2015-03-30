using System;
using System.Diagnostics;

namespace pacsun_coremtrics_synch
{
    class CommandUtilities
    {

        public static void ZipFile(string filename, string newfilename)
        {
            Console.WriteLine(filename);
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\zip.exe";
            process.StartInfo.Arguments = String.Concat(newfilename," ", filename);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }        
        
        public static void UnzipFile(string filename)
        {
            Console.WriteLine(filename);
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\unzip.exe";
            process.StartInfo.Arguments = String.Concat("-j ", filename);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }

        public static void RemoveFile(string filename)
        {
            Console.WriteLine(filename);
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\rm.exe";
            process.StartInfo.Arguments = String.Concat("-f ", filename);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }

        public static void RedirectFile(string awkScript, string filename)
        {
            Console.WriteLine(filename);
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\gawk.exe";
            process.StartInfo.Arguments = String.Concat("-Fc -f ", awkScript, " ", filename);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }
    }

}

