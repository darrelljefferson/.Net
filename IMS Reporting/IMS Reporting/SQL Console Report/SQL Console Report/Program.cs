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
using System.Diagnostics;

namespace ConsoleApplication1
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            // Read in FTP and database authentication from the config file
            Console.WriteLine("== reading variables from config file ==\n");


            StreamReader filerow = new StreamReader("config.txt");
            StringBuilder currentline = new StringBuilder();
            string selectString;
            string reportfilename;
            string zipfilename;
            string basefilename;
            string[] demoA = new string[200];
            int a = 0;
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            string cleandemo = "";
            DataTable myDataTable;
            FileStream fs;
            string input;
            string output;

            //the first unused readline is to skip over the comment line in the config file
            string sqlauth;
            sqlauth = filerow.ReadLine();
            sqlauth = filerow.ReadLine();
            Console.WriteLine(sqlauth + "\n");
            string ftppath;
            ftppath = filerow.ReadLine();
            ftppath = filerow.ReadLine();
            Console.WriteLine(ftppath + "\n");
            string ftpuser;
            ftpuser = filerow.ReadLine();
            ftpuser = filerow.ReadLine();
            Console.WriteLine(ftpuser + "\n");
            string ftppass;
            ftppass = filerow.ReadLine();
            ftppass = filerow.ReadLine();
            Console.WriteLine(ftppass + "\n");
            string demographics;


            Console.WriteLine("\n== querying demographics ==");

            selectString = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME = 'members_'";
            myDataTable = callsql(selectString, sqlauth);
            foreach (DataRow myDataRow in myDataTable.Rows)
            {
                currentline.Append("member." + myDataRow["COLUMN_NAME"] + ",");
            }
            demographics = currentline.ToString();

            //Parse csv demographics into an array


            for (int i = 7; i < demographics.Length; i++)
            {
                if (char.Equals(demographics[i], ','))
                {
                    //if it's a comma, time to save the demographic to the array
                    demoA[a] = res.ToString();
                    cleandemo += res.ToString(); cleandemo += ","; 
                    //clear string builder and advance to next array value
                    res.Length = 0;
                    a++;
                    i = i + 7;
                }
                else
                {
                    //add to string for that value
                    res.Append(demographics[i]);
                }

            }

            try
            {
// ########################################################################################################################################################################################
// Initiallizing complete, now running reporting


                // #############################################################
                // Report 1
                // #############################################################
                basefilename = "new-Report1 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";


                ReportClass1 Report1 = new ReportClass1();
                Report1.Report1(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report1 = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename+".zip";
                input = @basefilename +".csv";
                CommandUtilities.ZipFile(output, input);

                //After it's zipped, delete the CSV
                //File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 1 ==");
                ftpitclass ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();

                /*

                // #############################################################
                // Report 2
                // #############################################################

                basefilename = "new-Report2 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";

                ReportClass2 Report2 = new ReportClass2();
                Report2.Report2(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report2 = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename + ".zip";
                input = @basefilename + ".csv";
                CommandUtilities.ZipFile(input, output);

                //After it's zipped, delete the CSV
                File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 2 ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();

                // #############################################################
                // Report 3
                // #############################################################

                basefilename = "new-Report3 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";

                ReportClass3 Report3 = new ReportClass3();
                Report3.Report3(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report3 = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename + ".zip";
                input = @basefilename + ".csv";
                CommandUtilities.ZipFile(input, output);

                //After it's zipped, delete the CSV
                File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 3 ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();

                // #############################################################
                // Report 4
                // #############################################################

                basefilename = "new-Report4 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";

                ReportClass4 Report4 = new ReportClass4();
                Report4.Report4(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report4 = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename + ".zip";
                input = @basefilename + ".csv";
                CommandUtilities.ZipFile(input, output);

                //After it's zipped, delete the CSV
                File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 4 ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();


                // #############################################################
                // Report 6
                // #############################################################

                basefilename = "new-Report6 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";

                ReportClass6 Report6 = new ReportClass6();
                Report6.Report6(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report6 = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename + ".zip";
                input = @basefilename + ".csv";
                CommandUtilities.ZipFile(input, output);

                //After it's zipped, delete the CSV
                File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 6 ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();


                // #############################################################
                // Report 5
                // #############################################################

                basefilename = "new-Report5 " + DateTime.Now.ToString("dd-MM-yy hh.mm");
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename;
                zipfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".zip";
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + basefilename + ".csv";
                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                fs.Close();

                ReportClass5 Report5 = new ReportClass5();
                Report5.Report5(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report5 = null;
                GC.Collect();

                ReportClass5add Report5add = new ReportClass5add();
                Report5add.Report5add(demographics, sqlauth, demoA, cleandemo, reportfilename, a);
                Report5add = null;
                GC.Collect();

                Console.WriteLine("== zipping ==");
                Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
                output = @basefilename + ".zip";
                input = @basefilename + ".csv";
                CommandUtilities.ZipFile(input, output);

                //After it's zipped, delete the CSV
                File.Delete(reportfilename);

                Console.WriteLine("== running FTP upload of file report 5 ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, zipfilename);
                ftpit = null;
                GC.Collect();



                //notify that the upload is done
                reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "report999.txt";
                fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                fs.Close();

                Console.WriteLine("== running FTP upload notify file ==");
                ftpit = new ftpitclass();
                ftpit.ftpit(ftppath, ftpuser, ftppass, reportfilename);
                ftpit = null;
                GC.Collect();
            */


            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                Console.WriteLine(currentline);
                string holdopen = Console.ReadLine();
            }


            //end of main
            //debugging to hold open the console
            // string holdopen = Console.ReadLine();
        }

        static private void uploadFile(string FTPAddress, string filePath, string username, string password)
        {
            
            //Create FTP request
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FTPAddress + "/" + Path.GetFileName(filePath));

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = false;

            //Load the file
            FileStream stream = File.OpenRead(filePath);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, buffer.Length);
            stream.Close();

            //Upload file

            Stream requestStream = request.GetRequestStream();
            //stream.Write(bs, 0, (int)bs.Length);
            int bufferSize = 8192;
            byte[] bytes = new byte[bufferSize];
            Stream s = new MemoryStream(buffer);
            int read = 0;
            while ((read = s.Read(bytes, 0, bytes.Length)) != 0)
            {
                requestStream.Write(bytes, 0, read);
            }
            requestStream.Flush();
            requestStream.Close();
            Console.WriteLine("FTP Upload Success");
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
        static private bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        public static void ZipFile(string input, string output)
        {
            Process process = new Process();
            process.StartInfo.FileName = "c:\\cygwin\\bin\\zip.exe";
            process.StartInfo.Arguments = String.Concat(output, " ", input);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }

        //end of functions
    }
}
