using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace GuidePostWeeklyReporting
{
    class ReportBuilder
    {

        // method to populate the aggregate based reporting tables to then build the csv rows off of
        public static void BuildAggregateReport(string siteid, string mlid, string startDate, string endDate, DataTable table)
        {
            // build the api call
            string messageQueryListDataString = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?type=message&activity=Query-ListData&version=2",
                "&input=%3CDATASET%3E",
                "%3CSITE_ID%3E", siteid, "%3C/SITE_ID%3E",
                "%3CMLID%3E", mlid,"%3C/MLID%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22START_DATE%22%3E", startDate,"%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22END_DATE%22%3E", endDate, "%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22DG%22%3Eon%3C/DATA%3E",
                "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E",
                "%3C/DATASET%3E");

            // load API Xml into a XDocument
            XDocument foo = XDocument.Load(messageQueryListDataString);

            // build linq query on the mid value
            var queryListData = from item in foo.Descendants("message")
                                 select new
                                 {
                                     messageid = item.Element("mid").Value,
                                     sent = item.Element("sent").Value,
                                     statsSent = item.Element("stats-sent").Value
                                 };

            // now modify query to only find valid sent mailings
            var newQuery = from items in queryListData
                           where (Convert.ToDecimal(items.statsSent) > 0)
                           select items;

            // iterate through the message node and then run the populateData call to build the reporting Data table
            foreach (var item in newQuery)
            {
                DataBuilder.populateDataTable(siteid, mlid, item.messageid.ToString(), table);
            }
        }

        // method to populate in the click data for each segment on a mailing
        public static void BuildTop5ClicksReport(string siteid, string mlid, string startDate, string endDate, string segmentid, DataTable table)
        {
            // build the api call
            string messageQueryListDataString = String.Concat("https://www.elabs6.com/API/",
                "mailing_list.html?type=message&activity=Query-ListData&version=2",
                "&input=%3CDATASET%3E",
                "%3CSITE_ID%3E",siteid, "%3C/SITE_ID%3E",
                "%3CMLID%3E", mlid,"%3C/MLID%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22START_DATE%22%3E", startDate,"%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22END_DATE%22%3E", endDate, "%3C/DATA%3E",
                "%3CDATA%20TYPE=%22EXTRA%22%20ID=%22DG%22%3Eon%3C/DATA%3E",
                "%3CDATA%20type=%22extra%22%20id=%22password%22%3EGu1dep0st%3C/DATA%3E%3C/DATASET%3E");

            // run the api call and load the resulting xml into a XDocument
            XDocument foo = XDocument.Load(messageQueryListDataString);

            // build the linq query to find messageid and segmentid
            var queryListData = from item in foo.Descendants("message")
                                select new
                                {
                                    messageid = item.Element("mid").Value,
                                    segmentID = item.Element("segment-id").Value,
                                    sent = item.Element("stats-sent").Value
                                };
            // now mofify to only find valid mailings and segments
            var validSent = from items in queryListData
                            where (Convert.ToDecimal(items.sent) > 0 && Convert.ToDecimal(items.segmentID) > 0)
                            select items;

            // iterate through the message node to run the populateSegment method call to build the segment data table
            foreach (var item in validSent)
            {
                // only run if we find a segment that matches the method input
                if (segmentid == (string)(item.segmentID.ToString()))
                {
                    DataBuilder.populateSegmentTable(siteid,mlid,item.messageid.ToString(),table);
                }
            }
        }
        
        // method to build the aggregate reprting file
        // we pass in the reporting table created above
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
                string header = string.Concat("\"","Deploy Date", seperator, "Message Name", seperator, "segment", seperator,
                    "Campaign", seperator, "Sent", seperator, "Delivered", seperator, "% Delivered", seperator,
                    "Opened", seperator, "% Opened", seperator, "U. Opened", seperator, "%U. Opened", seperator,"Clicked",seperator,
                    "% Clicked", seperator,"U. Clicked", seperator, "%U. Clicked", seperator, "Unsubs", seperator, "% Unsubs", "\"");

                // write the file header
                sw.WriteLine(header);

                // For each line in the xml result, write a comma delimited row into the CSV file
                foreach (DataRow myDataRow in reportingTable.Rows)
                {
                    StringBuilder currentline = new StringBuilder();
                   
                    // build the csv row
                    currentline.Append("\"");
                    currentline.Append(myDataRow["Deploy Date"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Message Name"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["segment"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["campaign"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["sent"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["delivered"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["delivered%"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["opened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["%opened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Uopened"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Uopened%"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["click"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["click%"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Uclick"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["Uclick%"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["unsub"]);
                    currentline.Append(seperator);
                    currentline.Append(myDataRow["unsub%"]);
                    currentline.Append("\"");
    
  
                    // pass the string into the csvLine variable
                    csvLine = currentline.ToString();

                    // write the line
                    sw.WriteLine(csvLine);

                }
            }

            


        }

        public static void CreateSegmentCSV(ArrayList segmentList, string filename)
        {
            // placeholder for the csvLine to write
            string csvLine;

            //string reportfilename = "YWI NL - Top5Clicks.csv";
            
            // initiate the filestream object
            FileStream fs = new FileStream((filename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            using (StreamWriter sw = new StreamWriter(fs))
            //open stream writer
            {
                
                // use a double quote delimiter and have fileds use double quotes
                string seperator = "\",\"";

                string header = string.Concat("\"","Date", seperator, "Category",seperator,
                    "Segment", seperator, "Campaign", seperator,"Url", seperator, "Clicks", "\"");

                // write the header line
                sw.WriteLine(header);

                // parse out the DataTable objects in the arraylist to only write the top 5 clicks to file
                foreach (DataTable dt in segmentList)
                {
                    Console.WriteLine("iterating through arraylist");
                    
                    // create a dataview to sort with
                    DataView dv = dt.DefaultView;
                    dv.Sort = "clicks desc";
                    
                    

                    // now iterate through up to the first 5 rows of the dataview, which is sorted by top clicks
                    for (int i = 0; i < 5; i++)
                    {
                        // Check to make sure we don't have less than 5 rows of data in the dataview
                        if (i >= dv.Count)
                        {
                            break;
                        }
                        
                        // parse out the dataview fields and build a csv line
                        csvLine = string.Concat("\"", dv[i][0], seperator, dv[i][1], seperator,
                            dv[i][2], seperator, dv[i][3], seperator, dv[i][4], seperator,dv[i][5],"\"");
                        // write the line to file
                        sw.WriteLine(csvLine);

                  
                    }
                }


            }
            
            /*
            DataTable reportingTable = segmentTable;

            reportingTable.DefaultView.Sort = "url";

            DataView dv = reportingTable.DefaultView;
            dv.Sort = "clicks desc";

            for (int i = 0; i < dv.Count; i++)
            {
                Console.WriteLine("{0}, {1}, {2}",
                dv[i][0],
                dv[i][1],
                dv[i][2]);
            }
            /*
            foreach (DataRow dr in dv.s)
            {
                //Console.WriteLine(dr["segmentName"]);
                Console.WriteLine(dr["url"]);
                //Console.WriteLine(dr["clicks"]);
            }
            */
        }
    }
}
