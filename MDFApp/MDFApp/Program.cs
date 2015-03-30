using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MDFApp
{
    class Program
    {
        static void Main(string[] args)

        {
            Console.WriteLine("Machine is : " + Environment.MachineName);

            string connect = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            using (SqlConnection con = new SqlConnection(connect))
            {
            try
            {
                con.Open();
                string db = con.Database;
                string emailaddr = "test.com";
                string insstr = "insert into [email] values (" + "'" + @emailaddr + "'" + ")";
               
                SqlCommand cmdins = new SqlCommand(insstr, con);
                cmdins.Parameters.Add("email", SqlDbType.VarChar, 100);
                // cmdins.Parameters.AddWithValue(@"email", @emailaddr);
                cmdins.Parameters["email"].Value = emailaddr;
                int results = cmdins.ExecuteNonQuery();
                
                
            }
            catch (SqlException e)
            {

                Console.WriteLine("failed " + e.ToString());
            }
            con.Close();
        }
            


        }




    }
}
