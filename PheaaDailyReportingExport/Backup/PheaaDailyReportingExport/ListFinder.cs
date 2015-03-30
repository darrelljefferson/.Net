using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace PHEAA
{
    class ListFinder
    {
        public string[] listFinder()
        {
            string[] lists = new string[100];
            string siteID = "23394";
            string password = "87h3jps";

            // string to build the api call
            string recordQueryListDataString = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?type=list&activity=Query-Listdata",
                "&input=%3CDATASET%3E",
                "%3CSITE_ID%3E", siteID, "%3C/SITE_ID%3E",
                "%3CDATA%20type=%22extra%22%20id=%22password%22%3E", password, "%3C/DATA%3E",
                "%3C/DATASET%3E");

            //Error/Sanity############################################
            // declare the XDocument
            XDocument getRecords = new XDocument();
            // load the xml from the api call and check for error
            try
            {
                getRecords = XDocument.Load(recordQueryListDataString);
            }

            catch (Exception e)
            {
                using (StreamWriter sw = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "errlog.txt"))
                {
                    sw.WriteLine("The following execption was throw trying to load the message_Query-standard-message-report API call: " + e.Message);
                }
            }

            // since the api returned back for errors can be either upper or lower case, we have to check which one it is
            // assume upper case and adjust if we find lower case
            string dataset = "DATASET";
            string type = "TYPE";
            string data = "DATA";

            // load the xml as a string
            string checkXML = getRecords.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned

            if ((string)getRecords.Element(dataset).Element(type).Value == "error")
            {
                using (StreamWriter sw = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "errlog.txt"))
                {
                    sw.WriteLine(string.Concat("API call for siteid: ", siteID, " failed"));
                    sw.WriteLine("The following api call that failed: \n" + recordQueryListDataString);
                    sw.WriteLine("The failure reason was: " + getRecords.Element(dataset).Element(data).Value);
                    sw.WriteLine("");
                }
            }

            else if ((string)getRecords.Element(dataset).Element(type).Value == "norecords")
            {
                using (StreamWriter sw = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "errlog.txt"))
                {
                    sw.WriteLine(string.Concat("API call for siteid: ", siteID, " has no records"));
                    sw.WriteLine("The following api call has no records: \n" + recordQueryListDataString);
                    sw.WriteLine("");
                }

            }
            //Error Logging /Sanity############################################


            IEnumerable<XElement> elList =
                from el in getRecords.Descendants("DATA")
                select el;


            string listname = "";
            string listid = "";
            string status = "";

            int a = 0;


            foreach (XElement el in elList)
            {
                // populate in the demographic name field
                if ((string)el.Attribute("type") == "name")
                {
                    listname = el.Value.ToString();
                    listid = el.Attribute("id").ToString();
                    listid = Regex.Replace(listid, "[^0-9]", "");

                    Console.WriteLine(listname);
                    Console.WriteLine(listid);
                }
                // populate in the demographic ID field
                if ((string)el.Attribute("type") == "status")
                {
                    status = el.Value.ToString();
                    Console.WriteLine(status);

                    if (status == "active")
                    {
                        lists[a] = listid;
                        a++;
                        lists[a] = listname;
                        a++;
                    }

                }

            }

            return lists;
        }
    }
}
