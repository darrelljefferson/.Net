using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;


namespace HayhouseWeeklyReportGenerator
{
    class ReportBuilder
    {

        // method to populate the aggregate based reporting tables to then build the csv rows off of
        public static Dictionary<string,string> ParseMessageListData(string siteid, string mlid, string startDate, string endDate, 
            string password, DataTable table)
        {
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA TYPE=\"EXTRA\" ID=\"START_DATE\">", startDate, "</DATA>",
                "<DATA TYPE=\"EXTRA\" ID=\"END_DATE\">", endDate, "</DATA>",
                "<DATA TYPE=\"EXTRA\" ID=\"DG\">on</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);
            
            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);
            
            string messageQueryListdata = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?",
                "type=message&",
                "activity=Query-ListData&",
                "version=2",
                "&input=",encodedInput);


            Log.WriteLine(messageQueryListdata);

            // load API Xml into a XDocument
            XDocument foo = XDocument.Load(messageQueryListdata);

            // build linq query on the mid value
            var queryListData = from item in foo.Descendants("message")
                                select new
                                {
                                    messageid = item.Element("mid").Value,
                                    segmentid = item.Element("segment-id").Value,
                                    statsSent = item.Element("stats-sent").Value
                                };

            // now modify query to only find valid sent mailings
            var newQuery = from items in queryListData
                           where (Convert.ToDecimal(items.statsSent) > 0)
                           select items;

            //create the Dictionary to be return later

            Dictionary<string, string> messageidLookup = new Dictionary<string, string>();
            
            // iterate through the message node and then run the populateData call to build the reporting Data table
            foreach (var item in newQuery)
            {
                Console.WriteLine(item.messageid.ToString()+","+item.segmentid.ToString());
                // only add if we have a valid segment, to the id shouldn't be null
                if (item.segmentid.ToString() != "")
                {
                    messageidLookup.Add(item.messageid.ToString(), item.segmentid.ToString());
                }
            }

