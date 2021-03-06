﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Chilkat.MailMan mailman = new Chilkat.MailMan();

            // Any string argument automatically begins the 30-day trial.
            bool mailflag = mailman.UnlockComponent("LYRISCHttp_zUmhzUf4XV1i ");

            // Set the SMTP server hostname.
            mailman.SmtpHost = "EXVMBX015-4.exch015.msoutlookonline.net";

            // Create a simple email.
            Chilkat.Email email = new Chilkat.Email();

            email.Body = "This is the body";
            email.Subject = "This is the subject";
            email.AddTo("Chilkat Support", "darrelljefferson@hotmail.com");
            email.From = "Darrell Jefferson <djefferson@lyris.com>";

            // Send mail.
            bool success;
            success = mailman.SendEmail(email);
            if (success)
            {
                Console.WriteLine("Sent mail!");
            }
            else
            {
                Console.WriteLine(mailman.LastErrorText);
            }

        }
    }

}