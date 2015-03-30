using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PheaaDailyExport
{
    class XmlParser
    {

        public static string GetCSVDownloadLink()
        {

        }
        
        
        /// <summary>
        /// utility method to return a html encoded string
        /// </summary>
        /// <param name="input">html string</param>
        /// <returns>html encoded string</returns>
        public static string ReturnHTMLEnocodedString(string input)
        {
            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            // Any code begins the 30-day trial.
            crypt.UnlockComponent("30-day-trial");
            crypt.CryptAlgorithm = "none";
            crypt.EncodingMode = "url";
            return crypt.EncryptStringENC(input);


        }
    }
}
