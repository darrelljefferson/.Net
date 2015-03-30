using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UdReportSummay
{
    class GenerateCSV
    {

        public static string columnNames(DataTable dtSchemaTable, string delimiter)
        {
            string str1 = "";
            bool flag = delimiter.ToLower() == "tab" == false;
            if (!flag)
            {
                delimiter = "\t";
            }
            int i = 0;
            do
            {
                str1 = String.Concat(str1, dtSchemaTable.Rows[i][0].ToString());
                flag = i < dtSchemaTable.Rows.Count - 1 == false;
                if (!flag)
                {
                    str1 = String.Concat(str1, delimiter);
                }
                i++;
            IL_0070:
                flag = i < dtSchemaTable.Rows.Count;
            }
            while (flag);
            return str1;
        }

        public static void ExportReportSummary(string connectionString, string fileOut, string yesterday)
        {

            DateTime enddt = DateTime.Today;
            DateTime startdt = DateTime.Today.AddDays(-220);
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);



            string sqlQuery = string.Concat("select M.id, M.ListID, M.List ",
                        " from lyrReportSummaryData M  with (nolock) ",
                         "where  unique_opens = 0 and ",
                        " M.List in ( ",
                        "'atlanta', ",
                        "'boston', ",
                        "'chicago',",
                        "'dallas' ,",
                        "'la', ",
                        "'miami', ",
                        "'national',",
                        "'new_york', ",
                        "'sanfrancisco', ",
                        "'perks', ",
                        "'ski', ",
                        "'washington_dc', ",
                        "'jetset', ",
                        "'driven', ",
                        "'las_vegas') ");



            //" (select distinct memberid_ from clicktracking_ where urlid_ is NULL and timeclicked_ > getdate()-220)");
            //  "order by M.list_,O.created_" ) ;

            Console.WriteLine(sqlQuery);

            Log.WriteLine(sqlQuery);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following database connection error was returned: ", e.Message));
            }

            // Creates a SqlDataReader instance to read data from the table.

            sqlCommand.CommandTimeout = 900;

            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                // Retrieves the schema of the table.
                DataTable dataTable = sqlDataReader.GetSchemaTable();

                // Creates the CSV file as a stream, using the given encoding.
                StreamWriter streamWriter = new StreamWriter(fileOut, false, Encoding.ASCII);

                string strRow; // represents a full row

                // write in the column headers
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row

                while (sqlDataReader.Read())
                {


                    Int32 id = 0;
                    Int32 listid = 0;
                    string list = "";



                    id = sqlDataReader.GetInt32(0);
                    listid = sqlDataReader.GetInt32(1);
                    list = sqlDataReader.GetString(2);
 
                    string seperator = "\"|\"";

                    strRow = String.Concat(id, seperator, listid, seperator, list);

                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();
                Console.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));
                Log.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();

        }




        public static void ExportUnSummary(string connectionString, string fileOut, string yesterday)
        {

            DateTime enddt = DateTime.Today;
            DateTime startdt = DateTime.Today.AddDays(-220);
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);



            string sqlQuery = string.Concat("select M.RecipientID, M.MailingID, M.MemberID, FirstAttempt, FinalAttempt, CompletionStatusID ",
                        " from lyrUnSummarizedRecips M  with (nolock) ");

                        // "where M.MemberID in (select A.MemberID_ from Members_  A where M.MemberID = A.memberID_  ");


                        // " and A.List_  in ( ",
                        //"'atlanta', ",
                        //"'boston', ",
                        //"'chicago',",
                        //"'dallas' ,",
                        //"'la', ",
                        //"'miami', ",
                        //"'national',",
                        //"'new_york', ",
                        //"'sanfrancisco', ",
                        //"'perks', ",
                        //"'ski', ",
                        //"'washington_dc', ",
                        //"'jetset', ",
                        //"'driven', ",
                        //"'las_vegas'))" ) ;



            //" (select distinct memberid_ from clicktracking_ where urlid_ is NULL and timeclicked_ > getdate()-220)");
            //  "order by M.list_,O.created_" ) ;

            Console.WriteLine(sqlQuery);

            Log.WriteLine(sqlQuery);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following database connection error was returned: ", e.Message));
            }

            // Creates a SqlDataReader instance to read data from the table.

            sqlCommand.CommandTimeout = 900;

            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                // Retrieves the schema of the table.
                DataTable dataTable = sqlDataReader.GetSchemaTable();

                // Creates the CSV file as a stream, using the given encoding.
                StreamWriter streamWriter = new StreamWriter(fileOut, false, Encoding.ASCII);

                string strRow; // represents a full row

                // write in the column headers
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row

                while (sqlDataReader.Read())
                {



                    decimal  RecipientID = 0;
                    Int32 MailingID = 0;
                    Int32 MemberID = 0;
                    DateTime FirstAttempt ;
                    DateTime FinalAttempt ;
                    Int32 CompletionStatusID = 0;


                    RecipientID = sqlDataReader.GetDecimal(0);
                    MailingID = sqlDataReader.GetInt32(1);
                    MemberID = sqlDataReader.GetInt32(2);
                    FirstAttempt = sqlDataReader.GetDateTime(3);
                    FinalAttempt = sqlDataReader.GetDateTime(4);
                    CompletionStatusID = sqlDataReader.GetInt32(5);

                    string seperator = "\"|\"";

                    strRow = String.Concat(RecipientID, seperator, MailingID, seperator, FirstAttempt, seperator, FinalAttempt, seperator,  CompletionStatusID);

                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();
                Console.WriteLine(" ");
                Console.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));
                Log.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();

        }
    }
}

