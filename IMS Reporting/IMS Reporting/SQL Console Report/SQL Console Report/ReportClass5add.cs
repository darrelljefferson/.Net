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
        class ReportClass5add
        {
            public void Report5add(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 5 addition
                // #############################################################

                fs = new FileStream((reportfilename), FileMode.Append, FileAccess.Write, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    Console.WriteLine("== Select unsubs and add to report 5 ==");

                    selectString =
                        "select member.EmailAddr_,";
                    selectString += demographics;
                    selectString +=
                        "member.DateUnsub_ from members_ member with(nolock) "
                        + "where member.DateUnsub_ > (getdate()-1)";

                    myDataTable = callsql(selectString, sqlauth);

                    Console.WriteLine("== write unsubs to file and add to report 5 ==");

                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["DateUnsub_"].ToString());
                        //Mailing ID
                        currentline.Append("NA-Unsub record" + ",");
                        //MailingRefCode
                        currentline.Append("NA" + ",");
                        //Mailing Name
                        currentline.Append("NA" + ",");
                        //Date
                        currentline.Append(dt.ToShortDateString() + ",");
                        //Month
                        currentline.Append(dt.ToString("MMMM") + ",");
                        //Day of Week
                        currentline.Append(dt.DayOfWeek.ToString() + ",");
                        //Time
                        currentline.Append(dt.ToShortTimeString() + ",");
                        //Reason of failure (does not apply, this is unsubs)
                        currentline.Append("NA" + ",");
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


                    Console.WriteLine("== Select active sends and add to report 5 ==");

                    selectString =
                        "select recips.MailingID,mail.Title_,'hdrall_'=case when charindex('MailingRefCode', mail.hdrall_) > 0 then substring(mail.hdrall_, charindex('MailingRefCode',mail.hdrall_)+16,50) else 'no ID' end,recips.NextAttempt,member.EmailAddr_,member.MemberType_,member.First_Name_,member.Last_Name_,member.List_,member.Domain_,";
                    selectString += demographics;
                    selectString +=
                         "recips.SendTry from lyractiverecips recips with(nolock) "
                        + "inner join outmail_ mail on recips.MailingID = mail.messageid_ "
                        + "inner join members_ member on member.MemberID_=recips.MemberID ";

                    myDataTable = callsql(selectString, sqlauth);

                    Console.WriteLine("== write active sends to file and add to report 5 ==");

                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["NextAttempt"].ToString());
                        //Mailing ID
                        currentline.Append(myDataRow["MailingID"] + ",");
                        //MailingRefCode
                        currentline.Append(newfindheader(Convert.ToString(myDataRow["HdrAll_"])) + ",");
                        //Mailing Name
                        currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["Title_"])) + "\"" + ",");
                        //Date
                        currentline.Append(dt.ToShortDateString() + ",");
                        //Month
                        currentline.Append(dt.ToString("MMMM") + ",");
                        //Day of Week
                        currentline.Append(dt.DayOfWeek.ToString() + ",");
                        //Time
                        currentline.Append(dt.ToShortTimeString() + ",");
                        //Reason of failure (NA)
                        currentline.Append("STILL ACTIVE" + ",");
                        //Member Email
                        currentline.Append(myDataRow["EmailAddr_"] + ",");
                        //Send Try
                        currentline.Append(myDataRow["SendTry"] + ",");

                        for (int i = 0; i < (a - 1); i++)
                        {
                            currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                        }
                        currentline.Append("\"" + myDataRow[demoA[a - 1]] + "\"");


                        stofile = currentline.ToString();
                        sw.WriteLine(stofile);
                        //Console.WriteLine(stofile);
                    }

                }//close stream writer
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
                    static private string SanitizeString(string inputstring)
                    {
                        inputstring = Regex.Replace(inputstring, "\f|\r", "");
                        inputstring = Regex.Replace(inputstring, "[\"]", "\"\"");
                        // Return the sanitized string.
                        return inputstring;
                    }
    }
}
