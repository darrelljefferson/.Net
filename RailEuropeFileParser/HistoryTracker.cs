using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;

namespace RailEuropeFileParser
{
    class HistoryTracker
    {
        public static SqlCeConnection con = new SqlCeConnection();
        public static SqlCeConnection con2 = new SqlCeConnection();

        public static void Init()
        {

            string connection = @"Data Source=C:\Documents and Settings\djefferson\My Documents\Visual Studio 2010\Projects\RailEuropeFileParser\EmailTracker.sdf; Max Database Size=4000;Persist Security Info=false";  


              con.ConnectionString = connection;
              con2.ConnectionString = connection;
              try
              {
                  con.Open();
                  con2.Open();
              }
              catch (SqlCeException e)
              {
                  Console.WriteLine("Open failed " + e.ToString());
              }

        }

        public static Boolean DetermineIfEmailToSend(string email, string datafile)
        {
            try
            {
                string selstmt1 = String.Concat("SELECT count(*) as total FROM [email_history_data] as a WHERE email = ", "'", email, "'",
                  " AND datafile=", "'", datafile, "'");

                SqlCeCommand selcmd = new SqlCeCommand(selstmt1, con);

                int email_ct = 0;
                selcmd.Parameters.AddWithValue("@email", email);
                selcmd.Parameters.AddWithValue("@datafile", datafile);

                SqlCeDataReader reader = selcmd.ExecuteReader();
                while (reader.Read())
                {
                    email_ct = reader.GetInt32(0);
                    Console.WriteLine(email_ct);
                }


                if (email_ct == 0)
                    return true;

            }
            catch (SqlCeException e)
            {
                Console.WriteLine("Select Failed for prelookup " + e.ToString());
            }



            DateTime max_lst_day_sent = DateTime.Now.AddDays(+15);
            string selstmt = String.Concat("SELECT count(*) as total FROM [email_history_data] as a WHERE email = ", "'", email, "'",
                " AND datafile=", "'", datafile, "'", " AND send_dt = (select max(send_dt) FROM [email_history_data] as b ",
                                                              " WHERE a.email = b.email AND a.datafile = b.datafile",
                                                              " AND a.send_dt >= ", "'", max_lst_day_sent, "')");

            try
            {
                SqlCeCommand selcmd = new SqlCeCommand(selstmt, con);

                int email_ct = 0;
                selcmd.Parameters.AddWithValue("@email", email);
                selcmd.Parameters.AddWithValue("@max_lst_day_sent", max_lst_day_sent);
                selcmd.Parameters.AddWithValue("@datafile", datafile);

                        SqlCeDataReader reader = selcmd.ExecuteReader();
                        while (reader.Read())
                        {
                            email_ct = reader.GetInt32(0);
                            Console.WriteLine(email_ct);
                        }


                if (email_ct > 0) 
                    return true;
                else
                    return false;

            }

            catch (SqlCeException e)
            {

                Console.WriteLine("Select failed " + e.ToString());
                return false;

            }


        }
        

        public static void AddEmailAddr(string email, string datafile)
        {
            string insstmt = String.Concat("insert into [email_history_data] (email, send_dt, datafile)",
                         " values ( @email,  @send_dt , @datafile)");

            try
            {

                using (SqlCeCommand inscmd = new SqlCeCommand("insert into email_history_data (email, send_dt, datafile) " +
                          " values ( @email,  @send_dt , @datafile)", con))
                {
                    DateTime send_dt = DateTime.Now;
                    inscmd.Parameters.AddWithValue(@"email", email);
                    inscmd.Parameters.AddWithValue(@"send_dt", send_dt);
                    inscmd.Parameters.AddWithValue(@"datafile", datafile);
                    int results = inscmd.ExecuteNonQuery();
                } 

            }

            catch (SqlCeException e)
            {

                Console.WriteLine("Insert failed " + e.ToString());

            }
            catch (EntitySqlException e)
            {
                Console.WriteLine("Insert failed2 " + e.ToString());
            }



        }



        public static void PurgeOldEmails()
        {



        }


    }



}
