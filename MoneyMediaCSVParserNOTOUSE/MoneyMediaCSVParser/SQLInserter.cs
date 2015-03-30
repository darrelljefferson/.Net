using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace MoneyMediaCSVParser
{

    public class SQLInserter
    {

       public static DateTime today = DateTime.Now.AddDays(0);
       public static SqlConnection mmconn;
       public static SqlConnection mmconn2;
       public static SqlConnection mmconn3;

       // Emailaddr_|"List_"|"FullName_"|"First_Name"|"Last_Name"|"Memberid_"|"Mm_username"|"Mm_password"|"Modified_date"|"Membertype_"|"Demo1_"|"Demo2_"|"Demo3_"|"Demo4_"
       public static void CreateTempSortTbl()
       {




           //Console.WriteLine("creation of tmp tbl");
           string qry1 = "IF NOT EXISTS (SELECT * FROM sys.objects " +  
                            "WHERE object_id = OBJECT_ID(N'dbo.MM_SORT_TBL')) " +  
                             "BEGIN CREATE TABLE MM_SORT_TBL ( Emailaddr varchar(100) NOT NULL, " + 
                                                     "List  varchar(100) NOT NULL , " + 
                                                     "Fullname varchar(100), " +
                                                     "First_name varchar(50), " +
                                                     "Last_name varchar(50), " +
                                                     "Memberid int, " +
                                                     "Mm_userid varchar(20), " +
                                                     "Mm_password varchar(50), " +
                                                     "Modified_date datetime, " +
                                                     "Membertype varchar(20), " +
                                                     "Demo1 varchar(20), " +
                                                     "Demo2 varchar(20), " +
                                                     "Demo3 varchar(20), " +
                                                     "Demo4 varchar(20)  " +
                                                     "PRIMARY KEY (Emailaddr, List), " +
                                                     "UNIQUE (Emailaddr, List)) " +                                                   
                                          "END " +
                                           "ELSE " +
                                             "BEGIN " +
                                               "DROP TABLE MM_SORT_TBL " +
                                               " CREATE TABLE MM_SORT_TBL ( Emailaddr varchar(100) NOT NULL, " +
                                                     "List  varchar(100) NOT NULL , " +
                                                     "Fullname varchar(100), " +
                                                     "First_name varchar(50), " +
                                                     "Last_name varchar(50), " +
                                                     "Memberid int, " +
                                                     "Mm_userid varchar(20), " +
                                                     "Mm_password varchar(50), " +
                                                     "Modified_date datetime, " +
                                                     "Membertype varchar(20), " +
                                                     "Demo1 varchar(20), " +
                                                     "Demo2 varchar(20), " +
                                                     "Demo3 varchar(20), " +
                                                     "Demo4 varchar(20)  " +
                                                     "PRIMARY KEY (Emailaddr, List), " +
                                                     "UNIQUE (Emailaddr, List)) " + 
                                               "END";

           SqlCommand myCreatestmt = new SqlCommand(qry1, mmconn);

           try
           {
              Console.WriteLine("row affected: {0}" , myCreatestmt.ExecuteNonQuery());
           }
           catch (Exception e)
           {
               Console.WriteLine("Creation fail");
           }

                                                          
       }

        public static void Connect(string host, string dbname, string userid, string password)
        {

            mmconn = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                                                       "password=" + password + ";" +
                                                       "Trusted_Connection=yes;" +
                                                       "database=" + dbname + ";" +
                                                       "connection timeout=60");

            mmconn2 = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                                           "password=" + password + ";" +
                                           "Trusted_Connection=yes;" +
                                           "database=" + dbname + ";" +
                                           "connection timeout=60");

            mmconn3 = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                               "password=" + password + ";" +
                               "Trusted_Connection=yes;" +
                               "database=" + dbname + ";" +
                               "connection timeout=60");
            try
            {
                mmconn.Open();
                mmconn2.Open();
                mmconn3.Open();
                Console.WriteLine("Open MSSQL connection");
                Console.WriteLine("MSSQL Version: " + mmconn.ServerVersion);
                Console.WriteLine("MSSQL Database: " + mmconn.Database);
                Console.WriteLine("MSSQL DataSource: " + mmconn.DataSource);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on Connection " + e.ToString());
                Log.WriteLine("Error on Connection " + e.ToString());
                Environment.Exit(99);
            }



       }


        public static void CloseConnection()
        {

            mmconn.Close();
            mmconn2.Close();
            mmconn3.Close();


        }



        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static void MM_Process_ListMgr()
        {

   
            try
            {
                SqlCommand command3 = new SqlCommand("dbo.sp_MM_build_List_mgrr", mmconn3);
                command3.CommandType = CommandType.StoredProcedure;
                command3.ExecuteNonQuery();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }




        }

        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static void LoadTmpTbl(string[] elements)
        {
         

           try 
           {
            SqlCommand insCmd = mmconn.CreateCommand();

            insCmd.CommandText = "INSERT INTO MM_SORT_TBL  VALUES ( @Emailaddr," +
                                                                   "@Fullname," +
                                                                   "@First_name," +
                                                                   "@Last_name," +
                                                                   "@Memberid," +
                                                                   "@Mm_userid," +
                                                                   "@Mm_password," +
                                                                   "@Modified_date," +
                                                                   "@Membertype," +
                                                                    "@Demo1," +
                                                                    "@Demo2," +
                                                                    "@Demo3 ," +
                                                                    "@Demo4 )";

                    insCmd.Parameters.Add("@Emailaddr", SqlDbType.VarChar, 100);
                    insCmd.Parameters.Add("@List" , SqlDbType.VarChar, 100);
                    insCmd.Parameters.Add("@Fullname" , SqlDbType.VarChar, 50);
                    insCmd.Parameters.Add("@First_name" , SqlDbType.VarChar, 100);
                    insCmd.Parameters.Add("@Last_name" , SqlDbType.VarChar, 50);
                    insCmd.Parameters.Add("@Memberid" , SqlDbType.Int);
                    insCmd.Parameters.Add("@Mm_userid" , SqlDbType.VarChar, 50);
                    insCmd.Parameters.Add("@Mm_password" , SqlDbType.VarChar, 50);
                    insCmd.Parameters.Add("@Modified_date" , SqlDbType.Date);
                    insCmd.Parameters.Add("@Membertype" , SqlDbType.VarChar, 50);
                    insCmd.Parameters.Add("@Demo1" , SqlDbType.VarChar, 10);
                    insCmd.Parameters.Add("@Demo2" , SqlDbType.VarChar, 10);
                    insCmd.Parameters.Add("@Demo3" , SqlDbType.VarChar, 10);
                    insCmd.Parameters.Add("@Demo4" , SqlDbType.VarChar, 10);
                    insCmd.Prepare();

                    insCmd.Parameters["@Emailaddr"].Value=elements[0];
                    insCmd.Parameters["@List"].Value=elements[1];

                    insCmd.Parameters["@Fullname"].Value=elements[2];
                    insCmd.Parameters["@First_name"].Value=elements[3];

                    insCmd.Parameters["@Last_name"].Value=elements[4];
                    insCmd.Parameters["@Memberid"].Value = elements[5];

                    insCmd.Parameters["@Mm_userid"].Value=elements[6];
                    insCmd.Parameters["@Mm_password"].Value=elements[7];

                    insCmd.Parameters["@Modified_date"].Value = today;           //elements[8];
                    insCmd.Parameters["@Membertype"].Value=elements[9];

                    insCmd.Parameters["@Demo1"].Value=" ";
                    insCmd.Parameters["@Demo2"].Value=" ";

                    insCmd.Parameters["@Demo3"].Value=" ";
                    insCmd.Parameters["@Demo4"].Value=" ";

                   
                    insCmd.ExecuteNonQuery();

           }  // end-of Try
            catch (SqlException e)
           {
                string err_line = "";
                foreach (string ln in elements) {
                        err_line += (ln + "|") ;
                }
                Log.WriteErrData(err_line + @"|Dupulicate Row");
                
            }

        }



    }

}
