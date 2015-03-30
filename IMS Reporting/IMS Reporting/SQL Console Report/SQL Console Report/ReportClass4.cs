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
        class ReportClass4
        {
            public void Report4(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 4
                // #############################################################

                Console.WriteLine("\n== querying database report 4 ==");
                selectString =
                "select click.messageid_,mail.hdrall_,click.TimeClicked_,click.IpAddress_,urls.urltext_,click.memberid_,";
                selectString += demographics;
                selectString +=
                "member.EmailAddr_ from clicktracking_ click with(nolock) "
                + "inner join outmail_ mail on click.messageid_ = mail.messageid_  "
                + "inner join urls_ urls on urls.urlid_ = click.urlid_ "
                + "inner join members_ member on member.MemberID_=click.MemberID_ "
                + "where click.urlid_ is not null AND TimeClicked_ > (getdate()-1) ";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== writing result to file for report 4 ==");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report4 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    // Write CSV Header
                    sw.WriteLine("Mailing ID,MailingRefCode,Date,Month,Day of Week,Time,IP Address,URL Text,Email Address,Member ID," + cleandemo);


                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["TimeClicked_"].ToString());
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

                        //IP Address
                        //IP is saved as an integer, convert this to readable 0.0.0.0 format
                        IPAddress ipAddress = new IPAddress(BitConverter.GetBytes(Convert.ToInt32(myDataRow["IpAddress_"])));
                        //this result is backwards, so pull out the bytes and print the reverse order
                        byte[] byteIP = ipAddress.GetAddressBytes();
                        currentline.Append("\"" + byteIP[3] + "." + byteIP[2] + "." + byteIP[1] + "." + byteIP[0] + "\"" + ",");

                        //URL Text
                        currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["urltext_"])) + "\"" + ",");
                        //Email Address
                        currentline.Append(myDataRow["EmailAddr_"] + ",");
                        //Member ID
                        currentline.Append(myDataRow["memberid_"] + ",");

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
