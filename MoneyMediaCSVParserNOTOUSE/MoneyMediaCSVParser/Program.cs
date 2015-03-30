using System;
using System.Collections.Generic;
using System.Text;




namespace MoneyMediaCSVParser
{
    class Program


    {

                
        static void Main(string[] args)
        {
            Log.Init("MoneyMdediaCSVImporter_");


            if (args.Length != 1) {

                Console.WriteLine("No config file present");
                Log.WriteLine("No config file present");
                try
                {
                    Console.WriteLine("Press any key to Terminate");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                catch (Exception e)
                { Log.WriteLine(e.ToString());}
            }

 /* -------------------------------------------------------------------------------------------------------------
  * 
  *  Read and build configuration dictionary 
  * -------------------------------------------------------------------------------------------------------------
  */
            lmapiSoap.lmapi = new lmapiSoap.lmapi();




           Dictionary<string, string>  ConfigurationDic = ConfigMgr.Init( args);

           CSVParser.Init(ConfigurationDic["DIRPATH"], ConfigurationDic["LOGDIR"], ConfigurationDic["ERRDIR"], 
               ConfigurationDic["DUPSDIR"],  ConfigurationDic["EXTENSION"]);

           Log.ErrorInit(ConfigurationDic["ERRDIR"]);

          // SQLInserter.Connect(ConfigurationDic["HOST"], ConfigurationDic["DBNAME"], ConfigurationDic["USERID"], ConfigurationDic["PASSWORD"]);
          
                       
           CSVParser.RetreiveCSVFiles();

           Log.final();
           
        }
    }
}
