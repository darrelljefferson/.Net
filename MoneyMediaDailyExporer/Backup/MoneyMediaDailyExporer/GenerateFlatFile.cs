using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace MoneyMediaDailyExporer
{
    class GenerateFlatFile
    {

        public static void ExportOpensToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            /*
             * source query
                select timeclicked_ as Timeclicked_,
                dbo.ipIntToString(IPAddress_) as IPAddress,
                MessageID_ as MailingID,
                mem.EmailAddr_ as [Email Address],
                mem.List_ as [List Name]
                from Clicktracking_ click inner join members_ mem
                on click.memberid_ = mem.memberid_
                where timeclicked_ > getdate() -1
                and urlid_ is null
             */
            string sqlQuery = string.Concat("select ", "timeclicked_ as Timeclicked,",
                "dbo.ipIntToString(IPAddress_) as IPAddress,",
                "MessageID_ as MailingID,",
                "mem.EmailAddr_ as [Email Address],",
                "mem.List_ as [List Name]",
                " from",
                " Clicktracking_ click inner join members_ mem",
                " on click.memberid_ = mem.memberid_",
                " where timeclicked_ between ","'",yesterday," 00:00:00","'",
                " and ","'",yesterday," 23:59:59","'",
                " and urlid_ is null");
                
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
                    
                    DateTime timecliked = sqlDataReader.GetDateTime(0);
                    string ipAddress = sqlDataReader.GetString(1);
                    string messageID = sqlDataReader.GetInt32(2).ToString();
                    string email = sqlDataReader.GetString(3);
                    string list = sqlDataReader.GetString(4);


                    string seperator = "\",\"";
                    strRow = String.Concat("\"", timecliked.ToString("yyyy-MM-dd HH:mm:ss"), seperator,
                        ipAddress, seperator,messageID,seperator,email,seperator,
                        list,"\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat("Report File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }

            
            sqlConnection.Close();
            
        }

        public static void ExportClicksToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            /*
             * source query
                select timeclicked_ as timeclicked,
                url.PrettyName_ as [URL Name],
                messageid_ as Malingid,
                mem.EmailAddr_ as [Email Address],
                mem.List_ as [List Name]
                from clicktracking_ click 
                inner join members_ mem
                on click.memberid_ = mem.memberid_
                inner join urls_ url
                on click.urlid_ = url.urlid_
                where timeclicked_ > getdate() -1
             */
            string sqlQuery = string.Concat("select ", "timeclicked_ as Timeclicked,",
                "url.PrettyName_ as [URL Name],",
                "MessageID_ as MailingID,",
                "mem.EmailAddr_ as [Email Address],",
                "mem.List_ as [List Name]",
                " from",
                " clicktracking_ click inner join members_ mem",
                " on click.memberid_ = mem.memberid_",
                " inner join urls_ url",
                " on click.urlid_ = url.urlid_",
                " where timeclicked_ between ", "'", yesterday, " 00:00:00", "'",
                " and ", "'", yesterday, " 23:59:59", "'");

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

                    DateTime timecliked = sqlDataReader.GetDateTime(0);
                    string urlName = sqlDataReader.GetString(1);
                    string messageID = sqlDataReader.GetInt32(2).ToString();
                    string email = sqlDataReader.GetString(3);
                    string list = sqlDataReader.GetString(4);


                    string seperator = "\",\"";
                    strRow = String.Concat("\"", timecliked.ToString("yyyy-MM-dd HH:mm:ss"), seperator,
                        urlName, seperator, messageID, seperator, email, seperator,
                        list, "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat("Report File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
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

