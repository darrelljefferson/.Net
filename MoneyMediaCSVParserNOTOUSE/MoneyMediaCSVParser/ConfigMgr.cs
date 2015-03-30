using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace MoneyMediaCSVParser
{
    class ConfigMgr
    {

        public static StreamReader filerow;
        public static Dictionary<string, string>  Init(string[] cli_ini)
        {

            Console.WriteLine("cmd file name " + cli_ini[0]);
            StringBuilder currentline = new StringBuilder();

            try
            {
                 filerow = new StreamReader(cli_ini[0]);
                 // Read in FTP and database authentication from the config file
                 Console.WriteLine(" reading variables from config file ");
            }
            catch (Exception e)
            {
                Log.WriteLine("Failed to find config file " + e.ToString());
                Console.WriteLine("Failed to find config file " + e.ToString());
                Environment.Exit(1);
            }


            Dictionary<string, string> configDic = new Dictionary<string,string>();

            //the first unused readline is to skip over the comment line in the config file
            string cfg_line;
            while ((cfg_line = filerow.ReadLine()) != null)
            {
                Log.WriteLine(cfg_line);
                if (!cfg_line.StartsWith("#") && !cfg_line.StartsWith(" "))
                {
                    Log.WriteLine(cfg_line);
                    string[] cmd_arry;
 
                    cmd_arry = cfg_line.Split('=');



                    switch (cmd_arry[0])
                    {
                        case "DBNAME":
                                
                              
                                configDic.Add("DBNAME", cmd_arry[1]);
                                break;
                            
                        case "HOST":
                               
                                configDic.Add("HOST", cmd_arry[1]);
                               break;
                        case "PORT":

                               configDic.Add("PORT", cmd_arry[1]);
                               break;
                        case "USERID":
                               
                               configDic.Add("USERID", cmd_arry[1]);
                               break;
                        case "PASSWORD":
                               
                               configDic.Add("PASSWORD", cmd_arry[1]);
                                break;
                        case "DIRPATH":
                               
                               configDic.Add("DIRPATH", cmd_arry[1]);
                               break;
                        case "FILENAME":
                               
                               configDic.Add("FILENAME", cmd_arry[1]);
                               break;
                        case "LOGPATH":

                               configDic.Add("LOGDIR", cmd_arry[1]);
                               break;
                        case "ERRPATH":

                               configDic.Add("ERRDIR", cmd_arry[1]);
                               break;
                        case "DUPSPATH":

                               configDic.Add("DUPSDIR", cmd_arry[1]);
                               break;
                        case "EXTENSION":

                               configDic.Add("EXTENSION", cmd_arry[1]);
                               break;                              

                    } // End Case

                }



            }
            filerow.Close();


            //string sqlauth;
            //sqlauth = filerow.ReadLine();
            //sqlauth = filerow.ReadLine();
            //Console.WriteLine(sqlauth + "\n");
            //string ftppath;
            //ftppath = filerow.ReadLine();
            //ftppath = filerow.ReadLine();
            //Console.WriteLine(ftppath + "\n");
            //string ftpuser;
            //ftpuser = filerow.ReadLine();
            //ftpuser = filerow.ReadLine();
            //Console.WriteLine(ftpuser + "\n");
            //string ftppass;
            //ftppass = filerow.ReadLine();
            //ftppass = filerow.ReadLine();
            //Console.WriteLine(ftppass + "\n");
            //string demographics;
            //demographics = filerow.ReadLine();
            //demographics = filerow.ReadLine();
            //Console.WriteLine(demographics + "\n\n");

            return configDic;
        }
    }

}