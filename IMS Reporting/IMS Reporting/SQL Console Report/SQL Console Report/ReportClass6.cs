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
        class ReportClass6
        {
            public void Report6(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
            {
                StringBuilder currentline = new StringBuilder();
                string selectString;
                System.Text.StringBuilder res = new System.Text.StringBuilder();
                DateTime dt;
                string stofile;

                DataTable myDataTable;
                FileStream fs;


                // #############################################################
                // Report 6
                // #############################################################

                Console.WriteLine("\n== querying database report 6==");
                selectString =
                "select click.messageid_,click.TimeClicked_,click.IpAddress_,member.EmailAddr_,mail.Title_,mail.List_,"
                + "'hdrall_'=case when charindex('MailingRefCode', mail.hdrall_) > 0 then substring(mail.hdrall_, charindex('MailingRefCode',mail.hdrall_)+16,50) else 'no ID' end "
                + "from clicktracking_ click with(nolock) "
                + "inner join members_ member on member.MemberID_=click.MemberID_ "
                + "inner join outmail_ mail on click.messageid_ = mail.messageid_ "
                + "where click.urlid_ is null AND TimeClicked_ > (getdate()-1) ";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== writing result to file for report 6 ==");

                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs))
                {//open stream writer

                    // Write CSV Header
                    sw.WriteLine("Email Address,MailingID,MailingRefCode,Mailing Name,List,IP Address,Open Date");

                    foreach (DataRow myDataRow in myDataTable.Rows)
                    {
                        currentline = new StringBuilder();
                        dt = Convert.ToDateTime(myDataRow["TimeClicked_"].ToString());

                        //Email Address
                        currentline.Append(myDataRow["EmailAddr_"] + ",");

                        //Mailing ID
                        currentline.Append(myDataRow["MessageID_"] + ",");

                        //MailingRefCode
                        currentline.Append(newfindheader(Convert.ToString(myDataRow["HdrAll_"])) + ",");

                        //Mailing Name
                        currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["Title_"])) + "\"" + ",");

                        //List
                        currentline.Append(myDataRow["List_"] + ",");

                        //IP Address
                        //IP is saved as an integer, convert this to readable 0.0.0.0 format
                        IPAddress ipAddress = new IPAddress(BitConverter.GetBytes(Convert.ToInt32(myDataRow["IpAddress_"])));
                        //this result is backwards, so pull out the bytes and print the reverse order
                        byte[] byteIP = ipAddress.GetAddressBytes();
                        currentline.Append("\"" + byteIP[3] + "." + byteIP[2] + "." + byteIP[1] + "." + byteIP[0] + "\"" + ",");

                        //Date
                        currentline.Append(myDataRow["TimeClicked_"]);

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
