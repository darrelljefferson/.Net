using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HayhouseWeeklyReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Log.Init(@"c:\\lm_custom\log");

            //declare variables
            string exportDirectoryPath = @"c:\lm_custom\export\";

            string siteid = "12346080";
            string mlid = "20949";
            string apiPassword = "hayhou$se";

            DateTime start = Convert.ToDateTime("2011-06-26");
            //DateTime start = DateTime.Now.AddDays(-9);
            DateTime end= Convert.ToDateTime("2011-07-02");
            //DateTime end = DateTime.Now.AddDays(-3);
            DateTime today = DateTime.Now.AddDays(0);

            string startDate = start.ToString("yyyy-MM-dd");
            Console.WriteLine(startDate);
            string endDate = end.ToString("yyyy-MM-dd");
            Console.WriteLine(endDate);
            string todayDate = today.ToString("yyyy-MM-dd");
            Console.WriteLine(todayDate);

            // build up the Dictionary of segmentid's
            Dictionary<string, string> segmentPairs = new Dictionary<string, string>()
            {
                {"42606","A: Bill Phillips"},
                {"42607","A: Carol Ritberger, Ph.D. News"},
                {"42570","A: Caroline Myss Newsletter"},
                {"42513","B: Master:Virtue"},
                {"40330","B: Master:Northrup"},
                {"39710"," B: Master:Dyer"},
                {"42564","A: Gary Renard Enews"},
                {"40335","B: HYL:Get Healthy Newsletter"},
                {"43235","Affiliates"},
                {"55266","B: Hay House Australia Newsletter"},
                {"47574","BL: Hay House Book Launch"},
                {"39703","B: Master: HH"},
                {"40329","B: Master:HHR"},
                {"40645"," B: HYL:HealYourLifeNewsletter"},
                {"49567"," B: ICANDOIT.net"},
                {"42572","A: Immaculee Ilibagiza: LTT"},
                {"40336","B: HYL:Inspiration Newsletter"},
                {"40333","B: HYL:Inspiration Newsletter"},
                {"42573"," A: Joan Borysenko,Ph.D. E-news"},
                {"42566","A: John Holland E-newsletter"},
                {"40328","B: Master:LH"},
                {"49246","B: Oh My God Movie Newsletter"},
                {"40332","B: Master:LH"},
                {"42568","A: PlayGround Pump Enews"},
                {"42567","A: Sonia Choquette Newsletter"},
                {"40334","B: HYL:SuccessAbundanceNewsletter"},
                {"42605","A: SuzeOrman Updates[DontSend]"},
                {"53528","Wisdom Community"},
                {"43279","Northrup:WWC only"},
                {"49221","B: You Can Heal Your Life Movie"},
                {"42565","A: ColetteBaronReid Newsletter"},
                {"49566","B: The Shift Movie"} 
            };
            
            
            // Declare a datatable for the aggreage reports
            DataTable aggregateReportingTable = utility.reportingTable();

            Dictionary<string, string> messageidSegmentidLookup = ReportBuilder.ParseMessageListData(siteid, mlid, startDate, 
                endDate, apiPassword, aggregateReportingTable);

            // iterate through dictionary to build up the individual segment reports
            foreach (var segPairs in segmentPairs)
            {
                // each segment needs it's own datatable
                DataTable reportTable = utility.reportingTable();

                //Console.WriteLine("current segmentid: " + segPairs.Key);
                
                // loop through the messageids to see if we have a message id that matches the segment
                foreach (var messagePairs in messageidSegmentidLookup)
                {
                    //Console.WriteLine("current messageid: " + messagePairs.Key);
                    
                    // if we find a match of the segmentid in the first loop to the segmentid of a matching messageid
                    // then we run the api call to build up the datatable
                    if (segPairs.Key == messagePairs.Value)
                    {
                        Log.WriteLine("current segment: "+segPairs.Key+"\n and current messageid is: "+messagePairs.Key);
                        ReportBuilder.BuildAggregateData(siteid, mlid, messagePairs.Key, apiPassword, reportTable, aggregateReportingTable);

                        Console.WriteLine("curent data table count: " + reportTable.Rows.Count);
                        Console.WriteLine("aggregate data table count: " + aggregateReportingTable.Rows.Count);

                    }

                }

                // now build the segment csv reports
                if (reportTable.Rows.Count > 0)
                {
                    ReportBuilder.CreateReportCSV(reportTable, segPairs.Key + "_" + todayDate + ".csv");
                }

            }

            // and build the aggregate table now
            ReportBuilder.CreateReportCSV(aggregateReportingTable, "aggregate_" + todayDate + ".csv");

           
            
            
        }
    }
}
