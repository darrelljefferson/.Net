using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HayhouseMonthlyReportGenerator
{
    class utility
    {
        public static string ReturnHTMLEnocodedString(string input)
        {
            
            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
            // Any code begins the 30-day trial.
            crypt.UnlockComponent("LYRISCCrypt_4NataCGcVVkW ");
            crypt.CryptAlgorithm = "none";
            crypt.EncodingMode = "url";
            
            return crypt.EncryptStringENC(input);


        }

        // DataTable for the reporting
        public static DataTable reportingTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Subject", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Sent_Date", typeof(string));
            table.Columns.Add("From", typeof(string));
            table.Columns.Add("Segment", typeof(string));
            table.Columns.Add("Sent", typeof(string));
            table.Columns.Add("Total_Opened", typeof(string));
            table.Columns.Add("Unique_Opened", typeof(string));
            table.Columns.Add("%Unique_Opened", typeof(string));
            table.Columns.Add("Unique_Clicked", typeof(string));
            table.Columns.Add("%Unique_Clicked", typeof(string));
            table.Columns.Add("Unsubscribed", typeof(string));
            table.Columns.Add("Bounced", typeof(string));
            table.Columns.Add("%Bounced", typeof(string));
            table.Columns.Add("Hard_Bounced", typeof(string));
            table.Columns.Add("%Hard_Bounced", typeof(string));
            table.Columns.Add("Soft_Bounced", typeof(string));
            table.Columns.Add("%Soft_Bounced", typeof(string));
            table.Columns.Add("Delivered", typeof(string));
            table.Columns.Add("%Delivered", typeof(string));
            table.Columns.Add("Spam_Complaints", typeof(string));
            table.Columns.Add("Unique_Referrers", typeof(string));


            return table;


        }

        public static Dictionary<string,string>SegmentIDTable()
        {

            // build up the Dictionary of segmentid's
            Dictionary<string, string> segmentPairs = new Dictionary<string, string>()
            {
                {"42606","A: Bill Phillips"},
                {"42607","A: Carol Ritberger, Ph.D. News"},
                {"42570","A: Caroline Myss Newsletter"},
                {"42513","B: Master:Virtue"},
                {"40330","B: Master:Northrup"},
                {"39710"," B: Master:Dyer"},
                {"42564","A: Gary Renard Enews"},
                {"40335","B: HYL:Get Healthy Newsletter"},
                {"43235","Affiliates"},
                {"55266","B: Hay House Australia Newsletter"},
                {"47574","BL: Hay House Book Launch"},
                {"39703","B: Master: HH"},
                {"40329","B: Master:HHR"},
                {"40645"," B: HYL:HealYourLifeNewsletter"},
                {"49567"," B: ICANDOIT.net"},
                {"42572","A: Immaculee Ilibagiza: LTT"},
                {"40336","B: HYL:Inspiration Newsletter"},
                {"40333","B: HYL:Inspiration Newsletter"},
                {"42573"," A: Joan Borysenko,Ph.D. E-news"},
                {"42566","A: John Holland E-newsletter"},
                {"40328","B: Master:LH"},
                {"49246","B: Oh My God Movie Newsletter"},
                {"40332","B: Master:LH"},
                {"42568","A: PlayGround Pump Enews"},
                {"42567","A: Sonia Choquette Newsletter"},
                {"40334","B: HYL:SuccessAbundanceNewsletter"},
                {"42605","A: SuzeOrman Updates[DontSend]"},
                {"53528","Wisdom Community"},
                {"43279","Northrup:WWC only"},
                {"49221","B: You Can Heal Your Life Movie"},
                {"42565","A: ColetteBaronReid Newsletter"},
                {"49566","B: The Shift Movie"} 
            };

            return segmentPairs;
        }

        public static Dictionary<string, string[]> SubscribeUnsubscribeIDTable()
        {
            // build up the segment and opt-out id's to be parsed out later
            Dictionary<string, string[]> SubUnsubDemographicIDS = new Dictionary<string, string[]>();
            
            
            //Bill Philips
            // set the array to be passed in
            string[] id0 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id0[0] = "65917";
            id0[1] = "65918";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42606", id0);
            //Carol Ritberger
            // set the array to be passed in
            string[] id1 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id1[0]="65239";
            id1[1]="65240";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42607", id1);
            //Caroline Myss Newsletter
            // set the array to be passed in
            string[] id2 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id2[0] = "65241";
            id2[1] = "65242";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42570", id2);
            //Doreen Virtue
            // set the array to be passed in
            string[] id3 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id3[0] = "65215";
            id3[1] = "65216";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42513", id3);
            //Dr. Christiane Northrup Enews
            // set the array to be passed in
            string[] id4 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id4[0] = "65217";
            id4[1] = "65218";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40330", id4);
            //Dr. Wayne W. Dyer
            // set the array to be passed in
            string[] id5 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id5[0] = "65199";
            id5[1] = "65200";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("39710", id5);
            //Gary Renard Enews
            // set the array to be passed in
            string[] id6 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id6[0] = "65245";
            id6[1] = "65246";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42564", id6);
            //Get Healthy Newsletter
            // set the array to be passed in
            string[] id7 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id7[0] = "65201";
            id7[1] = "65202";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40335", id7);
            //Hay House Affiliate Newsletter
            // set the array to be passed in
            string[] id8 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id8[0] = "65229";
            id8[1] = "65230";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("43235", id8);
            //Hay House Australia Newsletter
            // set the array to be passed in
            string[] id9 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id9[0] = "65231";
            id9[1] = "65232";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("55266", id9);
            //Hay House Book Launch
            // set the array to be passed in
            string[] id10 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id10[0] = "65235";
            id10[1] = "65236";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("47574", id10);
            //Hay House Newsletter
            // set the array to be passed in
            string[] id11 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id11[0] = "65193";
            id11[1] = "65194";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("39703", id11);
            //Hay House Radio Guide
            // set the array to be passed in
            string[] id12 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id12[0] = "65195";
            id12[1] = "65196";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40329", id12);
            //Heal Your Life Newsletter
            // set the array to be passed in
            string[] id13 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id13[0] = "65203";
            id13[1] = "65204";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40645", id13);
            //I Can Do It! Event Enews
            // set the array to be passed in
            string[] id14 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id14[0] = "65213";
            id14[1] = "65214";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("49567", id14);
            //Immaculee Ilibagiza: Left to Tell
            // set the array to be passed in
            string[] id15 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id15[0] = "65247";
            id15[1] = "65248";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42572", id15);
            //Inspiration Newsletter
            // set the array to be passed in
            string[] id16 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id16[0] = "65205";
            id16[1] = "65206";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40336", id16);
            //Intuitive Guidance Newsletter
            // set the array to be passed in
            string[] id17 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id17[0] = "65207";
            id17[1] = "65208";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40333", id17);
            //Joan Borysenko, Ph.D. E-newsletter
            // set the array to be passed in
            string[] id18 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id18[0] = "65249";
            id18[1] = "65250";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42573", id18);
            //John Holland E-newsletter
            // set the array to be passed in
            string[] id19 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id19[0] = "65251";
            id19[1] = "65252";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42566", id19);
            //Louise L. Hay Enews
            // set the array to be passed in
            string[] id20 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id20[0] = "65197";
            id20[1] = "65198";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40328", id20);
            //Oh My God Newsletter
            // set the array to be passed in
            string[] id22 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id22[0] = "65219";
            id22[1] = "65220";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("49246", id22);
            //Personal Growth Newsletter 
            // set the array to be passed in
            string[] id23 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id23[0] = "65209";
            id23[1] = "65210";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40332", id23);
            //PlayGround Pump Enews
            // set the array to be passed in
            string[] id24 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id24[0] = "65253";
            id24[1] = "65254";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42568", id24);
            //Sonia Choquette Newsletter
            // set the array to be passed in
            string[] id25 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id25[0] = "65255";
            id25[1] = "65256";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42567", id25);
            //Success & Abundance Newsletter
            // set the array to be passed in
            string[] id26 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id26[0] = "65211";
            id26[1] = "65212";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("40334", id26);
            //Suze Orman Ups
            // set the array to be passed in
            string[] id27 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id27[0] = "65257";
            id27[1] = "65258";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42605", id27);
            //Wisdom Community
            // set the array to be passed in
            string[] id28 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id28[0] = "65225";
            id28[1] = "65226";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("53528", id28);
            //Womens Wisdom Circle
            // set the array to be passed in
            string[] id29 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id29[0] = "65227";
            id29[1] = "65228";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("43279", id29);
            //You Can Heal Your Life Movie
            // set the array to be passed in
            string[] id30 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id30[0] = "65223";
            id30[1] = "65224";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("49221", id30);
            //Colette Baron-Reid Newsletter
            // set the array to be passed in
            string[] id31 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id31[0] = "65243";
            id31[1] = "65244";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("42565", id31);
            //The Shift Movie
            // set the array to be passed in
            string[] id32 = new string[3];
            //set the values for array (should be subscribeid, unsubid)
            id32[0] = "65221";
            id32[1] = "65222";
            //add the corresponding segmentid and array of the demographic ids
            SubUnsubDemographicIDS.Add("49566", id32);



            return SubUnsubDemographicIDS;
        }
    }
}
