using System;
using System.Collections.Generic;
using System.Text;

namespace NbcOlympicsWeeklyReporting
{
    class Email
    {
        public static void CreateEmail(string file, string errorMessages)
        {

            // Create a mailman object for sending email.
            Chilkat.MailMan mailman = new Chilkat.MailMan();

            // Any string argument automatically begins the 30-day trial.
            mailman.UnlockComponent("30-day trial");

            // Set the SMTP server hostname.
            mailman.SmtpHost = "guna.corp.lyris.com";

            // Create a simple email.
            Chilkat.Email email = new Chilkat.Email();

            email.Body = string.Concat("The following exception was throw",
                " in today's reporting\n\n",errorMessages);
            email.Subject = "NBC Olympics Weekly Reporting Error Notification";
            //email.Subject = "This is a test, so please ignore";
            email.AddTo("Aaron Harmon", "Aaron.Harmon@nbcuni.com");
            email.AddCC("Susan Barber", "sbarber@lyris.com");
            email.AddCC("David Walsh", "dwalsh@lyris.com");
            email.AddCC("Brian Mueller", "bmueller@lyris.com");
            email.From = "Brian Mueller <bmueller@lyris.com>";

            // Now add a container to use to add in the file attachments.
            string contentType = email.AddFileAttachment(file);

            bool success;
            success = mailman.SendEmail(email);
            if (success)
            {

                //Console.WriteLine("sent mail");
                Log.WriteLine("email successfulll sent");
            }
            else
            {

                //Console.WriteLine(mailman.LastErrorText);
                Log.WriteLine(mailman.LastErrorText);
            }

            

        }

        public static void CreateErrorMessageBody(string latestError)
        {
            DateTime today = DateTime.Now;
            string todayDate = today.ToString("yyyy-MM-dd");

            if (latestError != "")
            {
                
                CreateEmail(@"c:\lm_custom\Weekly_Reporting_log" + todayDate + ".txt", latestError);

            }

           
        }


    }
}
