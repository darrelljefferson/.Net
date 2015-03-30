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
        class ReportClass2
        {
            public void Report2(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 2
                // #############################################################

                Console.WriteLine("\n== querying database report 2 ==");

                selectString = "select min(MessageID_) as ID from outmail_ where Created_ > (getdate() - 60)";
                myDataTable = callsql(selectString, sqlauth);
                string oldid = "";
                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    oldid = myDataRow["ID"].ToString();
                    Console.WriteLine("lowest ID from select: " + oldid);
                }



                selectString =
                "DECLARE @tempreporting TABLE "
                + "(messageid_ int,urlid_ int,memberid_ int,total int) "

                + "DECLARE @tempreporting2 TABLE "
                + "(messageid_ int,urlid_ int,total int,totalu int) "

                + "INSERT INTO @tempreporting "
                + "select messageid_, urlid_, memberid_, count(*) as total "
                + "from clicktracking_ with(nolock) "
                + "where urlid_ is not null AND messageid_ > ";

                selectString += oldid;
                selectString +=

                " group by messageid_, urlid_, memberid_ "

                + "INSERT INTO @tempreporting2 "
                + "select messageid_,urlid_, sum(total) as total, count(*) as totalu "
                + "from @tempreporting group by messageid_,urlid_ "

                + "select temp.messageid_,mail.hdrall_, urls.urltext_, mail.created_,temp.total,temp.totalu,report.received,report.opens "
                + "from @tempreporting2 temp "
                + "inner join outmail_ mail on temp.messageid_ = mail.messageid_ "
                + "inner join lyrReportSummaryData report on temp.messageid_ = report.id "
                + "inner join urls_ urls on urls.urlid_ = temp.urlid_ ";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== writing result to file for report 2 ==");

                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    // Write CSV Header
                    sw.WriteLine("Mailing ID,MailingRefCode,URL Text,Date,Month,Day of Week,Time,Unique Click,Gross Clicks,CTR,CTOR");

                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["Created_"].ToString());
                        //Mailing ID
                        currentline.Append(myDataRow["MessageID_"] + ",");
                        //MailingRefCode
                        currentline.Append(findheader(Convert.ToString(myDataRow["HdrAll_"])) + ",");
                        //URL Text
                        currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["urltext_"])) + "\"" + ",");
                        //Date
                        currentline.Append(dt.ToShortDateString() + ",");
                        //Month
                        currentline.Append(dt.ToString("MMMM") + ",");
                        //Day of Week
                        currentline.Append(dt.DayOfWeek.ToString() + ",");
                        //Time
                        currentline.Append(dt.ToShortTimeString() + ",");
                        //Unique Click
                        currentline.Append(myDataRow["totalu"] + ",");
                        //Gross Clicks
                        currentline.Append(myDataRow["total"] + ",");
                        //CTR
                        currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["total"]) / Convert.ToDouble(myDataRow["received"]) * 100) + "%" + ",");
                        //CTOR
                        currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["total"]) / Convert.ToDouble(myDataRow["opens"]) * 100) + "%");

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
                    static private string SanitizeString(string inputstring)
                    {
                        inputstring = Regex.Replace(inputstring, "\f|\r", "");
                        inputstring = Regex.Replace(inputstring, "[\"]", "\"\"");
                        // Return the sanitized string.
                        return inputstring;
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
