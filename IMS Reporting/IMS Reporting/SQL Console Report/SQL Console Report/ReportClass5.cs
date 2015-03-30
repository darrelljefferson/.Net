using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
        class ReportClass5
        {
            public void Report5(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 5
                // #############################################################

                Console.WriteLine("\n== querying database report 5==");
                selectString =
                    "select recips.MailingID,mail.Title_,recips.FinalAttempt,look.Description,member.EmailAddr_,";
                selectString += demographics;
                selectString +=
                    "'hdrall_'=case when charindex('MailingRefCode', mail.hdrall_) > 0 then substring(mail.hdrall_, charindex('MailingRefCode',mail.hdrall_)+16,50) else 'no ID' end"
                    + " from lyrcompletedrecips recips with(nolock) "
                    + "inner join outmail_ mail on recips.MailingID = mail.messageid_ "
                    + "inner join members_ member on member.MemberID_=recips.MemberID "
                    + "inner join lyrLookupCompletionStatus look on recips.CompletionStatusID=look.CompletionStatusID "
                    + "where recips.FinalAttempt > (getdate()-1)";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== writing result to file for report 5 ==");
                fs = new FileStream((reportfilename), FileMode.Append, FileAccess.Write, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    // Write CSV Header
                    sw.WriteLine("Mailing ID,MailingRefCode,Mailing Name,Date,Month,Day of Week,Time,Failure Reason,Member email,Send Try," + cleandemo);

                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["FinalAttempt"].ToString());
                        //Mailing ID
                        currentline.Append(myDataRow["MailingID"] + ",");
                        //MailingRefCode
                        currentline.Append(newfindheader(Convert.ToString(myDataRow["HdrAll_"])) + ",");
                        //Mailing Name
                        currentline.Append("\"" + myDataRow["Title_"] + "\"" + ",");
                        //Date
                        currentline.Append(dt.ToShortDateString() + ",");
                        //Month
                        currentline.Append(dt.ToString("MMMM") + ",");
                        //Day of Week
                        currentline.Append(dt.DayOfWeek.ToString() + ",");
                        //Time
                        currentline.Append(dt.ToShortTimeString() + ",");
                        //Reason of failure
                        currentline.Append("\"" + myDataRow["Description"] + "\"" + ",");
                        //Member Email
                        currentline.Append(myDataRow["EmailAddr_"] + ",");
                        //Send try (completed mailing, does not apply)
                        currentline.Append("NA" + ",");

                        for (int i = 0; i < (a - 1); i++)
                        {
                            currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                        }
                        currentline.Append("\"" + myDataRow[demoA[a - 1]] + "\"");

                        stofile = currentline.ToString();
                        sw.WriteLine(stofile);
                        //Console.WriteLine(stofile);
                    }
                    
                }
                fs.Close();

                return;
            }


        static private DataTable callsql(string selectString, string connectionString)
        {
            SqlConnection mySqlConnection =
            new SqlConnection(connectionString);

            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(selectString, mySqlConnection);
            mySqlDataAdapter.SelectCommand.CommandTimeout = 500;

            DataSet myDataSet = new DataSet();

            mySqlConnection.Open();

            string dataTableName = "Data";
            mySqlDataAdapter.Fill(myDataSet, dataTableName);

            DataTable myDataTable = myDataSet.Tables[dataTableName];

            mySqlConnection.Close();

            return myDataTable;
        }
                    static private string newfindheader(string selText)
        {
                    if (selText != "No ID")
                    {
                        int first = selText.IndexOf("\n");
                        if (first != -1)
                        {
                        //truncate anything left after the header value
                        selText = selText.Substring(0, first);
                        }
                    }

            return selText;
        }
    }
}
