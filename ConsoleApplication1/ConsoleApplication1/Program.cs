using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            /*
            // moneymedia config
            //Log.Init(@"c:\lm_custom\MoneyMediaDailyExport_log"); */

            string connString = "Data Source=db13.hosting.lyris.net;Initial Catalog=moneymedia;User ID=moneymedia;" +
                "Password=money060607;Connect Timeout=600";

            
            // moneymedia 2 config
            // Log.Init(@"c:\lm_custom\MoneyMedia2DailyExport_log");
            //string connString = "Data Source=db18.hosting.lyris.net;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
            //    "Password=m0ney$$$!!11;Connect Timeout=600";
            connString = "Data Source=10.11.4.18;Initial Catalog=moneymedia2;User ID=moneymedia2;" +
                "Password=m0ney$$$!!11;Connect Timeout=600";
            Console.Out.WriteLine("Hello World");
            SqlConnection sqlConnection = new SqlConnection(connString);

            String sqlQuery = string.Concat("select count(*) ",
                " from",
             " members_");

            Console.Out.WriteLine(sqlQuery);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Failed to connect", e.Message);
              //  Log.WriteLine(String.Concat("The following database connection error was returned: ", e.Message));
            }

            // Creates a SqlDataReader instance to read data from the table.

            sqlCommand.CommandTimeout = 600;

            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                // Retrieves the schema of the table.
                DataTable dataTable = sqlDataReader.GetSchemaTable();

                // Creates the CSV file as a stream, using the given encoding.
                //StreamWriter streamWriter = new StreamWriter(fileOut, false, Encoding.ASCII);

 

                // write in the column headers
                //streamWriter.WriteLine(columnNames(dataTable, ","));

   

                while (sqlDataReader.Read())
                {

                    string rowcount = sqlDataReader.GetInt32(0).ToString();
                    Console.Out.WriteLine(rowcount);

                }


               // Log.WriteLine(String.Concat("Report File Created with ", count, " records"));
            }
            catch (Exception e)

            {
                Console.Out.WriteLine("failure ", e.Message);
               // Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();





        }
    }
}
