using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NbcOlympicsWeeklyReporting
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
            Log.WriteLine(String.Concat("Number of Abaondon rows deleted: ", i));
            sqlConnection.Close();
        }

        public static void InsertListData(string connectString)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            try
            {
                sqlConnection.Open();
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
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                        " select mail.list_, count(mail.list_) as count, getdate(), 'sent'",
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
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                        " and cnt.dates > '", today, " 00:00:00'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of unsub records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                        " and cnt.dates > '", today, " 00:00:00'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of subscribe records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                        " and cnt.dates > '", today, " 00:00:00'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of list total records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
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
                        " and cnt.dates > '", today, " 00:00:00'");
                    Log.WriteLine(sqlString);
                    SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                    sqlCommand.CommandTimeout = 600;
                    int i = sqlCommand.ExecuteNonQuery();
                    Log.WriteLine(String.Concat("Number of list sent records updated :", i));
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Command Execution Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
            }
            sqlConnection.Close();
        }

        public static void UpdateWeeklyReporting(string connectString, string lastWeek, string yesterday)
        {
            SqlConnection sqlConnection = new SqlConnection(connectString);
            int result;
            try
            {
                
                sqlConnection.Open();
                // delete the old data
                try
                {
                    SqlCommand deleteCommand = sqlConnection.CreateCommand();
                    deleteCommand.CommandText = "delete from nbcolympics.tmp_weekly_reporting";
                    deleteCommand.CommandTimeout = 600;
                    result = deleteCommand.ExecuteNonQuery();
                    Log.WriteLine("The following rows were deleted: " + result);
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Delete Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                   
                }

                // create the list data
                try
                {
                    SqlCommand insertCommand = sqlConnection.CreateCommand();
                    insertCommand.CommandText = string.Concat("insert into nbcolympics.tmp_weekly_reporting (List_Name)",
                        " select list_",
                        " from members_",
                        " where list_ <> 'n_lm'",
                        " group by list_ having count(*) > 0",
                        " order by list_ asc");
                    insertCommand.CommandTimeout = 600;
                    result = insertCommand.ExecuteNonQuery();
                    Log.WriteLine("The following rows were inserted: " + result);
                }
                catch (Exception e)
                {

                    string errorText = String.Concat("Insert Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }
                
                // update the unsub data
                try
                {
                    SqlCommand updateUnsubs = sqlConnection.CreateCommand();
                    string updateSql = string.Concat("update tmp",
                        " set tmp.unsubs = foo.count",
                        " from nbcolympics.tmp_weekly_reporting tmp",
                        " inner join",
                        " (select list, sum(count) as count",
                        " from nbcolympics.tmp_counts",
                        " where type = 'unsub'",
                        " and cast(dates as datetime) between '",lastWeek," 00:00:00' and '",lastWeek," 23:59:59'",
                        " group by list) foo",
                        " on tmp.List_Name = foo.list");
                    updateUnsubs.CommandTimeout = 600;
                    updateUnsubs.CommandText = updateSql;
                    Log.WriteLine(updateSql);
                    result = updateUnsubs.ExecuteNonQuery();
                    Log.WriteLine("The following unsub rows were updated: " + result);
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Unsub Update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }

                // update the sub data
                try
                {
                    SqlCommand updateSubs = sqlConnection.CreateCommand();
                    string updateSql = string.Concat("update tmp",
                        " set tmp.subscriptions = foo.count",
                        " from nbcolympics.tmp_weekly_reporting tmp",
                        " inner join",
                        " (select list, sum(count) as count",
                        " from nbcolympics.tmp_counts",
                        " where type = 'sub'",
                        " and cast(dates as datetime) between '", lastWeek, " 00:00:00' and '", lastWeek, " 23:59:59'",
                        " group by list) foo",
                        " on tmp.List_Name = foo.list");
                    updateSubs.CommandText = updateSql;
                    updateSubs.CommandTimeout = 600;
                    Log.WriteLine(updateSql);
                    result = updateSubs.ExecuteNonQuery();
                    Log.WriteLine("The following sub rows were updated: " + result);
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("Subscription Update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }

                // update the list totals
                try
                {
                    SqlCommand updateListTotals = sqlConnection.CreateCommand();
                    string updateSql = string.Concat("update tmp",
                        " set tmp.list_total = foo.count",
                        " from nbcolympics.tmp_weekly_reporting tmp",
                        " inner join",
                        " (select list, avg(count) as count",
                        " from nbcolympics.tmp_counts",
                        " where type = 'total'",
                        " and cast(dates as datetime) between '", lastWeek, " 00:00:00' and '", lastWeek, " 23:59:59'",
                        " group by list) foo",
                        " on tmp.List_Name = foo.list");
                    updateListTotals.CommandText = updateSql;
                    Log.WriteLine(updateSql);
                    updateListTotals.CommandTimeout = 600;
                    result = updateListTotals.ExecuteNonQuery();
                    Log.WriteLine("The following list total rows were updated: " + result);
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("List Total Update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }

                // update the list sent
                try
                {
                    SqlCommand updateListSent = sqlConnection.CreateCommand();
                    string updateSql = string.Concat("update tmp",
                        " set tmp.list_sent = foo.count",
                        " from nbcolympics.tmp_weekly_reporting tmp",
                        " inner join",
                        " (select list, sum(count) as count",
                        " from nbcolympics.tmp_counts",
                        " where type = 'sent'",
                        " and cast(dates as datetime) between '", lastWeek, " 00:00:00' and '", lastWeek, " 23:59:59'",
                        " group by list) foo",
                        " on tmp.List_Name = foo.list");
                    updateListSent.CommandText = updateSql;
                    Log.WriteLine(updateSql);
                    updateListSent.CommandTimeout = 600;
                    result = updateListSent.ExecuteNonQuery();
                    Log.WriteLine("The following sent rows were updated: " + result);
                }
                catch (Exception e)
                {
                    string errorText = String.Concat("List Sent Update Command Exception found with the following message: ", e.Message);
                    Log.WriteLine(errorText);
                    Email.CreateErrorMessageBody(errorText);
                }

            }
            catch (Exception e)
            {
                string errorText = String.Concat("Open Connection Exception found with the following message: ", e.Message);
                Log.WriteLine(errorText);
                Email.CreateErrorMessageBody(errorText);
                
            }
        }


    }
}
