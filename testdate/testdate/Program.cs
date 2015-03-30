using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testdate
{
    class Program
    {
        static void Main(string[] args)
        {
            Chilkat.FileAccess fo = new Chilkat.FileAccess();
            Chilkat.ZipEntry zo = null;


            string localpath = @"c:\temp\";
            Chilkat.Zip zip = new Chilkat.Zip();


            // bool unlocked = zip.UnlockComponent("LYRISCHttp_zUmhzUf4XV1i ");
            bool unlocked = zip.UnlockComponent("Anything for 30 days ");

            // To open a password-protected zip, set the password
            // prior to opening the Zip.



            //fullpathname = String.Concat(localpath, IN_zipfile, ".zip");

            bool success = zip.OpenZip(@"c:\temp\ama.disparities.eletter-BOUNCED.zip");
            if (success)
            {
               // zo = zip.GetEntryByIndex(0);
               // zo.FileName = zip.FileName;
            }

            // if it does not yet exist:
            int numFilesUnzipped = zip.Unzip(@"c:\temp");
            if (numFilesUnzipped == -1)
            {
                Console.WriteLine(zip.LastErrorText);  //need logic for catch error
            }
        }
    }
}