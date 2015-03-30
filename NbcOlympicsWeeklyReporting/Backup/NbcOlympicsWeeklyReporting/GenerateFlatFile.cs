using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace NbcOlympicsWeeklyReporting
{
    class GenerateFlatFile
    {

        public static void ExportReportToCSVFile(string connectionString, string fileOut)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string sqlQuery = string.Concat("select List_Name, Unsubs, Subscriptions, List_Total, List_sent ",
                " from nbcolympics.tmp_weekly_reporting");

            Log.WriteLine(sqlQuery);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                string errorText = String.Concat("The following database connection error was returned: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }

            // Creates a SqlDataReader instance to read data from the table.

            sqlCommand.CommandTimeout = 600;

            try
            {
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                // Retrieves the schema of the table.
                DataTable dataTable = sqlDataReader.GetSchemaTable();

                // Creates the CSV file as a stream, using the given encoding.
                StreamWriter streamWriter = new StreamWriter(fileOut, false, Encoding.ASCII);

                string strRow; // represents a full row

                // write in the column headers
                streamWriter.WriteLine(columnNames(dataTable, ","));

                int count = 0; //used to keep track of the # of rows

                // build the csv row


                while (sqlDataReader.Read())
                {
                    string name = sqlDataReader.GetString(0);
                    string unsubs = sqlDataReader.GetString(1);
                    string subs = sqlDataReader.GetString(2);
                    string totals = sqlDataReader.GetString(3);
                    string sent = sqlDataReader.GetString(4);

                    string seperator = "\",\"";
                    strRow = String.Concat("\"", name, seperator, unsubs,seperator,
                        subs, seperator,totals, seperator,sent,"\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat("Report File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                string errorText = string.Concat("The following exception was thrown: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }


            sqlConnection.Close();

        }

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


    }
}
