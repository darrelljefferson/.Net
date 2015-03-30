using System;
using System.Collections;
using System.IO;


namespace MoneyMediaCSVParser
{

    public class Unzip
    {
        

        public static void UnzipFile(string zippath)
        {


            
            Chilkat.Zip zip = new Chilkat.Zip();
            //bool chilkatsuccess = zip.UnlockComponent("LYRISCSSH_bYK5kMZA7Vna "); does not work
            //bool chilkatsuccess = zip.UnlockComponent("LYRISCHttp_zUmhzUf4XV1i ");
            bool chilkatsuccess = zip.UnlockComponent("Anything for 30 days");

            // To open a password-protected zip, set the password
            // prior to opening the Zip.
            zip.SetPassword("M0n3ymedia");

            foreach ( string zipfilename in Directory.GetFiles(zippath, "*." + @"zip"))
            {

                // This test file has been uploaded to:
                // http://www.example-code.com/testData/passwordProtected.zip
                bool success = zip.OpenZip(zipfilename);
                if (!success)
                {
                    // MessageBox.Show(zip.LastErrorText); need logic for catch error
                    return;
                }

                // Unzip into a relative subdirectory, which is created
                // if it does not yet exist:
                int numFilesUnzipped = zip.Unzip(zippath);
                if (numFilesUnzipped == -1)
                {
                    // MessageBox.Show(zip.LastErrorText); need logic for catch error
                }
            }

        }
    }
}
