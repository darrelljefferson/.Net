using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace nbcorlandoDailyDataDump
{
    class GenerateFlatFile
    {

        public static void ExportSuccessToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = "select lyr.MailingID as MailingID, lyr.MemberID as MemberID, " +
                "mem.EmailAddr_ as EmailAddr,lyr.FinalAttempt as FinalAttempt, " +
                "lyr.FirstAttempt as FirstAttempt, cast(lyr.SendTry as int) as SendTry, " +
                "lyr.CompletionStatusID as StatusID " +
                "from dbo.lyrCompletedRecips lyr with (nolock) join dbo.members_ mem with (nolock) " +
                "on lyr.MemberID = mem.MemberID_ where lyr.completionstatusid = 300 " +
                "and FinalAttempt is not null and SendTry > 0 " +
                "and lyr.FinalAttempt between \'" + yesterday + " 21:00:00\' and \'" + today + " 21:00:00\'";
            //Log.WriteLine(sqlQuery);
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
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row
                while (sqlDataReader.Read())
                {
                    int mailingID = sqlDataReader.GetInt32(0);
                    int memberID = sqlDataReader.GetInt32(1);
                    string EmailAddr = sqlDataReader.GetString(2);
                    DateTime FinalAttempt = sqlDataReader.GetDateTime(3);
                    DateTime FirstAttempt = sqlDataReader.GetDateTime(4);
                    int SendTry = sqlDataReader.GetInt32(5);
                    int StatusID = sqlDataReader.GetInt32(6);

                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", mailingID.ToString(), seperator, memberID.ToString(), seperator,
                        EmailAddr.ToString(), seperator, FinalAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator,
                        FirstAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator, SendTry.ToString(), seperator,
                        StatusID.ToString(), seperator, StatusID.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat("Success File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }

            
            sqlConnection.Close();
            
        }

        public static void ExportTemporaryRejectionToCSVFile(string connectionString, string fileOut, string today, string tomorrow)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = "select MailingID as MailingID, MemberID as MemberID, " +
                "username + \'@\' + domain  as EmailAddr,FirstAttempt as FirstAttempt, " +
                "NextAttempt as NextAttempt, TransactionLog as Message " +
                "from lyrActiveRecips with (nolock) " +
                "where NextAttempt between \'" + today + " 21:00:00\' and \'" + tomorrow + " 21:00:00\'";
            //Log.WriteLine(sqlQuery);
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
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                int memberid = 0;
                string memberID = "";
                // build the csv row
                while (sqlDataReader.Read())
                {
                    int mailingID = sqlDataReader.GetInt32(0);
                    //int memberID = sqlDataReader.GetInt32(1);
                    if (!(sqlDataReader.IsDBNull(1)))
                    {
                        memberid = sqlDataReader.GetInt32(1);
                        memberID = memberid.ToString();
                    }
                    else
                    {
                        memberID = "";
                    }
               

                    string EmailAddr = sqlDataReader.GetString(2);
                    DateTime FirstAttempt = sqlDataReader.GetDateTime(3);
                    DateTime NextAttempt = sqlDataReader.GetDateTime(4);
                    string TransactionLog = sqlDataReader.GetString(5);

                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", mailingID.ToString(), seperator, memberID, seperator,
                        EmailAddr.ToString(), seperator, FirstAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator,
                        NextAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator, TransactionLog.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat("Temporary Rejection File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();
            
        }

        public static void ExportPermanentRejectionToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = "select lyr.MailingID as MailingID, lyr.MemberID as MemberID, " +
                "mem.EmailAddr_ as EmailAddr,lyr.FinalAttempt as FinalAttempt, lyr.FirstAttempt as FirstAttempt, " +
                "cast(lyr.SendTry as int) as SendTry,lyr.TransactionLog as Message " +
                "from lyrCompletedRecips lyr with (nolock) " +
                "join members_ mem with (nolock) on lyr.MemberID = mem.MemberID_ " +
                "where lyr.completionstatusid <> 300 " +
                "and FinalAttempt is not null and SendTry > 0 " +
                "and FinalAttempt between \'" + yesterday + " 21:00:00\' and \'" + today + " 21:00:00\'";
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
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row
                string Message = "";

                while (sqlDataReader.Read())
                {
                    int MailingID = sqlDataReader.GetInt32(0);
                    int MemberID = sqlDataReader.GetInt32(1);
                    string EmailAddr = sqlDataReader.GetString(2);
                    DateTime FinalAttempt = sqlDataReader.GetDateTime(3);
                    DateTime FirstAttempt = sqlDataReader.GetDateTime(4);
                    int SendTry = sqlDataReader.GetInt32(5);
                    //string Message = sqlDataReader.GetString(6);
                    if (!(sqlDataReader.IsDBNull(6)))
                    {
                        Message = sqlDataReader.GetString(6);
                    }

                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", MailingID.ToString(), seperator, MemberID.ToString(), seperator,
                        EmailAddr.ToString(), seperator, FinalAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator,
                        FirstAttempt.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator, SendTry.ToString(), seperator,
                        Message.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();
                Log.WriteLine(String.Concat("Permanet Rejection File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();
            
        }

        public static void ExportOpensToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = "select click.MessageID_ as MailingID_ ,click.MemberID_ as MemberID_,  " +
                "mem.EmailAddr_ as EmailAddr_ ,dbo.ipIntToString(click.IPAddress_) as IPAddress_, " +
                "click.TimeClicked_ as TimeClicked " +
            "from clicktracking_ click with (nolock) " +
            "join members_ mem with (nolock) on click.memberid_ = mem.memberid_ " +
            "where click.urlid_ is null and click.StreamWebPageName_ is null " +
            "and click.TimeClicked_ between \'" + yesterday + " 21:00:00\' and \'" + today + " 21:00:00\'";
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
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row
                while (sqlDataReader.Read())
                {
                    int MailingID = sqlDataReader.GetInt32(0);
                    int MemberID = sqlDataReader.GetInt32(1);
                    string EmailAddr = sqlDataReader.GetString(2);
                    string IPAddress = sqlDataReader.GetString(3).Trim();
                    DateTime TimeClicked = sqlDataReader.GetDateTime(4);

                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", MailingID.ToString(), seperator, MemberID.ToString(), seperator,
                        EmailAddr.ToString(), seperator, IPAddress.ToString(), seperator,
                        TimeClicked.ToString("yyyy-MM-dd HH:mm:ss.000"), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();
                Log.WriteLine(String.Concat("Opens File Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exepction was thrown: ", e.Message));
            }

            
            sqlConnection.Close();
            
        }

        public static void ExportClickthroughsToCSVFile(string connectionString, string fileOut, string today, string yesterday)
        {
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = "select click.MessageID_ as MailingID, click.MemberID_ as MemberID, " +
                "mem.EmailAddr_ as EmailAddr_, click.TimeClicked_ as TimeClicked, click.UrlID_ as UrlID_, " +
                "url.PrettyName_ as PrettyName, url.UrlText_ as UrlText_ " +
                "from clicktracking_ click with (nolock) " +
                "join members_ mem with (nolock) on click.MemberID_ = mem.MemberID_ " +
                "join urls_ url with (nolock) on click.urlid_ = url.urlid_ " +
                "where click.TimeClicked_ between \'" + yesterday + " 21:00:00\' and \'" + today + " 21:00:00\'";
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
                streamWriter.WriteLine(columnNames(dataTable, "|"));

                int count = 0; //used to keep track of the # of rows

                // build the csv row
                while (sqlDataReader.Read())
                {
                    int MailingID = sqlDataReader.GetInt32(0);
                    int MemberID = sqlDataReader.GetInt32(1);
                    string EmailAddr = sqlDataReader.GetString(2);
                    DateTime TimeClicked = sqlDataReader.GetDateTime(3);
                    int UrlID = sqlDataReader.GetInt32(4);
                    string PrettyName = sqlDataReader.GetString(5);
                    string UrlText = sqlDataReader.GetString(6);

                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", MailingID.ToString(), seperator, MemberID.ToString(), seperator,
                        EmailAddr.ToString(), seperator, TimeClicked.ToString("yyyy-MM-dd HH:mm:ss.000"), seperator,
                        UrlID.ToString(), seperator, PrettyName.ToString(), seperator, UrlText.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();
                Log.WriteLine(String.Concat("Clicktracking File Created with ", count, " records"));
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
