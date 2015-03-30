using System;
using System.Collections;
using System.IO;


namespace AMAMergeList
{

    public class Unzip
    {

        public static string fullpathname = "";


        public static void UnzipFile(string IN_zipfile)
        {

             Chilkat.FileAccess fo = new Chilkat.FileAccess();
             Chilkat.ZipEntry zo = null;


            string localpath = @"e:\projects\AMA\";
            Chilkat.Zip zip = new Chilkat.Zip();
            

            // bool unlocked = zip.UnlockComponent("LYRISCHttp_zUmhzUf4XV1i ");
            bool unlocked = zip.UnlockComponent("Anything for 30 days ");

            // To open a password-protected zip, set the password
            // prior to opening the Zip.



            fullpathname = String.Concat(@"e:\projects\AMA\", IN_zipfile, ".zip");
            bool success = zip.OpenZip(fullpathname);
            if (success)
            {
                zo = zip.GetEntryByIndex(0);
                zo.FileName = zip.FileName;
            }
            if (!success)
            {
                // MessageBox.Show(zip.LastErrorText); need logic for catch error
                return;
            }

            // Unzip into a relative subdirectory, which is created
            // if it does not yet exist:
            int numFilesUnzipped = zip.Unzip(localpath);
            if (numFilesUnzipped == -1)
            {
                Console.WriteLine(zip.LastErrorText);  //need logic for catch error
            }





        }
    }
}

   