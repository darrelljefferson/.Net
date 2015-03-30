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
        class ReportClass3
        {
            public void Report3(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 3
                // #############################################################

                Console.WriteLine("\n== querying database report 3==");
                selectString =
                "DECLARE @tempreporting TABLE "
                + "(domain_ varchar(250),messageid_ int,total int,success int,bounced int,bad_domain int,full_box int,bad_user int,content_block int) "

                + "INSERT INTO @tempreporting "
                + "select member.domain_, mail.messageid_, count(*) as total, "
                + "SUM( CASE WHEN  CompletionStatusID = 5002 THEN 1 ELSE 0 END ) AS success, "
                + "SUM( CASE WHEN  CompletionStatusID <> 5002 THEN 1 ELSE 0 END ) AS bounced, "
                + "SUM( CASE WHEN  CompletionStatusID = 2002 THEN 1 ELSE 0 END ) AS bad_domain, "
                + "SUM( CASE WHEN  CompletionStatusID = 3001 THEN 1 ELSE 0 END ) AS full_box, "
                + "SUM( CASE WHEN  (CompletionStatusID >= 2000 AND CompletionStatusID <= 2999) THEN 1 ELSE 0 END ) AS bad_user, "
                + "SUM( CASE WHEN  (CompletionStatusID >= 8000 AND CompletionStatusID <= 8999) THEN 1 ELSE 0 END ) AS content_block "
                + "from lyrcompletedrecips recips with(nolock) "
                + "inner join members_ member on member.MemberID_=recips.MemberID "
                + "inner join outmail_ mail on mail.messageid_ = recips.mailingID "
                + "where mail.created_ > (getdate()-1) "
                + "group by member.domain_, mail.messageid_ "

                + "select temp.domain_,temp.messageid_,temp.total,temp.success,temp.bounced,temp.bad_domain,temp.full_box,temp.bad_user,temp.content_block,mail.hdrall_,mail.created_ "
                + "from @tempreporting temp "
                + "inner join outmail_ mail on temp.messageid_ = mail.messageid_ ";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== writing result to file for report 3 ==");

                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    // Write CSV Header
                    sw.WriteLine("Mailing ID,MailingRefCode,Date,Month,Day of Week,Time,Domain,Delivered Quantity by Domain,Bounced Quantity by Domain,Email Does Not Exist,Domain Does Not Exist,Mailbox Full,Content Filtered");


                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["created_"].ToString());
                        //Mailing ID
                        currentline.Append(myDataRow["MessageID_"] + ",");
                        //MailingRefCode
                        currentline.Append(findheader(Convert.ToString(myDataRow["HdrAll_"])) + ",");
                        //Date
                        currentline.Append(dt.ToShortDateString() + ",");
                        //Month
                        currentline.Append(dt.ToString("MMMM") + ",");
                        //Day of Week
                        currentline.Append(dt.DayOfWeek.ToString() + ",");
                        //Time
                        currentline.Append(dt.ToShortTimeString() + ",");
                        //Domain
                        currentline.Append(myDataRow["domain_"] + ",");
                        //Delivered Quantity by Domain
                        currentline.Append(myDataRow["success"] + ",");
                        //Bounced Quantity by Domain
                        currentline.Append(myDataRow["bounced"] + ",");
                        //Email Does Not Exist
                        currentline.Append(myDataRow["bad_user"] + ",");
                        //Domain Does Not Exist
                        currentline.Append(myDataRow["bad_domain"] + ",");
                        //Mailbox Full
                        currentline.Append(myDataRow["full_box"] + ",");
                        //Content Filtered
                        currentline.Append(myDataRow["content_block"]);

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
        static private string findheader(string selText)
        {
            int first = selText.IndexOf("MailingRefCode: ");
            if (first != -1)
            {
                //16 characters is how long the header is, this should put the index at the start of the actual value
                first = first + 16;

                //truncate the front part of the header
                selText = selText.Substring(first, selText.Length - first);
                first = selText.IndexOf("\n");
                if (first != -1)
                {
                    //truncate anything left after the header value
                    selText = selText.Substring(0, first);
                }

                return selText;
            }
            else
            {
                //if no such header was found, return information that this was the case
                return "no ID";
            }
        }
    }
}
