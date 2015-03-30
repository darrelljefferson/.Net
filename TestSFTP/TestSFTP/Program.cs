using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            Chilkat.SFtp sftp = new Chilkat.SFtp();

            //  Any string automatically begins a fully-functional 30-day trial.
            bool success;

           // success = sftp.UnlockComponent("LYRISCCrypt_4NataCGcVVkW "); did not work 
            success = sftp.UnlockComponent("LYRISCSSH_bYK5kMZA7Vna ");
            

            //success = sftp.UnlockComponent("Anything for 30 days"); did not work

            if (success != true)
            {
                Console.WriteLine(sftp.LastErrorText);
                return;
            }
        }
    }
}
