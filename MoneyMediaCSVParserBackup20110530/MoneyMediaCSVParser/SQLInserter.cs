using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Data.SqlTypes;
using System.Globalization;

namespace MoneyMediaCSVParser
{

    public class SQLInserter
    {

       public static DateTime today = DateTime.Now.AddDays(0);
       public static SqlConnection mmconn;
       public static SqlConnection mmconn2;
       public static SqlConnection mmconn3;
       public static SqlConnection mmconn4;
       public static SqlConnection mmconn5;
       public static SqlConnection mmconn6; //Added to check whether or not a member already exists
    
  
       public static void CreateTempSortTbl()
       {

           //Console.WriteLine("creation of tmp tbl");
           string qry1 = "IF NOT EXISTS (SELECT * FROM dbo.sysobjects " +  
                            "WHERE id = OBJECT_ID(N'mm_sort_tbl')) " +  
                             "BEGIN CREATE TABLE mm_sort_tbl ( Emailaddr varchar(100) NOT NULL, " + 
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
                                               "TRUNCATE TABLE mm_sort_tbl "  + 
                                              "END";

           SqlCommand myCreatestmt = new SqlCommand(qry1, mmconn);

           try
           {
              Console.WriteLine("row affected: {0}" , myCreatestmt.ExecuteNonQuery());
           }
           catch (Exception e)
           {
               Console.WriteLine(e.ToString());
               Console.WriteLine("Creation fail");
               Environment.Exit(9);
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

             mmconn4 = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                               "password=" + password + ";" +
                               "Trusted_Connection=yes;" +
                               "database=" + dbname + ";" +
                               "connection timeout=60");

             mmconn5 = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                               "password=" + password + ";" +
                               "Trusted_Connection=yes;" +
                               "database=" + dbname + ";" +
                               "connection timeout=60");
             mmconn6 = new SqlConnection("Data Source=" + host + ";" + "user id=" + userid + ";" +
                               "password=" + password + ";" +
                               "Trusted_Connection=yes;" +
                               "database=" + dbname + ";" +
                               "connection timeout=60");
             try
            {
                mmconn.Open();
                mmconn2.Open();
                mmconn3.Open();
                mmconn4.Open();
                mmconn5.Open();
                mmconn6.Open();
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
        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static void MM_Process_ListMgr()
        {

               Console.WriteLine("Begin MM_Process_ListMgr()");
               string drv_members = "select * from dbo.mm_sort_tbl";
            

               SqlCommand command4 = new SqlCommand();
               command4.CommandType = CommandType.Text;
               command4.Connection = mmconn4;
               command4.CommandText = drv_members;
         
               SqlDataReader MyDataReader = command4.ExecuteReader(CommandBehavior.CloseConnection);
               
               while (MyDataReader.Read())
               {

                   Console.WriteLine("About to call FindExistingList");
                   if (!FindExistingList(Convert.ToString(MyDataReader[1])))   // If not on Database return true of false
                   {

                       MM_insert_lists(Convert.ToString(MyDataReader[1]));
    
                   } 
                   //Console.WriteLine(MyDataReader[0] + " " + MyDataReader[1]);
                   MM_insert_replace_member(Convert.ToString(MyDataReader[0]), 
                                            Convert.ToString(MyDataReader[1]), 
                                            Convert.ToString(MyDataReader[2]),
                                            Convert.ToString(MyDataReader[3]), 
                                            Convert.ToString(MyDataReader[4]),
                                            Convert.ToInt32(MyDataReader[5]),
                                            Convert.ToString(MyDataReader[6]),
                                            Convert.ToString(MyDataReader[7]) ,
                                            Convert.ToDateTime(MyDataReader[8]),
                                            Convert.ToString(MyDataReader[9]),
                                            Convert.ToString(MyDataReader[10]),
                                            Convert.ToString(MyDataReader[11]),
                                            Convert.ToString(MyDataReader[12]),
                                            Convert.ToString(MyDataReader[13]));


                                             
               }

               Console.WriteLine("End MM_Process_ListMgr()");

        }
        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static void CloseConnection()
        {

            mmconn.Close();
            mmconn2.Close();
            mmconn3.Close();
            mmconn4.Close();
            mmconn5.Close();
            mmconn6.Close();
        }
        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static Boolean FindExistingList(string IN_LIST) 
        {
               Console.WriteLine("Start FindExistingList() " + IN_LIST);
               SqlCommand command5 = new SqlCommand();
               string drv_list = "select name_ from dbo.lists_ where name_ = " + "'" +  @IN_LIST  + "'";
               
               
               command5.CommandType = CommandType.Text;
               command5.Connection = mmconn5;
               command5.CommandText = drv_list;
               if (command5.ExecuteNonQuery() > 0)
               {
                   Console.WriteLine("End FindExistingList()");
                   return (true);
               }
               else
               {
                   Console.WriteLine("End FindExistingList()");
                   return (false);
               }

        }

        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        public static int GetNextListId()
        {
            Console.WriteLine("Start GetNextListId()");
            SqlCommand command6 = new SqlCommand();
            string drv_getid = "select max(listid_) from dbo.[lists_] ";

            command6.CommandType = CommandType.Text;
            command6.Connection = mmconn5;
            command6.CommandText = drv_getid;
            try
            {
                SqlDataReader MyDataReader = command6.ExecuteReader(CommandBehavior.CloseConnection);
                if (MyDataReader.HasRows)
                {
                    Console.WriteLine("About to End GetNextListId()");
                    return (Convert.ToInt32(MyDataReader[0]) + 1);
                }
                else
                { return (0); }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Fetch fail for max list id " + e.ToString());
                Log.WriteLine("Fetch fail for max list id " + e.ToString());

                Console.WriteLine("End GetNextListId()");
                // Environment.Exit(99);
                return (0);
            }

   
        }

        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/

        public static int MM_insert_replace_member(string SQL_Emailaddr,
                                                        string SQL_List,
                                                        string SQL_Fullname,
                                                        string SQL_First_name,
                                                        string SQL_Last_name,
                                                        Int32 SQL_Memberid,
                                                        string SQL_Mm_userid,
                                                        string SQL_Mm_password,
                                                        DateTime SQL_Modified_date,
                                                        string SQL_Membertype,
                                                        string SQL_Demo1,
                                                        string SQL_Demo2,
                                                        string SQL_Demo3,
                                                        string SQL_Demo4)
        {

            Console.WriteLine("Start MM_insert_replace_member()");
            //if you let users to set the MemberType then check that membertype is one of the allowed member types
            SQL_Membertype = SQL_Membertype.ToLower(); //all valid membertypes are lower case
            string[] allowedType = { "normal", "unsub", "held", "private", "expired", "needs-goodbye", "needs-hello", "needs-confirm" };
            if (!allowedType.Contains(SQL_Membertype))
            {
                Console.WriteLine("Membertype '" + SQL_Membertype + "' is not allowed");
                Log.WriteLine("Membertype '" + SQL_Membertype + "' is not allowed");
                Console.WriteLine("End MM_insert_replace_member()");
                return (999);
            }


            //get email parts from emailddr_
            string[] emailParts = SQL_Emailaddr.Split('@');
            string usernameLC = emailParts[0].ToLower();
            string domain = emailParts[1].ToLower();


            bool memberAlreadyExists = MM_MemberExists(SQL_Emailaddr, SQL_List);

            if (memberAlreadyExists)
            {
                //Performe UPDATE
                try
                {

                    string inscmd = "UPDATE Members_ SET  " +
                                                        "fullname_ = @FULLNAME_, " +
                                                        "userid_ = @USERID_ " +
                                                        "Demo1 = @Demo1 " +
                                                        "Demo2 = @Demo2 " +
                                                        "Demo3 = @Demo3 " +
                                                        "Demo4 = @Demo4 " +
                                                        "WHERE list_ = @LISTNAME_ " +
                                                        "AND domain_ = @DOMAIN_ " +
                                                        "AND usernamelc_ = @USERNAMELC_ ";

                    Console.WriteLine(inscmd);
                    SqlCommand command2 = new SqlCommand(inscmd, mmconn2);

                    command2.Parameters.Add(new SqlParameter("@LISTNAME_", SQL_List));
                    command2.Parameters.Add(new SqlParameter("@DOMAIN_", domain));
                    command2.Parameters.Add(new SqlParameter("@USERNAMELC_", usernameLC));
                    command2.Parameters.Add(new SqlParameter("@FULLNAME_", SQL_Fullname));
                    command2.Parameters.Add(new SqlParameter("@USERID_", SQL_Mm_userid));
                    command2.Parameters.Add(new SqlParameter("@Demo1", SQL_Demo1));
                    command2.Parameters.Add(new SqlParameter("@Demo2", SQL_Demo2));
                    command2.Parameters.Add(new SqlParameter("@Demo3", SQL_Demo3));
                    command2.Parameters.Add(new SqlParameter("@Demo4", SQL_Demo4));


                    Console.WriteLine("End MM_insert_replace_member()");
                    return (command2.ExecuteNonQuery());


                }
                catch (Exception e)
                {
                    Console.WriteLine("Count not update Members_ Table " + e.ToString());
                    Log.WriteLine("Count not update Members_ Table");
                    Console.WriteLine("End MM_insert_replace_member()");
                    return (999);
                }
            }
            else
            {
                //Performe INSERT

                //Simple insert - only provide values for mandatory columns
                //let LM assign default values for non-mandatory columns
                try
                {

                    string inscmd = "INSERT INTO Members_ (emailaddr_, " +
                                                                "domain_, " +
                                                                "usernamelc_, " +
                                                                "datejoined_, " +
                                                                "membertype_, " +
                                                                "list_, " +
                                                                "fullname_, " +
                                                                "userid_, " +
                                                                "Demo1, " +
                                                                "Demo2, " +
                                                                "Demo3, " +
                                                                "Demo4) " +
                                                                "VALUES (@EMAILADDR_, " +
                                                                "@DOMAIN_, " +
                                                                "@USERNAMELC_, " +
                                                                "@DATEJOINED_, " +
                                                                "@MEMBERTYPE_, " +
                                                                "@LISTNAME_, " +
                                                                "@FULLNAME_, " +
                                                                "@USERID_, " +
                                                                "@Demo1, " +
                                                                "@Demo2, " +
                                                                "@Demo3, " +
                                                                "@Demo4 " +
                                                                ")";

                    Console.WriteLine(inscmd);
                    SqlCommand command2 = new SqlCommand(inscmd, mmconn2);

                    command2.Parameters.Add(new SqlParameter("@EMAILADDR_", SQL_Emailaddr));
                    command2.Parameters.Add(new SqlParameter("@DOMAIN_", domain));
                    command2.Parameters.Add(new SqlParameter("@USERNAMELC_", usernameLC));
                    command2.Parameters.Add(new SqlParameter("@DATEJOINED_", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    command2.Parameters.Add(new SqlParameter("@MEMBERTYPE_", SQL_Membertype));
                    command2.Parameters.Add(new SqlParameter("@LISTNAME_", SQL_List));
                    command2.Parameters.Add(new SqlParameter("@FULLNAME_", SQL_Fullname));
                    command2.Parameters.Add(new SqlParameter("@USERID_", SQL_Mm_userid));
                    command2.Parameters.Add(new SqlParameter("@Demo1", SQL_Demo1));
                    command2.Parameters.Add(new SqlParameter("@Demo2", SQL_Demo2));
                    command2.Parameters.Add(new SqlParameter("@Demo3", SQL_Demo3));
                    command2.Parameters.Add(new SqlParameter("@Demo4", SQL_Demo4));



                    Console.WriteLine("End MM_insert_replace_member()");
                    return (command2.ExecuteNonQuery());


                }
                catch (Exception e)
                {
                    Console.WriteLine("Count not insert into Members_ Table " + e.ToString());
                    Log.WriteLine("Count not insert into Members_ Table");
                    Console.WriteLine("End MM_insert_replace_member()");
                    return (999);
                }
            }


        }


         /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
          public static void MM_insert_lists(string SQL_Listname)
            {
                Console.WriteLine("Start MM_insert_lists()");
                string  additional_ = "NULL";                              
                string  admin_ =  "'admin'";
                string  addheadersandfooters_ = "'A'";
                string  adminsend_ = "'T'";
                string  allowcross_ = "'T'";
                string  allowdupe_ =  "'F'";
                string  allowinfo_ =  "'F'";
                string  anonymous_ =  "'F'";
                string  anyonepost_ =  "'F'";
                int  approvenum_ = -1;
                int  archivdays_ = 0;
                int  archivnum_ =  0;
                string  blnksubjok_ =  "'F'";
                string cachedatetime_ = DateTime.Today.ToShortDateString();   
                int  cachenormalmembercount_ = 48190;
                int  cacheheldmembercount_ =   9015;
                int  cacheunsubmembercount_ =  68;
                int  cacheothermembercount_ =  0;
                string  child_ =  "'T'";
                string  cleanauto_ = "'F'"; 
                int  cleandays_ = 0;
                int  cleannotif_ = 0;
                int  cleanunsub_ = 0;
                string  comment_ =  "NULL";
                string  commentsid_ = "NULL";
                string  conferencepost_ = "'E'";
                string  conferencevisibility_ = "'E'";
                int  conferenceduration_ = 24;
                int  confdays_ = 0;
                int  confnotify_ = 0;
                int  confunsub_ = 2;
                string creatstamp_ = DateTime.Today.ToShortDateString();
                string  crossclean_ =  "'T'";
                string  defaultfrom_ = "'login'";
                string  defaultto_ =  @"'%%nameemail%%'";
                string  defaultsubject_ = "NULL";
                int  defaultsubsetid_ =   0;   
                int  deliveryreports_ =  0;
                int  desclongid_ = 0;                 // default was NULL
                string  descshort_ =  "'MoneyMedia'" ;
                string  detecthtmlbydefault_ =  "'T'";
                string  detectopenbydefault_ = "'F'";
                string  digestftr_ = @"'--- You are currently subscribed to %%list.name%% as: %%emailaddr%% To unsubscribe click here: %%url.unsub%% or send a blank email to %%email.unsub%%' ";
                string  digesthdr_ = "NULL";
                string  disabled_ =  "'F'";   
                string  dklistsign_ = "NULL";
                string  dksenderlistsign_ = "'T'";    //  ver. 11.0
                string  dkimlistsign_ =  "'T'";
                int  errhold_ = 2;
                int  expiredays_ =  0;
                string  from_ = "NULL";
                string  hdrremove_ = "'precedence received return-path'";
                int  keepoutmailpostings_ = 10;
                string  keywords_ =  "NULL";
                int listid_ = GetNextListId();                
                string  listsubj_ =  "'F'";
                int  mailstreamid_ = 1;
                int  maxmembers_ =  0;
                int  maxmessnum_ = 1000; 
                int  maxmesssiz_ = 1000;           // ver. 11.0
                int  maxperuser_ =  0;
                int  maxquoting_ =  0;
                int  mergeallowbody_ = 0;
                int  mergecapabilities_ = 2;
                int  mergecapoverride_ = 3;
                string  messageftr_ =      @"'---  You are currently subscribed to %%list.name%% as: %%emailaddr%%.  To unsubscribe click here: %%url.unsub%%  or send a blank email to %%email.unsub%%'";
                string  messageftrhtml_ = string.Concat("'<html>      <head>      </head>      <body>          <p>          <title></title>          </p>          <p>---</p>",
                                                      "<p>You are currently subscribed to %%list.name%% as: <a href=\"mailto:%%emailaddr%%\">%%emailaddr%%</a>.</p>", 
                                                          "<p>To unsubscribe click here: <a href=\"%%url.unsub%%\">%%url.unsub%%</a></p>" , 
                                                          "<p>(It may be necessary to cut and paste the above URL if the line is broken)</p>          ", 
                                                          "<p>or send a blank email to <a href=\"mailto:%%email.unsub%%\">%%email.unsub%%</a></p>      </body>  </html>'");
                string  messagehdr_ =      "NULL";
                string  messagehdrhtml_ =  "NULL";
                string  modhdrdate_     =  "'F'";
                string  moderated_      =  "'all'";
                string  mrivisibility_ =   "'H'";
                string name_ = "'" + SQL_Listname +  "'";                 
                int  namereqd_ =  0;
                string  noarchive_ =  "'F'";
                string  nobodyok_ =   "'F'";
                string  noconfirm_ =  "'F'";
                string  noemail_ =    "'F'";
                string  noemailsub_ =  "'F'";
                string  nolisthdr_ =   "'F'";
                string  nomidrewr_ =   "'F'";
                string  nonntp_ =      "'T'";
                string  nosearch_ =    "'F'";
                int  passwdreqd_ = 1;
                string  pgmafter_ =  "NULL";
                string  pgmbefore_ = "NULL";
                string  pgmsub1_   =   "NULL";
                string  pgmsub2_   =   "NULL";
                string  pgmunsub1_ = "NULL";
                string  pgmunsub2_ = "NULL";
                int  postpass_      =  0;
                string  privapprov_ =  "NULL";
                int  privdays_ = 0;
                string  prsrvxtags_ = "'F'";
                int  recencydaycount_ = 7;
                string  recencyemailenabled_ =    "'F'";
                int  recencymailcount_ =     3;
                string  recencyoperator_ =  "'m'";
                string  recencysequentialenabled_ =  "'F'";
                string  recencytriggeredenabled_ = "'F'";
                string  recencywebenabled_ =  "'F'";                      
                string  recipientlogginglevel_ =  "'E'";
                int  referralpurgedays_ = 0;
                int  referralsperday_ = 20;
                int  relhour_ = 0;
                int  relpending_ = 0;
                string  replyto_ = "'nochange'";            // need distinct values
                int  reviewperm_ = 0;                              
                string  smtpfrom_ =  "NULL";
                string  smtphdrs_ = "NULL"; 
                string  security_ = "'open'";
                string  simplesub_ = "'T'";
                int  sponsorgid_ =   0;                     // Ver. 11.0
                string  subnotdays_ = "NULL";
                string  subpasswd_ =  "NULL";
                string  syncconnid_ =  "NULL";
                string  syncdeletemode_ =  "'N'";
                string  syncinsertmode_ =  "'Q'";
                int  syncmaxmalformed_ = 10;
                string syncnexttime_ = DateTime.Today.ToShortDateString();                                            //DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"); 
                string syncprevioussuccesstime_ = DateTime.Today.ToShortDateString();
                string syncprevioustime_ = DateTime.Today.ToShortDateString();
                string  syncschedule_ =  "'N'";
                string  synctimerule_ = "'A'";
                string  syncupdatemode_ = "'N'";
                int  syncweekday_ = 1;
                string syncwindowend_ = DateTime.Today.ToShortDateString();
                string syncwindowstart_ = DateTime.Today.ToShortDateString();
                string  tclmergeinit_ = "NULL";
                string  to_ = "'%%nameemail%%'";
                string  topic_ = "'main'";  
                string  trackallurls_ =  "'T'";
                int  translatelangid_ = 0;
                string  urllist_ =  "NULL";
                string  urllogo_ =  "NULL";
                string  visibglobl_ = "'F'";
                string  visitors_ =  "'F'";
                string  warnnorecips_ = "'T'";
                string  piperusername_ =   "NULL";
                string  piperpassword_ =  "NULL";
                string  pipercompany_ =   "NULL";
                string  watracking_ = "'T'";
                //string  dkimlistsign= "NULL";
                //string  dksenderlistsign= "'F'";



                string InsetStmt = "insert into dbo.lists_ values (" + additional_ + "," +
                                                                     admin_ + "," +
                                                                     addheadersandfooters_ + "," +
                                                                     adminsend_ + "," +
                                                                     allowcross_ + "," +
                                                                     allowdupe_ + "," +
                                                                     allowinfo_ + "," +
                                                                     anonymous_ + "," +
                                                                     anyonepost_ + "," +
                                                                     approvenum_ + "," +
                                                                     archivdays_ + "," +
                                                                     archivnum_ + "," +
                                                                     blnksubjok_ + "," +
                                                                     cachedatetime_ + "," +
                                                                     cachenormalmembercount_ + "," +
                                                                     cacheheldmembercount_ + "," +
                                                                     cacheunsubmembercount_ + "," +
                                                                     cacheothermembercount_ + "," +
                                                                     child_ + "," +
                                                                     cleanauto_ + "," +
                                                                     cleandays_ + "," +
                                                                     cleannotif_ + "," +
                                                                     cleanunsub_ + "," +
                                                                     comment_ + "," +
                                                                     commentsid_ + "," +
                                                                     conferencepost_ + "," +
                                                                     conferencevisibility_ + "," +
                                                                     conferenceduration_ + "," +
                                                                     confdays_ + "," +
                                                                     confnotify_ + "," +
                                                                     confunsub_ + "," +
                                                                     creatstamp_ + "," +
                                                                     crossclean_ + "," +
                                                                     defaultfrom_ + "," +
                                                                     defaultto_ + "," +
                                                                     defaultsubject_ + "," +
                                                                     defaultsubsetid_ + "," +
                                                                     deliveryreports_ + "," +
                                                                     desclongid_ + "," +
                                                                     descshort_ + "," +
                                                                     detecthtmlbydefault_ + "," +
                                                                     detectopenbydefault_ + "," +
                                                                     digestftr_ + "," +
                                                                     digesthdr_ + "," +
                                                                     disabled_ + "," +
                                                                     dklistsign_ + "," +
                                                                     dksenderlistsign_ + "," +
                                                                     dkimlistsign_ + "," +
                                                                     errhold_ + "," +
                                                                     expiredays_ + "," +
                                                                     from_ + "," +
                                                                     hdrremove_ + "," +
                                                                     keepoutmailpostings_ + "," +
                                                                     keywords_ + "," +
                                                                     listid_ + "," +
                                                                     listsubj_ + "," +
                                                                     mailstreamid_ + "," +
                                                                     maxmembers_ + "," +
                                                                     maxmessnum_ + "," +
                                                                     maxmesssiz_ + "," +
                                                                     maxperuser_ + "," +
                                                                     maxquoting_ + "," +
                                                                     mergeallowbody_ + "," +
                                                                     mergecapabilities_ + "," +
                                                                     mergecapoverride_ + "," +
                                                                     messageftr_ + "," +
                                                                     messageftrhtml_ + "," +
                                                                     messagehdr_ + "," +
                                                                     messagehdrhtml_ + "," +
                                                                     modhdrdate_ + "," +
                                                                     moderated_ + "," +
                                                                     mrivisibility_ + "," +
                                                                     name_ + "," +
                                                                     namereqd_ + "," +
                                                                     noarchive_ + "," +
                                                                     nobodyok_ + "," +
                                                                     noconfirm_ + "," +
                                                                     noemail_ + "," +
                                                                     noemailsub_ + "," +
                                                                     nolisthdr_ + "," +
                                                                     nomidrewr_ + "," +
                                                                     nonntp_ + "," +
                                                                     nosearch_ + "," +
                                                                     passwdreqd_ + "," +
                                                                     pgmafter_ + "," +
                                                                     pgmbefore_ + "," +
                                                                     pgmsub1_ + "," +
                                                                     pgmsub2_ + "," +
                                                                     pgmunsub1_ + "," +
                                                                     pgmunsub2_ + "," +
                                                                     postpass_ + "," +
                                                                     privapprov_ + "," +
                                                                     privdays_ + "," +
                                                                     prsrvxtags_ + "," +
                                                                     recencydaycount_ + "," +
                                                                     recencyemailenabled_ + "," +
                                                                     recencymailcount_ + "," +
                                                                     recencyoperator_ + "," +
                                                                     recencysequentialenabled_ + "," +
                                                                     recencytriggeredenabled_ + "," +
                                                                     recencywebenabled_ + "," +
                                                                     recipientlogginglevel_ + "," +
                                                                     referralpurgedays_ + "," +
                                                                     referralsperday_ + "," +
                                                                     relhour_ + "," +
                                                                     relpending_ + "," +
                                                                     replyto_ + "," +
                                                                     reviewperm_ + "," +
                                                                     smtpfrom_ + "," +
                                                                     smtphdrs_ + "," +
                                                                     security_ + "," +
                                                                     simplesub_ + "," +
                                                                     sponsorgid_ + "," +
                                                                     subnotdays_ + "," +
                                                                     subpasswd_ + "," +
                                                                     syncconnid_ + "," +
                                                                     syncdeletemode_ + "," +
                                                                     syncinsertmode_ + "," +
                                                                     syncmaxmalformed_ + "," +
                                                                     syncnexttime_ + "," +
                                                                     syncprevioussuccesstime_ + "," +
                                                                     syncprevioustime_ + "," +
                                                                     syncschedule_ + "," +
                                                                     synctimerule_ + "," +
                                                                     syncupdatemode_ + "," +
                                                                     syncweekday_ + "," +
                                                                     syncwindowend_ + "," +
                                                                     syncwindowstart_ + "," +
                                                                     tclmergeinit_ + "," +
                                                                     to_ + "," +
                                                                     topic_ + "," +
                                                                     trackallurls_ + "," +
                                                                     translatelangid_ + "," +
                                                                     urllist_ + "," +
                                                                     urllogo_ + "," +
                                                                     visibglobl_ + "," +
                                                                     visitors_ + "," +
                                                                     warnnorecips_ + "," +
                                                                     piperusername_ + "," +
                                                                     piperpassword_ + "," +
                                                                     pipercompany_ + "," +
                                                                     watracking_ + ")";
																	 //dkimlistsign_            + "," +
                                                                     //dksenderlistsign_  + ")";

                            Log.WriteLine(InsetStmt);
                    try
                    {
                            SqlCommand command3 = new SqlCommand();
                            command3.Parameters.Add(new SqlParameter("@additional_ ", additional_));
				            command3.Parameters.Add(new SqlParameter("@admin_" , admin_));
				            command3.Parameters.Add(new SqlParameter("@addheadersandfooters_" , addheadersandfooters_ ));
				            command3.Parameters.Add(new SqlParameter("@adminsend_ ", adminsend_));
                            command3.Parameters.Add(new SqlParameter("@allowcross_" , allowcross_ ));
                            command3.Parameters.Add(new SqlParameter("@allowdupe_ ", allowdupe_ ));
                            command3.Parameters.Add(new SqlParameter("@allowinfo_" , allowinfo_ ));
                            command3.Parameters.Add(new SqlParameter("@anonymous_", anonymous_));
                            command3.Parameters.Add(new SqlParameter("@anyonepost_" , anyonepost_ ));
                            command3.Parameters.Add(new SqlParameter("@approvenum_" , approvenum_ ));
                            command3.Parameters.Add(new SqlParameter("@archivdays_ ", archivdays_ ));
                            command3.Parameters.Add(new SqlParameter("@archivnum_" , archivnum_  ));
                            command3.Parameters.Add(new SqlParameter("@blnksubjok_" ,blnksubjok_ ));
                            command3.Parameters.Add(new SqlParameter("@cachedatetime_" , cachedatetime_));
                            command3.Parameters.Add(new SqlParameter("@cachenormalmembercount_" , cachenormalmembercount_ ));
                            command3.Parameters.Add(new SqlParameter("@cacheheldmembercount_" , cacheheldmembercount_));
                            command3.Parameters.Add(new SqlParameter("@cacheunsubmembercount_" , cacheunsubmembercount_));
                            command3.Parameters.Add(new SqlParameter("@cacheothermembercount_" , cacheothermembercount_));
                            command3.Parameters.Add(new SqlParameter("@child_" , child_));
                            command3.Parameters.Add(new SqlParameter("@cleanauto_" ,cleanauto_));
                            command3.Parameters.Add(new SqlParameter("@cleandays_", cleandays_ ));
                            command3.Parameters.Add(new SqlParameter("@cleannotif_", cleannotif_));
                            command3.Parameters.Add(new SqlParameter("@cleanunsub_" , cleanunsub_));
                            command3.Parameters.Add(new SqlParameter("@comment_" , comment_ ));
                            command3.Parameters.Add(new SqlParameter("@commentsid_" , commentsid_));
                            command3.Parameters.Add(new SqlParameter("@conferencepost_" , conferencepost_));
                            command3.Parameters.Add(new SqlParameter("@conferencevisibility_" , conferencevisibility_));
                            command3.Parameters.Add(new SqlParameter("@conferenceduration_" , conferenceduration_));
                            command3.Parameters.Add(new SqlParameter("@confdays_" , confdays_));
                            command3.Parameters.Add(new SqlParameter("@confnotify_", confnotify_));
                            command3.Parameters.Add(new SqlParameter("@confunsub_", confunsub_ ));
                            command3.Parameters.Add(new SqlParameter("@creatstamp_" ,creatstamp_));
                            command3.Parameters.Add(new SqlParameter("@crossclean_" , crossclean_));
                            command3.Parameters.Add(new SqlParameter("@defaultfrom_",  defaultfrom_));
                            command3.Parameters.Add(new SqlParameter("@defaultto_" ,  defaultto_ ));
                            command3.Parameters.Add(new SqlParameter("@defaultsubject_" , defaultsubject_));
                            command3.Parameters.Add(new SqlParameter("@defaultsubsetid_" ,defaultsubsetid_));
                            command3.Parameters.Add(new SqlParameter("@deliveryreports_" , deliveryreports_));
                            command3.Parameters.Add(new SqlParameter("@desclongid_" , desclongid_));
                            command3.Parameters.Add(new SqlParameter("@descshort_" ,  descshort_));
                            command3.Parameters.Add(new SqlParameter("@detecthtmlbydefault_" , detecthtmlbydefault_));
                            command3.Parameters.Add(new SqlParameter("@detectopenbydefault_" ,detectopenbydefault_));
                            command3.Parameters.Add(new SqlParameter("@digestftr_" , digestftr_));
                            command3.Parameters.Add(new SqlParameter("@digesthdr_" , digesthdr_));
                            command3.Parameters.Add(new SqlParameter("@disabled_" , disabled_ ));
                            command3.Parameters.Add(new SqlParameter("@dklistsign_" , dklistsign_ ));
                            command3.Parameters.Add(new SqlParameter("@dksenderlistsign_" ,  dksenderlistsign_ ));
                            command3.Parameters.Add(new SqlParameter("@dkimlistsign_" ,  dkimlistsign_));
                            command3.Parameters.Add(new SqlParameter("@errhold_" ,  errhold_));
                            command3.Parameters.Add(new SqlParameter("@expiredays_", expiredays_));
                            command3.Parameters.Add(new SqlParameter("@from_", from_));
                            command3.Parameters.Add(new SqlParameter("@hdrremove_" ,  hdrremove_));
                            command3.Parameters.Add(new SqlParameter("@keepoutmailpostings_", keepoutmailpostings_));
                            command3.Parameters.Add(new SqlParameter("@keywords_" ,  keywords_));
                            command3.Parameters.Add(new SqlParameter("@listid_" , listid_ ));                                
                            command3.Parameters.Add(new SqlParameter("@listsubj_" ,  listsubj_ ));
                            command3.Parameters.Add(new SqlParameter("@mailstreamid_" , mailstreamid_));
                            command3.Parameters.Add(new SqlParameter("@maxmembers_" , maxmembers_));
                            command3.Parameters.Add(new SqlParameter("@maxmessnum_" , maxmessnum_ ));
                            command3.Parameters.Add(new SqlParameter("@maxmesssiz_" , maxmesssiz_ ));
                            command3.Parameters.Add(new SqlParameter("@maxperuser_" ,  maxperuser_));
                            command3.Parameters.Add(new SqlParameter("@maxquoting_" ,  maxquoting_));
                            command3.Parameters.Add(new SqlParameter("@mergeallowbody_" , mergeallowbody_));
                            command3.Parameters.Add(new SqlParameter("@mergecapabilities_" , mergecapabilities_));
                            command3.Parameters.Add(new SqlParameter("@mergecapoverride_" , mergecapoverride_));
                            command3.Parameters.Add(new SqlParameter("@messageftr_" , messageftr_ ));
                            command3.Parameters.Add(new SqlParameter("@messageftrhtml_" , messageftrhtml_));
                            command3.Parameters.Add(new SqlParameter("@messagehdr_" , messagehdr_));
                            command3.Parameters.Add(new SqlParameter("@messagehdrhtml_" , messagehdrhtml_));
                            command3.Parameters.Add(new SqlParameter("@modhdrdate_" , modhdrdate_));
                            command3.Parameters.Add(new SqlParameter("@moderated_" , moderated_));
                            command3.Parameters.Add(new SqlParameter("@mrivisibility_" , mrivisibility_ ));
                            command3.Parameters.Add(new SqlParameter("@name_" ,  name_));
                            command3.Parameters.Add(new SqlParameter("@namereqd_" ,  namereqd_));
                            command3.Parameters.Add(new SqlParameter("@noarchive_" , noarchive_));
                            command3.Parameters.Add(new SqlParameter("@nobodyok_",  nobodyok_));
                            command3.Parameters.Add(new SqlParameter("@noconfirm_",  noconfirm_ ));
                            command3.Parameters.Add(new SqlParameter("@noemail_" ,  noemail_));
                            command3.Parameters.Add(new SqlParameter("@noemailsub_" , noemailsub_));
                            command3.Parameters.Add(new SqlParameter("@nolisthdr_",  nolisthdr_));
                            command3.Parameters.Add(new SqlParameter("@nomidrewr_",  nomidrewr_ ));
                            command3.Parameters.Add(new SqlParameter("@nonntp_", nonntp_));
                            command3.Parameters.Add(new SqlParameter("@nosearch_", nosearch_));
                            command3.Parameters.Add(new SqlParameter("@passwdreqd_", passwdreqd_));
                            command3.Parameters.Add(new SqlParameter("@pgmafter_", pgmafter_ ));
                            command3.Parameters.Add(new SqlParameter("@pgmbefore_", pgmbefore_));
                            command3.Parameters.Add(new SqlParameter("@pgmsub1_" ,  pgmsub1_));
                            command3.Parameters.Add(new SqlParameter("@pgmsub2_" , pgmsub2_));
                            command3.Parameters.Add(new SqlParameter("@pgmunsub1_" , pgmunsub1_));
                            command3.Parameters.Add(new SqlParameter("@pgmunsub2_" , pgmunsub2_));
                            command3.Parameters.Add(new SqlParameter("@postpass_" , postpass_));
                            command3.Parameters.Add(new SqlParameter("@privapprov_" ,privapprov_));
                            command3.Parameters.Add(new SqlParameter("@privdays_" , privdays_));
                            command3.Parameters.Add(new SqlParameter("@prsrvxtags_" , prsrvxtags_ ));
                            command3.Parameters.Add(new SqlParameter("@recencydaycount_" , recencydaycount_));
                            command3.Parameters.Add(new SqlParameter("@recencyemailenabled_" ,recencyemailenabled_));
                            command3.Parameters.Add(new SqlParameter("@recencymailcount_" , recencymailcount_));
                            command3.Parameters.Add(new SqlParameter("@recencyoperator_", recencyoperator_));
                            command3.Parameters.Add(new SqlParameter("@recencysequentialenabled_",  recencysequentialenabled_));
                            command3.Parameters.Add(new SqlParameter("@recencytriggeredenabled_" ,recencytriggeredenabled_ ));
                            command3.Parameters.Add(new SqlParameter("@recencywebenabled_",  recencywebenabled_));
                            command3.Parameters.Add(new SqlParameter("@recipientlogginglevel_", recipientlogginglevel_ ));
                            command3.Parameters.Add(new SqlParameter("@referralpurgedays_" , referralpurgedays_));
                            command3.Parameters.Add(new SqlParameter("@referralsperday_" ,referralsperday_));
                            command3.Parameters.Add(new SqlParameter("@relhour_" , relhour_));
                            command3.Parameters.Add(new SqlParameter("@relpending_",  relpending_));
                            command3.Parameters.Add(new SqlParameter("@replyto_",  replyto_));
                            command3.Parameters.Add(new SqlParameter("@reviewperm_", reviewperm_));
                            command3.Parameters.Add(new SqlParameter("@smtpfrom_" , smtpfrom_));
                            command3.Parameters.Add(new SqlParameter("@smtphdrs_", smtphdrs_));
                            command3.Parameters.Add(new SqlParameter("@security_" , security_));
                            command3.Parameters.Add(new SqlParameter("@simplesub_" ,  simplesub_));
                            command3.Parameters.Add(new SqlParameter("@sponsorgid_" , sponsorgid_));
                            command3.Parameters.Add(new SqlParameter("@subnotdays_" , subnotdays_));
                            command3.Parameters.Add(new SqlParameter("@subpasswd_" , subpasswd_));
                            command3.Parameters.Add(new SqlParameter("@syncconnid_", syncconnid_));
                            command3.Parameters.Add(new SqlParameter("@syncdeletemode_" , syncdeletemode_));
                            command3.Parameters.Add(new SqlParameter("@syncinsertmode_" , syncinsertmode_));
                            command3.Parameters.Add(new SqlParameter("@syncmaxmalformed_" , syncmaxmalformed_));
                            command3.Parameters.Add(new SqlParameter("@syncnexttime_" ,  syncnexttime_));
                            command3.Parameters.Add(new SqlParameter("@syncprevioussuccesstime_" , syncprevioussuccesstime_));
                            command3.Parameters.Add(new SqlParameter("@syncprevioustime_", syncprevioustime_));
                            command3.Parameters.Add(new SqlParameter("@syncschedule_" , syncschedule_));
                            command3.Parameters.Add(new SqlParameter("@synctimerule_" , synctimerule_));
                            command3.Parameters.Add(new SqlParameter("@syncupdatemode_" ,syncupdatemode_));
                            command3.Parameters.Add(new SqlParameter("@syncweekday_" ,syncweekday_));
                            command3.Parameters.Add(new SqlParameter("@syncwindowend_" , syncwindowend_));
                            command3.Parameters.Add(new SqlParameter("@syncwindowstart_" ,syncwindowstart_));
                            command3.Parameters.Add(new SqlParameter("@tclmergeinit_" , tclmergeinit_));
                            command3.Parameters.Add(new SqlParameter("@to_" , to_));
                            command3.Parameters.Add(new SqlParameter("@topic_" ,topic_));
                            command3.Parameters.Add(new SqlParameter("@trackallurls_" , trackallurls_));
                            command3.Parameters.Add(new SqlParameter("@translatelangid_" , translatelangid_));
                            command3.Parameters.Add(new SqlParameter("@urllist_" , urllist_));
                            command3.Parameters.Add(new SqlParameter("@urllogo_" , urllogo_));
                            command3.Parameters.Add(new SqlParameter("@visibglobl_" , visibglobl_));
                            command3.Parameters.Add(new SqlParameter("@visitors_" , visitors_));
                            command3.Parameters.Add(new SqlParameter("@warnnorecips_", warnnorecips_));
                            command3.Parameters.Add(new SqlParameter("@piperusername_", piperusername_));
                            command3.Parameters.Add(new SqlParameter("@piperpassword_" , piperpassword_));
                            command3.Parameters.Add(new SqlParameter("@pipercompany_" ,pipercompany_));
                            command3.Parameters.Add(new SqlParameter("@watracking_", watracking_));

                          //  command3.Parameters.Add(new SqlParameter("@dkimlistsign_",  dkimlistsign_ ));
                          //  command3.Parameters.Add(new SqlParameter("@dksenderlistsign_" , dksenderlistsign_));


                    command3.CommandType = CommandType.Text;
                    command3.CommandText = InsetStmt;
                    command3.Connection = mmconn3;
                    command3.ExecuteNonQuery();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

                    Console.WriteLine("End MM_insert_lists()");


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

            insCmd.CommandText = "INSERT INTO mm_sort_tbl  VALUES ( @Emailaddr," +
                                                                   "@List," +
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
                    Double Num = 0;
                    bool isNum = double.TryParse(elements[5], out Num);
   
                    if (isNum)
                         {
                              insCmd.Parameters["@Memberid"].Value = elements[5];
                        }
                    else
                      {
                        insCmd.Parameters["@Memberid"].Value = 0;
                        }


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
               //Console.WriteLine("Sql Insert failed: " + e.ToString());
                string err_line = "";
                foreach (string ln in elements) {
                        err_line += (ln + "|") ;
                }
                Log.WriteErrData(err_line + @"|Dupulicate Row");
                
            }

        }
        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        /* -----------------------------------------------------------------------------------------------------------
         *  Funnc:
         *  Purpose:
         * 
         * 
         * -----------------------------------------------------------------------------------------------------------*/
        private static bool MM_MemberExists(string emailaddr, string listname)
        {
            string[] emailParts = emailaddr.Split('@');
            string usernameLC = emailParts[0].ToLower();
            string domain = emailParts[1].ToLower();

            string sql = "SELECT CASE WHEN EXISTS (SELECT NULL FROM members_ WHERE list_ = '" + listname + "'" + 
                                " AND usernamelc_ = '" + usernameLC+ "'" +
                                " AND domain_ = '" + domain + "') THEN 1 ELSE 0 END E";

            SqlCommand command6 = new SqlCommand(sql, mmconn6);
            int exists = (int)command6.ExecuteScalar();

            return Convert.ToBoolean(exists);
        }
    }

}
