using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidePostWeeklyReporting
{
    class DataBuilder
    {
        // method to figure out how many mailings are for the week
        // to be used to figure out how large to make the segment string array
        public static int entryCount(string siteID, string mlid, string startDate, string endDate)
        {
            /*
            string siteID = "26018";
            string mlid = "25238";
            string startDate = "2009-10-31";
            string endDate = "2009-11-04";
            */
             
            // string to build the api call
            string messageQueryListDataString = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?type=message&activity=Query-ListData&version=2",
                "&input=%3CDATASET%3E",
                "%3CSITE_ID%3E",siteID, "%3C/SITE_ID%3E",
                "%3CMLID%3E", mlid,"%3C/MLID%3E%",
                "3CDATA%20TYPE=%22EXTRA%22%20ID=%22START_DATE%22%3E", startDate,"%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22END_DATE%22%3E", endDate, "%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22DG%22%3Eon%3C/DATA%3E",
                "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E",
                "%3C/DATASET%3E");

            
            // run the api call and load in the xml data returned
            XDocument foo = XDocument.Load(messageQueryListDataString);

            // query to find out the messageid
            var queryListData = from item in foo.Descendants("message")
                                select new
                                {
                                    messageid = item.Element("mid").Value
                                };
            int count = 0;
            // iterate to increment the count variable to then return it
            foreach (var item in queryListData)
            {
                count++;

            }


            return count;
        }

        // funtion to store the segmentid's in a array
        public static string[] segmentIDs(string siteID, string mlid, string startDate, string endDate)
        {

            
            // run the entryCount method to see how big the array should be
            string[] segementID = new string[entryCount(siteID,mlid,startDate, endDate)];


            // build the api call
            string messageQueryListDataString = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?type=message&activity=Query-ListData&version=2",
                "&input=%3CDATASET%3E",
                "%3CSITE_ID%3E",siteID, "%3C/SITE_ID%3E",
                "%3CMLID%3E", mlid,"%3C/MLID%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22START_DATE%22%3E", startDate,"%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22END_DATE%22%3E", endDate, "%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22DG%22%3Eon%3C/DATA%3E",
                "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E",
                "%3C/DATASET%3E");
            
            
            
            // run the api call and load results into a xml document
            XDocument foo = XDocument.Load(messageQueryListDataString);

            // build linq query to find segmentid
            var queryListData = from item in foo.Descendants("message")
                                select new
                                {
                                    segmentid = item.Element("segment-id").Value,
                                    // we need to check to make sure it's actually sent
                                    sent = item.Element("sent").Value
                                };
            
            // count variable to keep track of which mailing we are on
            int count = 0;
            
            // iterate through message nodes
            foreach (var item in queryListData)
            {
                // set the flag to false by default
                bool isMatch = false;

                
                for (int i = 0; i <= count; i++)
                {

                    // check to see if the array already had a segmentid populated
                    // if so, then set flag to true so we don't over-ride the value
                    if (segementID[i] == item.segmentid.ToString())
                    {
                        isMatch = true;
                        
                    }

                }
                // now we set the value in the array to either the segment id or to nothing
                // We also check to make sure mailing has been sent and there was a segment used.
                if (!isMatch && (string)item.sent.ToString() == "yes" && Convert.ToInt32(item.segmentid.ToString()) > 0)
                {
                    segementID[count] = item.segmentid.ToString();
                    //Console.WriteLine("The current segmentid is "+segementID[count]);
                    
                }
                else
                {
                    segementID[count] = "";
                    
                }

                // now increment the count
                count++;

                
            }

            // placeholder to build the size of the final array
            int newcount = 0;
            // now we iterate through to increment the newCount variable if the current array entry is not blank
            for (int i = 0; i < segementID.Length; i++)
            {
                // if not blank, then we increment
                if (segementID[i] != "")
                {
                    newcount++;
                }

            }

            // now we declare the final array
            string[] newSegementIDArray = new string[newcount];

            //reset the variable back to 0
            newcount = 0;
            
            // and then we put in the values for the new string array where there are no blank values in the old segment array
            for (int i = 0; i < segementID.Length; i++)
            {
                // now fill in the values if it's not blank
                if (segementID[i] != "")
                {
                    newSegementIDArray[newcount] = segementID[i];
                    // increment the newcount variable for filling in the next array item
                    newcount++;
                }

            }

            // return the new segment array
            return newSegementIDArray;
        }

        // DataTable for the reporting
        public static DataTable reportingTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Deploy Date", typeof(string));
            table.Columns.Add("Message Name", typeof(string));
            table.Columns.Add("segment", typeof(string));
            table.Columns.Add("campaign", typeof(string));
            table.Columns.Add("sent", typeof(string));
            table.Columns.Add("delivered", typeof(string));
            table.Columns.Add("delivered%", typeof(string));
            table.Columns.Add("opened", typeof(string));
            table.Columns.Add("%opened", typeof(string));
            table.Columns.Add("Uopened", typeof(string));
            table.Columns.Add("Uopened%", typeof(string));
            table.Columns.Add("click", typeof(string));
            table.Columns.Add("click%", typeof(string));
            table.Columns.Add("Uclick", typeof(string));
            table.Columns.Add("Uclick%", typeof(string));
            table.Columns.Add("unsub", typeof(string));
            table.Columns.Add("unsub%", typeof(string));

            return table;


        }

        // DataTable for top5segments
        public static DataTable top5segments()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Deploy Date", typeof(string));
            table.Columns.Add("category", typeof(string));
            table.Columns.Add("segmentName", typeof(string));
            table.Columns.Add("campaign", typeof(string));
            table.Columns.Add("url", typeof(string));
            table.Columns.Add("clicks", typeof(int));

            return table;
        }
        
        // method populate in the aggregate data table
        public static void populateDataTable(string siteID, string mlid, string mid, DataTable csvDataTable)
        {
            // build the api call
            string messageQueryStandardMessageReportString = string.Concat("https://www.elabs6.com/API/mailing_list.html?",
                    "type=message&activity=Query-standard-message-report&input=%3CDATASET%3E",
                    "%3CSITE_ID%3E", siteID, "%3C/SITE_ID%3E",
                    "%3CMLID%3E", mlid, "%3C/MLID%3E",
                    "%3CMID%3E", mid, "%3C/MID%3E",
                    "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E",
                    "%3C/DATASET%3E");

            //Console.WriteLine(messageQueryStandardMessageReportString);

            // load the xml document from the api call
            XDocument getMessageDetails = XDocument.Load(messageQueryStandardMessageReportString);


            // provide linq syntax to parse xml with
            var queryMessageDetails = from messages in getMessageDetails.Descendants("record")

                                      select new ParseMessageData
                                      {
                                          Date = messages.Element("date").Value,
                                          Category = messages.Element("messagename").Value,
                                          Segment = messages.Element("targetsegment").Value,
                                          Campaign = messages.Element("messagesubject").Value,
                                          Sent = messages.Element("mailed").Value,
                                          Delivered = messages.Element("received").Value,
                                          // equals delivered divided by sent
                                          PercentDelivered = (Convert.ToDecimal(messages.Element("received").Value) / Convert.ToDecimal(messages.Element("mailed").Value)),
                                          Opened = messages.Element("totalopens").Value,
                                          // equals opened divied by delivered
                                          PercentOpened = (Convert.ToDecimal(messages.Element("totalopens").Value) / Convert.ToDecimal(messages.Element("received").Value)),
                                          UniqueOpened = messages.Element("uniqueopens").Value,
                                          // equals unique opens divied by delivered
                                          PercentUniqueOpened = (Convert.ToDecimal(messages.Element("uniqueopens").Value) / Convert.ToDecimal(messages.Element("received").Value)),
                                          Clicks = messages.Element("clicks").Element("total").Value,
                                          // equals clicks divied by opens
                                          PercentClicks = (Convert.ToDecimal(messages.Element("clicks").Element("total").Value) / Convert.ToDecimal(messages.Element("totalopens").Value)),
                                          UniqueClicks = messages.Element("clicks").Element("unique").Value
                                          

                                      };
           


            // now iterate through record nodes to build the reporting data
            
            foreach (var newItem in queryMessageDetails)
            {

                // parse out date value
                string date = newItem.Date.ToString();
                // use regex to strip out time and only put in date information
                // parse on 2 alpha-numeric characters, 1 while space, 2 or 3 non-white space characters, 1 white space, 4 alpha-numeric characters
                date = Regex.Match(date, @"\b\w{3}\W.{2}|.{3}\W\w{4}\b").Value;
                // remove the commas
                date = date.Replace(",", "");

                /*
                // now need to deal with calculating %'s that have 0 clicks in them
                decimal percentDelivered = 0;
                decimal percentOpened = 0;
                decimal percentUniqueOpened = 0;
                decimal percentClick = 0;
                decimal percentUniqueClick = 0;
                

                //Console.WriteLine(newItem.Opened.ToString());

                if (Convert.ToDecimal(newItem.Sent.ToString()) > 0)
                {
                    percentDelivered = newItem.PercentDelivered / Convert.ToDecimal(newItem.Sent.ToString());
                }
                if (Convert.ToDecimal(newItem.Delivered.ToString()) > 0)
                {
                    percentOpened = newItem.PercentOpened / Convert.ToDecimal(newItem.Delivered.ToString());
                    percentUniqueOpened = newItem.PercentUniqueOpened / Convert.ToDecimal(newItem.Delivered.ToString());
                }
                if (Convert.ToDecimal(newItem.Opened.ToString()) > 0)
                {
                    percentClick = newItem.PercentClicks / Convert.ToDecimal(newItem.Opened.ToString());
                }

                if (Convert.ToDecimal(newItem.UniqueOpened.ToString()) > 0)
                {
                    percentUniqueClick = newItem.PercentUniqueClicks / Convert.ToDecimal(newItem.UniqueOpened.ToString());
                }
                */


                // debug statement
                Console.WriteLine(date + ","
                    + newItem.Date.ToString() + ","
                    + newItem.Category.ToString() + ","
                    + newItem.Segment.ToString() + ","
                    + newItem.Campaign.ToString() + ","
                + newItem.Sent.ToString() + ","
                + newItem.Delivered.ToString() + ","
                    + newItem.PercentDelivered.ToString() + ","
                + newItem.Opened.ToString() + ","
                + newItem.PercentOpened.ToString("0.0") + ","
                + newItem.UniqueOpened.ToString() + ","
                + newItem.PercentUniqueOpened.ToString() + ","
                + newItem.Clicks.ToString() + ","
                + newItem.PercentClicks.ToString() + ",");
           
              




 


                // we now need to parse through click data to figure out unsub information
                // build linq query
                var queryHTMLClicks = from clicks in getMessageDetails.Descendants("html_click")

                                      select new ParseUnsubscribe
                                      {
                                          url = clicks.Element("link").Value,
                                          totalUnsubs = clicks.Element("html_total_clicks").Value,
                                          uniqueUnsubs = clicks.Element("html_unique_clicks").Value
                                          
                                      };

                // declare unsubscribe variables
                string totalUnsubscribed = "0";
                string uniqueUnsubscribed = "0";
                decimal unsubscribedPercentage = 0;

                // iterate through click data to to figure out unsubscribed, based on link click data
                foreach (var nextItem in queryHTMLClicks)
                {
                    // do regex match to see if the link is a unsubscribe link
                    Regex match = new Regex(@"[Uu]nsubscribe.?&activity=submit&email=");
                    // if it matches, then we fill in the unsub stats
                    if (match.IsMatch(nextItem.url.ToString()))
                    {
                        // strip out commas and declare the values to use to calculate click #'s
                        string delivered = Regex.Replace(newItem.Delivered.ToString(), ",", "");
                        totalUnsubscribed = Regex.Replace(nextItem.totalUnsubs.ToString(), ",", "");
                        uniqueUnsubscribed = Regex.Replace(nextItem.uniqueUnsubs.ToString(), ",", "");
                        // calculate unsub percentages
                        unsubscribedPercentage = (Convert.ToDecimal(uniqueUnsubscribed) / Convert.ToDecimal(delivered));

                    }
                }

                // now we need to adjust unique click data to minus out unsubs
                int totalClicks = 0;
                int uniqueClicks = 0;
                // strip out commas
                string tClicks = Regex.Replace(newItem.Clicks.ToString(), ",", "");
                string uClicks = Regex.Replace(newItem.UniqueClicks.ToString(), ",", "");
                // now calculate the new total click values to minus out total unsubs
                totalClicks = Convert.ToInt32(tClicks) - Convert.ToInt32(totalUnsubscribed);
                // and calculate the total click %'s to divide by opens
                decimal totalClickPercentage = Convert.ToDecimal(totalClicks) / Convert.ToDecimal(newItem.Opened.ToString());
                // now calculate the new unique click values to minus out unique unsubs
                uniqueClicks = Convert.ToInt32(uClicks) - Convert.ToInt32(uniqueUnsubscribed);
                // and calculate the unique click %'s to divide by opens
                decimal uniqueClickPercentage = Convert.ToDecimal(uniqueClicks) / Convert.ToDecimal(newItem.UniqueOpened.ToString());

                // now we add in the rows to the DataTable to be used to generate the csv later
                csvDataTable.Rows.Add(date,
                     newItem.Category.ToString(),
                     newItem.Segment.ToString(),
                     newItem.Campaign.ToString(),
                     newItem.Sent.ToString(),
                     newItem.Delivered.ToString(),
                     newItem.PercentDelivered.ToString("00.000%"),
                     //percentDelivered.ToString("00.000%"),
                     newItem.Opened.ToString(),
                     newItem.PercentOpened.ToString("00.000%"),
                     //percentOpened.ToString("00.000%"),
                     newItem.UniqueOpened.ToString(),
                     newItem.PercentUniqueOpened.ToString("00.000%"),
                     //percentUniqueOpened.ToString("00.000%"),
                     //newItem.Clicks.ToString(),
                     totalClicks.ToString(),
                     //newItem.PercentClicks.ToString("00.000%"),
                     totalClickPercentage.ToString("00.000%"),
                     //percentClick.ToString("00.000%"),
                     uniqueClicks.ToString(),
                     //newItem.UniqueClicks.ToString(),
                     uniqueClickPercentage.ToString("00.000%"),
                     //percentUniqueClick.ToString("00.000%"),
                     uniqueUnsubscribed,
                     unsubscribedPercentage.ToString("00.000%"));
            

            }
            // write how many lines are added
            Console.WriteLine(csvDataTable.Rows.Count);

        }

        
        // method to create the top 5 segment clicks data tables
        public static void populateSegmentTable(string siteID, string mlid, string mid, DataTable csvDataTable)
        {
            // build the api call
            string messageQueryStandardMessageReportString = string.Concat("https://www.elabs6.com/API/mailing_list.html?",
                    "type=message&activity=Query-standard-message-report&input=%3CDATASET%3E",
                    "%3CSITE_ID%3E", siteID, "%3C/SITE_ID%3E",
                    "%3CMLID%3E", mlid, "%3C/MLID%3E",
                    "%3CMID%3E", mid, "%3C/MID%3E",
                    "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E",
                    "%3C/DATASET%3E");

            // run the api cal and load the xml into a XDocument
            XDocument getMessageDetails = XDocument.Load(messageQueryStandardMessageReportString);


            // linq query to find date, segment, and campaign info
            var queryMessageDetails = from messages in getMessageDetails.Descendants("record")

                                      select new ParseMessageData
                                      {
                                          Date = messages.Element("date").Value,
                                          Category = messages.Element("messagename").Value,
                                          Segment = messages.Element("targetsegment").Value,
                                          Campaign = messages.Element("messagesubject").Value


                                      };
            // iterate through record node
            foreach (var newItems in queryMessageDetails)
            {
                
                //run regex to pull out the just the Date only
                string date = newItems.Date.ToString();
                //use regex to match 2 alpha characters, 1 white space, 2 or 3 characters, 1 while space, 4 alpha characters
                date = Regex.Match(date, @"\b\w{3}\W.{2}|.{3}\W\w{4}\b").Value;
                // remove comma from it
                date = date.Replace(",", "");
                
                // linq query to find individaul url and clicks
                var queryHTMLClicks = from clicks in getMessageDetails.Descendants("html_click")

                                      select new ParseClickData
                                      {
                                          url = clicks.Element("link").Value,
                                          clicks = clicks.Element("html_unique_clicks").Value
                                      };



                // iterate through html_click nodes to find unique click data
                foreach (var nextItem in queryHTMLClicks)
                {


                    bool flag = true;

                    int rowCount = 0;
                    int numberClicks = 0;
                    int currentRow = 0;
                    // now iterate through the passed in datatable to see if the url already exists
                    // if so, then we need to add up the clicks and remove the old url row
                    // if not, we just add a row with the new url
                    foreach (DataRow dr in csvDataTable.Rows)
                    {
                        if (nextItem.url.ToString() == (string)dr["url"])
                        {
                            string removeCommas = Regex.Replace(nextItem.clicks.ToString(), ",", "");
                            numberClicks = (int)dr["clicks"];
                            // increment the # of clicks
                            numberClicks += Int32.Parse(removeCommas);
                            // set so we know which row to delete from the datatable
                            currentRow = rowCount;
                            // set the flag to false, so we know we need to delete the existing url row
                            flag = false;
                        }
                        // increment the rowcount
                        rowCount++;

                    }

                    // if false, then we need to delete
                    if (!flag)
                    {
                        // set the datarow to the current row in the datatable
                        DataRow dr = csvDataTable.Rows[currentRow];
                        // now delete the datarow
                        dr.Delete();
                        // and then add in the updated values
                        csvDataTable.Rows.Add(date,
                                newItems.Category.ToString(),
                                newItems.Segment.ToString(),
                                newItems.Campaign.ToString(),
                                nextItem.url.ToString(),
                                numberClicks);
                    }


                    // if true, then we just add
                    if (flag)
                    {
                        /*
                        // debug statements
                        Console.WriteLine("date " + date);
                        Console.WriteLine("segment " + newItems.Segment.ToString());
                        Console.WriteLine("campaing " + newItems.Campaign.ToString());
                        Console.WriteLine("click count " + Int32.Parse(nextItem.clicks.Replace(",","").ToString()));
                        */

                        csvDataTable.Rows.Add(date,
                                newItems.Category.ToString(),
                                newItems.Segment.ToString(),
                                newItems.Campaign.ToString(),
                                nextItem.url.ToString(),
                                Int32.Parse(nextItem.clicks.Replace(",","").ToString()));
                    }


                }
            }

            // debug statement to make sure we're seeing the right # of rows per mailings parsed
            // the count should only slightly increase per mailing, since there will be duplicate urls
            Console.WriteLine(csvDataTable.Rows.Count+"\n");
        }

    


    }
}
