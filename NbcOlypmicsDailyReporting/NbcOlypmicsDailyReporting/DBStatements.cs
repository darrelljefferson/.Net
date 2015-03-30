using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NbcOlypmicsDailyReporting
{
    class DBStatements
    {
        public static void DeleteDailyReportingData(string connectString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            Log.WriteLine("Begin deleting old reporting Data");
            sqlConnection.Open();
            SqlCommand deleteCommand = new SqlCommand("delete from nbcolympics.tmp_daily_reporting", sqlConnection);
            deleteCommand.CommandTimeout = 600;
            int i = deleteCommand.ExecuteNonQuery();
            Log.WriteLine(String.Concat("Number of daily reporting rows deleted: ", i));
            sqlConnection.Close();
        }

        public static void InsertListData(string connectString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                // add in extra logic to deal with a bad delete statement
                try
                {
                    string sqlString = string.Concat("select count(*) as count",
                    " from nbcolympics.tmp_daily_reporting");
                    SqlCommand selectCommand = new SqlCommand(sqlString, sqlConnection);
                    selectCommand.CommandTimeout = 600;
                    SqlDataReader dataReader = selectCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int count = Convert.ToInt32(dataReader["count"].ToString());
                        if (count > 0)
                        {
                            DBStatements.DeleteDailyReportingData(connectString);
                            Log.WriteLine("Deletion failed, so trying again");
                        }
                    }
                    dataReader.Close();

                }
                catch (Exception e)
                {
                    string errorText = string.Concat("Delete Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }

                try
                {
                    string sqlString = string.Concat("insert into nbcolympics.tmp_daily_reporting (List_Name)",
                        " select list_",
                        " from members_",
                        " where list_ <> 'n_lm'",
                        " group by list_ having count(*) > 0",
                        " order by list_ asc");
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of lists created :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("Insert Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void InsertUnsubData(string connectString, string yesterday)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                // add in extra logic to make sure we don't insert if the records already exist for the previous day
                try
                {
                    string sqlString = string.Concat("insert into nbcolympics.tmp_counts (list, count, dates, type)",
                        " select list_, count(*) as count, getdate(), 'unsub'",
                        " from members_ with (nolock)",
                        " where dateunsub_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                        " group by list_ having count(list_) > 0");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of unsubscribe records inserted :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("Unsub Insert Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }


            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void InsertSubscriptionData(string connectString, string yesterday)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("insert into nbcolympics.tmp_counts (list, count, dates, type)",
                        " select list_, count(*) as count, getdate(), 'sub'",
                        " from members_ with (nolock)",
                        " where datejoined_ between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                        " group by list_ having count(list_) > 0");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of subscribe records inserted :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("Subscription Insert Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void InsertListTotalData(string connectString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("insert into nbcolympics.tmp_counts (list, count, dates, type)",
                        " select list_, count(*) as count, getdate(), 'total'",
                        " from members_ with (nolock)",
                        " group by list_ having count(list_) > 0");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of list total records inserted :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("List Total Insertion Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void InsertSentData(string connectString, string yesterday)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("insert into nbcolympics.tmp_counts (list, count, dates, type)",
                        " select substring(mail.list_,0,32), count(mail.list_) as count, getdate(), 'sent'",
                        " from lyrCompletedRecips recips with (nolock)",
                        " inner join outmail_ mail with (nolock) on recips.mailingid = mail.messageid_",
                        " where recips.FinalAttempt between '", yesterday, " 00:00:00'", " and '", yesterday, " 23:59:59'",
                        " group by mail.list_ having count(mail.list_) > 0");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of sent records inserted :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("List Sent Insertion Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void UpdateUnsubReportingData(string connectString, string today)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("update tmp",
                        " set tmp.unsubs = cnt.count",
                        " from nbcolympics.tmp_daily_reporting tmp",
                        " inner join nbcolympics.tmp_counts cnt on tmp.List_Name = cnt.list",
                        " where cnt.type = 'unsub'",
                        " and cast(cnt.dates as datetime) > '", today, " 00:00:00'");
                        //" and cast(cnt.dates as datetime) between '", today, " 00:00:00' and '",today," 23:59:59'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of unsub records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("unsub update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void UpdateSubscribeReportingData(string connectString, string today)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("update tmp",
                        " set tmp.subscriptions = cnt.count",
                        " from nbcolympics.tmp_daily_reporting tmp",
                        " inner join nbcolympics.tmp_counts cnt on tmp.List_Name = cnt.list",
                        " where cnt.type = 'sub'",
                        " and cast(cnt.dates as datetime) > '", today, " 00:00:00'");
                        //" and cast(cnt.dates as datetime) between '", today, " 00:00:00' and '", today, " 23:59:59'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of subscribe records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("subscription update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }


        public static void UpdateListTotalsReportingData(string connectString, string today)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("update tmp",
                        " set tmp.List_Total = cnt.count",
                        " from nbcolympics.tmp_daily_reporting tmp",
                        " inner join nbcolympics.tmp_counts cnt on tmp.List_Name = cnt.list",
                        " where cnt.type = 'total'",
                        " and cast(cnt.dates as datetime) > '", today, " 00:00:00'");
                        //" and cast(cnt.dates as datetime) between '", today, " 00:00:00' and '", today, " 23:59:59'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of list total records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("list total update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void UpdateListSentReportingData(string connectString, string today)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                try
                {
                    string sqlString = string.Concat("update tmp",
                        " set tmp.List_Sent = cnt.count",
                        " from nbcolympics.tmp_daily_reporting tmp",
                        " inner join nbcolympics.tmp_counts cnt on tmp.List_Name = cnt.list",
                        " where cnt.type = 'sent'",
                        " and cast(cnt.dates as datetime) > '", today, " 00:00:00'");
                        //" and cast(cnt.dates as datetime) between '", today, " 00:00:00' and '", today, " 23:59:59'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of list sent records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("sent update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void CheckForPreviousInserts(string connectString, string type, string yesterday, string today)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
                // add in extra logic to make sure we don't insert if the records already exist for the previous day
                try
                {
                    string selectString = string.Concat("select count(*) as count",
                        " from nbcolympics.tmp_counts",
                        " where type = '",type,"'",
                        " and cast(dates as DateTime) > '", today, " 00:00:00'");
                    SqlCommand selectCommand = new SqlCommand(selectString, sqlConnection);
                    selectCommand.CommandTimeout = 600;
                    SqlDataReader dataReader = selectCommand.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int count = Convert.ToInt32(dataReader["count"].ToString());
                        if (count == 0 && type == "unsub")
                        {
                            InsertUnsubData(connectString, yesterday);
                        }
                        else if (type == "unsub")
                        {
                            Log.WriteLine("Unsub Data already exists for today");
                        }

                        if (count == 0 && type == "sub")
                        {
                            InsertSubscriptionData(connectString, yesterday);

                        }
                        else if (type == "sub")
                        {
                            Log.WriteLine("Subscribe Data already exists for today");
                        }

                        if (count == 0 && type == "total")
                        {
                            InsertListTotalData(connectString);
                        }
                        else if (type == "total")
                        {
                            Log.WriteLine("List Total Data already exists for today");
                        }

                        if (count == 0 && type == "sent")
                        {
                            InsertSentData(connectString, yesterday);
                        }
                        else if (type == "sent")
                        {
                            Log.WriteLine("Sent Data already exists for today");
                        }
                        
                    }
                    dataReader.Close();
                }
                catch (Exception e)
                {
                    string errorText = string.Concat("Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = string.Concat("Open Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

     
    }
}


        


    
   

