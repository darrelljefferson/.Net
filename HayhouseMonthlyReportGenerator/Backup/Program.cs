using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace HayhouseMonthlyReportGenerator
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

            DateTime start = Convert.ToDateTime("2010-09-11");
            //DateTime start = DateTime.Now.AddDays(-9);
            DateTime end = Convert.ToDateTime("2010-09-18");
            //DateTime end = DateTime.Now.AddDays(-3);
            DateTime today = DateTime.Now.AddDays(0);

            string startDate = start.ToString("yyyy-MM-dd");
            Console.WriteLine(startDate);
            string endDate = end.ToString("yyyy-MM-dd");
            Console.WriteLine(endDate);
            string todayDate = today.ToString("yyyy-MM-dd");
            Console.WriteLine(todayDate);

            // build up the Dictionary of segmentid's
            Dictionary<string, string> segmentLookup = utility.SegmentIDTable();

            // build up the Dictionary of subscribe and unsubscribe date demographic id's
            Dictionary<string, string[]> segmentSubscribeUnsubscribeLookup 
                = utility.SubscribeUnsubscribeIDTable();

            // create an array list to use to store the created filterid;s
            ArrayList filterList = new ArrayList();

            //ReportBuilder.GenerateSegmentResults(siteid, mlid, "42607", "bmueller@lyris.com", apiPassword);

            //since regeneration of segments takes a long time, start by kicking that off
            /*
            //loop though the segmentid's and kick off the jobs
            foreach (var segment in segmentPairs)
            {
                ReportBuilder.GenerateSegmentResults(siteid, mlid, segment.Key, "bmueller@lyris.com", apiPassword);
            }
            

            foreach (var ids in segmentSubscribeUnsubscribeLookup)
            {

                string[] id = ids.Value.ToArray();
                Console.WriteLine(ids.Key+","+id[0] + "," + id[1]);
                // create the filter
                string filterid = ReportBuilder.GenerateSubscribeUnsubscribeFilter(siteid,
                    mlid, "Lyris1 " + ids.Key + " monthly reporting", "08/01/10", "08/30/10", id[0], id[1], apiPassword);
                Console.WriteLine(filterid);
                // add the filterid to the 3rd array element and then add the updated array
                // to an arraylist
                id[2] = filterid;
                filterList.Add(id);
                // now generate the segment
                if (filterid != "false")
                {
                    ReportBuilder.GenerateSegmentResults(siteid, mlid, filterid, "bmueller@lyris.com",
                       apiPassword);
                }
            }
            

            string test = ReportBuilder.GenerateSubscribeUnsubscribeFilter(siteid, mlid, "LyrisFoo", "08/01/10",
                "08/30/10", "65917", "65918", apiPassword);
            Console.WriteLine(test);
            */

            string[] foo = new string[3] {"65243","65244","42565"};
            int[] count = ReportBuilder.FilterQueryData(siteid, mlid,"62204",apiPassword,foo);




        }
    }
}