            return messageidLookup;
        }

        public static void BuildAggregateData(string siteid, string mlid, string mid, string password,
            DataTable segmentTable, DataTable aggregateTable)
        {
            // build the api call
            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<MID>",mid,"</MID>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            // encode the string
            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            string messageQueryStandardMessageReport = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?",
                "type=message&",
                "activity=Query-standard-message-report&",
                "input=", encodedInput);

            // load API Xml into a XDocument
            XDocument foo = XDocument.Load(messageQueryStandardMessageReport);

            var queryMessageDate = from item in foo.Descendants("record")
                                   select new
                                   {
                                       subject = item.Element("messagesubject").Value,
                                       category = item.Element("category").Value,
                                       sentDate = item.Element("date").Value,
                                       sender = item.Element("sender").Value,
                                       segment = item.Element("targetsegment").Value,
                                       sent = item.Element("mailed").Value,
                                       totalOpened = item.Element("opens").Element("total").Value,
                                       uniqueOpened = item.Element("uniqueopens").Value,
                                       uniqueOpenedPercent = item.Element("uniqueopens_percent").Value,
                                       uniqueClicks = item.Element("clicks").Element("unique").Value,
                                       uniqueClicksPercent = item.Element("clicks").Element("uniqueclicks_percent").Value,
                                       unsubs = Convert.ToInt32(item.Element("unsubscribes").Value),
                                       bounced = item.Element("bounces").Element("sum").Value,
                                       bouncedPerecent = item.Element("bounces").Element("bounce_percent").Value,
                                       hardBounced = item.Element("bounces").Element("hard").Value,
                                       hardBouncedPerecent = Convert.ToDecimal(item.Element("bounces").Element("hard").Value) / 
                                                             Convert.ToDecimal(item.Element("mailed").Value),
                                       softBounced = item.Element("bounces").Element("soft").Value,
                                       softBouncedPercent = Convert.ToDecimal(item.Element("bounces").Element("soft").Value) /
                                                            Convert.ToDecimal(item.Element("mailed").Value),
                                       delivered = item.Element("received").Value,
                                       deliveredPercent = Convert.ToDecimal(item.Element("received").Value) /
                                                          Convert.ToDecimal(item.Element("mailed").Value),
                                       spamComplaint = item.Element("spam_complaints").Value,
                                       uniqueReferrals = item.Element("unique_total_referrers").Value,
                                       


                                   };

            foreach (var item in queryMessageDate)
            {
                Console.WriteLine(item.subject.ToString());

                
                // now build up the variable and add in row
                string subject = item.subject.ToString();
                string category = item.category.ToString();
                string sentDate = item.sentDate.ToString();
                string sender = item.sender.ToString();
                string segment = item.segment.ToString();
                string sent = item.sent.ToString();
                string totalOpens = item.totalOpened.ToString();
                string uniqueOpens = item.uniqueOpened.ToString();
                string percentOpens = item.uniqueOpenedPercent.ToString();
                string uniqueClicks = item.uniqueClicks.ToString();
                string percentUniqueClicks = item.uniqueClicksPercent.ToString();
                string unsubs = item.unsubs.ToString();
                string bounced = item.bounced.ToString();
                string perecentBounced = item.bouncedPerecent.ToString();
                string hardbounced = item.hardBounced.ToString();
                string percentHardbounced = item.hardBouncedPerecent.ToString("00.0%");
                string softbounced = item.softBounced.ToString();
                string percentSoftbounced = item.softBouncedPercent.ToString("00.0%");
                string delievered = item.delivered.ToString();
                string percentDelivered = item.deliveredPercent.ToString("00.0%");
                string spamcomplaints = item.spamComplaint.ToString();
                string uniqueReferrals = item.uniqueReferrals.ToString();

                Log.WriteLine(string.Concat(subject,category, sentDate, 
                    sender, 
                    segment, sent, totalOpens, uniqueOpens, percentOpens,
                    uniqueClicks, percentUniqueClicks, unsubs, bounced, perecentBounced, hardbounced, percentHardbounced,
                    softbounced, percentSoftbounced, delievered, percentDelivered, spamcomplaints, uniqueReferrals));
                
                // add in the row for the segment and aggregate tables
                segmentTable.Rows.Add(subject, category, sentDate, sender, segment, sent, totalOpens, uniqueOpens,
                    percentOpens, uniqueClicks, percentUniqueClicks, unsubs, bounced, perecentBounced,
                    hardbounced, percentHardbounced, softbounced, percentSoftbounced, delievered,
                    percentDelivered, spamcomplaints, uniqueReferrals);

                aggregateTable.Rows.Add(subject, category, sentDate, sender, segment, sent, totalOpens, uniqueOpens,
                    percentOpens, uniqueClicks, percentUniqueClicks, unsubs, bounced, perecentBounced,
                    hardbounced, percentHardbounced, softbounced, percentSoftbounced, delievered,
                    percentDelivered, spamcomplaints, uniqueReferrals);
                
            }
        }

        public static void CreateReportCSV(DataTable reportingTable, string filename)
        {
            string csvLine;

            // create a new instance for the reporting
            DataTable myReportingTable = reportingTable;

            // sort on the segment field
            reportingTable.DefaultView.Sort = "segment";


            // create the filestream
            FileStream fs = new FileStream((filename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            using (StreamWriter sw = new StreamWriter(fs))
            //open stream writer
            {



                // use a comma deliter and have field in double quotes
                string seperator = "\",\"";

                // build the header string
                string header = string.Concat("\"", "Subject", seperator, "Category",
                    seperator, "Sent_Date", seperator,"From",seperator,"Segment",seperator,
                    "Sent",seperator,"Total_Opened",seperator,"Unique_Opened",seperator,
                    "%Unique_Opened",seperator,"Unique_Clicked",seperator,"%Unique_Clicked",
                    seperator,"Unsubscribed",seperator,"Bounced",seperator,"%Bounced",seperator,
                    "Hard_Bounced",seperator,"%Hard_Bounced",seperator,"Soft_Bounced",seperator,
                    "%Soft_Bounced",seperator,"Delivered",seperator,"%Delivered",seperator,
                    "Spam Complaints",seperator,"Unique_Referrers","\"");


                // write the file header
                sw.WriteLine(header);

                // For each line in the xml result, write a comma delimited row into the CSV file
                foreach (DataRow myDataRow in reportingTable.Rows)
                {
                    StringBuilder currentline = new StringBuilder();

                    // build the csv row
                    currentline.Append("\"");
                    currentline.Append(myDataRow["Subject"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Category"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Sent_Date"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["From"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Segment"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Sent"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Total_Opened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Unique_Opened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Unique_Opened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Unique_Clicked"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Unique_Clicked"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Unsubscribed"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Hard_Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Hard_Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Soft_Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Soft_Bounced"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Delivered"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%Delivered"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Spam_Complaints"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Unique_Referrers"]);
                    currentline.Append("\"");


                    // pass the string into the csvLine variable
                    csvLine = currentline.ToString();

                    // write the line
                    sw.WriteLine(csvLine);

                }
            }




        }

    }
}
