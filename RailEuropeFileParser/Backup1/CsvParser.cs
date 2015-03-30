using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RailEuropeFileParser
{
    class CsvParser
    {
        public static void ParseAndCreateNewCsvFile (string workingFile,
            Dictionary<string,string[]> email, string newFileName, string triggerID,
            Boolean hasOrderID)
        {
            StreamReader reader = new StreamReader(workingFile);

            
            using(StreamWriter file = new StreamWriter(newFileName))
            {

                
                string seperator = ",";
                
                // read in the header
                string line = reader.ReadLine();

                string[] value = line.Split(',');

                // change the first header to email
                value[0] = "email";

                // delare the csvLine
                string csvLine = "";

                // include orderid if we're working on scenario 5
                if (hasOrderID == true)
                {
                    // build the line
                    // and remove the double quotes
                    csvLine = string.Concat(value[0].Replace("\"",""),
                        seperator, value[2].Replace("\"",""));
                }
                // if not, then build it using the name
                else
                {
                    // build the line
                    csvLine = string.Concat(value[0],
                        seperator,value[1].Replace("\"",""));
                }

                // write the line to file
                file.WriteLine(csvLine);

                // read throught the rest of the file and write the new line

                while (reader.EndOfStream == false)
                {
                    // read in the line
                    value = reader.ReadLine().Split(',');

                    // run a method make sure it will pass the el email validator
                    bool isValidEmail = EmailLabsEmailValidator(value[0].Replace("\"",""));

                    // check to make sure not a blacklisted email
                    if (value[0].Replace("\"", "") == "spamdump@earthlink.net")
                    {
                        isValidEmail = false;
                    }


                    // only add rows if the email doesn't exist in the dictionary
                    if (email.ContainsKey(value[0]) == false)
                    {

                        // check to see if the value is unknown and overwrite
                        if (value[1].Replace("\"","").ToLower() == "unknown" ||
                            value[1].Replace("\"","").ToLower() == "undefined")
                        {
                            value[1] = "";
                        }

                        // include orderid if we're working on scenario 5
                        // since using many recipients, only add to hte file if the email is valid
                        if (hasOrderID == true && isValidEmail == true)
                        {
                            
                            // build the line
                            csvLine = string.Concat(value[0].Replace("\"",""),
                                seperator,value[2].Replace("\"",""));
                            
                        }
                        // if not, then build it using the name
                        else if (hasOrderID == false)
                        {
                            // build the line
                            csvLine = string.Concat(value[0].Replace("\"", ""),
                            seperator, value[1].Replace("\"",""));
                        }

                        // write the line to file
                        file.WriteLine(csvLine);

                        // add the key value to the dictionary
                        email.Add(value[0], value);

                    }

                }
            }           
        }

        public static void ParseAndSendMessages(string file, string siteid,
            string mlid, string triggerid, string apiPassword, Boolean hasMultipleEmails,
            string ftpFilePath)
        {
            StreamReader reader = new StreamReader(file);

            // ignore the file headers
            string line = reader.ReadLine();

            Boolean needsComma = false;

            string recipients = "";

            while (reader.EndOfStream == false)
            {
                if (needsComma == true & hasMultipleEmails == true)
                {
                    // apend the comma before the value
                    recipients += ",";
                }

                // read in the line and split it
                string[] value = reader.ReadLine().Split(',');

                // if we don't care about the file, then just send messages one at a time
                if (hasMultipleEmails == false)
                {
                    SendEmail.SendSingeRecipientEmails(siteid, mlid, triggerid,
                        value[0], apiPassword);
                }
                // if we do care, then just append in the email to the string
                else
                {
                    recipients += value[0];
                }

                // change the value to true for the next line
                needsComma = true;

            }

            // now send to the remaining recipients

            // send this only if we want to send to multiple emails
            if (hasMultipleEmails == true)
            {
                //SendEmail.SendMultipleRecipientsEmail(siteid, mlid, triggerid, recipients,
                //    ftpFilePath, apiPassword);
                SendEmail.SendEmailsPostRequest(siteid, mlid, triggerid, recipients,
                    ftpFilePath, apiPassword);
            }
        }

        public static Boolean EmailLabsEmailValidator(string email)
        {
            Boolean isValid = true;

            string[] domainParts = { "aero", "arpa", "asia", "biz", "cat", "com", 
                                       "coop", "edu", "gov", "info", "int", "jobs", 
                                       "mil", "mobi", "museum", "name", "net", "org", 
                                       "pro", "travel" };

            string[] countryCodes = { "ac", "ad", "ae", "af", "ag", "ai", "al", "am", 
                                        "an", "ao", "aq", "ar", "as", "at", "au", "aw", 
                                        "az", "ba", "bb", "bd", "be", "bf", "bg", "bh", 
                                        "bi", "bj", "bm", "bn", "bo", "br", "bs", "bt", 
                                        "bv", "bw", "by", "bz", "ca", "cc", "cd", "cf", 
                                        "cg", "ch", "ci", "ck", "cl", "cm", "cn", "co", 
                                        "cr", "cu", "cv", "cx", "cy", "cz", "de", "dj", 
                                        "dk", "dm", "do", "dz", "ec", "ee", "eg", "er", 
                                        "es", "et", "eu", "fi", "fj", "fk", "fm", "fo", 
                                        "fr", "ga", "gb", "gd", "ge", "gf", "gg", "gh", 
                                        "gi", "gl", "gm", "gn", "gp", "gq", "gr", "gs", 
                                        "gt", "gu", "gw", "gy", "hk", "hm", "hn", "hr", 
                                        "ht", "hu", "id", "ie", "il", "im", "in", "io", 
                                        "iq", "ir", "is", "it", "je", "jm", "jo", "jp", 
                                        "ke", "kg", "kh", "ki", "km", "kn", "kr", "kw", 
                                        "ky", "kz", "la", "lb", "lc", "li", "lk", "lr", 
                                        "ls", "lt", "lu", "lv", "ly", "ma", "mc", "md", 
                                        "me", "mg", "mh", "mk", "ml", "mm", "mn", "mo", 
                                        "mp", "mq", "mr", "ms", "mt", "mu", "mv", "mw", 
                                        "mx", "my", "mz", "na", "nc", "ne", "nf", "ng", 
                                        "ni", "nl", "no", "np", "nr", "nu", "nz", "om", 
                                        "pa", "pe", "pf", "pg", "ph", "pk", "pl", "pm", 
                                        "pn", "pr", "ps", "pt", "pw", "py", "qa", "re", 
                                        "ro", "rs", "ru", "rw", "sa", "sb", "sc", "sd", 
                                        "se", "sg", "sh", "si", "sj", "sk", "sl", "sm", 
                                        "sn", "so", "sr", "st", "su", "sv", "sy", "sz", 
                                        "tc", "td", "tf", "tg", "th", "tj", "tk", "tl", 
                                        "tm", "tn", "to", "tp", "tr", "tt", "tv", "tw", 
                                        "tz", "ua", "ug", "uk", "um", "us", "uy", "uz", 
                                        "va", "vc", "ve", "vg", "vi", "vn", "vu", "wf", 
                                        "ws", "ye", "yt", "yu", "za", "zm", "zw" };

            // shift the address to lower
            email = email.ToLower();

            // split the email on the @ smbol
            string[] emailparts = email.Split('@');

            // do a basic check to see if there are just 2 parts
            // also not null
            if (emailparts.Length != 2 || emailparts[0] == "" || emailparts[1] == "")
            {
                isValid = false;
                Log.WriteLine("the following email address failed basic validation: " + email);
                return isValid;
            }

            // do a regex on the username part
            string pattern = @"\w+([-+.]\w+)*";
            Regex emailCheck = new Regex(pattern);
            isValid = emailCheck.IsMatch(emailparts[0]);
            
            if (isValid == false)
            {
                // failed validation, so now log it
                Log.WriteLine("the following email address failed username validation: " + email);
                return isValid;
            }

            // now on the domain part
            pattern = @"\w+([-.]\w+)*\.\w+([-.]\w+)*";
            isValid = emailCheck.IsMatch(emailparts[0]);
            if (isValid == false)
            {
                // failed validation, so now log it
                Log.WriteLine("the following email address failed domain validation: " + email);
                return isValid;
            }

            // check the length, which can't be > 50
            if (email.Length > 50)
            {
                isValid = false;
                // failed validation, so now log it
                Log.WriteLine("the following email address failed email length validation: " + email);
                return isValid;
            }

            // the domain can't have more than 1 period it, so check
            string[] subdomains = emailparts[1].Split('.');

            // check to see if there are more than 2 sub domain
            if (subdomains.Length > 2)
            {


                
                isValid = false;
                // failed validation, so now log it
                Log.WriteLine("the following email address failed sub domain validation: " + email);
                return isValid;
                
            }

            // find the last postion
            int lastPostion = subdomains.Length - 1;

            //reset the flag
            isValid = false;

            // check the last part against the arrays to see if they don't mach
            foreach (string item in domainParts)
            {


                if (subdomains[lastPostion].ToLower() == item)
                {
                    isValid = true;
                }
            }

            foreach (string item in countryCodes)
            {
                // return any items that aren't in the list
                if (subdomains[lastPostion].ToLower() == item)
                {
                    isValid = true;
                }
            }

            

            // log it failed validation and reset the flag back to true

            if (isValid == false)
            {
                Log.WriteLine("The following email address failed domain name " +
                    "and country code valiation: " + email);
            }

           

            // we passed all the validation, so return true
            return true;



        }

       
    }
}
