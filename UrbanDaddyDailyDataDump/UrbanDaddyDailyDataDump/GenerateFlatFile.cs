using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UrbanDaddyDump
{
    class GenerateFlatFile
    {

        public static void ExportSentToCSVFile(string connectionString, string fileOut, string yesterday)
        {
            DateTime enddt = DateTime.Today;
            DateTime startdt = DateTime.Today.AddDays(-220);


            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            /*
             * source query
                Select RecipientID, BounceMailingID, CompletionStatusID,
                Domain, FirstAttempt, FinalAttempt, MailingID, MemberID,
                SendTry, TransactionLog, UserName
                from lyrCompletedRecips with (nolock)
                where FinalAttempt between 00:00:00 and 23:59:59
            */
            string sqlQuery = string.Concat("select ", "RecipientID,", "BounceMailingID,",
                "CompletionStatusID,", "Domain,", "FirstAttempt,", "FinalAttempt,",
                "MailingID,", "MemberID,", "SendTry,", "TransactionLog,", "UserName",
                " from lyrCompletedRecips with (nolock)",
                " where FinalAttempt between '", startdt, " 00:00:00'", " and '", enddt, " 23:59:59'");
               // " where FinalAttempt between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'");


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

            sqlCommand.CommandTimeout = 1200;

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
                    string bounceMessageID = "";
                    string domain = "";
                    string firstAttempt = "";
                    string finalAttempt = "";
                    string memberID = "";
                    string sendTry = "";
                    string message = "";
                    string username = "";

                    decimal recipientid = sqlDataReader.GetDecimal(0);
                    if (!(sqlDataReader.IsDBNull(1)))
                    {
                        bounceMessageID = sqlDataReader.GetInt32(1).ToString();
                    }
                    int completionStatusID = sqlDataReader.GetInt32(2);
                    if (!(sqlDataReader.IsDBNull(3)))
                    {
                        domain = sqlDataReader.GetString(3);
                        
                        domain = domain.Replace("\\", "\\\\");
                        domain = domain.Replace("\"", "\\\"");
                    }
                    if (!(sqlDataReader.IsDBNull(4)))
                    {
                        DateTime FirstAttempt = sqlDataReader.GetDateTime(4);
                        firstAttempt = FirstAttempt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (!(sqlDataReader.IsDBNull(5)))
                    {
                        DateTime FinalAttempt = sqlDataReader.GetDateTime(5);
                        finalAttempt = FinalAttempt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    int mailingID = sqlDataReader.GetInt32(6);
                    if (!(sqlDataReader.IsDBNull(7)))
                    {
                        memberID = sqlDataReader.GetInt32(7).ToString();
                    }
                    if (!(sqlDataReader.IsDBNull(8)))
                    {
                        sendTry = sqlDataReader.GetByte(8).ToString();
                    }
                    if (!(sqlDataReader.IsDBNull(9)))
                    {
                        message = sqlDataReader.GetString(9);
                        
                        message = message.Replace("\\", "\\\\");
                        message = message.Replace("\"", "\\\"");
                    }
                    if (!(sqlDataReader.IsDBNull(10)))
                    {
                        username = sqlDataReader.GetString(10);
                        
                        username = username.Replace("\\", "\\\\");
                        username = username.Replace("\"", "\\\"");
                    }


                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", recipientid.ToString(), seperator,
                        bounceMessageID.ToString(), seperator, completionStatusID.ToString(),
                        seperator, domain.ToString(), seperator,
                        firstAttempt.ToString(), seperator,
                        finalAttempt.ToString(), seperator, 
                        mailingID.ToString(),seperator,memberID.ToString(),seperator,
                        sendTry.ToString(),seperator,message.ToString(),seperator,
                        username.ToString(), "\"");
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

        public static void ExportMessageToCSVFile(string connectionString, string fileOut, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            /*
             * source query
            select mail.created_ as created, mail.title_ as title, 
            mail.messageid_ as mailingid, mail.list_ as list, sub.name_ as [name]
            from outmail_ mail with (nolock) 
            left outer join subsets_ sub with (nolock)
            on mail.subsetid_ = sub.subsetid_ 
            where mail.created_ between 
            order by messageid_ asc
            */
            string sqlQuery = string.Concat("select ", "mail.created_ as created,",
                "mail.title_ as title,", "mail.messageid_ as mailingid,", " mail.list_ as list,",
                "sub.name_ as [name]",
                " from outmail_ mail with (nolock)",
                " left outer join subsets_ sub with (nolock)",
                " on mail.subsetid_ = sub.subsetid_ ",
                " where created_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                " order by messageid_ asc");


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
                    string title = "";
                    string name = "";
                    string list = "";
                    
                    DateTime created = sqlDataReader.GetDateTime(0);
                    
                    if (!(sqlDataReader.IsDBNull(1)))
                    {
                        title = sqlDataReader.GetString(1);
                        
                        title = title.Replace("\\", "\\\\");
                        title = title.Replace("\"", "\\\"");
                    }                   
                    
                    int mailingID = sqlDataReader.GetInt32(2);

                    if (!(sqlDataReader.IsDBNull(3)))
                    {
                        list = sqlDataReader.GetString(3);
                        
                        list = list.Replace("\\", "\\\\");
                        list = list.Replace("\"", "\\\"");
                    }  
                    
                    if (!(sqlDataReader.IsDBNull(4)))
                    {
                        name = sqlDataReader.GetString(4);
                        
                        name = name.Replace("\\", "\\\\");
                        name = name.Replace("\"", "\\\"");
                    }
                   


                    string seperator = "\"|\"";
                    strRow = String.Concat("\"", created.ToString("yyyy-MM-dd HH:mm:ss"), 
                        seperator, title.ToString(), seperator, mailingID.ToString(),
                        seperator, list.ToString(), seperator,
                        name.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat(fileOut+" Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();

        }

        public static void ExportBounceMessagesToCsv(string connectionString, string fileOut, string yesterday)
        {
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            // source query

            /*
            select body_, messageid_, to_ 
            from inmail_ 
            where charindex('bounce recorded',Transact_) > 0 
            and created_ between 00:00:00 and 23:59:59
            */

            string sqlQuery = string.Concat("Select ", "body_ as 'Body',", "messageid_ as 'InmailID',", "to_ as 'MailingID',",
                "to_ as 'MemberID'",
                " from inmail_ with (nolock)",
                " where charindex('bounce recorded',Transact_) > 0",
                " and created_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'");

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
                    string body = "";

                    if (!(sqlDataReader.IsDBNull(0)))
                    {
                        body = sqlDataReader.GetString(0);

                        // replace " with \" in the string
                        body = body.Replace("\\", "\\\\");
                        body = body.Replace("\"", "\\\"");
                    }
                    
                    
                    
                    string messageID = sqlDataReader.GetInt32(1).ToString();
                    
                    // we now need to deal with pulling out the bounce memberid and mailingid
                    // so we create a regex object and then do the grouping to find it

                    // first create regex pattern
                    Regex match = new Regex(@"^bounce-(\d+)-(\d+)@");
                    // now pull in the bounce message id
                    string mailingID = sqlDataReader.GetString(2);
                    //mailingID = isMatch.Match(mailingID).Value;
                    // declare a match object to match it
                    MatchCollection isMatch = match.Matches(mailingID);
                    // and then pull out the group info to find the memberid and mailingid's
                    string memberid = isMatch[0].Groups[2].Value;
                    memberid = memberid.Replace("\"", "\\\"");
                    memberid = memberid.Replace("\\", "\\\\");
                    
                    mailingID = isMatch[0].Groups[1].Value;
                    
                    mailingID = mailingID.Replace("\\", "\\\\");
                    mailingID = mailingID.Replace("\"", "\\\"");

                    string seperator = "\"|\"";

                    strRow = String.Concat("\"", body, seperator, messageID, seperator,
                        mailingID,seperator,memberid,"\"");
                        
                    streamWriter.WriteLine(strRow);
                    
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));

                
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();

        }

        public static void ExportOpensToCSVFile(string connectionString, string fileOut, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            //Brian 20091111 - Making change to remove uniqueness constraint

            /*
            old source query
              
            select click.clickid_ as 'Unique Identifier',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            click.MessageID_ as 'MessageID_',
            click.TimeClicked_ as 'TimeClicked_',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.List_ as 'List_',
            mem.SubType_ as 'SubType_',
            mem.UserID_ as 'UserID_',
            mem.Text_Only as 'Text_Only'
            from clicktracking_ click with (nolock)
            -- inner join to subquery for unique opens
            inner join
            (select max(clickid_) as clickid,messageid_ as messageid, memberid_ as memberid, ipaddress_ as ipaddress
            from clicktracking_ with (nolock)
            where timeclicked_ > getdate() -1
            and urlid_ is null
            group by messageid_, memberid_, ipaddress_) foo
            on click.clickid_ = foo.clickid
            and click.messageid_ = foo.messageid
            and click.memberid_ = foo.memberid
            and click.ipaddress_ = foo.ipaddress
            --iner join to members for member information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            order by clickid_ asc
           
            
             * new source query
             * 
             * select click.clickid_ as 'Unique Identifier',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            click.MessageID_ as 'MessageID_',
            click.TimeClicked_ as 'TimeClicked_',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.List_ as 'List_',
            mem.SubType_ as 'SubType_',
            mem.UserID_ as 'UserID_',
            mem.Text_Only as 'Text_Only'
            from clicktracking_ click with (nolock)
            --iner join to members for member information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            where timeclicked_ > getdate() -1
            and urlid_ is null
            order by clickid_ asc
          
            */

            //Brian 20091111 - Making change to remove uniqueness constraint

            string sqlQuery = string.Concat("select ", "click.clickid_ as 'Unique Identifier',",
                "click.clickid_ as 'ClickID_',", "click.MemberID_ as 'MemberID_',", "click.MessageID_ as 'MessageID_',",
                "click.TimeClicked_ as 'TimeClicked_',", "mem.domain_ as 'Domain_',", " mem.EmailAddr_ as 'Email Address',",
                " dbo.ipIntToString(click.IPAddress_) as 'IP',", "mem.List_ as 'List_',", "mem.SubType_ as 'SubType_',", 
                " mem.UserID_ as 'UserID_',","mem.Text_Only as 'Text_Only'",
                " from clicktracking_ click with (nolock)",
                "\n--iner join to members for member information\n",
                " inner join members_ mem with (nolock)",
                " on click.memberid_ = mem.memberid_",
                " where timeclicked_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                " and urlid_ is null",
                " order by clickid_ asc");


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
                    string userid = "";
                    string textOnly = "";

                    int uniqueID = sqlDataReader.GetInt32(0);
                    int clickID = sqlDataReader.GetInt32(1);
                    int memberID = sqlDataReader.GetInt32(2);
                    int messageID = sqlDataReader.GetInt32(3);
                    DateTime timeClicked = sqlDataReader.GetDateTime(4);
                    string domain = sqlDataReader.GetString(5);
                    domain = domain.Replace("\"", "\\\"");
                    domain = domain.Replace("\\", "\\\\");
                    string emailAddress = sqlDataReader.GetString(6);
                    emailAddress = emailAddress.Replace("\"", "\\\"");
                    emailAddress = emailAddress.Replace("\\", "\\\\");
                    string ip = sqlDataReader.GetString(7);
                    ip = ip.Replace("\"", "\\\"");
                    ip = ip.Replace("\\", "\\\\");
                    string list = sqlDataReader.GetString(8);
                    list = list.Replace("\"", "\\\"");
                    list = list.Replace("\\", "\\\\");
                    string subtype = sqlDataReader.GetString(9);
                    subtype = subtype.Replace("\"", "\\\"");
                    subtype = subtype.Replace("\\", "\\\\");
                    if (!(sqlDataReader.IsDBNull(10)))
                    {
                        userid = sqlDataReader.GetString(10);
                        
                        userid = userid.Replace("\\", "\\\\");
                        userid = userid.Replace("\"", "\\\"");
                    }
                    if (!(sqlDataReader.IsDBNull(11)))
                    {
                        textOnly = sqlDataReader.GetString(11);
                        
                        textOnly = textOnly.Replace("\\", "\\\\");
                        textOnly = textOnly.Replace("\"", "\\\"");
                    }

                    



                    string seperator = "\"|\"";
             
                    strRow = String.Concat("\"",uniqueID.ToString(),seperator,clickID.ToString(),
                        seperator,memberID.ToString(),seperator,messageID.ToString(),seperator,
                        timeClicked.ToString("yyyy-MM-dd HH:mm:ss"),seperator,domain.ToString(),
                        seperator,emailAddress.ToString(),seperator,ip.ToString(),seperator,
                        list.ToString(),seperator,subtype.ToString(),seperator,userid.ToString(),
                        seperator, textOnly.ToString(), "\"");
                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat(fileOut + " Created with ", count, " records"));
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following exception was thrown: ", e.Message));
            }


            sqlConnection.Close();

        }


        public static void ExportClicksToCSVFile(string connectionString, string fileOut, string yesterday)
        {

            DateTime enddt = DateTime.Today;
            DateTime startdt = DateTime.Today.AddDays(-220);
            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Brian 20091111 - Making change to remove uniqueness constraint

            //Old Query
            /*
            source query
            select click.clickid_ as 'Unique Identifier',
            click.TimeClicked_ as 'Time',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            mem.UserID_ as 'UserID_',
            click.MessageID_ as 'MessageID_',
            mem.List_ as 'List',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            url.urltext_ as 'URL',
            mem.Text_Only as 'Text_Only',
            mail.Title_ as 'Mailing Name'
            from clicktracking_ click with (nolock)
            -- inner join to subquery unique clicks
            inner join
            (select max(clickid_) as clickid,messageid_ as messageid, memberid_ as memberid, ipaddress_ as ipaddress
            from clicktracking_ with (nolock)
            where timeclicked_ > getdate() -1
            and urlid_ is not null
            group by messageid_, memberid_, ipaddress_) foo
            on click.clickid_ = foo.clickid
            and click.messageid_ = foo.messageid
            and click.memberid_ = foo.memberid
            and click.ipaddress_ = foo.ipaddress
            --inner join to urls_ for url information
            inner join urls_ url with (nolock) on click.urlid_ = url.urlid_
            --inner join to members_ for members information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            --inner join to urls_ for url text information
            inner join urls_ url with (nolock)
            on click.urlid_ = url.urlid_
            --inner join to outmail_ for title_ information
            inner join outmail_ mail with (nolock)
            on click.messageid_ = mail.messageid_
            order by clickid_ asc
            */

            //new query
            /*
            source query
            select click.clickid_ as 'Unique Identifier',
            click.TimeClicked_ as 'Time',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            mem.UserID_ as 'UserID_',
            click.MessageID_ as 'MessageID_',
            mem.List_ as 'List',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            url.urltext_ as 'URL',
            mem.Text_Only as 'Text_Only',
            mail.Title_ as 'Mailing Name'
            from clicktracking_ click with (nolock)
            --inner join to urls_ for url information
            inner join urls_ url with (nolock) on click.urlid_ = url.urlid_
            --inner join to members_ for members information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            --inner join to urls_ for url text information
            inner join urls_ url with (nolock)
            on click.urlid_ = url.urlid_
            --inner join to outmail_ for title_ information
            inner join outmail_ mail with (nolock)
            on click.messageid_ = mail.messageid_
            where timeclicked_ > getdate() -1
            and urlid_ is not null
            order by clickid_ asc
            */


            //Brian 20091111 - Making change to remove uniqueness constraint

            /*
            string sqlQuery = string.Concat("select ", "click.clickid_ as 'Unique Identifier',",
                "click.TimeClicked_ as 'Time',", "click.clickid_ as 'ClickID_',", "click.MemberID_ as 'MemberID_',",
                "mem.UserID_ as 'UserID_',", "click.MessageID_ as 'MessageID_',", "mem.List_ as 'List',",
                " dbo.ipIntToString(click.IPAddress_) as 'IP',", "mem.domain_ as 'Domain_',", "mem.EmailAddr_ as 'Email Address',",
                "url.urltext_ as 'URL',", "mem.Text_Only as 'Text_Only',","mail.Title_ as 'Mailing Name'",
                " from clicktracking_ click with (nolock)",
                "\n--inner join to urls_ for url information\n",
                " inner join urls_ url with (nolock)",
                " on click.urlid_ = url.urlid_",
                "\n--iner join to members for member information\n",
                " inner join members_ mem with (nolock)",
                " on click.memberid_ = mem.memberid_",
                "\n--inner join to outmail_ for title_ information\n",
                " inner join outmail_ mail with (nolock)",
                " on click.messageid_ = mail.messageid_",
                " where timeclicked_ between '", startdt, "' and '", enddt, " '",
                " and click.urlid_ is not null",
                " order by clickid_ asc");
            */

            string sqlQuery = string.Concat( "select M.emailaddr_, M.list_, M.Membertype_, O.messageid_,O.created_,O.subject_, M.domain_ ",
                        " from members_ M ",
                        "join clicktracking_ C with (nolock) on M.memberid_=C.memberid_ ", 
                        "join outmail_ O with (nolock) on C.messageid_=O.messageid_ ",
                        "join lyrcompletedrecips R with (nolock) on O.messageid_=R.mailingid and M.memberid_=R.memberid ",
                        "where O.type_='list'  ",
                        // "where O.type_='list' and  R.completionstatusid in (5001,5002,5003,5004) ",
                        "and ", 
                        "M.membertype_='normal' and M.domain_ in ('gmail.com','hotmail.com','msn.com','live.com') and M.List_ in ( ",
                        "'atlanta', ",
                        "'boston', ", 
                        "'chicago',",
                        "'dallas' ,", 
                        "'la', ", 
                        "'miami', ",
                        "'national'," , 
                        "'new_york', ", 
                        "'sanfrancisco', ", 
                        "'perks', ",
                        "'ski', ", 
                        "'washington_dc', ", 
                        "'jetset', ",
                        "'driven', ", 
                        "'las_vegas') ",
                        " and M.memberid_  in ",
                        "(select distinct memberid_ from clicktracking_ where timeclicked_ >= getdate()-220)");
                        
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
                    string userid = "";
                    string textOnly = "";
                    string name = "";

                    string emailaddr = "";
                    string lists_ = "";
                    string membertype = "";
                    int messageid = 0;
                    DateTime created_dt ;
                    string subject = "";
                    string domain = "";

                    emailaddr = sqlDataReader.GetString(0);
                    lists_ = sqlDataReader.GetString(1);
                    membertype = sqlDataReader.GetString(2);
                    messageid = sqlDataReader.GetInt32(3);
                    created_dt = sqlDataReader.GetDateTime(4);
                    subject = sqlDataReader.GetString(5);
                    domain = sqlDataReader.GetString(6);


    /*
                    int uniqueID = sqlDataReader.GetInt32(0);
                    DateTime timeClicked = sqlDataReader.GetDateTime(1);
                    int clickID = sqlDataReader.GetInt32(2);
                    int memberID = sqlDataReader.GetInt32(3);
                    if (!(sqlDataReader.IsDBNull(4)))
                    {
                        userid = sqlDataReader.GetString(4);
                        
                        userid = userid.Replace("\\", "\\\\");
                        userid = userid.Replace("\"", "\\\"");
                    }
                    int messageID = sqlDataReader.GetInt32(5);
                    string list = sqlDataReader.GetString(6);
                    list = list.Replace("\"", "\\\"");
                    list = list.Replace("\\", "\\\\");
                    string ip = sqlDataReader.GetString(7);
                    ip = ip.Replace("\"", "\\\"");
                    ip = ip.Replace("\\", "\\\\");
                    string domain = sqlDataReader.GetString(8);
                    domain = domain.Replace("\"", "\\\"");
                    domain = domain.Replace("\\", "\\\\");
                    string emailAddress = sqlDataReader.GetString(9);
                    emailAddress = emailAddress.Replace("\"", "\\\"");
                    emailAddress = emailAddress.Replace("\\", "\\\\");
                    string url = sqlDataReader.GetString(10);
                    if (!(sqlDataReader.IsDBNull(11)))
                    {
                        textOnly = sqlDataReader.GetString(11);
                        
                        textOnly = textOnly.Replace("\\", "\\\\");
                        textOnly = textOnly.Replace("\"", "\\\"");
                    }
                    if (!(sqlDataReader.IsDBNull(12)))
                    {
                        name = sqlDataReader.GetString(12);
                        
                        name = name.Replace("\\", "\\\\");
                        name = name.Replace("\"", "\\\"");
                    }
                    
*/
                    string seperator = "\"|\"";

                    strRow = String.Concat(emailaddr, seperator, lists_, seperator, membertype, 
                        seperator, messageid, seperator, created_dt, seperator, subject, seperator, domain);

                    /*
                    strRow = String.Concat("\"", uniqueID.ToString(), seperator, timeClicked.ToString("yyyy-MM-dd HH:mm:ss"),
                        seperator,clickID.ToString(),seperator, memberID.ToString(),seperator, userid.ToString(),
                        seperator, messageID.ToString(),seperator,list.ToString(),seperator, ip.ToString(),
                        seperator, domain.ToString(),seperator, emailAddress.ToString(),seperator,url.ToString(),
                        seperator, textOnly.ToString(),seperator,name.ToString(), "\"");
                    */
                        
                       
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


        public static void ExportMembersToCSVFile(string connectionString, string fileOut, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);


            /*
            source query
            select memberid_ as 'Unique Identifier',
            list_ as 'List Name',
            'Date Complained' = case 
            when cast(substring(convert(varchar(50),Additional_),40,46) as int) > 0 then (select created_ from outmail_ with (nolock) where messageid_ = substring(convert(varchar(50),Additional_),40,46))
            end,
            DateJoined_,
            DateHeld_,
            DateUnsub_,
            Domain_,
            EmailAddr_,
            MemberID_,
            MemberType_,
            NumAppNeed_,
            NumBounces_,
            ReadsHtml_,
            SubType_,
            UnsubMessageID_,
            UserID_,
            UserNameLC_,
            First_Name,
            Last_Name,
            dedicated,
            Text_Only,
            User_Status,
            num_favorites,
            editions,
            notactive,
            nukitchen,
            Invites,
            mail_format,
            address_edition_id 
            from members_ with (nolock)
            where list_ <> 'n_lm'
            */
            string sqlQuery = string.Concat("select ", "memberid_ as 'Unique Identifier',",
                "list_ as 'List Name',", 
                "'Date Complained' = case",
                " when cast(substring(convert(varchar(50),Additional_),40,46) as int) > 0 ",
                " then (select created_ from outmail_ with (nolock)",
                " where messageid_ = substring(convert(varchar(50),Additional_),40,46))",
                " end,",
                "'Complaint MailingID' = case",
                " when cast(substring(convert(varchar(50),Additional_),40,46) as int) > 0 ",
                " then substring(convert(varchar(50),Additional_),40,46)",
                " end,",
                "DateJoined_,", "DateHeld_,","DateUnsub_,","Domain_,","EmailAddr_,",
                "MemberID_,","MemberType_,","NumAppNeed_,","NumBounces_,","ReadsHtml_,","SubType_,",
                "UnsubMessageID_,","UserID_,","UserNameLC_,","First_Name,","Last_Name,","dedicated,",
                "Text_Only,","User_Status,","num_favorites,","editions,","notactive,","nukitchen,",
                "Invites,","mail_format,","address_edition_id",
                " from members_ with (nolock)",
                " where list_ <> 'n_lm'",
                " order by memberid_ asc");
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
                    string complaint = "";
                    string complaintID = "";
                    string dateJoined = "";
                    string dateHeld = "";
                    string dateUnsub = "";
                    string unsubid = "";
                    string userid = "";
                    string firstName = "";
                    string lastName = "";
                    string dedicated = "";
                    string textOnly = "";
                    string userStatus = "";
                    string numFavorites = "";
                    string editions = "";
                    string notactive = "";
                    string nukitchen = "";
                    string invites = "";
                    string mailFormat = "";
                    string addressEditionID = ""; 


                    int uniqueID = sqlDataReader.GetInt32(0);
                    string list = sqlDataReader.GetString(1);
                    
                    if (!(sqlDataReader.IsDBNull(2)))
                    {
                        DateTime Complaint = sqlDataReader.GetDateTime(2);
                        complaint = Complaint.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (!(sqlDataReader.IsDBNull(3)))
                    {
                        complaintID = sqlDataReader.GetString(3);
                        
                        complaintID = complaintID.Replace("\\", "\\\\");
                        complaintID = complaintID.Replace("\"", "\\\"");
                    }
                    
                    if (!(sqlDataReader.IsDBNull(4)))
                    {
                        DateTime joined = sqlDataReader.GetDateTime(4);
                        dateJoined = joined.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (!(sqlDataReader.IsDBNull(5)))
                    {
                        DateTime held = sqlDataReader.GetDateTime(5);
                        dateHeld = held.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    if (!(sqlDataReader.IsDBNull(6)))
                    {
                        DateTime unsubDate = sqlDataReader.GetDateTime(6);
                        dateUnsub = unsubDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    string domain = sqlDataReader.GetString(7);
                    
                    domain = domain.Replace("\\", "\\\\");
                    domain = domain.Replace("\"", "\\\"");
                    string email = sqlDataReader.GetString(8);
                    
                    email = email.Replace("\\", "\\\\");
                    email = email.Replace("\"", "\\\"");
                    int memberID = sqlDataReader.GetInt32(9);
                    string membertype = sqlDataReader.GetString(10);
                    
                    membertype = membertype.Replace("\\", "\\\\");
                    membertype = membertype.Replace("\"", "\\\"");
                    short NumAppNeed = sqlDataReader.GetInt16(11);
                    short NumBounces = sqlDataReader.GetInt16(12);
                    string readsHtml = sqlDataReader.GetString(13);
                    
                    readsHtml = readsHtml.Replace("\\", "\\\\");
                    readsHtml = readsHtml.Replace("\"", "\\\"");
                    string subtype = sqlDataReader.GetString(14);
                    
                    subtype = subtype.Replace("\\", "\\\\");
                    subtype = subtype.Replace("\"", "\\\"");

                    if (!(sqlDataReader.IsDBNull(15)))
                    {
                        unsubid = sqlDataReader.GetInt32(15).ToString();
                        
                        unsubid = unsubid.Replace("\\", "\\\\");
                        unsubid = unsubid.Replace("\"", "\\\"");
                        
                    }

                    if (!(sqlDataReader.IsDBNull(16)))
                    {
                        userid = sqlDataReader.GetString(16);
                        
                        userid = userid.Replace("\\", "\\\\");
                        userid = userid.Replace("\"", "\\\"");
                    }

                    string username = sqlDataReader.GetString(17);
                    
                    username = username.Replace("\\", "\\\\");
                    username = username.Replace("\"", "\\\"");

                    if (!(sqlDataReader.IsDBNull(18)))
                    {
                        firstName = sqlDataReader.GetString(18);
                        
                        firstName = firstName.Replace("\\", "\\\\");
                        firstName = firstName.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(19)))
                    {
                        lastName = sqlDataReader.GetString(19);
                        
                        lastName = lastName.Replace("\\", "\\\\");
                        lastName = lastName.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(20)))
                    {
                        dedicated = sqlDataReader.GetString(20);
                        
                        dedicated = dedicated.Replace("\\", "\\\\");
                        dedicated = dedicated.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(21)))
                    {
                        textOnly = sqlDataReader.GetString(21);
                        
                        textOnly = textOnly.Replace("\\", "\\\\");
                        textOnly = textOnly.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(22)))
                    {
                        int UserStatus = sqlDataReader.GetInt32(22);
                        userStatus = UserStatus.ToString();
                        
                        userStatus = userStatus.Replace("\\", "\\\\");
                        userStatus = userStatus.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(23)))
                    {
                        numFavorites = sqlDataReader.GetInt32(23).ToString();
                        
                        numFavorites = numFavorites.Replace("\\", "\\\\");
                        numFavorites = numFavorites.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(24)))
                    {
                        editions = sqlDataReader.GetString(24);
                        
                        editions = editions.Replace("\\", "\\\\");
                        editions = editions.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(25)))
                    {
                        notactive = sqlDataReader.GetInt32(25).ToString();
                        
                        notactive = notactive.Replace("\\", "\\\\");
                        notactive = notactive.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(26)))
                    {
                        nukitchen = sqlDataReader.GetInt32(26).ToString();
                        
                        nukitchen = nukitchen.Replace("\\", "\\\\");
                        nukitchen = nukitchen.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(27)))
                    {
                        invites = sqlDataReader.GetInt32(27).ToString();
                        
                        invites = invites.Replace("\\", "\\\\");
                        invites = invites.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(28)))
                    {
                        mailFormat = sqlDataReader.GetInt32(28).ToString();
                       
                        mailFormat = mailFormat.Replace("\\", "\\\\");
                        mailFormat = mailFormat.Replace("\"", "\\\"");
                    }

                    if (!(sqlDataReader.IsDBNull(29)))
                    {
                        addressEditionID = sqlDataReader.GetInt32(29).ToString();
                        
                        addressEditionID = addressEditionID.Replace("\\", "\\\\");
                        addressEditionID = addressEditionID.Replace("\"", "\\\"");
                    }




                    string seperator = "\"|\"";

                    strRow = String.Concat("\"", uniqueID.ToString(), seperator, list.ToString(), seperator,
                                            complaint.ToString(), seperator, complaintID,seperator,
                                            dateJoined.ToString(), seperator, dateHeld.ToString(), seperator,
                                            dateUnsub.ToString(), seperator, domain.ToString(), seperator, email.ToString(), seperator,
                                            memberID.ToString(), seperator, membertype.ToString(), seperator, NumAppNeed.ToString(), seperator,
                                            NumBounces.ToString(), seperator, readsHtml.ToString(), seperator, subtype.ToString(), seperator,
                                            unsubid.ToString(), seperator, userid.ToString(), seperator, username.ToString(), seperator,
                                            firstName.ToString(), seperator, lastName.ToString(), seperator, dedicated.ToString(), seperator,
                                            textOnly.ToString(), seperator, userStatus.ToString(), seperator, numFavorites.ToString(), seperator,
                                            editions.ToString(), seperator, notactive.ToString(), seperator, nukitchen.ToString(), seperator,
                                            invites.ToString(), seperator, mailFormat.ToString(), seperator, addressEditionID.ToString(), "\"");             


                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

                Log.WriteLine(String.Concat(fileOut+" Created with ", count, " records"));
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

        public static void NewExportClicksToCSVFile(string connectionString, string fileOut, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Brian 20091111 - Making change to remove uniqueness constraint

            //Old Query
            /*
            source query
            select click.clickid_ as 'Unique Identifier',
            click.TimeClicked_ as 'Time',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            mem.UserID_ as 'UserID_',
            click.MessageID_ as 'MessageID_',
            mem.List_ as 'List',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            url.urltext_ as 'URL',
            mem.Text_Only as 'Text_Only',
            mail.Title_ as 'Mailing Name'
            from clicktracking_ click with (nolock)
            -- inner join to subquery unique clicks
            inner join
            (select max(clickid_) as clickid,messageid_ as messageid, memberid_ as memberid, ipaddress_ as ipaddress
            from clicktracking_ with (nolock)
            where timeclicked_ > getdate() -1
            and urlid_ is not null
            group by messageid_, memberid_, ipaddress_) foo
            on click.clickid_ = foo.clickid
            and click.messageid_ = foo.messageid
            and click.memberid_ = foo.memberid
            and click.ipaddress_ = foo.ipaddress
            --inner join to urls_ for url information
            inner join urls_ url with (nolock) on click.urlid_ = url.urlid_
            --inner join to members_ for members information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            --inner join to urls_ for url text information
            inner join urls_ url with (nolock)
            on click.urlid_ = url.urlid_
            --inner join to outmail_ for title_ information
            inner join outmail_ mail with (nolock)
            on click.messageid_ = mail.messageid_
            order by clickid_ asc
            */

            //new query
            /*
            source query
            select click.clickid_ as 'Unique Identifier',
            click.TimeClicked_ as 'Time',
            click.clickid_ as 'ClickID_',
            click.MemberID_ as 'MemberID_',
            mem.UserID_ as 'UserID_',
            click.MessageID_ as 'MessageID_',
            mem.List_ as 'List',
            dbo.ipIntToString(click.IPAddress_) as 'IP',
            mem.domain_ as 'Domain_',
            mem.EmailAddr_ as 'Email Address',
            url.urltext_ as 'URL',
            mem.Text_Only as 'Text_Only',
            mail.Title_ as 'Mailing Name'
            from clicktracking_ click with (nolock)
            --inner join to urls_ for url information
            inner join urls_ url with (nolock) on click.urlid_ = url.urlid_
            --inner join to members_ for members information
            inner join members_ mem with (nolock)
            on click.memberid_ = mem.memberid_
            --inner join to urls_ for url text information
            inner join urls_ url with (nolock)
            on click.urlid_ = url.urlid_
            --inner join to outmail_ for title_ information
            inner join outmail_ mail with (nolock)
            on click.messageid_ = mail.messageid_
            where timeclicked_ > getdate() -1
            and urlid_ is not null
            order by clickid_ asc
            */


            //Brian 20091111 - Making change to remove uniqueness constraint

            string sqlQuery = string.Concat("select ", "click.clickid_ as 'Unique Identifier',",
                "click.TimeClicked_ as 'Time',", "click.clickid_ as 'ClickID_',", "click.MemberID_ as 'MemberID_',",
                "mem.UserID_ as 'UserID_',", "click.MessageID_ as 'MessageID_',", "mem.List_ as 'List',",
                " dbo.ipIntToString(click.IPAddress_) as 'IP',", "mem.domain_ as 'Domain_',", "mem.EmailAddr_ as 'Email Address',",
                "url.urltext_ as 'URL',", "mem.Text_Only as 'Text_Only'",
                " from clicktracking_ click with (nolock)",
                "\n--inner join to urls_ for url information\n",
                " inner join urls_ url with (nolock)",
                " on click.urlid_ = url.urlid_",
                "\n--iner join to members for member information\n",
                " inner join members_ mem with (nolock)",
                " on click.memberid_ = mem.memberid_",
                " where timeclicked_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                " and click.urlid_ is not null",
                " order by clickid_ asc");


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
                    string userid = "";
                    string textOnly = "";
                   


                    int uniqueID = sqlDataReader.GetInt32(0);
                    DateTime timeClicked = sqlDataReader.GetDateTime(1);
                    int clickID = sqlDataReader.GetInt32(2);
                    int memberID = sqlDataReader.GetInt32(3);
                    if (!(sqlDataReader.IsDBNull(4)))
                    {
                        userid = sqlDataReader.GetString(4);
                        
                        userid = userid.Replace("\\", "\\\\");
                        userid = userid.Replace("\"", "\\\"");
                    }
                    int messageID = sqlDataReader.GetInt32(5);
                    string list = sqlDataReader.GetString(6);
                    
                    list = list.Replace("\\", "\\\\");
                    list = list.Replace("\"", "\\\"");
                    string ip = sqlDataReader.GetString(7);
                    
                    ip = ip.Replace("\\", "\\\\");
                    ip = ip.Replace("\"", "\\\"");
                    string domain = sqlDataReader.GetString(8);
                    
                    domain = domain.Replace("\\", "\\\\");
                    domain = domain.Replace("\"", "\\\"");
                    string emailAddress = sqlDataReader.GetString(9);
                    
                    emailAddress = emailAddress.Replace("\\", "\\\\");
                    emailAddress = emailAddress.Replace("\"", "\\\"");
                    string url = sqlDataReader.GetString(10);
                    if (!(sqlDataReader.IsDBNull(11)))
                    {
                        textOnly = sqlDataReader.GetString(11);
                        
                        textOnly = textOnly.Replace("\\", "\\\\");
                        textOnly = textOnly.Replace("\"", "\\\"");
                    }


                    string seperator = "\"|\"";

                    strRow = String.Concat("\"", uniqueID.ToString(), seperator, timeClicked.ToString("yyyy-MM-dd HH:mm:ss"),
                        seperator, clickID.ToString(), seperator, memberID.ToString(), seperator, userid.ToString(),
                        seperator, messageID.ToString(), seperator, list.ToString(), seperator, ip.ToString(),
                        seperator, domain.ToString(), seperator, emailAddress.ToString(), seperator, url.ToString(),
                        seperator, textOnly.ToString(), "\"");



                    streamWriter.WriteLine(strRow);
                    count++;
                }

                streamWriter.Close();

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
