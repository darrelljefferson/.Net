using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Timers;
using System.Data;
using System.IO;



namespace PHEAALoadTrigger
{
    class Program
    {

        public static string ftpExternalHostName = "ftp.lyris.com";
        public static string ftpInternalHostName = "ftp.corp.lyris.com";
        public static string ftpUserName = "support";
        public static string ftpPassword = "ftpP0w3r!";
        public static string ftpDirectory = "private/pheaa";
        //public static string notifyEmail = "djefferson@lyris.com";
        public static string notifyEmail = "kbrown1@aessuccess.org";




        static void Main(string[] args)
        {
            Log.Init(@"c:\custom\PHEAALoadTriggerQA11Test_log_");

            // sftp information
            string sftpServer = "cmfiles.pheaa.org";
            string sftpUsername = "lyrisssh";
            string sftpPassword = "#487EaTs4";

            // farm info for the upload call for production use
            string siteid = "23394";
            string apiPassword = "87h3jps";

           // string siteid = "32424476";                       // for testing 32424476
           // string apiPassword = "Magnolia01";               // test must remove


             DateTime today = DateTime.Now.AddDays(0);
            // DateTime today = Convert.ToDateTime("2011-07-08");  // test remove
            string todayDate = today.ToString("yyyyMMdd");

            //string TESTTriggerID = "9193";

            string aescorrTriggerID = "8285";
            string aesinstallTriggerID = "8286";
            string aesintTriggerID = "8287";
            string aesintnotTriggerID = "8289";
            string aesrepayTriggerID = "8290";
            string fdcorrTriggerID = "8280";
            string fdinstallTriggerID = "8281";
            string fdintTriggerID = "8282";
            string fdintnotTriggerID = "8283";
            string fdrepayTriggerID = "8284";

            Utility.InitDataTables();


            // Dictionary to hold  list and mlid info
            Dictionary<string, string> listMlidMap = new Dictionary<string, string>()
            {
                
                
                {"ECORR_AESCORR","99505"},
                {"ECORR_AESINSTALL","99510"},
                {"ECORR_AESINT","99513"},
                {"ECORR_AESINTNOT","99514"},
                {"ECORR_AESREPAY","99515"},
                {"ECORR_FLSCORR","99496"},
                {"ECORR_FLSINSTALL","99497"},
                {"ECORR_FLSINT","99498"},
                {"ECORR_FLSINTNOT","99499"},
                {"ECORR_FLSREPAY","99500"},
                {"ECORR_DUPS_AESCORR","99505"},
                {"ECORR_DUPS_AESINSTALL","99510"},
                {"ECORR_DUPS_AESINT","99513"},
                {"ECORR_DUPS_AESINTNOT","99514"},
                {"ECORR_DUPS_AESREPAY","99515"},
                {"ECORR_DUPS_FLSCORR","99496"},
                {"ECORR_DUPS_FLSINSTALL","99497"},
                {"ECORR_DUPS_FLSINT","99498"},
                {"ECORR_DUPS_FLSINTNOT","99499"},
                {"ECORR_DUPS_FLSREPAY","99500"}
                
                
            };

            string localPath = @"c:\custom\pheaa_import\";

            foreach (KeyValuePair<string, string> item in listMlidMap)
            {

                string localFileName = string.Concat(item.Key,
                    "_", todayDate, ".txt");

                string localFilePath = string.Concat(localPath, localFileName);

                string annonymousFtpString = string.Concat(
                    "ftp://", "anonymous:a@", ftpExternalHostName, "/", ftpDirectory,
                    "/", localFileName);
                // string notifyEmail = "bmueller@lyris.com";
                //


                Console.WriteLine(annonymousFtpString);


                SFTP.GetFlatFile(sftpServer, sftpUsername, sftpPassword, localFilePath, localFileName);


                Boolean successUplaod = Utility.SimpleFtpUplaod(localFilePath, localFileName,
                    ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory);
            }


            // ----------------------------- Process files from PHEAA -----------------------------------------

            Log.WriteLine(" Process files from PHEAA");


            foreach (KeyValuePair<string, string> item in listMlidMap)
            {
                string localFileName = string.Concat(item.Key,
                    "_", todayDate, ".txt");

                string localFilePath = string.Concat(localPath, localFileName);


                // iterate through the files and send the messages

 
                if (item.Key == "ECORR_AESCORR")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, aescorrTriggerID, apiPassword, localFilePath, notifyEmail);

                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_AESINSTALL")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, aesinstallTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_AESINT")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, aesintTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_AESINTNOT")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, aesintnotTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_AESREPAY")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data" + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, aesrepayTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }

                }
                else if (item.Key == "ECORR_FLSCORR")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, fdcorrTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }

                }
                else if (item.Key == "ECORR_FLSINSTALL")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, fdinstallTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }

                }
                else if (item.Key == "ECORR_FLSINT")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, fdintTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_FLSINTNOT")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, fdintnotTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }
                else if (item.Key == "ECORR_FLSREPAY")
                {
                    if (File.Exists(localFilePath))
                    {
                        Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        Console.WriteLine("Start Processing " + localFilePath);
                        Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                            item.Value, fdrepayTriggerID, apiPassword, localFilePath, notifyEmail);
                        Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                    }
                    else
                    {
                        Log.WriteLine("This file was not found for processing: " + localFilePath);
                    }
                }   
                    else if (item.Key == "ECORR_DUPS_AESCORR")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, aescorrTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_AESINSTALL")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, aesinstallTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_AESINT")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, aesintTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
;
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_AESINTNOT")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, aesintnotTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_AESREPAY")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, aesrepayTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }

                    }
                    else if (item.Key == "ECORR_DUPS_FLSCORR")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, fdcorrTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }

                    }
                    else if (item.Key == "ECORR_DUPS_FLSINSTALL")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, fdinstallTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }

                    }
                    else if (item.Key == "ECORR_DUPS_FLSINT")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, fdintTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_FLSINTNOT")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, fdintnotTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("End Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }
                    else if (item.Key == "ECORR_DUPS_FLSREPAY")
                    {
                        if (File.Exists(localFilePath))
                        {
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                            Console.WriteLine("Start Processing " + localFilePath);
                            Utility.ParseFileAndSendMessage(ftpInternalHostName, ftpUserName, ftpPassword, ftpDirectory, siteid,
                                item.Value, fdrepayTriggerID, apiPassword, localFilePath, notifyEmail);
                            Log.WriteLine("Start Processing PHEAA Data " + localFilePath);
                        }
                        else
                        {
                            Log.WriteLine("This file was not found for processing: " + localFilePath);
                        }
                    }

         }
                   
                Log.WriteLine("End Processing all PHEAA Files");

            }

        }
    }

   
    

