using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;


namespace HayhouseMonthlyReportGenerator
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

        public static void GenerateSegmentResults(string siteid, string mlid,
            string filterid, string notifyEmail, string password)
        {

            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"extra\" id=\"email\">",notifyEmail,"</DATA>",
                "<DATA type=\"id\">",filterid,"</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string filterGenerate = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?",
                "type=filter&",
                "activity=generate&",
                "&input=", encodedInput);


            Log.WriteLine(filterGenerate);

            // load API Xml into a XDocument
            XDocument getFilterGenerationResults = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                getFilterGenerationResults = XDocument.Load(filterGenerate);
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = getFilterGenerationResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getFilterGenerationResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + filterGenerate);
                Log.WriteLine("The failure reason was: " + getFilterGenerationResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getFilterGenerationResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + filterGenerate);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                Log.WriteLine(getFilterGenerationResults.Element(dataset).Element(data).Value);
            }


        }

        public static string GenerateSubscribeUnsubscribeFilter(string siteid, string mlid, string name,
            string startDate, string endDate, string subscribeDemoID, string optoutDemoID,
            string password)
        {

            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"name\">",name,"</DATA>",
                "<DATA type=\"demographic\" id=\"",subscribeDemoID,"\">",
                startDate,"</DATA>",
                "<DATA type=\"demographic\" id=\"", subscribeDemoID, "\">", 
                endDate, "</DATA>",
                "<DATA type=\"extra\" id=\"between\">D-",subscribeDemoID,"</DATA>",
                "<DATA type=\"demographic\" id=\"",optoutDemoID,"\" GROUP=\"1\" LOGIC=\"OR\">",
                startDate,"</DATA>",
                "<DATA type=\"demographic\" id=\"", optoutDemoID, "\">",
                endDate, "</DATA>",
                 "<DATA type=\"extra\" id=\"between\">D-",optoutDemoID, "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string filterAdd = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?",
                "type=filter&",
                "activity=add&",
                "&input=", encodedInput);


            Log.WriteLine(filterAdd);


            // load API Xml into a XDocument
            XDocument getFilterAddResults = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                getFilterAddResults = XDocument.Load(filterAdd);
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = getFilterAddResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getFilterAddResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + filterAdd);
                Log.WriteLine("The failure reason was: " + getFilterAddResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getFilterAddResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + filterAdd);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                Log.WriteLine(getFilterAddResults.Element(dataset).Element(data).Value);
                return getFilterAddResults.Element(dataset).Element(data).Value;
            }

            return "false";
        }

        public static int[] FilterQueryData(string siteid, string mlid, string filterid,
            string password, string[] demographicIds)
        {
            // delclare an int array to be used later on for the counts
            int[] counts = new int[2] { 0, 0 };
            
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA type=\"id\">",filterid,"</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string filterQueryData = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?",
                "type=filter&",
                "activity=query-data&",
                "&input=", encodedInput);


            Log.WriteLine(filterQueryData);

            // load API Xml into a XDocument
            XDocument getFilterQueryResults = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                getFilterQueryResults = XDocument.Load(filterQueryData);
            }
            catch (Exception e)
            {
                Log.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = getFilterQueryResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getFilterQueryResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + filterQueryData);
                Log.WriteLine("The failure reason was: " + getFilterQueryResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getFilterQueryResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + filterQueryData);
                Log.WriteLine("");
            }
            // if no errors, then continue with the call
            // declare the int array to use to keep track of the counts
            
            else
            {

                
                IEnumerable<XElement> queryData =
                from qd in getFilterQueryResults.Descendants("RECORD")
                select qd;

                // delcare the variables to use
                int subscribeCount = 0;
                int optoutcount = 0;

                // now interatte through the results to build the reporting
                foreach (XElement qd in queryData)
                {

                    Console.WriteLine(qd.Value);

                    // check to see if the subscribe demographic is populated
                    // if so, then increment the count
                    if ((string)qd.Attribute("type") == "demographic"
                        && (string)qd.Attribute("id") == demographicIds[0]
                        && (string)qd.Value != "")
                    {
                        subscribeCount++;
                        Console.WriteLine("subs updated");
                    }

                    // check to see if the optout demographic is populated
                    // if so, then increment the count
                    if ((string)qd.Attribute("type") == "demographic"
                        && (string)qd.Attribute("id") == demographicIds[1]
                        && (string)qd.Value != "")
                    {
                        optoutcount++;
                        Console.WriteLine("unsubs updated");
                    }

                }

                //update the int array to reflect the values

                counts[0] = subscribeCount;
                counts[1] = optoutcount;

                return counts;
            }

            return counts;
        }
        

    }
}
