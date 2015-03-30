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
            class ReportClass1
            {
                public void Report1(string demographics, string sqlauth, string[] demoA, string cleandemo, string reportfilename, int a)
                {
                    StringBuilder currentline = new StringBuilder();
                    string selectString;
                    System.Text.StringBuilder res = new System.Text.StringBuilder();
                    DateTime dt;
                    string stofile;

                    DataTable myDataTable;
                    FileStream fs;
                    double membertotal;


                    // #############################################################
                    // Report 1
                    // #############################################################

                    Console.WriteLine("== querying database report 1 ==");
                    //Pull in the SQL result that will be used for constructing the report
                    selectString = "select "
                    + "outmail_.HdrAll_, outmail_.MessageID_, outmail_.Created_, outmail_.Subject_, outmail_.Title_,lyrReportSummaryData.OutmailTo,lyrReportSummaryData.mailed, lyrReportSummaryData.successes, lyrReportSummaryData.InmailBodySize, lyrReportSummaryData.unique_opens, lyrReportSummaryData.opens, lyrReportSummaryData.unique_clicks, lyrReportSummaryData.total_clicks, lyrReportSummaryData.unsubs, lyrReportSummaryData.complaints, lyrReportSummaryData.bounces "
                    + "from outmail_, lyrReportSummaryData with(nolock) "
                    + "where outmail_.MessageID_ = lyrReportSummaryData.id AND lyrReportSummaryData.Created > (getdate()-90) "
                    + "order by outmail_.MessageID_ desc";
                    myDataTable = callsql(selectString, sqlauth);

                    Console.WriteLine("== writing result to file report 1 ==");
                    //write the file into the directory where the application is running from

                    fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                    using (StreamWriter sw = new StreamWriter(fs))
                    {//open stream writer

                        // Write CSV Header for the file
                        sw.WriteLine("Mailing ID,MailingRefCode,Date,Month,Day of Week,Time,Subject Line,Mailing Title,Final List Quantity,Delivered List Quantity,Bytes,Total kb,Bandwidth Total MB,Opens (unique),Opens (gross),Open Rate,Clicks (unique),Clicks (gross),CTR,CTOR,Bounces,% of Delivered,Unsubscribes,Complaints");

                        // For each line in the SQL result, write a comma delimited row into the CSV file
                        foreach (DataRow myDataRow in myDataTable.Rows)
                        {
                            currentline = new StringBuilder();
                            //ahead of time, read the date value string into a date time object
                            dt = Convert.ToDateTime(myDataRow["Created_"].ToString());

                            if (isNumeric(Regex.Replace((Convert.ToString(myDataRow["OutmailTo"])), "[(]|[ recipients)]", ""), System.Globalization.NumberStyles.Integer))
                            {
                                membertotal = Convert.ToDouble(Regex.Replace((Convert.ToString(myDataRow["OutmailTo"])), "[(]|[ recipients)]", ""));
                            }
                            else
                            {
                                membertotal = 1;
                            }
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
                            //Subject Line
                            currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["Subject_"])) + "\"" + ",");
                            //Mailing Title
                            currentline.Append("\"" + SanitizeString(Convert.ToString(myDataRow["Title_"])) + "\"" + ",");
                            //Final List Quantity
                            currentline.Append((Convert.ToString(membertotal)) + ",");
                            //Delivered List Quantity
                            currentline.Append(myDataRow["successes"] + ",");
                            //Bytes
                            currentline.Append(myDataRow["InmailBodySize"] + ",");
                            //Total kb
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["InmailBodySize"]) * Convert.ToDouble(myDataRow["mailed"]) / 1024) + ",");
                            //Bandwidth Total MB
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["InmailBodySize"]) * Convert.ToDouble(myDataRow["mailed"]) / 1048576) + ",");
                            //Opens (unique)
                            currentline.Append(myDataRow["unique_opens"] + ",");
                            //Opens (gross)
                            currentline.Append(myDataRow["opens"] + ",");
                            //Open Rate
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["opens"]) / Convert.ToDouble(myDataRow["mailed"])) + ",");
                            //Clicks (unique)
                            currentline.Append(myDataRow["unique_clicks"] + ",");
                            //Clicks (gross)
                            currentline.Append(myDataRow["total_clicks"] + ",");
                            //CTR
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["total_clicks"]) / Convert.ToDouble(myDataRow["mailed"])) + ",");
                            //CTOR
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["total_clicks"]) / Convert.ToDouble(myDataRow["opens"])) + ",");
                            //Bounces
                            currentline.Append(myDataRow["bounces"] + ",");
                            //% of Delivered
                            currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["successes"]) / membertotal * 100) + "%" + ",");
                            //Unsubscribes
                            currentline.Append(myDataRow["unsubs"] + ",");
                            //Complaints
                            currentline.Append(myDataRow["complaints"]);

                            //this line of the CSV file is now constricted, append the line to the CSV file
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

                        static private bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
                        {
                            Double result;
                            return Double.TryParse(val, NumberStyle,
                                System.Globalization.CultureInfo.CurrentCulture, out result);
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
