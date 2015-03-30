using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.SqlClient;
using System.Data;
using System.Data.SqlServerCe;

namespace SDFaPP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Machine is : " + Environment.MachineName);

           // string conString = Properties.Settings.Default.Database2ConnectionString;



            string conString = String.Concat( @"Data Source=c:\documents and settings\djefferson\my documents\visual studio 2010\Projects\SDFaPP\SDFaPP\Database2.sdf;",
                                             " Persist Security Info=false");

            string dbfile = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\Database2.sdf"; 


            //SqlCeConnection con = new SqlCeConnection("datasource=" + dbfile); 
            
           
             try
                {

                    using (SqlCeConnection con = new SqlCeConnection(conString))
                    {
                        con.Open();

                        // Insert into the SqlCe table. ExecuteNonQuery is best for inserts.
                        int num = 5;
                        using (SqlCeCommand com = new SqlCeCommand("INSERT INTO email_history_data VALUES(@email, @datafile, @send_dt)", con))
                        {
                            string emailaddr = "sandrymulton@yahoo.com";
                            string datafile = "testfileRailEurope";
                            DateTime send_dt =  DateTime.Now;
                            com.Parameters.AddWithValue("@email", emailaddr);
                            com.Parameters.AddWithValue("@datafile", datafile);
                            com.Parameters.AddWithValue("@send_dt", send_dt);
                            int results = com.ExecuteNonQuery();
                            con.Close();
                        }
                    }

                   // con.Open();
                   // string db = con.Database;
                   // string emailaddr = "test.com";
                   // string insstr = "insert into [email_history_data] values ('djefferson@hotmail.com')";
                   //// SqlCeDataAdapter adapter = new SqlCeDataAdapter("select * from email_history_data", con);
                   // SqlCeDataAdapter adapter = new SqlCeDataAdapter(insstr, con);
                   // DataSet data = new DataSet(); 
                   // adapter.Fill(data);

                   // // Add a row to the test_table (assume that table consists of a text column)  
                   // data.Tables[0].Rows.Add(new object[] { "djefferson@hotmail.com" });          // Save data back to the databasefile    
                   ////adapter.Update(data);
                   //adapter.Update(data);
                }
                catch (SqlCeException e)
                {

                    Console.WriteLine("failed " + e.ToString());
                }
              
            }

         

        }
    }
