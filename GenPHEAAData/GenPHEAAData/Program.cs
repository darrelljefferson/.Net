using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace GenPHEAAData
{
    class Program
    {
        static void Main(string[] args)
        {

            FileStream fs  = new FileStream(@"c:\custom\pheaa_import\TESTFILE3_06262011.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Random rand_ = new Random(200000);


            Dictionary<string, int> emailaddr = new Dictionary<string, int>
            {
                {"darrelljefferson@lyris.com", 5000},
                {"ps_lyris1@lyris.com", 15000},
                {"ps_lyris2@lyris.com", 15000},
                {"karenannmartinez@lyris.com", 10000},
                {"djefferson@lyris.com", 2500},
                {"dj@lyris.com", 150000},
                {"strawvoss1960@lyris.com", 5000},


            };


            foreach (KeyValuePair<string, int> item in emailaddr)
            {
                if (item.Key == "darrelljefferson@lyris.com")
                {
                    Console.WriteLine("Building " + item.Key);
                    for (int i = 1; i < item.Value; i++)
                    {
                        //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                        CreateEmailMember(i + item.Key);
                        sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                    }

                }
                else
                    if (item.Key == "ps_lyris1@lyris.com")
                    {
                        Console.WriteLine("Building " + item.Key);
                        for (int i = 1; i < item.Value; i++)
                        {
                            CreateEmailMember(i + item.Key);
                            //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                            sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                        }

                    }
                    else
                        if (item.Key == "ps_lyris2@lyris.com")
                        {
                        Console.WriteLine("Building " + item.Key);
                        for (int i = 1; i < item.Value; i++)
                        {
                            CreateEmailMember(i + item.Key);
                            //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                            sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                        }

                    }
                else
                if (item.Key == "karenannmartinez@lyris.com")
                {
                    Console.WriteLine("Building " + item.Key);
                    for (int i = 1; i < item.Value; i++)
                    {
                        CreateEmailMember(i + item.Key);
                        //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                        sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                    }

                }
                else
                if (item.Key == "djefferson@lyris.com")
                {
                    Console.WriteLine("Building " + item.Key);
                    for (int i = 1; i < item.Value; i++)
                    {
                        CreateEmailMember(i + item.Key);
                        //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                        sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                    }

                }
                else
                    if (item.Key == "strawvoss1960@lyris.com")
                    {
                        Console.WriteLine("Building " + item.Key);
                        for (int i = 1; i < item.Value; i++)
                        {
                            CreateEmailMember(i + item.Key);
                            //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                            sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                        }

                    }
                    else
                        if (item.Key == "dj@lyris.com")
                        {
                            Console.WriteLine("Building " + item.Key);
                            for (int i = 1; i < item.Value; i++)
                            {
                                CreateEmailMember(i + item.Key);
                                //ARC|CMOD_UID|ACCOUNT#|LETTER_ID|DOC_TYPE|DUE_DATE|JOIN_DATE|EMAIL 
                                sw.WriteLine(String.Concat("Lyris", "|", rand_.Next(), "|", "Acct" + i, "|", rand_.Next(), "|", "Test - 1", "|", "06/26/2012", "|", "06/25/2011", "|", i + item.Key.ToString()));
                            }

                        }
            }


            sw.Close();
            fs.Close();

        }

       public static void CreateEmailMember(string email)
           {

               string mlid = "35197";

               string site_id = "32424476";                       // for testing 32424476
               string password = "Magnolia01"; 

               string datasetInput = String.Concat("<DATASET>",
                  "<SITE_ID>", site_id, "</SITE_ID>",
                  "<MLID>", mlid, "</MLID>",
                  "<DATA type=\"email\" >",email, "</DATA>",
                  "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                  "</DATASET>");



               Console.WriteLine(datasetInput);

               string encodedInput = ReturnHTMLEnocodedString(datasetInput);

               Console.WriteLine(encodedInput);

               string recordUpload = String.Concat("https://www.elabs5.com/API/",            
                   "mailing_list.html?",
                   "type=record&",
                   "activity=add",
                   "&input=", encodedInput);


               Console.WriteLine(recordUpload);


               // load API Xml into a XDocument
               XDocument uploadFile = new XDocument();
               // load the xml from the api call and check for errors
               try
               {
                   uploadFile = XDocument.Load(recordUpload);

               }
               catch (Exception e)
               {
                   Console.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
               }



               // since the api returned back for errors can be either upper or lower case, we have to check which one it is
               // assume upper case and adjust if we find lower case
               string dataset = "DATASET";
               string type = "TYPE";
               string data = "DATA";

               // load the xml as a string
               string checkXML = uploadFile.ToString();

               // now check to see case and re-populate the variables if they are lower case
               if (checkXML.Contains("dataset"))
               {
                   dataset = dataset.ToLower();
                   type = type.ToLower();
                   data = data.ToLower();
               }

               // now check and log if we encouter an error or find no records returned
               if ((string)uploadFile.Element(dataset).Element(type).Value == "error")
               {
                   Console.WriteLine(string.Concat("API call for siteid: ", site_id, " mlid: ", mlid,
                       " failed"));
                   Console.WriteLine("The following api call that failed: \n" + recordUpload);
                   Console.WriteLine("The failure reason was: " + uploadFile.Element(dataset).Element(data).Value);
                   Console.WriteLine("");
               }
               else if ((string)uploadFile.Element(dataset).Element(type).Value == "norecords")
               {
                   Console.WriteLine(string.Concat("API call for siteid: ", site_id, " mlid: ", mlid,
                       " has no records"));
                   Console.WriteLine("The following api call has no records: \n" + recordUpload);
                   Console.WriteLine("");
               }
               // if not errors, then we just log the data results
               else
               {
                   // log the id, just in case we need it later
                 Console.WriteLine(uploadFile.Element(dataset).Element(data).Value);


               }
            }


       public static string ReturnHTMLEnocodedString(string input)
       {

           Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
           // Any code begins the 30-day trial.
           crypt.UnlockComponent("LYRISCCrypt_4NataCGcVVkW ");
           crypt.CryptAlgorithm = "none";
           crypt.EncodingMode = "url";

           return crypt.EncryptStringENC(input);


       }
    }
}
