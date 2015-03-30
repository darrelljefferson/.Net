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
    class PersonsLookup
    {

        public void personsLookup(string mlid, string listname)
        {
            StringBuilder currentline = new StringBuilder();
            System.Text.StringBuilder res = new System.Text.StringBuilder();

            string reportfilename = System.AppDomain.CurrentDomain.BaseDirectory + listname + ".csv";
            FileStream fs2;
            fs2 = new FileStream((reportfilename), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (StreamWriter sw2 = new StreamWriter(fs2))
            {//open stream writer

                

                string siteID = "23394";
                string password = "87h3jps";
                bool fin = false;
                int page = 1;
                string email = "";
                string demoval = "";
                string demoid = "";
                string[] userarry = new string[200];

                //queury demographics for the list we're doing now
                string[,] demos = new string[2, 200];
                DemoIDFind demoget = new DemoIDFind();
                demos = demoget.demoIDfinder(mlid);
                demoget = null;
                GC.Collect();


                //WRITE DEMO HEADER
                int x = 0;
                currentline.Append("\"" + "Email Address" + "\"" + ",");
                while (demos[1,x+1] != null)
                {
                currentline.Append("\"" + demos[1,x] + "\"" + ",");
                x++;
                }
                currentline.Append("\"" + demos[1, x] + "\"");

                int democount = x;
                sw2.WriteLine(currentline.ToString());
                currentline = new StringBuilder();


                while (fin == false)
                {
                    // string to build the api call
                    string recordQueryListDataString = String.Concat("https://www.elabs7.com/API/",
                        "mailing_list.html?type=record&activity=Query-Listdata",
                        "&input=%3CDATASET%3E",
                        "%3CSITE_ID%3E", siteID, "%3C/SITE_ID%3E",
                        "%3CMLID%3E", mlid, "%3C/MLID%3E",
                        "%3CDATA%20type=%22extra%22%20id=%22password%22%3E", password, "%3C/DATA%3E",
                        "%3CDATA%20type=%22extra%22%20id=%22page%22%3E", page.ToString(), "%3C/DATA%3E",
                        "%3CDATA%20type=%22extra%22%20id=%22pagelimit%22%3E10000%3C/DATA%3E",
                        "%3CDATA%20type=%22extra%22%20id=%22type%22%3Eactive%3C/DATA%3E",
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
                            sw.WriteLine(string.Concat("API call for siteid: ", siteID, " mlid: ", mlid, " failed"));
                            sw.WriteLine("The following api call that failed: \n" + recordQueryListDataString);
                            sw.WriteLine("The failure reason was: " + getRecords.Element(dataset).Element(data).Value);
                            sw.WriteLine("");
                        }
                    }

                    else if ((string)getRecords.Element(dataset).Element(type).Value == "norecords")
                    {
                        using (StreamWriter sw = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "errlog.txt"))
                        {
                            sw.WriteLine(string.Concat("API call for siteid: ", siteID, " mlid: ", mlid, " has no records"));
                            sw.WriteLine("The following api call has no records: \n" + recordQueryListDataString);
                            sw.WriteLine("");
                        }
                    }
                    //Error Logging /Sanity############################################

                    IEnumerable<XElement> elList =
                    from el in getRecords.Descendants("DATA")
                    select el;



                    fin = true;
                    currentline = new StringBuilder();


                        foreach (XElement el in elList)
                        {
                            //if we get into the loop, then there is still data, otherwise the loop will exit from being set tryue
                            fin = false;

                            // pull in the email
                            if ((string)el.Attribute("type") == "email")
                            {
                                x=0;
                                if(currentline.Length != 0) // this prevents a blank line at the start of a new page
                                {
                                    while(x<democount)
                                    {
                                        currentline.Append("\"" + userarry[x] + "\"" + ",");
                                        x++;
                                    }
                                    currentline.Append("\"" + userarry[x] + "\"");

                                sw2.WriteLine(currentline.ToString());
                                }

                                currentline = new StringBuilder();
                                x = 0;
                                email = el.Value.ToString();
                                //debug lines
                                Console.WriteLine(email);
                                currentline.Append("\"" + email + "\"" + ",");
                            }
                            // pull in the demographic and it's ID
                            if ((string)el.Attribute("type") == "demographic")
                            {
                                demoval = el.Value.ToString();
                                //escape double quotes for CSV
                                demoval = demoval.Replace("\"", "\"\"");
                                demoid = el.Attribute("id").ToString();
                                demoid = Regex.Replace(demoid, "[^0-9]", "");
                                //debug
                                Console.WriteLine(demoval);
                                Console.WriteLine(demoid);

                                //search for the matching demoid and write into array in proper order
                                x = 0;
                                while (demos[0,x] != demoid)
                                {
                                    x++;
                                }
                                userarry[x] = demoval;


                            }

                        }
                        //write in the last record of the page
                        if (fin == false)
                        {   
                            //otherwise last line is double written
                            x = 0;
                            while (x < democount)
                            {
                                currentline.Append("\"" + userarry[x] + "\"" + ",");
                                x++;
                            }
                            currentline.Append("\"" + userarry[x] + "\"");
                            sw2.WriteLine(currentline.ToString());
                        }
                        //on to the next page
                        page++;
                    }
                
            }//close stream writer
            return;
        }


    }
}
