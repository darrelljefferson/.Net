using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;


namespace PheaaDailyReportingExport
{
    class ReportBuilder
    {

        
        // method to Figure out which emails were sent, opened, clicked
        // bounce, or unsubed from a trigger and return a list with the
        // email lists in them
        public static void ParseTriggerDetailedData(string siteid,
            string mlid, string triggerID, string startDate,string endDate,
            string password,List<string[]> sentEmails,
            List<string[]> openEmails, List<string[]> clickEmails,
            List<string[]> bounceEmails,
            List<string[]> unsubEmails, List<string> emailList)
        {
            
            
            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA TYPE=\"extra\" ID=\"trigger_id\">", triggerID, "</DATA>",
                "<DATA TYPE=\"extra\" ID=\"details\">", "detailed", "</DATA>",
                "<DATA type=\"extra\" id=\"start_date\">", startDate, "</DATA>",
                "<DATA type=\"extra\" id=\"end_date\">", endDate, "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            //Log.WriteLine(datasetInput);

            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            //Log.WriteLine(encodedInput);

            string triggerQueryData = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=triggers&",
                "activity=Query-Data&",
                "&input=", encodedInput);

                       // load API Xml into a XDocument
            XDocument getTriggerQueryDataResults = new XDocument();         
            // load the xml from the api call and check for errors
            try
            {
                getTriggerQueryDataResults = XDocument.Load(triggerQueryData);
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
            string checkXML = getTriggerQueryDataResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getTriggerQueryDataResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + triggerQueryData);
                Log.WriteLine("The failure reason was: " + getTriggerQueryDataResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getTriggerQueryDataResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + triggerQueryData);
                Log.WriteLine("");
            }
            // if no errors, then continue with the call
            else
            {
                // load it into a Enumerable interface element
                IEnumerable<XElement> elList =
                    from el in getTriggerQueryDataResults.Descendants("DATA")
                    select el;


                // iterate through the elements and then check to see if the demographic is populated

                // declare the email string
                string email = "";


                foreach (XElement el in elList)
                {
                    

                    // set the email
                    if ((string)el.Attribute("type") == "email_address")
                    {
                        // set the Email Value
                        email = el.Value.ToString();
                        
                        
                    }
                    // only add values if they are greater than 0
                    // add to the sent list
                    else if ((string)el.Attribute("type") == "total_sent"
                        && Convert.ToInt16(el.Value.ToString()) > 0)
                    {

                        // declare the array to be used in the dictionary
                        // only set the emial and triggerid's for now
                        string[] emailTriggerID = new string[3];
                        emailTriggerID[0] = email;
                        emailTriggerID[1] = triggerID;
                        sentEmails.Add(emailTriggerID);
                        
                        // only add to the master email list if the email doesn't
                        // already exist
                        if (emailList.Contains(email) == false)
                        {
                            emailList.Add(email);
                        }
                    }
                    // add to the opens list
                    else if ((string)el.Attribute("type") == "total_opened"
                        && Convert.ToInt16(el.Value.ToString()) > 0)
                    {
                        // declare the array to be used in the dictionary
                        // only set the emial and triggerid's for now
                        string[] emailTriggerID = new string[3];
                        emailTriggerID[0] = email;
                        emailTriggerID[1] = triggerID;
                        
                        openEmails.Add(emailTriggerID);
                        // only add to the master email list if the email doesn't
                        // already exist
                        if (emailList.Contains(email) == false)
                        {
                            emailList.Add(email);
                        }
                    }
                    // add to the clicks list
                    else if ((string)el.Attribute("type") == "total_clicked"
                        && Convert.ToInt16(el.Value.ToString()) > 0)
                    {

                        // declare the array to be used in the dictionary
                        // only set the emial and triggerid's for now
                        string[] emailTriggerID = new string[3];
                        emailTriggerID[0] = email;
                        emailTriggerID[1] = triggerID;
                        
                        clickEmails.Add(emailTriggerID);
                        // only add to the master email list if the email doesn't
                        // already exist
                        if (emailList.Contains(email) == false)
                        {
                            emailList.Add(email);
                        }
                    }
                    // add to the unsub list
                    else if ((string)el.Attribute("type") == "unsubscribed"
                        && Convert.ToInt16(el.Value.ToString()) > 0)
                    {

                        // declare the array to be used in the dictionary
                        // only set the emial and triggerid's for now
                        string[] emailTriggerID = new string[3];
                        emailTriggerID[0] = email;
                        emailTriggerID[1] = triggerID;
                        
                        unsubEmails.Add(emailTriggerID);
                        // only add to the master email list if the email doesn't
                        // already exist
                        if (emailList.Contains(email) == false)
                        {
                            emailList.Add(email);
                        }
                    }
                    // add to the bounce list and then reset email value
                    // this ia always the last of each xml element
                    else if ((string)el.Attribute("type") == "bounced"
                        && Convert.ToInt16(el.Value.ToString()) > 0)
                    {
                        // declare the array to be used in the dictionary
                        // only set the emial and triggerid's for now
                        string[] emailTriggerID = new string[3];
                        emailTriggerID[0] = email;
                        emailTriggerID[1] = triggerID;
                        
                        bounceEmails.Add(emailTriggerID);
                        // only add to the master email list if the email doesn't
                        // already exist
                        if (emailList.Contains(email) == false)
                        {
                            emailList.Add(email);
                        }
                        email = "";
                    }
                }

                
            }

            

        }
        
        
        // method to build up the demographic's for each email address / list combo
        public static Dictionary<string, string[]> ParseDemographicValues(string siteid, string mlid,
            string password, string email, string[] demographicIds, 
            Dictionary<string,string[]> emailDemographicLookup, 
            Dictionary<string,string[]> demographicIDLookup)
        {
        

            // create the array to be used to store the demographic values
            string[] demographicValues = new string[5];
            
            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<DATA TYPE=\"EMAIL\" ID=\"START_DATE\">",email, "</DATA>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            //Log.WriteLine(datasetInput);
            
            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            //Log.WriteLine(encodedInput);
            
            string recordQueryData = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=record&",
                "activity=Query-Data&",
                "&input=",encodedInput);


            //Log.WriteLine(recordQueryData);

            // load API Xml into a XDocument
            XDocument getRcordQueryDataResults = new XDocument();         
            // load the xml from the api call and check for errors
            try
            {
                getRcordQueryDataResults = XDocument.Load(recordQueryData);
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
            string checkXML = getRcordQueryDataResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + recordQueryData);
                Log.WriteLine("The failure reason was: " + getRcordQueryDataResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getRcordQueryDataResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + recordQueryData);
                Log.WriteLine("");
            }
            // if no errors, then continue with the call
            // declare the int array to use to keep track of the counts

            else
            {
         
                // load it into a Enumerable interface element
                IEnumerable<XElement> elList =
                    from el in getRcordQueryDataResults.Descendants("DATA")
                    select el;


                // we'll use to keep track how how many items we parsed
                // and also use module to know which element to populate into the corresponding array
                int count = 0;

                // iterate through the elements and then check to see if the demographic is populated

                foreach (XElement el in elList)
                {
                    XAttribute name = el.Attribute("id");
                    
                    // this is always the first xml element, so reset the count
                    if ((string)el.Attribute("type") == "extra" && (string)el.Attribute("id") == "state")
                    {
                        count = 0;
                        //this should only happen the first time
                        //add the email, and demographic array to the dictionary
                        //if it doesn't already exist
                        if (email != "" & !emailDemographicLookup.Keys.Contains(email))
                        {
                            emailDemographicLookup.Add(email, demographicValues);
                        }
                    }
                    
                    // populate in the email field
                    else if ((string)el.Attribute("type") == "extra"
                        && (string)el.Attribute("id") == "email")
                    {

                        email = el.Value.ToString();
                        Console.WriteLine(email);

                    }

                    /*
                    // array could be up to 4, so we do this else if up to 4 times
                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == demographicIds[count]
                        && count < demographicIds.Length)
                    {
                        //this should be the first element of the array
                        //add it to the 1st element of the demographic value array
                        demographicValues[count] = el.Value.ToString();
                        Console.WriteLine(demographicValues[count]);
                        //increment the count
                        count++;
                    }
                    */

                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == "1")
                    {
                        string[] value = {"first name",el.Value.ToString()};
                        demographicIDLookup["1"] = value;
                        demographicValues[0] = el.Value.ToString();
                        
                    }

                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == "2")
                    {
                        string[] value = { "last name", el.Value.ToString() };
                        demographicIDLookup["2"] = value;
                        demographicValues[1] = el.Value.ToString();
                    }

                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == "53418")
                    {
                        string[] value = { "UNQUID", el.Value.ToString() };
                        demographicIDLookup["53418"] = value;
                        demographicValues[2] = el.Value.ToString();
                    }

                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == "52534")
                    {
                        string[] value = { "ARC", el.Value.ToString() };
                        demographicIDLookup["52534"] = value;
                        demographicValues[3] = el.Value.ToString();
                    }

                    else if ((string)el.Attribute("type") == "demographic"
                        && (string)el.Attribute("id") == "33630")
                    {
                        string[] value = { "JOIN_DATE", el.Value.ToString() };
                        demographicIDLookup["33630"] = value;
                        demographicValues[4] = el.Value.ToString();
                    }
                    

                    





                }

                return emailDemographicLookup;

            }

            // if we errored on the call, return it anyways
            return emailDemographicLookup;
        
        }

        public static Dictionary<string,string[]> BuildEmailDemographicLookup(
            string siteid, string mlid ,string password,string[] demographicIds,
            List<string> emailList, Dictionary<string,string[]> demographicIDLookup)
        {
            // create the Dictionary element to be passed back later
            Dictionary<string,string[]> emailLookup = new Dictionary<string,string[]>();

            // iterate through the list
            foreach (var el in emailList)
            {
                // add items to the Dictonary on each pass
                emailLookup = ParseDemographicValues(siteid, mlid, password,
                    el.ToString(), demographicIds, emailLookup,demographicIDLookup);
                Console.WriteLine(emailLookup.Count);
            }

            return emailLookup;
        }

        public static void CreateCsvFile(string filepath, string date, string listname,
            List<string[]> emails,
            Dictionary<string, string[]> emailDemographics, string[] demographicHeader,
            string type)
        {
            // create the file stream
            //FileStream file = new FileStream((filepath), FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            using (StreamWriter sw = new StreamWriter(filepath))
            // open the streamwriter
            {


                // use a comma deliter and have field in double quotes
                string seperator = "\",\"";

                // build the header string

                string header = "";
                // to use to make sure we don't add an extra comma
                int lastDemographicHeaderPosition = demographicHeader.Length - 1;


                header = string.Concat("\"", "email", seperator, "list_name",
                    seperator, "trigger_id", seperator, "date", seperator);

                
                // now add in the demographic header
                for (int i = 0; i < demographicHeader.Length; i++)
                {
                    // need to treat the last header differnetly
                    if (i != lastDemographicHeaderPosition)
                    {
                        header = string.Concat(header, demographicHeader[i], seperator);
                    }
                    else
                    {
                        header = string.Concat(header, demographicHeader[i], "\"");
                    }

                }
              
                // write the header file
                sw.WriteLine(header);


                // iterate through the lists to create teh appropriate csv file
                // only for click data will email array have a value for the 2nd position
                foreach (string[] item in emails)
                {
                    // for creating the csvs
                    string csvLine = "";


                    //build the base values
                    csvLine = string.Concat("\"", item[0], seperator,
                        listname, seperator, item[1], seperator,
                        date, seperator);

                    // make sure email is found in the dictionary
                    if (emailDemographics.Keys.Contains(item[0]))
                    {
                        // find the demographic array for the email key
                        string[] demographicValues = emailDemographics[item[0]];

                        
                        // use to make sure we don't add an extra comma to the last entry
                        int lastDemographicValuePosition = demographicValues.Length - 1;

                        // add in the demographics
                        for (int i = 0; i < demographicValues.Length; i++)
                        {
                            // need to treat the last header differnetly
                            if (i != lastDemographicValuePosition)
                            {
                                csvLine = string.Concat(csvLine, demographicValues[i], seperator);
                            }
                            else
                            {
                                csvLine = string.Concat(csvLine, demographicValues[i], "\"");
                            }

                        }//endfor

                        sw.WriteLine(csvLine);

                    }//endif

                }//endforeach


            }

        }

    
            
            /*
            // used later to make sure we only add lines for unique clicks
            List<string> uniqueURLs = new List<string>();

            // create the file stream
            //FileStream file = new FileStream((filepath), FileMode.Create, FileAccess.ReadWrite, FileShare.None);

            using (StreamWriter sw = new StreamWriter(filepath))
            // open the streamwriter
            {
                // use a comma deliter and have field in double quotes
                string seperator = "\",\"";

                
                
                // build the header string

                string header = "";
                // to use to make sure we don't add an extra comma
                int lastDemographicHeaderPosition = demographicHeader.Length - 1;

                if (type == "sent")
                {
                    header = string.Concat("\"", "email", seperator, "list_name",
                        seperator, "trigger_id", seperator);
                    
                    // now add in the demographic header
                    for (int i = 0; i < demographicHeader.Length; i++)
                    {
                        // need to treat the last header differnetly
                        if (i != lastDemographicHeaderPosition)
                        {
                            header = string.Concat(header, demographicHeader[i], seperator);
                        }
                        else
                        {
                            header = string.Concat(header, demographicHeader[i], "\"");
                        }

                    }

                }
                else if (type == "bounce")
                {
                    header = string.Concat("\"", "email", seperator, "list_name",
                        seperator, "message_id", seperator,"bounce_reason",seperator);

                    // now add in the demographic header
                    for (int i = 0; i < demographicHeader.Length; i++)
                    {
                        // need to treat the last header differnetly
                        if (i != lastDemographicHeaderPosition)
                        {
                            header = string.Concat(header, demographicHeader[i], seperator);
                        }
                        else
                        {
                            header = string.Concat(header, demographicHeader[i], "\"");
                        }

                    }

                }

                else if (type == "open")
                {
                    header = string.Concat("\"", "email", seperator, "list_name",
                        seperator, "message_id", seperator);

                    // now add in the demographic header
                    for (int i = 0; i < demographicHeader.Length; i++)
                    {
                        // need to treat the last header differnetly
                        if (i != lastDemographicHeaderPosition)
                        {
                            header = string.Concat(header, demographicHeader[i], seperator);
                        }
                        else
                        {
                            header = string.Concat(header, demographicHeader[i], "\"");
                        }

                    }

                }

                else if (type == "click")
                {
                    header = string.Concat("\"", "email", seperator, "list_name",
                        seperator, "message_id", seperator, "link_name", seperator);

                    // now add in the demographic header
                    for (int i = 0; i < demographicHeader.Length; i++)
                    {
                        // need to treat the last header differnetly
                        if (i != lastDemographicHeaderPosition)
                        {
                            header = string.Concat(header, demographicHeader[i], seperator);
                        }
                        else
                        {
                            header = string.Concat(header, demographicHeader[i], "\"");
                        }

                    }

                }

                // write the file header
                sw.WriteLine(header);
                sw.WriteLine("foo");

                // now build the csv lines
                

                // first in the header line first
                string line = filestream.ReadLine();

                Console.WriteLine(line);

                // need to make sure the url doesn't change
                string[] value = line.Split(',');

                if (type == "click" && value[6].ToLower() != "detail")
                {
                    Log.WriteLine("Error: Detail column not found");
                }

                // iterate through the remaining lines to then add rows to the final csv
                while (!filestream.EndOfStream)
                {
                    string csvLine = "";
                    
                    // read in the line
                    value = filestream.ReadLine().Split(',');

                    //need to deal with the different type
                    //should be email,listname,messageid,demographics
                    if (type == "sent")
                    {
                        // build the base values
                        csvLine = string.Concat("\"", value[1], seperator, listname,
                            seperator, value[0], seperator);

                        // make sure email is found in the dictionary
                        if (emailDemographics.Keys.Contains(value[1]))

                        {
                            // find the demographic array for the email key
                            string[] demographicValues = emailDemographics[value[1]];

                            // use to make sure we don't add an extra comma to the last entry
                            int lastDemographicValuePosition = demographicValues.Length - 1;

                            // add in the demographics
                            for (int i = 0; i < demographicValues.Length; i++)
                            {
                                // need to treat the last header differnetly
                                if (i != lastDemographicValuePosition)
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], seperator);
                                }
                                else
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], "\"");
                                }

                            }//endfor

                            sw.WriteLine(csvLine);

                        }//endif
                        
                    }//endif

                    else if (type == "bounce")
                    {
                        // build the base values
                        csvLine = string.Concat("\"", value[1], seperator, listname,
                            seperator, value[0], seperator, value[6], seperator);

                        // make sure email is found in the dictionary
                        if (emailDemographics.Keys.Contains(value[1]))
                        {
                            // find the demographic array for the email key
                            string[] demographicValues = emailDemographics[value[1]];

                            // use to make sure we don't add an extra comma to the last entry
                            int lastDemographicValuePosition = demographicValues.Length - 1;

                            // add in the demographics
                            for (int i = 0; i < demographicValues.Length; i++)
                            {
                                // need to treat the last header differnetly
                                if (i != lastDemographicValuePosition)
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], seperator);
                                }
                                else
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], "\"");
                                }

                            }//endfor

                            sw.WriteLine(csvLine);

                        }//endif

                    }//endif

                    else if (type == "open")
                    {
                        // build the base values
                        csvLine = string.Concat("\"", value[1], seperator, listname,
                            seperator, value[0], seperator);

                        // make sure email is found in the dictionary
                        if (emailDemographics.Keys.Contains(value[1]))
                        {
                            // find the demographic array for the email key
                            string[] demographicValues = emailDemographics[value[1]];

                            // use to make sure we don't add an extra comma to the last entry
                            int lastDemographicValuePosition = demographicValues.Length - 1;

                            // add in the demographics
                            for (int i = 0; i < demographicValues.Length; i++)
                            {
                                // need to treat the last header differnetly
                                if (i != lastDemographicValuePosition)
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], seperator);
                                }
                                else
                                {
                                    csvLine = string.Concat(csvLine, demographicValues[i], "\"");
                                }

                            }//endfor

                            sw.WriteLine(csvLine);

                        }//endif

                    }//endif

                    else if (type == "click")
                    {
                        
                        
                        //since email,messageid,and URL are unique, concat them
                        string uniqueEmailMessageid = string.Concat(value[1], value[0],value[6]);

                        //only write the line if we don't find a previous entry
                        if (uniqueURLs.Contains(uniqueEmailMessageid) == false)
                        {
                            // add the unique entry
                            uniqueURLs.Add(uniqueEmailMessageid);

                            // build the base values
                            csvLine = string.Concat("\"", value[1], seperator, listname,
                                seperator, value[0], seperator, value[6], seperator);

                            // make sure email is found in the dictionary
                            if (emailDemographics.Keys.Contains(value[1]))
                            {
                                // find the demographic array for the email key
                                string[] demographicValues = emailDemographics[value[1]];

                                // use to make sure we don't add an extra comma to the last entry
                                int lastDemographicValuePosition = demographicValues.Length - 1;

                                // add in the demographics
                                for (int i = 0; i < demographicValues.Length; i++)
                                {
                                    // need to treat the last header differnetly
                                    if (i != lastDemographicValuePosition)
                                    {
                                        csvLine = string.Concat(csvLine, demographicValues[i], seperator);
                                    }
                                    else
                                    {
                                        csvLine = string.Concat(csvLine, demographicValues[i], "\"");
                                    }

                                }//endfor

                                sw.WriteLine(csvLine);

                            }//endif

                        }//endif

                    }//endif
                }//end while loop

            }//end using
        */
        



        /*


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
         
                 // method to generate a csv link, to be used to get email address
        // for sent, bounce, click, and open data
        public static string GenerateCSVLink(string siteid, string mlid, string mid,
            string action,string startDate, string endDate,string password,
            Dictionary<string,DateTime> messageids)
        {

            // build the api call

            string datasetInput = String.Concat("<DATASET>",
                "<SITE_ID>", siteid, "</SITE_ID>",
                "<MLID>", mlid, "</MLID>",
                "<MID>",mid,"</MID>",
                "<ACTION>", action, "</ACTION>",
                "<ACTIVITY_START>",startDate, "</ACTIVITY_START>",
                "<ACTIVITY_END>",endDate, "</ACTIVITY_END>",
                "<DATA type=\"extra\" id=\"password\">", password, "</DATA>",
                "</DATASET>");

            Log.WriteLine(datasetInput);

            string encodedInput = utility.ReturnHTMLEnocodedString(datasetInput);

            Log.WriteLine(encodedInput);

            string queryActivity = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=message&",
                "activity=query-activity&",
                "&input=", encodedInput);


            Log.WriteLine(queryActivity);


            // load API Xml into a XDocument
            XDocument getCsvLink = new XDocument();
            // load the xml from the api call and check for errors
            try
            {
                getCsvLink = XDocument.Load(queryActivity);
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
            string checkXML = getCsvLink.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getCsvLink.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + queryActivity);
                Log.WriteLine("The failure reason was: " + getCsvLink.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getCsvLink.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + queryActivity);
                Log.WriteLine("");
            }
            // if not errors, then we just log the data results
            else
            {
                Log.WriteLine(getCsvLink.Element(dataset).Element(data).Value);
                return getCsvLink.Element(dataset).Element(data).Value;
            }

            return "";
        }
         * 
                public static Dictionary<string,DateTime> ParseMessageListData(string siteid, string mlid, 
            string startDate, string endDate, string password)
        {

            // create the dictionary object to be populated later and returned
            Dictionary<string, DateTime> messageidLookup = new Dictionary<string, DateTime>();
            
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
            
            string messageQueryListdata = String.Concat("https://www.elabs7.com/API/",
                "mailing_list.html?",
                "type=message&",
                "activity=Query-ListData&",
                "version=2",
                "&input=",encodedInput);


            Log.WriteLine(messageQueryListdata);

            // load API Xml into a XDocument
            XDocument getMessageQueryListResults = new XDocument();         
            // load the xml from the api call and check for errors
            try
            {
                getMessageQueryListResults = XDocument.Load(messageQueryListdata);
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
            string checkXML = getMessageQueryListResults.ToString();

            // now check to see case and re-populate the variables if they are lower case
            if (checkXML.Contains("dataset"))
            {
                dataset = dataset.ToLower();
                type = type.ToLower();
                data = data.ToLower();
            }

            // now check and log if we encouter an error or find no records returned
            if ((string)getMessageQueryListResults.Element(dataset).Element(type).Value == "error")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " failed"));
                Log.WriteLine("The following api call that failed: \n" + messageQueryListdata);
                Log.WriteLine("The failure reason was: " + getMessageQueryListResults.Element(dataset).Element(data).Value);
                Log.WriteLine("");
            }
            else if ((string)getMessageQueryListResults.Element(dataset).Element(type).Value == "norecords")
            {
                Log.WriteLine(string.Concat("API call for siteid: ", siteid, " mlid: ", mlid,
                    " has no records"));
                Log.WriteLine("The following api call has no records: \n" + messageQueryListdata);
                Log.WriteLine("");
            }
            // if no errors, then continue with the call
            // declare the int array to use to keep track of the counts
            
            else
            {

                
                // build linq query on the mid value
                var queryListData = from item in getMessageQueryListResults.Descendants("message")
                                select new
                                {
                                    messageid = item.Element("mid").Value,
                                    sendDate = Convert.ToDateTime(item.Element("date").Value),
                                    statsSent = item.Element("stats-sent").Value
                                };

            // now modify query to only find valid sent mailings
            var newQuery = from items in queryListData
                           where (Convert.ToDecimal(items.statsSent) > 0)
                           select items;

            
            // iterate through the message node and then run the populateData call to build the reporting Data table
            foreach (var item in newQuery)
            {
                Console.WriteLine(item.messageid.ToString()+","+item.sendDate.ToString());
                
                
                
                messageidLookup.Add(item.messageid.ToString(),item.sendDate);
                
            }

            return messageidLookup;
            }

            return messageidLookup;
        }

         
         
         
         */



    }
}
