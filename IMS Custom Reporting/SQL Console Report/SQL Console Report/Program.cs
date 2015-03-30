using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;

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
            demographics = filerow.ReadLine();
            demographics = filerow.ReadLine();
            Console.WriteLine(demographics + "\n\n");

            //Parse csv demographics into an array
            string[] demoA = new string[150];
            int a = 0;
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            DateTime dt;
            string stofile;

            for (int i = 7; i < demographics.Length; i++)
            {
                if (char.Equals(demographics[i], ','))
                {
                    //if it's a comma, time to save the demographic to the array
                    demoA[a] = res.ToString();
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
            demoA[a] = res.ToString();
            a++;

            /*

            // #############################################################
            // Report 1
            // #############################################################

            Console.WriteLine("== querying database report 1 ==");
            //Pull in the SQL result that will be used for constructing the report
            string selectString = "select "
            + "outmail_.HdrAll_, outmail_.MessageID_, outmail_.Created_, outmail_.Subject_, outmail_.Title_,lyrReportSummaryData.mailed, lyrReportSummaryData.successes, lyrReportSummaryData.InmailBodySize, lyrReportSummaryData.unique_opens, lyrReportSummaryData.opens, lyrReportSummaryData.unique_clicks, lyrReportSummaryData.total_clicks, lyrReportSummaryData.unsubs, lyrReportSummaryData.complaints, lyrReportSummaryData.bounces "
            + "from outmail_, lyrReportSummaryData with(nolock) "
            + "where outmail_.MessageID_ = lyrReportSummaryData.id AND lyrReportSummaryData.Created > (getdate()-90) "
            + "order by outmail_.MessageID_ desc";
            DataTable myDataTable = callsql(selectString, sqlauth);

            Console.WriteLine("== writing result to file report 1 ==");
            //write the file into the directory where the application is running from
            string reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report1 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
            FileStream fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
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
                    currentline.Append(SanitizeString(Convert.ToString(myDataRow["Subject_"])) + ",");
                    //Mailing Title
                    currentline.Append(myDataRow["Title_"] + ",");
                    //Final List Quantity
                    currentline.Append(myDataRow["mailed"] + ",");
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
                    currentline.Append(Convert.ToString(Convert.ToDouble(myDataRow["successes"]) / Convert.ToDouble(myDataRow["mailed"]) * 100) + "%" + ",");
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

            Console.WriteLine("== running FTP upload of file report 1 ==");
            //the file is constricted, now ftp the file to the external server
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);



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
            reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report2 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
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
                    currentline.Append("\"" + myDataRow["urltext_"] + "\"" + ",");
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

            Console.WriteLine("== running FTP upload of file for report 2 ==");
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);


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
            reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report3 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
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

            Console.WriteLine("== running FTP upload of file for report 3 ==");
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);


            // #############################################################
            // Report 4
            // #############################################################

            Console.WriteLine("\n== querying database report 4 ==");
            selectString =
            "select click.messageid_,mail.hdrall_,click.TimeClicked_,click.IpAddress_,urls.urltext_,click.memberid_,member.EmailAddr_,member.FullName_,";
            selectString += demographics;
            selectString +=
            " from clicktracking_ click with(nolock) "
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
                sw.WriteLine("Mailing ID,MailingRefCode,Date,Month,Day of Week,Time,IP Address,URL Text,Email Address,Member ID," + demographics + ",Full Name");


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
                    currentline.Append("\"" + myDataRow["urltext_"] + "\"" + ",");
                    //Email Address
                    currentline.Append(myDataRow["EmailAddr_"] + ",");
                    //Member ID
                    currentline.Append(myDataRow["memberid_"] + ",");

                    for (int i = 0; i < a; i++)
                    {
                        currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                    }
                    //Full Name
                    currentline.Append(myDataRow["fullname_"]);

                    stofile = currentline.ToString();
                    sw.WriteLine(stofile);
                    //Console.WriteLine(stofile);
                }

            }//close stream writer

            Console.WriteLine("== running FTP upload of file for report 4 ==");
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);


            // #############################################################
            // Report 6
            // #############################################################

            Console.WriteLine("\n== querying database report 6==");
            selectString =
            "select click.messageid_,click.TimeClicked_,click.IpAddress_,member.EmailAddr_ "
            + "from clicktracking_ click with(nolock) "
            + "inner join members_ member on member.MemberID_=click.MemberID_ "
            + "where click.urlid_ is null AND TimeClicked_ > (getdate()-1) ";

            myDataTable = callsql(selectString, sqlauth);

            Console.WriteLine("== writing result to file for report 6 ==");
            reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report6 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
            fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (StreamWriter sw = new StreamWriter(fs))
            {//open stream writer

                // Write CSV Header
                sw.WriteLine("Email Address,MailingID,IP Address,Open Date");

                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    currentline = new StringBuilder();
                    dt = Convert.ToDateTime(myDataRow["TimeClicked_"].ToString());

                    //Email Address
                    currentline.Append(myDataRow["EmailAddr_"] + ",");

                    //Mailing ID
                    currentline.Append(myDataRow["MessageID_"] + ",");

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

            Console.WriteLine("== running FTP upload of file for report 6 ==");
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);


            GC.Collect();
             * 
            */
            // #############################################################
            // Report 5
            // #############################################################

            DataTable myDataTable;
            string selectString;
            string reportfilename;
            FileStream fs;

            Console.WriteLine("\n== querying database report 5==");
            selectString =
                "select recips.MailingID,case when charindex('MailingRefCode',hdrall_)>0 then substring(hdrall_,charindex('MailingRefCode',hdrall_)+15,15) else 'no ID' end,recips.FinalAttempt,look.Description,member.EmailAddr_,member.MemberType_,member.First_Name_,member.Last_Name_,member.List_,member.Domain_,";
            selectString += demographics;
            selectString +=
                 " from lyrcompletedrecips recips with(nolock) "
                + "inner join outmail_ mail on recips.MailingID = mail.messageid_ "
                + "inner join members_ member on member.MemberID_=recips.MemberID "
                + "inner join lyrLookupCompletionStatus look on recips.CompletionStatusID=look.CompletionStatusID "
                + "where recips.FinalAttempt > (getdate()-1)";

            myDataTable = callsql(selectString, sqlauth);

            Console.WriteLine("== writing result to file for report 5 ==");
            reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + "new-Report5 " + DateTime.Now.ToString("dd-MM-yy hh.mm") + ".csv";
            fs = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (StreamWriter sw = new StreamWriter(fs))
            {//open stream writer

                // Write CSV Header
                sw.WriteLine("Mailing ID,MailingRefCode,Date,Month,Day of Week,Time,Domain,List Name,Failure Reason,Member Type,Member email,First Name," + demographics + "Last Name");

                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    currentline = new StringBuilder();
                    dt = Convert.ToDateTime(myDataRow["FinalAttempt"].ToString());
                    //Mailing ID
                    currentline.Append(myDataRow["MailingID"] + ",");
                    //MailingRefCode
                    currentline.Append(findheader(Convert.ToString(myDataRow[2])) + ",");
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
                    currentline.Append(myDataRow["list_"] + ",");
                    //Bounced Quantity by Domain
                    currentline.Append(myDataRow["Description"] + ",");
                    //Email Does Not Exist
                    currentline.Append(myDataRow["MemberType_"] + ",");
                    //Domain Does Not Exist
                    currentline.Append(myDataRow["EmailAddr_"] + ",");
                    //Mailbox Full
                    currentline.Append(myDataRow["First_Name_"] + ",");

                    for (int i = 0; i < a; i++)
                    {
                        currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                    }

                    //Content Filtered
                    currentline.Append(myDataRow["Last_Name_"]);

                    stofile = currentline.ToString();
                    sw.WriteLine(stofile);
                    //Console.WriteLine(stofile);
                }
                GC.Collect();
                Console.WriteLine("== Select unsubs and add to report 5 ==");

                selectString =
                    "select member.EmailAddr_,member.MemberType_,member.First_Name_,member.Last_Name_,member.List_,member.Domain_,member.DateUnsub_,";
                selectString += demographics;
                selectString +=
                    " from members_ member with(nolock) "
                    + "where member.DateUnsub_ > (getdate()-60)";

                myDataTable = callsql(selectString, sqlauth);

                Console.WriteLine("== write unsubs to file and add to report 5 ==");

                foreach (DataRow myDataRow in myDataTable.Rows)
                {
                    currentline = new StringBuilder();
                    dt = Convert.ToDateTime(myDataRow["DateUnsub_"].ToString());
                    //Mailing ID
                    currentline.Append("NA" + ",");
                    //MailingRefCode
                    currentline.Append("NA" + ",");
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
                    currentline.Append(myDataRow["list_"] + ",");
                    //Bounced Quantity by Domain
                    currentline.Append("NA" + ",");
                    //Email Does Not Exist
                    currentline.Append(myDataRow["MemberType_"] + ",");
                    //Domain Does Not Exist
                    currentline.Append(myDataRow["EmailAddr_"] + ",");
                    //Mailbox Full
                    currentline.Append(myDataRow["First_Name_"] + ",");

                    for (int i = 0; i < a; i++)
                    {
                        currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                    }

                    //Content Filtered
                    currentline.Append(myDataRow["Last_Name_"]);

                    stofile = currentline.ToString();
                    sw.WriteLine(stofile);
                    //Console.WriteLine(stofile);
                }


                GC.Collect();
                Console.WriteLine("== Select active sends and add to report 5 ==");

                selectString =
                    "select recips.MailingID,mail.hdrall_,recips.NextAttempt,member.EmailAddr_,member.MemberType_,member.First_Name_,member.Last_Name_,member.List_,member.Domain_,";
                selectString += demographics;
                selectString +=
                     " from lyractiverecips recips with(nolock) "
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
                    currentline.Append(myDataRow["list_"] + ",");
                    //Bounced Quantity by Domain
                    currentline.Append("STILL ACTIVE" + ",");
                    //Email Does Not Exist
                    currentline.Append(myDataRow["MemberType_"] + ",");
                    //Domain Does Not Exist
                    currentline.Append(myDataRow["EmailAddr_"] + ",");
                    //Mailbox Full
                    currentline.Append(myDataRow["First_Name_"] + ",");

                    for (int i = 0; i < a; i++)
                    {
                        currentline.Append("\"" + myDataRow[demoA[i]] + "\"" + ",");
                    }

                    //Content Filtered
                    currentline.Append(myDataRow["Last_Name_"]);

                    stofile = currentline.ToString();
                    sw.WriteLine(stofile);
                    //Console.WriteLine(stofile);
                }






            }//close stream writer
            GC.Collect();
            myDataTable.Dispose();
            Console.WriteLine("== running FTP upload of file for report 5 ==");
            //now ftp the file
            uploadFile(ftppath, reportfilename, ftpuser, ftppass);
            //once the FTP is complete, delete the file
            File.Delete(reportfilename);






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
            Console.WriteLine("new sql Conection object created");
            
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(selectString, mySqlConnection);

            Console.WriteLine("new sqlDataAdapter created");
            
            mySqlDataAdapter.SelectCommand.CommandTimeout = 500;

            DataSet myDataSet = new DataSet();

            mySqlConnection.Open();

            Console.WriteLine("connection open");

            string dataTableName = "Data";

            mySqlDataAdapter.Fill(myDataSet, dataTableName);

            Console.WriteLine("Filling sqlDataAdapter");


            DataTable myDataTable = myDataSet.Tables[dataTableName];

            Console.WriteLine("Creating data table");

            mySqlConnection.Close();

            Console.WriteLine("Closing Connection");

            return myDataTable;

            Console.WriteLine("returning data table");

        }
        static private string SanitizeString(string selText)
        {
            System.Text.StringBuilder res = new System.Text.StringBuilder();

            for (int i = 0; i < selText.Length; i++)
            {
                if (char.IsLetterOrDigit(selText[i]))
                {
                    //if a regular character, add to string
                    res.Append(selText[i]);
                }
                else if (char.IsWhiteSpace(selText[i]))
                {
                    //if a space, add space to string
                    res.Append(' ');
                }
                else if (char.Equals(selText[i], '_'))
                {
                    //if an underscore, that is valid, add to string
                    res.Append('_');
                }
                else if (char.Equals(selText[i], '-'))
                {
                    //if an dash, that is valid, add to string
                    res.Append('-');
                }
                //any other character is dropped
            }

            // Return the sanitized string.
            return res.ToString();
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
        //end of functions
    }
}
