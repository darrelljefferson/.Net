using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace NbcorlandoDailyMemberDump
{
    class GenerateFlatFile
    {
        public static void ExportToCSVFile(string connectionString, string fileOut, string listName, string today, string yesterday)
        {

            // Connects to the database, and makes the select command.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string sqlQuery = String.Concat("select EmailAddr_,", "Demographics_Updated,", "Additional_ ,",
                "AppNeeded_,", " CanAppPend_,", " CleanAuto_,", " Comment_,", " ConfirmDat_,", "DateHeld_,",
                "DateJoined_,", " DateUnsub_,", " Domain_,", " EnableWYSIWYG_,",
                "ExpireDate_,", " FullName_,", " IsListAdm_,", " List_,", " MailFormat_,", " MemberID_,",
                "MemberType_,", "NoRepro_,", " NotifyErr_,", " NotifySubm_,", " cast(NumAppNeed_ as int),",
                "cast(NumBounces_ as int),", " Password_,", "PermissionGroupID_,", "RcvAdmMail_,", " ReadsHtml_,",
                "ReceiveAck_,", "SyncCanResubscribe_,", "SyncResubscribeType_,", "SubType_,",
                "UnsubMessageID_,", " UserID_,", " UserNameLC_,", "First_Name,", "Last_Name,",
                "Address1,", "Address2,", "City,State,", "Zip_Code,", "Country,", "Country_Code,",
                "Birth_Day,", "Birth_Month,", "Birth_Year,", "Client_Member_ID,", "Gender,",
                "Area_Code,Phone,", "Old_Email,", "Registration_date,", "IMPACT_Member_ID,",
                "IMPACT_Source_Code,", "IMPACT_Vendor_Id,", "Preferred_email_type,", "Source,",
                "Optins_Lists,", "CAP_UO_PASSTYPE,", "DOGSHOW_NUM_CATS,", "DOGSHOW_NUM_DOGS,",
                "GENRE_ACTION,", "Current_Custom_Attributes,", "GENRE_ADVENTURE,", "GENRE_ANIMATION,",
                "GENRE_CHILDREN,", "GENRE_COMEDY,", "GENRE_DRAMA,", "GENRE_FAMILY,", "GENRE_FOREIGN,",
                "GENRE_HORROR,", "GENRE_INDEPENDENT,", "GENRE_ROMANCE,", "GENRE_SCIFI,",
                "GENRE_SHORTS,", "GENRE_SUSPENSE,", "INTERNET_CONNECTION,", "PPV_CABLE_SAT_PROVIDER,",
                "PPV_FIND_CABLE_SAT_GUIDE,", "PPV_FIND_DIRECT_MAIL,", "PPV_FIND_INTERNET,",
                "PPV_FIND_MAGAZINE,", "PPV_FIND_NEWSPAPER,", "PPV_FIND_RADIO,", "PPV_FIND_TV_COMMERCIAL,",
                "PPV_FIND_TV_SNEAK_PREVIEW,", "PPV_FIND_WORD_OF_MOUTH,", "PPV_ORDER_EVENT,",
                "PPV_ORDER_MOVIE,", "PROGRAM,", "PROGRAM_NAME,", "SMS_DIN,SMS_PERMISSION,",
                "SMS_SERVICE_PROVIDER,", "New_Custom_Attributes,", "UO_PREV_VISITS,",
                "UO_MOST_RECENT_VISIT_MONTH,", "UO_MOST_RECENT_VISIT_YEAR,",
                "UO_NEXT_PLANNED_VISIT_MONTH,", "UO_NEXT_PLANNED_VISIT_YEAR,",
                "UPR_CHILD_1_BIRTH_YEAR,", "UPR_CHILD_2_BIRTH_YEAR,", "UPR_CHILD_3_BIRTH_YEAR,",
                "UPR_CHILD_4_BIRTH_YEAR,", "UO_HOTEL_SET_NEXT_VISIT,", "UO_STAYED_AT_ONSITE_HOTEL,",
                "UO_TICKET_TYPE,", "UO_ANNUAL_PASS_EXP_MONTH,", "UO_ANNUAL_PASS_EXP_YEAR,",
                "UO_ANNUAL_PASS_FLEX_PAY,", "UO_PURCHASED_EXPRESS,", "UO_TICKET_PURCHASE_MONTH,",
                "UO_TICKET_PURCHASE_YEAR,", "UO_TICKET_REDEEMED_MONTH,", "UO_TICKET_REDEEMED_YEAR,",
                "UO_SOURCE_PAGE,", "UO_NO_OPTIN,", "New_Attributes_Home_Ent,", "USHE_DVD_PURCHASE_MONTH,",
                "USHE_DVD_PURCHASE_TYPE,", "USHE_DVD_ONLINE_INFO_SOURCE,", "GENRE_TV_ON_DVD,",
                "USH_SALES_ACCOUNT,", "USH_SALES_GROUP,", "promo,custom_url,", "TridionID_,",
                "URLMap_,URLUnsub_,", "last_update ",
                "from members_ with (nolock) ",
                "where list_ = \'", listName, "\' ",
                "and demographics_updated between \'", yesterday, " 21:00:00\' and \'", today, " 21:00:00\'");
            Log.WriteLine(sqlQuery);
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            try
            {
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                Log.WriteLine(String.Concat("The following database error was returned: ", e.Message));
            }

            // Creates a SqlDataReader instance to read data from the table.
            sqlCommand.CommandTimeout = 1800;
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            // Retrieves the schema of the table.
            DataTable dataTable = sqlDataReader.GetSchemaTable();

            // Creates the CSV file as a stream, using the given encoding.
            StreamWriter streamWriter = new StreamWriter(fileOut, false, Encoding.ASCII);

            string strRow; // represents a full row

            // write in the column headers
            streamWriter.WriteLine(columnNames(dataTable, ","));

            int count = 0; //used to keep track of the # of rows

            DateTime fooDateTimePlaceKeeper;
            int fooIntPlaceKeeper;
           


            while (sqlDataReader.Read())
            {
                string Additional_ = "";
                string AppNeeded_ = "";
                string CanAppPend_ = "";
                string CleanAuto_ = "";
                string Comment_ = "";
                string ConfirmDat_ = "";
                string DateHeld_ = "";
                string DateJoined_ = "";
                string DateUnsub_ = "";
                string Domain_ = "";
                string EnableWYSIWYG_ = "";
                string ExpireDate_ = "";
                string FullName_ = "";
                string IsListAdm_ = "";
                string List_ = "";
                string MailFormat_ = "";
                string MemberType_ = "";
                string NoRepro_ = "";
                string NotifyErr_ = "";
                string NotifySubm_ = "";
                string NumAppNeed_ = "";
                string NumBounces_ = "";
                string Password_ = "";
                string PermissionGroupID_ = "";
                string RcvAdmMail_ = "";
                string ReadsHtml_ = "";
                string ReceiveAck_ = "";
                string SyncCanResubscribe_ = "";
                string SyncResubscribeType_ = "";
                string SubType_ = "";
                string UnsubMessageID_ = "";
                string UserID_ = "";
                string UserNameLC_ = "";
                string First_Name = "";
                string Last_Name = "";
                string Address1 = "";
                string Address2 = "";
                string City = "";
                string State = "";
                string Zip_Code = "";
                string Country = "";
                string Country_Code = "";
                string Birth_Day = "";
                string Birth_Month = "";
                string Birth_Year = "";
                string Client_Member_ID = "";
                string Gender = "";
                string Area_Code = "";
                string Phone = "";
                string Old_Email = "";
                string Registration_date = "";
                string IMPACT_Member_ID = "";
                string IMPACT_Source_Code = "";
                string IMPACT_Vendor_Id = "";
                string Preferred_email_type = "";
                string Source = "";
                string Optins_Lists = "";
                string CAP_UO_PASSTYPE = "";
                string DOGSHOW_NUM_CATS = "";
                string DOGSHOW_NUM_DOGS = "";
                string GENRE_ACTION = "";
                string Current_Custom_Attributes = "";
                string GENRE_ADVENTURE = "";
                string GENRE_ANIMATION = "";
                string GENRE_CHILDREN = "";
                string GENRE_COMEDY = "";
                string GENRE_DRAMA = "";
                string GENRE_FAMILY = "";
                string GENRE_FOREIGN = "";
                string GENRE_HORROR = "";
                string GENRE_INDEPENDENT = "";
                string GENRE_ROMANCE = "";
                string GENRE_SCIFI = "";
                string GENRE_SHORTS = "";
                string GENRE_SUSPENSE = "";
                string INTERNET_CONNECTION = "";
                string PPV_CABLE_SAT_PROVIDER = "";
                string PPV_FIND_CABLE_SAT_GUIDE = "";
                string PPV_FIND_DIRECT_MAIL = "";
                string PPV_FIND_INTERNET = "";
                string PPV_FIND_MAGAZINE = "";
                string PPV_FIND_NEWSPAPER = "";
                string PPV_FIND_RADIO = "";
                string PPV_FIND_TV_COMMERCIAL = "";
                string PPV_FIND_TV_SNEAK_PREVIEW = "";
                string PPV_FIND_WORD_OF_MOUTH = "";
                string PPV_ORDER_EVENT = "";
                string PPV_ORDER_MOVIE = "";
                string PROGRAM = "";
                string PROGRAM_NAME = "";
                string SMS_DIN = "";
                string SMS_PERMISSION = "";
                string SMS_SERVICE_PROVIDER = "";
                string New_Custom_Attributes = "";
                string UO_PREV_VISITS = "";
                string UO_MOST_RECENT_VISIT_MONTH = "";
                string UO_MOST_RECENT_VISIT_YEAR = "";
                string UO_NEXT_PLANNED_VISIT_MONTH = "";
                string UO_NEXT_PLANNED_VISIT_YEAR = "";
                string UPR_CHILD_1_BIRTH_YEAR = "";
                string UPR_CHILD_2_BIRTH_YEAR = "";
                string UPR_CHILD_3_BIRTH_YEAR = "";
                string UPR_CHILD_4_BIRTH_YEAR = "";
                string UO_HOTEL_SET_NEXT_VISIT = "";
                string UO_STAYED_AT_ONSITE_HOTEL = "";
                string UO_TICKET_TYPE = "";
                string UO_ANNUAL_PASS_EXP_MONTH = "";
                string UO_ANNUAL_PASS_EXP_YEAR = "";
                string UO_ANNUAL_PASS_FLEX_PAY = "";
                string UO_PURCHASED_EXPRESS = "";
                string UO_TICKET_PURCHASE_MONTH = "";
                string UO_TICKET_PURCHASE_YEAR = "";
                string UO_TICKET_REDEEMED_MONTH = "";
                string UO_TICKET_REDEEMED_YEAR = "";
                string UO_SOURCE_PAGE = "";
                string UO_NO_OPTIN = "";
                string New_Attributes_Home_Ent = "";
                string USHE_DVD_PURCHASE_MONTH = "";
                string USHE_DVD_PURCHASE_TYPE = "";
                string USHE_DVD_ONLINE_INFO_SOURCE = "";
                string GENRE_TV_ON_DVD = "";
                string USH_SALES_ACCOUNT = "";
                string USH_SALES_GROUP = "";
                string promo = "";
                string custom_url = "";
                string TridionID_ = "";
                string URLMap_ = "";
                string URLUnsub_ = "";
                string last_update = "";

                string email = sqlDataReader.GetString(0);
                DateTime demographics = sqlDataReader.GetDateTime(1);
                if (!(sqlDataReader.IsDBNull(2)))
                {
                    Additional_ = sqlDataReader.GetString(2);
                }
                if (!(sqlDataReader.IsDBNull(3)))
                {
                    AppNeeded_ = sqlDataReader.GetString(3);
                }
                if (!(sqlDataReader.IsDBNull(4)))
                {
                    CanAppPend_ = sqlDataReader.GetString(4);
                }
                if (!(sqlDataReader.IsDBNull(5)))
                {
                    CleanAuto_ = sqlDataReader.GetString(5);
                }
                if (!(sqlDataReader.IsDBNull(6)))
                {
                    Comment_ = sqlDataReader.GetString(6);
                }
                if (!(sqlDataReader.IsDBNull(7)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(7);
                    ConfirmDat_ = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }
                if (!(sqlDataReader.IsDBNull(8)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(8);
                    DateHeld_ = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }
                if (!(sqlDataReader.IsDBNull(9)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(9);
                    DateJoined_ = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }
                if (!(sqlDataReader.IsDBNull(10)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(10);
                    DateUnsub_ = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }
                if (!(sqlDataReader.IsDBNull(11)))
                {
                    Domain_ = sqlDataReader.GetString(11);
                }
                if (!(sqlDataReader.IsDBNull(12)))
                {
                    EnableWYSIWYG_ = sqlDataReader.GetString(12);
                }
                if (!(sqlDataReader.IsDBNull(13)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(13);
                    ExpireDate_ = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }
                if (!(sqlDataReader.IsDBNull(14)))
                {
                    FullName_ = sqlDataReader.GetString(14);
                }
                if (!(sqlDataReader.IsDBNull(15)))
                {
                    IsListAdm_ = sqlDataReader.GetString(15);
                }
                if (!(sqlDataReader.IsDBNull(16)))
                {
                    List_ = sqlDataReader.GetString(16);
                }
                if (!(sqlDataReader.IsDBNull(17)))
                {
                    MailFormat_ = sqlDataReader.GetString(17);
                }

                int MemberID_ = sqlDataReader.GetInt32(18);
                if (!(sqlDataReader.IsDBNull(19)))
                {
                    MemberType_ = sqlDataReader.GetString(19);
                }
                if (!(sqlDataReader.IsDBNull(20)))
                {
                    NoRepro_ = sqlDataReader.GetString(20);
                }
                if (!(sqlDataReader.IsDBNull(21)))
                {
                    NotifyErr_ = sqlDataReader.GetString(21);
                }
                if (!(sqlDataReader.IsDBNull(22)))
                {
                    NotifySubm_ = sqlDataReader.GetString(22);
                }
                if (!(sqlDataReader.IsDBNull(23)))
                {
                    fooIntPlaceKeeper = sqlDataReader.GetInt32(23);
                    NumAppNeed_ = fooIntPlaceKeeper.ToString();
                }
                if (!(sqlDataReader.IsDBNull(24)))
                {
                    fooIntPlaceKeeper = sqlDataReader.GetInt32(24);
                    NumBounces_ = fooIntPlaceKeeper.ToString();
                }
                if (!(sqlDataReader.IsDBNull(25)))
                {
                    Password_ = sqlDataReader.GetString(25);
                }
                if (!(sqlDataReader.IsDBNull(26)))
                {
                    fooIntPlaceKeeper = sqlDataReader.GetInt32(26);
                    PermissionGroupID_ = fooIntPlaceKeeper.ToString();
                }
                if (!(sqlDataReader.IsDBNull(27)))
                {
                    RcvAdmMail_ = sqlDataReader.GetString(27);
                }
                if (!(sqlDataReader.IsDBNull(28)))
                {
                    ReadsHtml_ = sqlDataReader.GetString(28);
                }
                if (!(sqlDataReader.IsDBNull(29)))
                {
                    ReceiveAck_ = sqlDataReader.GetString(29);
                }
                if (!(sqlDataReader.IsDBNull(30)))
                {
                    SyncCanResubscribe_ = sqlDataReader.GetString(30);
                }
                if (!(sqlDataReader.IsDBNull(31)))
                {
                    SyncResubscribeType_ = sqlDataReader.GetString(31);
                }
                if (!(sqlDataReader.IsDBNull(32)))
                {
                    SubType_ = sqlDataReader.GetString(32);
                }
                if (!(sqlDataReader.IsDBNull(33)))
                {
                    fooIntPlaceKeeper = sqlDataReader.GetInt32(33);
                    UnsubMessageID_ = fooIntPlaceKeeper.ToString();
                }
                if (!(sqlDataReader.IsDBNull(34)))
                {
                    UserID_ = sqlDataReader.GetString(34);
                }
                if (!(sqlDataReader.IsDBNull(35)))
                {
                    UserNameLC_ = sqlDataReader.GetString(35);
                }
                if (!(sqlDataReader.IsDBNull(36)))
                {
                    First_Name = sqlDataReader.GetString(36);
                }
                if (!(sqlDataReader.IsDBNull(37)))
                {
                    Last_Name = sqlDataReader.GetString(37);
                }
                if (!(sqlDataReader.IsDBNull(38)))
                {
                    Address1 = sqlDataReader.GetString(38);
                }
                if (!(sqlDataReader.IsDBNull(39)))
                {
                    Address2 = sqlDataReader.GetString(39);
                }
                if (!(sqlDataReader.IsDBNull(40)))
                {
                    City = sqlDataReader.GetString(40);
                }
                if (!(sqlDataReader.IsDBNull(41)))
                {
                    State = sqlDataReader.GetString(41);
                }
                if (!(sqlDataReader.IsDBNull(42)))
                {
                    Zip_Code = sqlDataReader.GetString(42);
                }
                if (!(sqlDataReader.IsDBNull(43)))
                {
                    Country = sqlDataReader.GetString(43);
                }
                if (!(sqlDataReader.IsDBNull(44)))
                {
                    Country_Code = sqlDataReader.GetString(44);
                }
                if (!(sqlDataReader.IsDBNull(45)))
                {
                    Birth_Day = sqlDataReader.GetString(45);
                }
                if (!(sqlDataReader.IsDBNull(46)))
                {
                    Birth_Month = sqlDataReader.GetString(46);
                }
                if (!(sqlDataReader.IsDBNull(47)))
                {
                    Birth_Year = sqlDataReader.GetString(47);
                }
                if (!(sqlDataReader.IsDBNull(48)))
                {
                    Client_Member_ID = sqlDataReader.GetString(48);
                }
                if (!(sqlDataReader.IsDBNull(49)))
                {
                    Gender = sqlDataReader.GetString(49);
                }
                if (!(sqlDataReader.IsDBNull(50)))
                {
                    Area_Code = sqlDataReader.GetString(50);
                }
                if (!(sqlDataReader.IsDBNull(51)))
                {
                    Phone = sqlDataReader.GetString(51);
                }
                if (!(sqlDataReader.IsDBNull(52)))
                {
                    Old_Email = sqlDataReader.GetString(52);
                }
                if (!(sqlDataReader.IsDBNull(53)))
                {
                    Registration_date = sqlDataReader.GetString(53);
                }
                if (!(sqlDataReader.IsDBNull(54)))
                {
                    IMPACT_Member_ID = sqlDataReader.GetString(54);
                }
                if (!(sqlDataReader.IsDBNull(55)))
                {
                    IMPACT_Source_Code = sqlDataReader.GetString(55);
                }
                if (!(sqlDataReader.IsDBNull(56)))
                {
                    IMPACT_Vendor_Id = sqlDataReader.GetString(56);
                }
                if (!(sqlDataReader.IsDBNull(57)))
                {
                    Preferred_email_type = sqlDataReader.GetString(57);
                }
                if (!(sqlDataReader.IsDBNull(58)))
                {
                    Source = sqlDataReader.GetString(58);
                }
                if (!(sqlDataReader.IsDBNull(59)))
                {
                    Optins_Lists = sqlDataReader.GetString(59);
                }
                if (!(sqlDataReader.IsDBNull(60)))
                {
                    CAP_UO_PASSTYPE = sqlDataReader.GetString(60);
                }
                if (!(sqlDataReader.IsDBNull(61)))
                {
                    DOGSHOW_NUM_CATS = sqlDataReader.GetString(61);
                }
                if (!(sqlDataReader.IsDBNull(62)))
                {
                    DOGSHOW_NUM_DOGS = sqlDataReader.GetString(62);
                }
                if (!(sqlDataReader.IsDBNull(63)))
                {
                    GENRE_ACTION = sqlDataReader.GetString(63);
                }
                if (!(sqlDataReader.IsDBNull(64)))
                {
                    Current_Custom_Attributes = sqlDataReader.GetString(64);
                }
                if (!(sqlDataReader.IsDBNull(65)))
                {
                    GENRE_ADVENTURE = sqlDataReader.GetString(65);
                }
                if (!(sqlDataReader.IsDBNull(66)))
                {
                    GENRE_ANIMATION = sqlDataReader.GetString(66);
                }
                if (!(sqlDataReader.IsDBNull(67)))
                {
                    GENRE_CHILDREN = sqlDataReader.GetString(67);
                }
                if (!(sqlDataReader.IsDBNull(68)))
                {
                    GENRE_COMEDY = sqlDataReader.GetString(68);
                }
                if (!(sqlDataReader.IsDBNull(69)))
                {
                    GENRE_DRAMA = sqlDataReader.GetString(69);
                }
                if (!(sqlDataReader.IsDBNull(70)))
                {
                    GENRE_FAMILY = sqlDataReader.GetString(70);
                }
                if (!(sqlDataReader.IsDBNull(71)))
                {
                    GENRE_FOREIGN = sqlDataReader.GetString(71);
                }
                if (!(sqlDataReader.IsDBNull(72)))
                {
                    GENRE_HORROR = sqlDataReader.GetString(72);
                }
                if (!(sqlDataReader.IsDBNull(73)))
                {
                    GENRE_INDEPENDENT = sqlDataReader.GetString(73);
                }
                if (!(sqlDataReader.IsDBNull(74)))
                {
                    GENRE_ROMANCE = sqlDataReader.GetString(74);
                }
                if (!(sqlDataReader.IsDBNull(75)))
                {
                    GENRE_SCIFI = sqlDataReader.GetString(75);
                }
                if (!(sqlDataReader.IsDBNull(76)))
                {
                    GENRE_SHORTS = sqlDataReader.GetString(76);
                }
                if (!(sqlDataReader.IsDBNull(77)))
                {
                    GENRE_SUSPENSE = sqlDataReader.GetString(77);
                }
                if (!(sqlDataReader.IsDBNull(78)))
                {
                    INTERNET_CONNECTION = sqlDataReader.GetString(78);
                }
                if (!(sqlDataReader.IsDBNull(79)))
                {
                    PPV_CABLE_SAT_PROVIDER = sqlDataReader.GetString(79);
                }
                if (!(sqlDataReader.IsDBNull(80)))
                {
                    PPV_CABLE_SAT_PROVIDER = sqlDataReader.GetString(80);
                }
                if (!(sqlDataReader.IsDBNull(81)))
                {
                    PPV_FIND_DIRECT_MAIL = sqlDataReader.GetString(81);
                }
                if (!(sqlDataReader.IsDBNull(82)))
                {
                    PPV_FIND_INTERNET = sqlDataReader.GetString(82);
                }
                if (!(sqlDataReader.IsDBNull(83)))
                {
                    PPV_FIND_MAGAZINE = sqlDataReader.GetString(83);
                }
                if (!(sqlDataReader.IsDBNull(84)))
                {
                    PPV_FIND_NEWSPAPER = sqlDataReader.GetString(84);
                }
                if (!(sqlDataReader.IsDBNull(85)))
                {
                    PPV_FIND_RADIO = sqlDataReader.GetString(85);
                }
                if (!(sqlDataReader.IsDBNull(86)))
                {
                    PPV_FIND_TV_COMMERCIAL = sqlDataReader.GetString(86);
                }
                if (!(sqlDataReader.IsDBNull(87)))
                {
                    PPV_FIND_TV_SNEAK_PREVIEW = sqlDataReader.GetString(87);
                }
                if (!(sqlDataReader.IsDBNull(88)))
                {
                    PPV_FIND_WORD_OF_MOUTH = sqlDataReader.GetString(88);
                }
                if (!(sqlDataReader.IsDBNull(89)))
                {
                    PPV_ORDER_EVENT = sqlDataReader.GetString(89);
                }
                if (!(sqlDataReader.IsDBNull(90)))
                {
                    PPV_ORDER_MOVIE = sqlDataReader.GetString(90);
                }
                if (!(sqlDataReader.IsDBNull(91)))
                {
                    PROGRAM = sqlDataReader.GetString(91);
                }
                if (!(sqlDataReader.IsDBNull(92)))
                {
                    PROGRAM_NAME = sqlDataReader.GetString(92);
                }
                if (!(sqlDataReader.IsDBNull(93)))
                {
                    SMS_DIN = sqlDataReader.GetString(93);
                }
                if (!(sqlDataReader.IsDBNull(94)))
                {
                    SMS_PERMISSION = sqlDataReader.GetString(94);
                }
                if (!(sqlDataReader.IsDBNull(95)))
                {
                    SMS_SERVICE_PROVIDER = sqlDataReader.GetString(95);
                }
                if (!(sqlDataReader.IsDBNull(96)))
                {
                    New_Custom_Attributes = sqlDataReader.GetString(96);
                }
                if (!(sqlDataReader.IsDBNull(97)))
                {
                    UO_PREV_VISITS = sqlDataReader.GetString(97);
                }
                if (!(sqlDataReader.IsDBNull(98)))
                {
                    UO_MOST_RECENT_VISIT_MONTH = sqlDataReader.GetString(98);
                }
                if (!(sqlDataReader.IsDBNull(99)))
                {
                    UO_MOST_RECENT_VISIT_YEAR = sqlDataReader.GetString(99);
                }
                if (!(sqlDataReader.IsDBNull(100)))
                {
                    UO_MOST_RECENT_VISIT_YEAR = sqlDataReader.GetString(100);
                }
                if (!(sqlDataReader.IsDBNull(101)))
                {
                    UO_NEXT_PLANNED_VISIT_YEAR = sqlDataReader.GetString(101);
                }
                if (!(sqlDataReader.IsDBNull(102)))
                {
                    UO_NEXT_PLANNED_VISIT_YEAR = sqlDataReader.GetString(102);
                }
                if (!(sqlDataReader.IsDBNull(103)))
                {
                    UPR_CHILD_2_BIRTH_YEAR = sqlDataReader.GetString(103);
                }
                if (!(sqlDataReader.IsDBNull(104)))
                {
                    UPR_CHILD_3_BIRTH_YEAR = sqlDataReader.GetString(104);
                }
                if (!(sqlDataReader.IsDBNull(105)))
                {
                    UPR_CHILD_4_BIRTH_YEAR = sqlDataReader.GetString(105);
                }
                if (!(sqlDataReader.IsDBNull(106)))
                {
                    UO_HOTEL_SET_NEXT_VISIT = sqlDataReader.GetString(106);
                }
                if (!(sqlDataReader.IsDBNull(107)))
                {
                    UO_STAYED_AT_ONSITE_HOTEL = sqlDataReader.GetString(107);
                }
                if (!(sqlDataReader.IsDBNull(108)))
                {
                    UO_TICKET_TYPE = sqlDataReader.GetString(108);
                }
                if (!(sqlDataReader.IsDBNull(109)))
                {
                    UO_ANNUAL_PASS_EXP_MONTH = sqlDataReader.GetString(109);
                }
                if (!(sqlDataReader.IsDBNull(110)))
                {
                    UO_ANNUAL_PASS_EXP_YEAR = sqlDataReader.GetString(110);
                }
                if (!(sqlDataReader.IsDBNull(111)))
                {
                    UO_ANNUAL_PASS_FLEX_PAY = sqlDataReader.GetString(111);
                }
                if (!(sqlDataReader.IsDBNull(112)))
                {
                    UO_PURCHASED_EXPRESS = sqlDataReader.GetString(112);
                }
                if (!(sqlDataReader.IsDBNull(113)))
                {
                    UO_TICKET_PURCHASE_MONTH = sqlDataReader.GetString(113);
                }
                if (!(sqlDataReader.IsDBNull(114)))
                {
                    UO_TICKET_PURCHASE_YEAR = sqlDataReader.GetString(114);
                }
                if (!(sqlDataReader.IsDBNull(115)))
                {
                    UO_TICKET_REDEEMED_MONTH = sqlDataReader.GetString(115);
                }
                if (!(sqlDataReader.IsDBNull(116)))
                {
                    UO_TICKET_REDEEMED_YEAR = sqlDataReader.GetString(116);
                }
                if (!(sqlDataReader.IsDBNull(117)))
                {
                    UO_SOURCE_PAGE = sqlDataReader.GetString(117);
                }
                if (!(sqlDataReader.IsDBNull(118)))
                {
                    UO_NO_OPTIN = sqlDataReader.GetString(118);
                }
                if (!(sqlDataReader.IsDBNull(119)))
                {
                    New_Attributes_Home_Ent = sqlDataReader.GetString(119);
                }
                if (!(sqlDataReader.IsDBNull(120)))
                {
                    USHE_DVD_PURCHASE_MONTH = sqlDataReader.GetString(120);
                }
                if (!(sqlDataReader.IsDBNull(121)))
                {
                    USHE_DVD_PURCHASE_TYPE = sqlDataReader.GetString(121);
                }
                if (!(sqlDataReader.IsDBNull(122)))
                {
                    USHE_DVD_ONLINE_INFO_SOURCE = sqlDataReader.GetString(122);
                }
                if (!(sqlDataReader.IsDBNull(123)))
                {
                    GENRE_TV_ON_DVD = sqlDataReader.GetString(123);
                }
                if (!(sqlDataReader.IsDBNull(124)))
                {
                    USH_SALES_ACCOUNT = sqlDataReader.GetString(124);
                }
                if (!(sqlDataReader.IsDBNull(125)))
                {
                    USH_SALES_GROUP = sqlDataReader.GetString(125);
                }
                if (!(sqlDataReader.IsDBNull(126)))
                {
                    promo = sqlDataReader.GetString(126);
                }
                if (!(sqlDataReader.IsDBNull(127)))
                {
                    custom_url = sqlDataReader.GetString(127);
                }
                if (!(sqlDataReader.IsDBNull(128)))
                {
                    TridionID_ = sqlDataReader.GetString(128);
                }
                if (!(sqlDataReader.IsDBNull(129)))
                {
                    URLMap_ = sqlDataReader.GetString(129);
                }
                if (!(sqlDataReader.IsDBNull(130)))
                {
                    URLUnsub_ = sqlDataReader.GetString(130);
                }
                if (!(sqlDataReader.IsDBNull(131)))
                {
                    fooDateTimePlaceKeeper = sqlDataReader.GetDateTime(131);
                    last_update = fooDateTimePlaceKeeper.ToString("yyyy-MM-dd HH:mm:ss.000");
                }



                string seperator = "\",\"";
                strRow = String.Concat("\"", email.ToString(), seperator, demographics.ToString("yyyy-MM-dd HH:mm:ss.000"),
                    seperator, Additional_.ToString(), seperator, AppNeeded_.ToString(), seperator,
                    CanAppPend_.ToString(), seperator, CleanAuto_.ToString(), seperator,
                    Comment_.ToString(), seperator, ConfirmDat_.ToString(), seperator,
                    DateHeld_.ToString(), seperator, DateJoined_.ToString(), seperator,
                    DateUnsub_.ToString(), seperator, Domain_.ToString(), seperator,
                    EnableWYSIWYG_.ToString(), seperator, ExpireDate_.ToString(), seperator,
                    FullName_.ToString(), seperator, IsListAdm_.ToString(), seperator,
                    List_.ToString(), seperator, MailFormat_.ToString(), seperator,
                    MemberID_.ToString(), seperator, MemberType_.ToString(), seperator,
                    NoRepro_.ToString(), seperator, NotifyErr_.ToString(), seperator,
                    NotifySubm_.ToString(), seperator, NumAppNeed_.ToString(), seperator,
                    NumBounces_.ToString(), seperator, Password_.ToString(), seperator,
                    PermissionGroupID_.ToString(), seperator, RcvAdmMail_.ToString(), seperator,
                    ReadsHtml_.ToString(), seperator, ReceiveAck_.ToString(), seperator,
                    SyncCanResubscribe_.ToString(), seperator, SyncResubscribeType_.ToString(), seperator,
                    SubType_.ToString(), seperator, UnsubMessageID_.ToString(), seperator,
                    UserID_.ToString(), seperator, UserNameLC_.ToString(), seperator,
                    First_Name.ToString(), seperator, Last_Name.ToString(), seperator,
                    Address1.ToString(), seperator, Address2.ToString(), seperator,
                    City.ToString(), seperator, State.ToString(), seperator,
                    Zip_Code.ToString(), seperator, Country.ToString(), seperator,
                    Country_Code.ToString(), seperator, Birth_Day.ToString(), seperator,
                    Birth_Month.ToString(), seperator, Birth_Year.ToString(), seperator,
                    Client_Member_ID.ToString(), seperator, Gender.ToString(), seperator,
                    Area_Code.ToString(), seperator, Phone.ToString(), seperator,
                    Old_Email.ToString(), seperator, Registration_date.ToString(), seperator,
                    IMPACT_Member_ID.ToString(), seperator, IMPACT_Source_Code.ToString(), seperator,
                    IMPACT_Vendor_Id.ToString(), seperator, Preferred_email_type.ToString(), seperator,
                    Source.ToString(), seperator, Optins_Lists.ToString(), seperator,
                    CAP_UO_PASSTYPE.ToString(), seperator, DOGSHOW_NUM_CATS.ToString(), seperator,
                    DOGSHOW_NUM_DOGS.ToString(), seperator, GENRE_ACTION.ToString(), seperator,
                    Current_Custom_Attributes.ToString(), seperator, GENRE_ADVENTURE.ToString(), seperator,
                    GENRE_ANIMATION.ToString(), seperator, GENRE_CHILDREN.ToString(), seperator,
                    GENRE_COMEDY.ToString(), seperator, GENRE_DRAMA.ToString(), seperator,
                    GENRE_FAMILY.ToString(), seperator, GENRE_FOREIGN.ToString(), seperator,
                    GENRE_HORROR.ToString(), seperator, GENRE_INDEPENDENT.ToString(), seperator,
                    GENRE_ROMANCE.ToString(), seperator, GENRE_SCIFI.ToString(), seperator,
                    GENRE_SHORTS.ToString(), seperator, GENRE_SUSPENSE.ToString(), seperator,
                    INTERNET_CONNECTION.ToString(), seperator, PPV_CABLE_SAT_PROVIDER.ToString(), seperator,
                    PPV_FIND_CABLE_SAT_GUIDE.ToString(), seperator, PPV_FIND_DIRECT_MAIL.ToString(), seperator,
                    PPV_FIND_INTERNET.ToString(), seperator, PPV_FIND_MAGAZINE.ToString(), seperator,
                    PPV_FIND_NEWSPAPER.ToString(), seperator, PPV_FIND_RADIO.ToString(), seperator,
                    PPV_FIND_TV_COMMERCIAL.ToString(), seperator, PPV_FIND_TV_SNEAK_PREVIEW.ToString(), seperator,
                    PPV_FIND_WORD_OF_MOUTH.ToString(), seperator, PPV_ORDER_EVENT.ToString(), seperator,
                    PPV_ORDER_MOVIE.ToString(), seperator, PROGRAM.ToString(), seperator,
                    PROGRAM_NAME.ToString(), seperator, SMS_DIN.ToString(), seperator,
                    SMS_PERMISSION.ToString(), seperator, SMS_SERVICE_PROVIDER.ToString(), seperator,
                    New_Custom_Attributes.ToString(), seperator, UO_PREV_VISITS.ToString(), seperator,
                    UO_MOST_RECENT_VISIT_MONTH.ToString(), seperator, UO_MOST_RECENT_VISIT_YEAR.ToString(), seperator,
                    UO_NEXT_PLANNED_VISIT_MONTH.ToString(), seperator, UO_NEXT_PLANNED_VISIT_YEAR.ToString(), seperator,
                    UPR_CHILD_1_BIRTH_YEAR.ToString(), seperator, UPR_CHILD_2_BIRTH_YEAR.ToString(), seperator,
                    UPR_CHILD_3_BIRTH_YEAR.ToString(), seperator, UPR_CHILD_4_BIRTH_YEAR.ToString(), seperator,
                    UO_HOTEL_SET_NEXT_VISIT.ToString(), seperator, UO_STAYED_AT_ONSITE_HOTEL.ToString(), seperator,
                    UO_TICKET_TYPE.ToString(), seperator, UO_ANNUAL_PASS_EXP_MONTH.ToString(), seperator,
                    UO_ANNUAL_PASS_EXP_YEAR.ToString(), seperator, UO_ANNUAL_PASS_FLEX_PAY.ToString(), seperator,
                    UO_PURCHASED_EXPRESS.ToString(), seperator, UO_TICKET_PURCHASE_MONTH.ToString(), seperator,
                    UO_TICKET_PURCHASE_YEAR.ToString(), seperator, UO_TICKET_REDEEMED_MONTH.ToString(), seperator,
                    UO_TICKET_REDEEMED_YEAR.ToString(), seperator, UO_SOURCE_PAGE.ToString(), seperator,
                    UO_NO_OPTIN.ToString(), seperator, New_Attributes_Home_Ent.ToString(), seperator,
                    USHE_DVD_PURCHASE_MONTH.ToString(), seperator, USHE_DVD_PURCHASE_TYPE.ToString(), seperator,
                    USHE_DVD_ONLINE_INFO_SOURCE.ToString(), seperator, GENRE_TV_ON_DVD.ToString(), seperator,
                    USH_SALES_ACCOUNT.ToString(), seperator, USH_SALES_GROUP.ToString(), seperator,
                    promo.ToString(), seperator, custom_url.ToString(), seperator,
                    TridionID_.ToString(), seperator, URLMap_.ToString(), seperator,
                    URLUnsub_.ToString(), seperator, last_update.ToString(),
                    "\"");
                streamWriter.WriteLine(strRow);
                count++;

            }

            streamWriter.Close();
            sqlConnection.Close();
            Log.WriteLine(String.Concat(listName, " Member File Created with ", count, " records"));
        }

        public static string columnNames(DataTable dtSchemaTable, string delimiter)
        {
            string str1 = "";
            bool flag = delimiter.ToLower() == "tab" == false;
            if (!flag)
            {
                delimiter = "\t";
            }
            int i = 0;
            do
            {
                str1 = String.Concat(str1, dtSchemaTable.Rows[i][0].ToString());
                flag = i < dtSchemaTable.Rows.Count - 1 == false;
                if (!flag)
                {
                    str1 = String.Concat(str1, delimiter);
                }
                i++;
            IL_0070:
                flag = i < dtSchemaTable.Rows.Count;
            }
            while (flag);
            return str1;
        }
    }
}
