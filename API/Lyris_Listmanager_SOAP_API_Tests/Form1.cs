/*
 Written by Byron Whitlock            
 Copyright 2004 Lyris Technologies.   
*/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Lyris_Listmanager_SOAP_API_Tests
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	/// 

	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.TextBox userName;
		public System.Windows.Forms.TextBox password;
		public System.Windows.Forms.RichTextBox consoleOut;
		private System.Windows.Forms.Button runAllTests_btn;

		private lmapiSoap.lmapi lm; 
		private int[] singleMemberID;
		private string newListName;
		private System.Windows.Forms.Button button7;
		private int newListID;
		public System.Windows.Forms.TextBox NumMembers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button test_members_btn;
		private System.Windows.Forms.Button test_lists_btn;
		private System.Windows.Forms.Button test_content_btn;
		private System.Windows.Forms.Button test_sql_btn;
		private System.Windows.Forms.Button test_segments_btn;
		private System.Windows.Forms.Button test_mailing_btn;
		private System.Windows.Forms.Button member_perf_btn;
		private System.Windows.Forms.Button s_member_perf_btn;
		private System.Windows.Forms.Button test_site_topic_btn;
		private int newSqlID;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//




		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.test_members_btn = new System.Windows.Forms.Button();
            this.test_lists_btn = new System.Windows.Forms.Button();
            this.test_content_btn = new System.Windows.Forms.Button();
            this.test_sql_btn = new System.Windows.Forms.Button();
            this.test_segments_btn = new System.Windows.Forms.Button();
            this.test_mailing_btn = new System.Windows.Forms.Button();
            this.runAllTests_btn = new System.Windows.Forms.Button();
            this.userName = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.consoleOut = new System.Windows.Forms.RichTextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.s_member_perf_btn = new System.Windows.Forms.Button();
            this.NumMembers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.member_perf_btn = new System.Windows.Forms.Button();
            this.test_site_topic_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // test_members_btn
            // 
			this.test_members_btn.Location = new System.Drawing.Point(224, 8);
            this.test_members_btn.Name = "test_members_btn";
			this.test_members_btn.Size = new System.Drawing.Size(88, 24);
            this.test_members_btn.TabIndex = 2;
            this.test_members_btn.Text = "Test Members";
            this.test_members_btn.Click += new System.EventHandler(this.test_members_btn_Click);
            // 
            // test_lists_btn
            // 
			this.test_lists_btn.Location = new System.Drawing.Point(224, 40);
            this.test_lists_btn.Name = "test_lists_btn";
			this.test_lists_btn.Size = new System.Drawing.Size(88, 24);
            this.test_lists_btn.TabIndex = 2;
            this.test_lists_btn.Text = "Test Lists";
            this.test_lists_btn.Click += new System.EventHandler(this.test_lists_btn_Click);
            // 
            // test_content_btn
            // 
			this.test_content_btn.Location = new System.Drawing.Point(224, 72);
            this.test_content_btn.Name = "test_content_btn";
			this.test_content_btn.Size = new System.Drawing.Size(88, 24);
            this.test_content_btn.TabIndex = 2;
            this.test_content_btn.Text = "Test Content";
            this.test_content_btn.Click += new System.EventHandler(this.test_content_btn_Click);
            // 
            // test_sql_btn
            // 
			this.test_sql_btn.Location = new System.Drawing.Point(328, 8);
            this.test_sql_btn.Name = "test_sql_btn";
			this.test_sql_btn.Size = new System.Drawing.Size(88, 24);
            this.test_sql_btn.TabIndex = 2;
            this.test_sql_btn.Text = "Test SQL";
            this.test_sql_btn.Click += new System.EventHandler(this.test_sql_btn_Click);
            // 
            // test_segments_btn
            // 
			this.test_segments_btn.Location = new System.Drawing.Point(328, 40);
            this.test_segments_btn.Name = "test_segments_btn";
			this.test_segments_btn.Size = new System.Drawing.Size(88, 24);
            this.test_segments_btn.TabIndex = 2;
            this.test_segments_btn.Text = "Test Segments";
            this.test_segments_btn.Click += new System.EventHandler(this.test_segments_btn_Click);
            // 
            // test_mailing_btn
            // 
			this.test_mailing_btn.Location = new System.Drawing.Point(328, 72);
            this.test_mailing_btn.Name = "test_mailing_btn";
			this.test_mailing_btn.Size = new System.Drawing.Size(88, 24);
            this.test_mailing_btn.TabIndex = 2;
            this.test_mailing_btn.Text = "Test Mailings";
            this.test_mailing_btn.Click += new System.EventHandler(this.test_mailing_btn_Click);
            // 
            // runAllTests_btn
            // 
			this.runAllTests_btn.Location = new System.Drawing.Point(432, 72);
            this.runAllTests_btn.Name = "runAllTests_btn";
            this.runAllTests_btn.Size = new System.Drawing.Size(96, 56);
            this.runAllTests_btn.TabIndex = 3;
            this.runAllTests_btn.Text = "Run All Tests";
            this.runAllTests_btn.Click += new System.EventHandler(this.runAllTests_btn_Click);
            // 
            // userName
            // 
            this.userName.Location = new System.Drawing.Point(88, 16);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(112, 20);
            this.userName.TabIndex = 4;
            this.userName.Text = "admin";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(88, 48);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(112, 20);
            this.password.TabIndex = 4;
            this.password.Text = "lyris";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(48, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "User";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // consoleOut
            // 
            this.consoleOut.AcceptsTab = true;
            this.consoleOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleOut.AutoSize = true;
            this.consoleOut.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.consoleOut.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.consoleOut.Location = new System.Drawing.Point(8, 144);
            this.consoleOut.Name = "consoleOut";
            this.consoleOut.Size = new System.Drawing.Size(520, 480);
            this.consoleOut.TabIndex = 6;
            this.consoleOut.Text = "";
            this.consoleOut.WordWrap = false;
            // 
            // button7
            // 
			this.button7.Location = new System.Drawing.Point(432, 40);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(96, 24);
            this.button7.TabIndex = 7;
            this.button7.Text = "Clear Window";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // s_member_perf_btn
            // 
			this.s_member_perf_btn.Location = new System.Drawing.Point(192, 104);
            this.s_member_perf_btn.Name = "s_member_perf_btn";
			this.s_member_perf_btn.Size = new System.Drawing.Size(120, 32);
            this.s_member_perf_btn.TabIndex = 2;
            this.s_member_perf_btn.Text = "CreateSingleMember Performance";
            this.s_member_perf_btn.Click += new System.EventHandler(this.s_member_perf_btn_Click);
            // 
            // NumMembers
            // 
            this.NumMembers.Location = new System.Drawing.Point(384, 112);
            this.NumMembers.Name = "NumMembers";
            this.NumMembers.Size = new System.Drawing.Size(32, 20);
            this.NumMembers.TabIndex = 4;
            this.NumMembers.Text = "100";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(328, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 32);
            this.label1.TabIndex = 5;
            this.label1.Text = "# of Members";
            // 
            // member_perf_btn
            // 
            this.member_perf_btn.Enabled = false;
			this.member_perf_btn.Location = new System.Drawing.Point(64, 104);
            this.member_perf_btn.Name = "member_perf_btn";
			this.member_perf_btn.Size = new System.Drawing.Size(120, 32);
            this.member_perf_btn.TabIndex = 2;
            this.member_perf_btn.Text = "CreateManyMember Performance";
            this.member_perf_btn.Click += new System.EventHandler(this.member_perf_btn_Click);
            // 
            // test_site_topic_btn
            // 
			this.test_site_topic_btn.Location = new System.Drawing.Point(432, 8);
            this.test_site_topic_btn.Name = "test_site_topic_btn";
			this.test_site_topic_btn.Size = new System.Drawing.Size(96, 24);
            this.test_site_topic_btn.TabIndex = 8;
            this.test_site_topic_btn.Text = "Test Site/Topic";
            this.test_site_topic_btn.Click += new System.EventHandler(this.test_site_topic_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 629);
            this.Controls.Add(this.test_site_topic_btn);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.consoleOut);
            this.Controls.Add(this.runAllTests_btn);
            this.Controls.Add(this.test_members_btn);
            this.Controls.Add(this.test_lists_btn);
            this.Controls.Add(this.test_content_btn);
            this.Controls.Add(this.test_sql_btn);
            this.Controls.Add(this.test_segments_btn);
            this.Controls.Add(this.test_mailing_btn);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.s_member_perf_btn);
            this.Controls.Add(this.NumMembers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.member_perf_btn);
            this.Name = "Form1";
            this.Text = "Lyris SOAP API Test Harness";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		private void initSoapVersion()
		{
			printToForm("\r\nInit Soap: ",false);
			printToForm("User/pass "+userName.Text+"/"+password.Text+"....");
			lm.Credentials = new System.Net.NetworkCredential(userName.Text, password.Text);
			lm.Timeout = -1; // never timeout

			printToForm("\r\nCurrent API version: " + lm.ApiVersion() + "\r\n");

			printToForm("Done!");

		}
		private void Form1_Load(object sender, System.EventArgs e)
		{
			// init credintials
			lm = new lmapiSoap.lmapi();
			//initSoapVersion();

			singleMemberID = new int[25];
			newListName = "new-test-list";
		
		}

		public void printToForm(string val)
		{
			printToForm(val, true);
		}
		public void printToForm(string val, bool newline)
		{		
			consoleOut.AppendText(val);			
			if (val.StartsWith("***FAULT"))
			{
				consoleOut.SelectionStart = consoleOut.Text.Length - val.Length;
				consoleOut.SelectionLength = val.Length;
				consoleOut.SelectionColor = Color.Red;
			} 
			else 
			{
				consoleOut.SelectionColor = Color.Black;		
			}
			if (newline) consoleOut.AppendText("\r\n");

			// auto-scroll the output
			consoleOut.Focus();
			consoleOut.SelectionStart = consoleOut.Text.Length;
			consoleOut.Refresh();
			consoleOut.ScrollToCaret();
		}

		private void runAllTests_btn_Click(object sender, System.EventArgs e)
		{
			consoleOut.Text = "";
			test_members_btn_Click(sender, e);
			test_lists_btn_Click(sender, e);
			test_content_btn_Click(sender, e);
			test_sql_btn_Click(sender, e);
			test_segments_btn_Click(sender, e);
			test_mailing_btn_Click(sender, e);
		}

		private void test_members_btn_Click(object sender, System.EventArgs e)
		{
			printToForm("=============== Members Tests ====================",false);
			try 
			{
				initSoapVersion();
				deleteMembers();

				createMemberBan();		
				createSingleMembers();
				// createManyMembers();			
				
				createListAdmin();
				updateListAdmin();
                testServerSiteAdmins();
				updateMemberDemographics();
				updateMemberKind();
				updateMemberEmail();
				updateMemberStatus();
				updateMemberPassword();
				copyMember();
				getMemberID();
				emailOnWhatLists();		
				emailPasswordOnWhatLists();			
				sendMemberDoc();		
				checkMemberPassword();		
				selectMembers();
				selectSimpleMembers( true );
				unsubscribe();
				SelectMembersEx();
				CreateMemberColumn();
				DeleteMemberColumn();

				//end up clean
				deleteMembers();
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....MEMBERS TESTS COMPLETE....\r\n"); 
		}

		private void test_lists_btn_Click(object sender, System.EventArgs e)
		{
			try 
			{
				initSoapVersion();
				printToForm("=======================  LIST TESTS ===============================");
				// first see what we have...
                selectListsEx();
				selectLists();

				// start out clean.
				deleteList();

				createList();		
				createListMembers();
				updateList();
				getListID();
				selectLists();		
			
		
				// end up clean.
				deleteList();
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....LIST TESTS COMPLETE....\r\n"); 
		}

		private void test_content_btn_Click(object sender, System.EventArgs e)
		{	
			
			try 
			{
				initSoapVersion();
				printToForm("=======================  CONTENT TESTS ===============================");
                testContent();
			    selectContent();
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....CONTENT TESTS COMPLETE....\r\n"); 
		}

		private void test_sql_btn_Click(object sender, System.EventArgs e)
		{
			
			try 
			{
				initSoapVersion();
				printToForm("=======================  SQL TESTS ===============================");
				sqlInsert();	
				sqlUpdate();	
				sqlSelect();	
				sqlDelete();	
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....SQL TESTS COMPLETE....\r\n"); 
		
		}

		private void test_segments_btn_Click(object sender, System.EventArgs e)
		{
			
			try 
			{
				initSoapVersion();
				printToForm("=======================  SEGMENT TESTS ===============================");
				selectSegment();
				createDeleteSegment();
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....SEGMENT TESTS COMPLETE....\r\n"); 
		
		}

		private void test_mailing_btn_Click(object sender, System.EventArgs e)
		{
			
			try 
			{
				initSoapVersion();
				printToForm("=======================  MAILING TESTS ===============================");
                mailingTest();
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm("....MAILING TESTS COMPLETE....\r\n"); 
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			consoleOut.Text = "";
		
		}
		

		/* ===========================================================================================================
		 *								MEMBERS TESTS
		 *  ===========================================================================================================
		 * 
		 */

		/*################################################################################
		#	SelectMembersEx 
		#  
		###################################################*/
		private void SelectMembersEx()
		{
			try
			{
                		string[] FieldsToFetch = new string[2];
                		FieldsToFetch[0] = "MemberID";
                		FieldsToFetch[1] = "EmailAddress";

                		string[] FilterCriteriaArray = new string[1];
                		FilterCriteriaArray[0] = "ListName=list1";

                		String[][] members = lm.SelectMembersEx(FieldsToFetch, FilterCriteriaArray);

                		if (members == null)
                    			printToForm(" NO MEMBERS RETURNED ");
                		else
				{
                    			for (int i = 1; i < members.Length; ++i)
                        			printToForm("MemberID: " + members[i][0] + " EmailAddress: " + members[i][1]);
				}

                		printToForm("... SelectMembersEx function finished successfully.");

			}
			catch (System.Web.Services.Protocols.SoapException ex)
			{
                		printToForm("***FAULT: " + ex.Message, false);
			}

        	}
		
		/*################################################################################
		#	CreateMemberColumn 
		#  
		###################################################*/
		private void CreateMemberColumn()
		{
			try
			{
				lm.CreateMemberColumn("test_field_name",lmapiSoap.FieldTypeEnum.varchar20);
				printToForm ("... Created member column successfully.");

			}
			catch ( System.Web.Services.Protocols.SoapException ex ){printToForm("***FAULT: " + ex.Message, false);}
		}
		/*################################################################################
		#	DeleteMemberColumn 
		#  
		####################*/
		private void DeleteMemberColumn()
		{
			try
			{
				lm.DeleteMemberColumn("test_field_name");
				printToForm ("... Deleted member column successfully.");
			}
			catch ( System.Web.Services.Protocols.SoapException ex ){printToForm("***FAULT: " + ex.Message, false);}
		 
		}
		
		/*
		################################################################################
		#	deleteMembers 
		#   sqlDelete
		####################
		*/
		private void deleteMembers()			
		{				
			/*
				 * DELETE MEMBERS
				 * */
				
			string[] deleteArray = new string[2];
			deleteArray[0] = "EmailAddress like %maileater%";
			deleteArray[1] = "ListName = list1";
			
			printToForm("\r\nRunning DeleteMembers:  ",false);
			int numDeleted = lm.DeleteMembers(deleteArray);
			printToForm(numDeleted + " Members Deleted");

			printToForm ("Deleting Member Ban",false);
			try 
			{
				lm.SqlDelete("bannedmembers_", "Domain_ = 'banned-server.com'");
				printToForm ("... Ban Deleted Successfully");
			}
			// dont cause app to quit if the user doens't have access to the sql interface. IE they are a list or site ADMIN
			catch ( System.Web.Services.Protocols.SoapException ex ){printToForm("***FAULT: " + ex.Message, false);}
		}

		/*
		################################################################################
		#	CreateMemberBan
		####################
		#  CreateMemberBan $MemberBanStruct 
		#	MemberBanStruct contains: {BanID ListName SiteName BanLogic UserName Domain}
		#		A = "accepted"
		#		C = "conditionally accepted"
		#		R = "banned"	
		*/		
		private void createMemberBan() 			
		{
			printToForm("\r\nRunning CreateMemberBan:  ", false);
			lmapiSoap.MemberBanStruct ban = new lmapiSoap.MemberBanStruct();
			ban.ListName = "list1";
			ban.BanLogic = lmapiSoap.BanLogicEnum.R;
            ban.BanLogicSpecified = true;
			ban.Domain = "banned-server.com";
			int result = lm.CreateMemberBan( ban );
			printToForm("Ban created with ID:" + result);
				
		}

		/*
		################################################################################
		#	CreateSingleMember
		####################
		*/
		private void createSingleMembers()
		{
			printToForm("\r\nRunning CreateSingleMember:  ",false);

			int memberID = 0;

				
			printToForm("\r\nCreating: donald_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("donald_the_duck@maileater.lyris.com", "Donald Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: darryl_the_duck@banned-server.com", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("darryl_the_duck@banned-server.com", "Darryl Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: darren_the_duck@banned-server.com", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("darren_the_duck@banned-server.com", "Darren Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: cathernie_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("cathernie_the_duck@maileater.lyris.com", "Cathernie Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: takeda_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("takeda_the_duck@maileater.lyris.com", "Takeda Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: eliot_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("eliot_the_duck@maileater.lyris.com", "Eliot Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }
				
			printToForm("\r\nCreating: howard_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("howard_the_duck@maileater.lyris.com", "Howard Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }

			printToForm("\r\nCreating: manor_the_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("manor_the_duck@maileater.lyris.com", "Manor Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }
					
			printToForm("\r\nCreating: snow_white@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("snow_white@maileater.lyris.com", "Snow White", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }
			
			printToForm("\r\nCreating: icy@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("icy@maileater.lyris.com", "Ice Queen Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }
			
			printToForm("\r\nCreating: daffy_miss_duck@maileater.lyris.com ", false);
            try
            {
                singleMemberID[memberID++] = lm.CreateSingleMember("daffy_miss_duck@maileater.lyris.com", "Daffy Duck", "list1");
            }
			catch ( System.Web.Services.Protocols.SoapException ex )
            {
                printToForm("***FAULT: " + ex.Message, false);
            }
			
			printToForm("Created " + memberID +" members");
		}

		/*###################################################
		 *# CreateManyMembers
		 *###################################################*/
		private void createManyMembers()
		{
			printToForm("....This version of CREATE MANY MEMBERS is DEPRICATED \r\n"); 
		}
			/*printToForm("\r\nCreateManyMembers: ",false);
			// there is a much better way to do this
			lmapiSoap.KeyValueType[] DaffyDemographics = new lmapiSoap.KeyValueType[4];
			for (int i=0; i < DaffyDemographics.Length; i++ ) DaffyDemographics[i] = new lmapiSoap.KeyValueType();		
			/* SETUP DAFFY DUCK */
			/* DaffyDemographics[0].Name  = "First_Name_";
			DaffyDemographics[0].Value = "Daffy";

			DaffyDemographics[1].Name = "Last_Name_";
			DaffyDemographics[1].Value = "Duck";

			DaffyDemographics[2].Name = "Age_";		
			DaffyDemographics[2].Value = "68";

			DaffyDemographics[3].Name = "Birthday_";	
			DaffyDemographics[3].Value = "April 17, 1935"; 
			/*	this is a string field, so the date is stored as is. You need to know the format of your demographics table to use these effectivly. */
		
			/* lmapiSoap.MemberStruct DaffyDuck	= new lmapiSoap.MemberStruct();
			DaffyDuck.EmailAddress	= "daffy_miss_duck@maileater.lyris.com";
			DaffyDuck.ListName		= "list1";
			DaffyDuck.Password		= "donttelldonald";
			DaffyDuck.ReadsHtml		= false;
			DaffyDuck.Demographics	= DaffyDemographics;


			/* SETUP SNOW WHITE */
			/* lmapiSoap.MemberStruct SnowWhite	= new lmapiSoap.MemberStruct();
			DateTime snowWhiteExpireDate = new DateTime(2006, 12, 31, 23, 59, 59);				
			SnowWhite.EmailAddress= "snow_white@maileater.lyris.com";
			SnowWhite.ListName = "list1";			
			SnowWhite.ExpireDate = snowWhiteExpireDate;// non demographic field dates must match xsd schema http://www.w3.org/TR/xmlschema-2/#dateTime
		
			/* SETUP ICE QUEEN */		
			/* lmapiSoap.MemberStruct IceQueen	= new lmapiSoap.MemberStruct();
			IceQueen.EmailAddress = "icy@maileater.lyris.com";
			IceQueen.ListName = "list1";
			IceQueen.MailFormat = lmapiSoap.MailFormatEnum.T ; // she reads text only.

			/* CREATE BANNED USER, THIS SHOULD FAIL SILENTLY */
			/* lmapiSoap.MemberStruct BannedUser	= new lmapiSoap.MemberStruct();
			BannedUser.EmailAddress  = "this_email_is_invalid@banned-server.com"; // this will be baned, but create many users fails quietly,so check the return value to see if it was created.
			BannedUser.ListName = "list1";

			lmapiSoap.MemberStruct[] CreateMembersAr = new lmapiSoap.MemberStruct[4] {IceQueen, SnowWhite, DaffyDuck, BannedUser};

			

			try 
			{ 
				lmapiSoap.SimpleMemberStruct[] created = null; //lm.CreateManyMembers(CreateMembersAr, true);	
				printToForm(created.Length + " members created.");;
				for (int i=0 ; i<created.Length ; i++ )
				{
					printToForm(created[i].EmailAddress + " created with member id "  + created[i].MemberID + " on list " + created[i].ListName);
						
				}
			} 
			catch ( System.Web.Services.Protocols.SoapException ex )
			{
				printToForm("***FAULT: " + ex.Message);
			}
		}*/

		/*
			################################################################################
			#	CreateListAdmin
			####################
			#  CreateListAdmin $EmailAddress $Password $ListName $FullName $RecieveListAdminMail $RecieveModerationNotification $BypassListModeration 
		*/
		private void createListAdmin()
			
		{
			int result = 0;
			printToForm("\r\nCreating ListAdmin", false);
			try	{ result = lm.CreateListAdmin("naturelover@maileater.lyris.com","lyris","list1","Test A Whanny",true,true,true);}
			catch ( System.Web.Services.Protocols.SoapException e )	{ printToForm(e.Message+ "\r\n"); }			
			printToForm(" Created with MemberID:" + result );
				
		}

		/*	################################################################################
			#	UpdateListAdmin
			####################
			# UpdateListAdmin {lmapiSoap.SimpleMemberStructIn	IsListAdmin	RecieveListAdminMail RecieveModerationNotification BypassListModeration}
		*/
		private void updateListAdmin()
			
		{
			bool result;
			printToForm( "\r\nUpdateListAdmin: ",false);

			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.EmailAddress = "naturelover@maileater.lyris.com";
			simpleMember.ListName = "list1";

			result = lm.UpdateListAdmin(simpleMember,false,false,false,false); 
			
			if (result)
				printToForm("Sucess! ");
			else 
				printToForm("!!!FAILED!!!");
				
		}


        private void testServerSiteAdmins()
        {
            int result = 0;
            // Server Admin first
            lmapiSoap.ServerAdminStruct serverAdmin = new lmapiSoap.ServerAdminStruct();
            serverAdmin.EmailAddress = "serveradmin@maileater.lyris.com";
            serverAdmin.Name = "Server Admin";
            serverAdmin.Password = "ser123";
            printToForm("\r\nCreating ServerAdmin... ", false);
            try { result = lm.CreateServerAdmin(serverAdmin); }
            catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
            if (result > 0)
                printToForm(" Created ServerAdmin with MemberID:" + result);
            else
                printToForm("Failed to create a server admin");

            bool bresult = false;
            if (result > 0)
            {
                serverAdmin.AdminID = result;
                serverAdmin.AdminIDSpecified = true;
                serverAdmin.Name = "Update server Admin";
                serverAdmin.Password = "server123";
                
                printToForm("\r\nUpdating ServerAdmin... ", false);
                try { bresult = lm.UpdateServerAdmin(serverAdmin); }
                catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
                if (bresult)
                    printToForm(" Updated successully");
                else
                    printToForm(" Update failed");

                bresult = false;
                printToForm("\r\nDeleting ServerAdmin... ", false);
                try { bresult = lm.DeleteServerAdmin(serverAdmin); }
                catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
                if (bresult)
                    printToForm(" Deleted successully");
                else
                    printToForm(" Deleted failed");
            }

            // Site Admin 
            lmapiSoap.SiteAdminStruct siteAdmin = new lmapiSoap.SiteAdminStruct();
            siteAdmin.EmailAddress = "siteadmin@maileater.lyris.com";
            siteAdmin.Name = "Site Admin";
            siteAdmin.Password = "site123";
            siteAdmin.WhatSites = new string[1] { "main" };
            
            result = 0;
            printToForm("\r\nCreating SiteAdmin... ", false);
            try { result = lm.CreateSiteAdmin(siteAdmin); }
            catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
            if (result > 0)
                printToForm(" Created SiteAdmin with MemberID:" + result);
            else
                printToForm(" Failed to create a site admin");

            if (result > 0)
            {
                siteAdmin.AdminID = result;
                siteAdmin.AdminIDSpecified = true;
                siteAdmin.Name = "Update site Admin";
                siteAdmin.Password = "site123";

                bresult = false;
                printToForm("\r\nUpdating SiteAdmin... ", false);
                try { bresult = lm.UpdateSiteAdmin(siteAdmin); }
                catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
                if (bresult)
                    printToForm(" Updated successully");
                else
                    printToForm(" Update failed");

                bresult = false;
                printToForm("\r\nDeleting SiteAdmin... ", false);
                try { bresult = lm.DeleteSiteAdmin(siteAdmin); }
                catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
                if (bresult)
                    printToForm(" Deleted successully");
                else
                    printToForm(" Deleted failed");
            }
            printToForm("\n");
        }

        private void testContent()
        {
            int result = 0;
            bool bresult = false;
            lmapiSoap.ContentStruct contentStruct = new lmapiSoap.ContentStruct();
            contentStruct.NativeTitle = "This content native title";			// Subject
            contentStruct.Title = "This content title";
            contentStruct.Description = "This content description";
            contentStruct.HeaderTo = "group@maileater.lyris.com";
            contentStruct.HeaderFrom = "sender@maileater.lyris.com";
            contentStruct.DocType = lmapiSoap.DocTypeEnum.CONTENTv2;
            contentStruct.DocTypeSpecified = true;
            contentStruct.DocParts = new lmapiSoap.DocPart[2];
            contentStruct.DocParts[0] = new lmapiSoap.DocPart();
            contentStruct.DocParts[1] = new lmapiSoap.DocPart();
            contentStruct.DocParts[0].MimePartName = "text";
            contentStruct.DocParts[0].Body = "This is text body of this content";
            contentStruct.DocParts[0].Encoding = lmapiSoap.MailSectionEncodingEnum.Item8bit;
            contentStruct.DocParts[0].EncodingSpecified = true;
            contentStruct.DocParts[0].CharSetID = 1;
            contentStruct.DocParts[0].CharSetIDSpecified = true;
            contentStruct.DocParts[1].MimePartName = "html";
            contentStruct.DocParts[1].Body = "<HTML>This is <B>text </B> body of this <U>content</U></HTML>";
            contentStruct.DocParts[1].Encoding = lmapiSoap.MailSectionEncodingEnum.Item8bit;
            contentStruct.DocParts[1].EncodingSpecified = true;
            contentStruct.DocParts[1].CharSetID = 1;
            contentStruct.DocParts[1].CharSetIDSpecified = true;

            printToForm("\r\nTesting CreateContent... ", false);
            try { result = lm.CreateContent(contentStruct); }
            catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
            if (result > 0)
                printToForm(" Created Content with ID:" + result);
            else
            {
                printToForm("Failed to create a content");
                return;
            }

            contentStruct.ContentID = result;
            contentStruct.ContentIDSpecified = true;
            contentStruct.Description = "Modified description of the content";
            contentStruct.Title = "This a modified content title";


            bresult = false;
            printToForm("\r\nTesting UpdateContent... ", false);
            try { bresult = lm.UpdateContent(contentStruct); }
            catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
            if (bresult)
                printToForm(" Update successful");
            else
                printToForm(" Failed to update a content");

            bresult = false;
            printToForm("\r\nTesting DeleteContent... ", false);
            try { bresult = lm.DeleteContent(contentStruct); }
            catch (System.Web.Services.Protocols.SoapException e) { printToForm(e.Message + "\r\n"); }
            if (bresult)
                printToForm(" Deleted successful");
            else
                printToForm(" Failed to delete a content");

            printToForm("\n");
        }



		/*	################################################################################
			#	SendMemberDoc
			####################
		*/
		private void sendMemberDoc()
			
		{
			int result;
			printToForm( "\r\nSendMemberDoc: ",false);

			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();
			
			// use either email address/ listname or memberID.
			// this will send to anyone, but listname is required to determine wich doc to send.
			simpleMember.EmailAddress ="daffy_miss_duck@maileater.lyris.com";
			simpleMember.ListName = "list1";

			result = lm.SendMemberDoc(simpleMember, lmapiSoap.MessageTypeEnum.confirm); 

			printToForm("Result:" + result );
				
		}

		/*	################################################################################
			#	CheckMemberPassword
			####################
		*/
		private void checkMemberPassword()
			
		{
			bool isPass;

			printToForm( "\r\nCheckMemberPassword",false);
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();
			// use either email address/ listname or memberID.
			simpleMember.EmailAddress = "daffy_miss_duck@maileater.lyris.com";
			simpleMember.ListName = "list1";

			isPass = lm.CheckMemberPassword(simpleMember,"is_this_her_pass"); 
			printToForm("Result: " + isPass );


			printToForm( "\r\nCheckMemberPassword",false);
			lmapiSoap.SimpleMemberStruct simpleMember2 = new lmapiSoap.SimpleMemberStruct();
			// use either email address/ listname or memberID.
			simpleMember2.EmailAddress = "daffy_miss_duck@maileater.lyris.com";
			simpleMember2.ListName = "list1" ;
			isPass = lm.CheckMemberPassword(simpleMember2,"donttelldonald"); 
			printToForm("Result: " + isPass );			
		}

		/*
		################################################################################
		#	SelectSimpleMembers
		####################
		*/
		private void selectSimpleMembers( bool display )	
		{
			printToForm( "\r\n SelectSimpleMembers: ");
			// Make the actual call
			string[] SimpleMemberSelectArray = new string[2] {"ListName = list1", "EmailAddress like %"};

			lmapiSoap.SimpleMemberStruct[] member = lm.SelectSimpleMembers( SimpleMemberSelectArray );
			
			if (member  !=null)
			{
				printToForm("Got " +  member.Length + " members:");
				
				if (display)
				{
					for (int i = 0; i < member.Length; i++) 
					{	
						printToForm(member[i].EmailAddress);
				
					}
				}
			}
		}

		/*
		################################################################################
		#	SelectMembers
		####################
		*/
		private void selectMembers()
			
		{
			printToForm( "\r\n SelectMembers: ");
			// Make the actual call
			string[] MemberSelectArray = new string[2] {"ListName=list1", "EmailAddress like %maileater%"};

			lmapiSoap.MemberStruct[] member = lm.SelectMembers( MemberSelectArray );
			
			if (member  !=null)
			{
				printToForm("Got " +  member.Length + " members:");

				for (int i = 0; i < member.Length; i++) 
				{		
					string demoStr = "\r\n";
					string name,val;

					lmapiSoap.KeyValueType[] demographics;

					demographics = member[i].Demographics;

					if (demographics != null) 
					{				
						for (int j = 0; j < demographics.Length; j++) 
						{						
							val = demographics[j].Value;
							name = demographics[j].Name;

							if ( Name != null && val != "") 
							{
								demoStr +=  name + " '" + val + "'\r\n";
							}

						}
					}
			

					printToForm( member[i].EmailAddress + " Joined: " + member[i].DateJoined + demoStr);
				}  		
			}
		}
		/*
			################################################################################
			#	UpdateMemberPassword
			####################
		*/
		private void updateMemberPassword()	
		{			

			bool isResult;	
			
			printToForm( "\r\nupdateMemberPassword ", false);
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.MemberID = singleMemberID[3] ;
            simpleMember.MemberIDSpecified = true;

			isResult = lm.UpdateMemberPassword(simpleMember, "newpassword");
			if (isResult)
				printToForm("MemberID: " + simpleMember.MemberID +" Password updated");
			else 
				printToForm("MemberID: "+simpleMember.MemberID +" Password NOT updated");

		}

		


		
		/*
			################################################################################
			#	UpdateMemberKind
			####################
			# memberkind can be {digest daymimedigest index nomail mail listmail}*/
		private void updateMemberKind()
			
		{

			bool isResult;	
			
			printToForm( "\r\nupdateMemberKind " );
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.EmailAddress = "cathernie_the_duck@maileater.lyris.com";
			simpleMember.ListName = "list1";

			isResult = lm.UpdateMemberKind(simpleMember, lmapiSoap.MemberKindEnum.nomail);

			if (isResult)
				printToForm("EmailAddress: "+simpleMember.EmailAddress+" set to recieve nomail");
			else 
				printToForm("EmailAddress: "+simpleMember.EmailAddress+" NOT UPDATED");

		}

		/*	################################################################################
			#	UpdateMemberEmail
			####################
		*/
		private void  updateMemberEmail()
			
		{
			bool isResult;	
			
			printToForm( "\r\nupdateMemberEmail ");		
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.EmailAddress = "howard_the_duck@maileater.lyris.com";
			simpleMember.ListName = "list1";
			
			isResult = lm.UpdateMemberEmail(simpleMember, "Howard_Duck@maileater.lyris.com");

			if (isResult)
				printToForm("EmailAddress: "+simpleMember.EmailAddress+" Updated to => Howard_Duck@maileater.lyris.com");
			else 
				printToForm("EmailAddress: "+simpleMember.EmailAddress+" NOT UPDATED");

		}
		/*	################################################################################
			#	UpdateMemberStatus
			####################
			# memberstatus can be {normal member confirm private expired held unsub referred needs-confirm needs-hello 
		*/

		private void  updateMemberStatus()
			
		{
			bool isResult;	
			
			
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();
			// use either email address/ listname or memberID.

			simpleMember.MemberID = singleMemberID[5] ; // should be donald
            simpleMember.MemberIDSpecified = true;

			
			printToForm( "\r\nupdateMemberStatus " );
			isResult = lm.UpdateMemberStatus(simpleMember, lmapiSoap.MemberStatusEnum.expired);

			if (isResult)
				printToForm("MemberID: "+simpleMember.MemberID+" set to expired");
			else 
				printToForm("MemberID: "+simpleMember.MemberID+" NOT UPDATED");

		}

		/*	################################################################################
			#	CopyMember 
			####################
		*/
		private  void copyMember()
			
		{
			int result;
			
			printToForm( "\r\ncopyMember ", false);
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.MemberID = singleMemberID[4];
            simpleMember.MemberIDSpecified = true;

			// simplemembber, email , fullname , listname
			result = lm.CopyMember(simpleMember, "aliot_clone_of_eliot@maileater.lyris.com","Aliot the bear", "list1");

			printToForm("Member: "+simpleMember.MemberID+" copied with MemberID: " + result );

		}

		/*
			################################################################################
			#	UpdateMemberDemographics
			####################
		*/
		private  void updateMemberDemographics()
			
		{
			bool isResult;

			printToForm( "\r\nupdateMemberDemographics: ");

			/* DONALD DUCK */
			lmapiSoap.KeyValueType[] DonaldDemographics = new lmapiSoap.KeyValueType[3];
			for (int i=0; i < DonaldDemographics.Length; i++ ) DonaldDemographics[i] = new lmapiSoap.KeyValueType();	
			lmapiSoap.SimpleMemberStruct Donald = new lmapiSoap.SimpleMemberStruct();		
			//this can be email addr/listname or memberID
			Donald.EmailAddress =  "donald_the_duck@maileater.lyris.com";
			Donald.ListName = "list1";
            

			DonaldDemographics[0].Name = "FullName_";		DonaldDemographics[0].Value = "Donald Duck";
			DonaldDemographics[1].Name = "Comment_";		DonaldDemographics[1].Value = "The Donaldest of them all";
			DonaldDemographics[2].Name = "Additional_";		DonaldDemographics[2].Value = "He's got the quackiest quack!";
			//DonaldDemographics[3].Name = "Birthday_";	DonaldDemographics[3].Value = "June 9, 1934";/*	this is a string field, so the date is stored as is. You need to know the format of your demographics table to use these effectivly. */			
			
			//isResult= lm.MyTestFunc(Donald,DonaldDemographics);
			//printToForm( "\r\ndid the func: ");
			isResult= lm.UpdateMemberDemographics(Donald, DonaldDemographics);
			printToForm( Donald.EmailAddress + ": " + DonaldDemographics.Length +" demographics updated ");



			/* ICE QUEEN */
			lmapiSoap.KeyValueType[] IceQueenDemographics = new lmapiSoap.KeyValueType[3];
			for (int i=0; i < IceQueenDemographics.Length; i++ ) IceQueenDemographics[i] = new lmapiSoap.KeyValueType();	
			lmapiSoap.SimpleMemberStruct IceQueen = new lmapiSoap.SimpleMemberStruct();	
			//this can be email addr/listname or memberID
			IceQueen.EmailAddress ="icy@maileater.lyris.com";
			IceQueen.ListName = "list1";
			
			IceQueenDemographics[0].Name = "FullName_";		IceQueenDemographics[0].Value = "Ice Queen";
			IceQueenDemographics[1].Name = "Comment_";		IceQueenDemographics[1].Value = "The Touch of Cold";
			IceQueenDemographics[2].Name = "Additional_";	IceQueenDemographics[2].Value = "36 yrs of Fickle Frostiness";
			//IceQueenDemographics[3].Name = "Birthday_";		IceQueenDemographics[3].Value = "June 9, 1968";/*	this is a string field, so the date is stored as is. You need to know the format of your demographics table to use these effectivly. */
			
			isResult= lm.UpdateMemberDemographics(IceQueen, IceQueenDemographics);
			printToForm( IceQueen.EmailAddress + ": " + IceQueenDemographics.Length +" demographics updated ");
			


			/* DAFFY DUCK */
			lmapiSoap.KeyValueType[] DaffyDemographics = new lmapiSoap.KeyValueType[1];
			for (int i=0; i < DaffyDemographics.Length; i++ ) DaffyDemographics[i] = new lmapiSoap.KeyValueType();	
			lmapiSoap.SimpleMemberStruct DaffyDuck = new lmapiSoap.SimpleMemberStruct();
			//this can be email addr/listname or memberID
			DaffyDuck.EmailAddress = "daffy_miss_duck@maileater.lyris.com";
			DaffyDuck.ListName = "list1";			
			
			DaffyDemographics[0].Name = "FullName_";	DaffyDemographics[0].Value = "Daffy Duck"; // daffy is forever young.			

			isResult= lm.UpdateMemberDemographics(DaffyDuck, DaffyDemographics);
			printToForm( DaffyDuck.EmailAddress + ": " + DaffyDemographics.Length +" demographics updated ");
		}
			
		

		/*
			################################################################################
			#	Unsubscribe
			####################
		*/
		private void  unsubscribe()
			
		{
			int result;
			
			printToForm( "\r\nunsubscribe ",false);
			lmapiSoap.SimpleMemberStruct[] simpleMembers = new lmapiSoap.SimpleMemberStruct[2];

			// use either email address/ listname or memberID.
			simpleMembers[0] = new lmapiSoap.SimpleMemberStruct();
			simpleMembers[1] = new lmapiSoap.SimpleMemberStruct();
			
			simpleMembers[0].MemberID = singleMemberID[3];
            simpleMembers[0].MemberIDSpecified = true;

			simpleMembers[1].EmailAddress = "icy@maileater.lyris.com";
			simpleMembers[1].ListName = "list1";

			result = lm.Unsubscribe(simpleMembers);

			printToForm(result  + " Members unsubscribed");

		}

		/*
			################################################################################
			#	GetMemberID
			####################
		*/
		private void getMemberID()
		{
			int result;
				
			printToForm( "\r\ngetMemberID ",false);
			lmapiSoap.SimpleMemberStruct simpleMember = new lmapiSoap.SimpleMemberStruct();

			// use either email address/ listname or memberID.
			simpleMember.EmailAddress = "icy@maileater.lyris.com";
			simpleMember.ListName = "list1";
				

			try 
			{				
				simpleMember.EmailAddress = "icy@maileater.lyris.com";
				result = lm.GetMemberID(simpleMember);
				printToForm(simpleMember.EmailAddress + " has MemberID of " + result);

				simpleMember.EmailAddress = "aliot_clone_of_eliot@maileater.lyris.com";
				result = lm.GetMemberID(simpleMember);
				printToForm(simpleMember.EmailAddress + " has MemberID of " + result);

				// to get ID member that doesn;'t exist...
				try 
				{ 
					simpleMember.ListName = "javajavacuckoo";
					result = lm.GetMemberID(simpleMember); 
					printToForm(simpleMember.EmailAddress + " has MemberID of " + result);
				} 
				catch ( System.Web.Services.Protocols.SoapException e ) 
				{ 
					printToForm("***FAULT:" + e.Message + "\r\n"); 
				}				
			}
			catch ( System.Web.Services.Protocols.SoapException e ) 
			{ 
				printToForm("***FAULT:" + e.Message + "\r\n"); 
			}
		}

		/*
			################################################################################
			#	EmailOnWhatLists
			####################
		*/
		private void  emailOnWhatLists()
			
		{			

			string lists = "";

			printToForm( "\r\nemailOnWhatLists ",false);
			
			string[] sublists = lm.EmailOnWhatLists("donald_the_duck@maileater.lyris.com");
			
			if (lists != null) 
			{
				for (int i =0; i< sublists.Length ; i++ ) 
					lists += sublists[i] + " ";	
			}
			

			printToForm("donald_the_duck@maileater.lyris.com Subscribed to:"  + lists);

		}
		/*
			################################################################################
			#	EmailPasswordOnWhatLists
			####################
		*/
		private  void emailPasswordOnWhatLists()
			
		{			
			string lists2="";
			string lists = "";

			printToForm( "\r\nemailPasswordOnWhatLists :");	
			string[] sublists = lm.EmailPasswordOnWhatLists("donald_the_duck@maileater.lyris.com", "badpdsass");
			if (lists != null) 
			{
				for (int i =0; i< sublists.Length ; i++ ) 
					lists += sublists[i] + " ";
			}
			printToForm("donald_the_duck@maileater.lyris.com (pass badpdsass) Subscribed to:"  + lists);
			
			string[] sublists2 = lm.EmailPasswordOnWhatLists("daffy_miss_duck@maileater.lyris.com", "donttelldonald");
			if (lists != null)
			{
				for (int j =0; j< sublists2.Length ; j++ ) 
					lists2 += sublists2[j] + " ";
			}
			printToForm("daffy_miss_duck@maileater.lyris.com (pass donttelldonald) Subscribed to:"  + lists2);

		}

		/* ===========================================================================================================
		*								LIST TESTS
		*  ===========================================================================================================*/
		
		/*
		################################################################################
		#	createList
		####################*/
		private void createList() 
			
		{
			printToForm("\r\nCreating new list");

			newListID = lm.CreateList(lmapiSoap.ListTypeEnum.marketing, newListName, "The Test List", "Barry White","barry@maileater.lyris.com","lyristest","");		
			printToForm("New list created with ID:" + newListID + "\r\n");
		}

		/*
		################################################################################
		#	createListMembers
		####################*/
		private void createListMembers() 
			
		{
			printToForm("\r\ncreateListMembers");
			
			int id = lm.CreateSingleMember("donald_the_duck@maileater.lyris.com","Donald Duck", newListName); 
			
			printToForm("createListMembers :" + id + "\r\n");
		}

	/*
		################################################################################
		#	updateList
		####################
	*/
		private void updateList() 
			
		{
			bool isResult;

			printToForm("Updating list: ",false);
			lmapiSoap.ListStruct list = new lmapiSoap.ListStruct();
			
			
			// okay .net is a pain this way, it cannot specify null values
			// so we must get all values of the list before we send it back for 
			// updating or there will be problems.
			printToForm("..Selecting.. ",false);
			list = (lm.SelectLists(newListName,"main"))[0];
	
			// you must set the listname
			list.ListName = newListName;		
			list.KeepOutmailPostings = 100; // keep the last 100 outmails
            list.KeepOutmailPostingsSpecified = true;
			
			// we don't want to allow email submissions
			list.NoEmail = true;
            list.NoEmailSpecified = true;
			
			list.DefaultSubject = "Barry's News Blog Direct";
            
			/*
				the wsdl says
				ConfirmUnsubEnum:
					{RequireWhenQuestionable 0}
					{AlwaysRequire 1}
					{NeverRequire 2}
			*/
			list.ConfirmUnsubscribes = lmapiSoap.ConfirmUnsubEnum.Item2; // this needs documentation. check the wsdl for more info.
            list.ConfirmUnsubscribesSpecified = true;
			list.MaxMessageSize = 5000; // 5 megs is enough
            list.MaxMessageSizeSpecified = true;
			
			// set some recency stuff...
			list.RecencyWebEnabled = true;
            list.RecencyWebEnabledSpecified = true;
			list.RecencyMailCount = 2;
            list.RecencyMailCountSpecified = true;
			list.RecencyOperator =  lmapiSoap.RecencyOperatorEnum.m ; // m is more than
            list.RecencyOperatorSpecified = true;
			// perl's SOAP::Lite doesn't like this arrggg so much for open source
			// lmapiSoap.SubscriptionReportEnum[] SubReportAr = new lmapiSoap.SubscriptionReportEnum[2] {lmapiSoap.SubscriptionReportEnum.thursdayemail, lmapiSoap.SubscriptionReportEnum.daily};

			string[] SubReportAr = new string[2]{"thursdayemail", "daily"};
			list.SubscriptionReports = SubReportAr;
            
			isResult = lm.UpdateList(list);	
			if (isResult) {
				printToForm(" List Updated\r\n");
			} else {
				printToForm(" LIST NOT UPDATED!\r\n");
			}
			
		}
	/*

		################################################################################
		#	SelectLists
		####################
		*/
		private void selectLists() 
			
		{
			lmapiSoap.ListStruct[] lists;
			printToForm("SelectLists : ");
			lists = lm.SelectLists("","");	// select all lists on server	

			if (lists != null) 
			{
				for (int i = 0 ;i< lists.Length ; i++)
				{
					printToForm("ListName:            " + lists[i].ListName);
					printToForm("KeepOutmailPostings: " + lists[i].KeepOutmailPostings);
					printToForm("NoEmail:             " + lists[i].NoEmail);
					printToForm("DefaultSubject:      " + lists[i].DefaultSubject);
					printToForm("MaxMessageSize:      " + lists[i].MaxMessageSize);
					printToForm("RecencyWebEnabled:   " + lists[i].RecencyWebEnabled);
					printToForm("RecencyMailCount:    " + lists[i].RecencyMailCount);
					printToForm("RecencyOperator:     " + lists[i].RecencyOperator );
					printToForm("SubscriptionReports: " );
					if (lists[i].SubscriptionReports != null )
					{
					
					for (int j = 0 ;j< lists[i].SubscriptionReports.Length ; j++)
						printToForm( "                     " + lists[i].SubscriptionReports[j] );
					}
					printToForm("");
				}		
			}
		}

        // Extended Functionality of SelectLists
        private void selectListsEx()
        {
            printToForm("SelectListsEx : ");
            string[] fieldArray = new string[3];
            fieldArray[0] = "ListName";
            fieldArray[1] = "Admin";
            fieldArray[2] = "CreationTimestamp";
            string[] criteriaArray = new string[1];
            criteriaArray[0] = "ListName like li%";
            
            String[][] lists = lm.SelectListsEx("", "", fieldArray, criteriaArray);	// select just 3 fields of all lists on server with specified criteria

            if (lists != null)
            {
                for (int i = 1; i < lists.Length; i++)
                {
                    printToForm("ListName:            " + lists[i][0]);
                    printToForm("Admin: " + lists[i][1]);
                    printToForm("CreationTimestamp:             " + lists[i][2]);
                    printToForm("");
                }
            }
            
        }

			/*
		################################################################################
		#	getListID
		####################*/
		private void getListID() 
			
		{
			int result;
			printToForm("getListID: " + newListName,false);
			result = lm.GetListID(newListName);		
			printToForm(" ListID:" + result);
			
			printToForm("getListID: list1",false);
			result = lm.GetListID("list1");		
			printToForm(" ListID:" + result);
			printToForm("");
		}

		/*
		################################################################################
		#	deleteList
		####################*/
		private void deleteList()
			
		{	
			try
			{			
				printToForm("Deleting List: " + newListName);
				lm.DeleteList(newListName);
			}
			catch ( System.Web.Services.Protocols.SoapException ex ){printToForm("***FAULT: " + ex.Message, false);}
		}


		

		/* ===========================================================================================================
		*								CONTENT TESTS
		*  ===========================================================================================================*/

		/*
		################################################################################
		#	SelectContent
		####################
		*/
		private void selectContent()
		{
			printToForm( "SelectContent ");
			// Make the actual call
			String[] ContentSelectArray = new string[2]{"Title like %sample%","IsTemplate = 1"};

			lmapiSoap.ContentStruct[] content = lm.SelectContent( ContentSelectArray );
	
				
			
			
			if (content != null) 
			{
				printToForm("Got " +  content.Length + " content:");
				for (int i = 0; i < content.Length; i++) 
				{
					printToForm( "HeaderTo:   " + content[i].HeaderTo );
					printToForm( "IsTemplate: " + content[i].IsTemplate);
					printToForm( "DocType:    " + content[i].DocType );
					printToForm( "ContentID:  " + content[i].ContentID );
					printToForm( "Description:" + content[i].Description );
					printToForm( "NativeTitle:" + content[i].NativeTitle );
					printToForm( "HeaderFrom: " + content[i].HeaderFrom );
					printToForm( "Title:      " + content[i].Title );
					printToForm( "IsReadOnly: " + content[i].IsReadOnly );
					printToForm( "DateCreated:" + content[i].DateCreated );
					printToForm( "");

				}  				
			}
		}
	




		/* ===========================================================================================================
		*								SQL TESTS
		*  ===========================================================================================================*/
		/*

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		################################################################################
		#	SqlInsert
		####################
		*/
		private void sqlInsert()
		{
			printToForm( "SqlInsert :", false);
			// Make the actual call
			lmapiSoap.KeyValueType[] InsertArray = new lmapiSoap.KeyValueType[4];
			for (int i=0; i < InsertArray.Length; i++ ) InsertArray[i] = new lmapiSoap.KeyValueType();	

			InsertArray[0].Name = "Title";	
			InsertArray[0].Value = "New Document";

			InsertArray[1].Name = "Descript"; 
			InsertArray[1].Value = "This is a test HTML doc created by soap.";

			InsertArray[2].Name = "Body";		
			InsertArray[2].Value = "<html><body><h1>This is a test</h1><b>Foo meets the bar</b></body></html>";

			InsertArray[3].Name = "WebDocTypeID"; 
			InsertArray[3].Value = "3";

			newSqlID = lm.SqlInsert("lyrWebDocs", InsertArray, true );

			printToForm( "Created new lyrWebDoc ID:  "+ newSqlID);

			
		}
		/*

		################################################################################
		#	SqlUpdate
		####################
	*/

		private void sqlUpdate()
		{
			bool isResult;
			printToForm( "SqlUpdate: ",false);
			// Make the actual call
			lmapiSoap.KeyValueType[] UpdateArray = new lmapiSoap.KeyValueType[4];
			for (int i=0; i < UpdateArray.Length; i++ ) UpdateArray[i] = new lmapiSoap.KeyValueType();	

			UpdateArray[0].Name  = "Title";	
			UpdateArray[0].Value = "Updated Title for you!";

			UpdateArray[1].Name  = "IsTemplate"; 
			UpdateArray[1].Value = "T"; // you must use native types correctly or there will be errors.

			UpdateArray[2].Name  = "IsReadonly";		
			UpdateArray[2].Value = "T";

			String where = "WebDocID = " + newSqlID;		
			
			isResult = lm.SqlUpdate("lyrWebDocs", UpdateArray , where );

			if (isResult )	{
				printToForm( "lyrWebDocs Updated successfully");
			} else {
				printToForm( "lyrWebDocs UPDATE FAILED!!");
			}		
		}
	/*
		################################################################################
		#	SqlSelect
		####################
	*/

		private void sqlSelect()
		{
		
			printToForm( "SqlSelect: ");
			String sql = "Select * from lyrWebDocs";			
			String[][] results = lm.SqlSelect( sql );
			
			String outRow = "";
			if ( results != null ) 
			{
				for (int i =0 ; i < results.Length ; i++)
				{
					// header is on first row
					if (i < 2)
						printToForm("-------------------------------------------------------------------------");

					outRow = "";
					//string[] row = new String();
					if (results[i] != null) 
					{
						for (int j=0; j<results[i].Length; j++)
						{	
							//row = results[i];

							// this is cell from the table
							if (results[i][j] != null)
							{
								if ( results[i][j].Length > 5 )
									outRow += results[i][j].Substring(0,5) + "...\t";
								else
									outRow += results[i][j] + "\t";
							}
					
						}
					}
				
					printToForm(outRow);
				

				}
				printToForm("");
		
			}
		}

		/*
			################################################################################
			#	SqlDelete
			####################
		*/
			private void sqlDelete()
			{
				bool isResult;
				printToForm( "SqlDelete: ",false);
				// Make the actual call


				String where = "WebDocID = " + newSqlID;		
				
				isResult = lm.SqlDelete("lyrWebDocs",  where );
				if (isResult )	{
					printToForm( "lyrWebDocs Deleted successfully");
				} else {
					printToForm( "lyrWebDocs DELETE FAILED!!");
				}		
			}





	
			/* ===========================================================================================================
			*								SEGMENT TESTS
			*  ===========================================================================================================*/

			
		/*
		################################################################################
		#	SelectSegment
		####################
		*/
		private void selectSegment()
	
		{
			printToForm( "SelectSegment ");
			// Make the actual call
			String[] SegmentSelectArray = {"ListName = list1"};

			lmapiSoap.SegmentStruct[] segment = lm.SelectSegments( SegmentSelectArray );		
				
			printToForm("Got " +  segment.Length + " segment:");
			if (segment != null ) 
			{
				for (int i = 0; i < segment.Length; i++) 
				{

					printToForm( "SegmentID:      " + segment[i].SegmentID );
					printToForm( "SegmentName:    " + segment[i].SegmentName);
					printToForm( "Description:    " + segment[i].Description );
					printToForm( "SegmentType:    " + segment[i].SegmentType );
					printToForm( "ListName:       " + segment[i].ListName );
					printToForm( "NumTestRecords: " + segment[i].NumTestRecords );
					printToForm( "");

				}  					
			}
		}


        /*
        ################################################################################
        #	createDeleteSegment
        ####################
        */
        private void createDeleteSegment()
		{
			lmapiSoap.SegmentStruct testSegment = new Lyris_Listmanager_SOAP_API_Tests.lmapiSoap.SegmentStruct();
			testSegment.SegmentName = "test segment";
			testSegment.ListName = "list1";
			testSegment.Description = "this is a test segment";
			testSegment.SegmentID = lm.CreateSegment(testSegment);
			printToForm("Segment created with id " + testSegment.SegmentID,true);

			lm.DeleteSegment(testSegment.SegmentID.Value);
			printToForm("Segment deleted Successfully",true);
		}

		/* ===========================================================================================================
		*								MAILING TESTS
		*  ===========================================================================================================*/
		private void mailingTest()
		
		{

				printToForm( "Selecting Content to send.");

				// find some content
				String[] ContentSelectArray = {"Title = content-sample-20"};
				lmapiSoap.ContentStruct[] content = lm.SelectContent( ContentSelectArray );		
				printToForm( "Got  ContentID: " + content[0].ContentID );

				printToForm( "\r\nImporting content "+ content[0].ContentID + " into Mailing");
				
				// use the import content proceedure to populate a MailngStruct
                int tempblah = (int)content[0].ContentID;
                lmapiSoap.SimpleMailingStruct smailing = lm.ImportContent(tempblah);
				
				// use the little utility function to update the mailing
				lmapiSoap.MailingStruct mailing = createMailingFromSimple( smailing );
				
				printToForm("Getting Login Email Address:  ",false);
				String myEmailAddress = lm.CurrentUserEmailAddress();
				printToForm("Got " + myEmailAddress );
				
				// setDontAttemptAfterDate

				mailing.DontAttemptAfterDate =  new DateTime(2005,1,1);
                mailing.DontAttemptAfterDateSpecified = true;
				mailing.ListName = "list1";
				mailing.RewriteDateWhenSent =  true ;
                mailing.RewriteDateWhenSentSpecified = true;
				mailing.BypassModeration =  true ;
                mailing.BypassModerationSpecified = true;
				mailing.From =  myEmailAddress ;
				mailing.Subject = "This is a test of the SOAP API system. This message was sent with C#.NET!!";
                mailing.ReplyTo = "foo@foo.com";
                
				
				//preview mailing
				lmapiSoap.PreviewStruct pvTestMail = new Lyris_Listmanager_SOAP_API_Tests.lmapiSoap.PreviewStruct();
			    pvTestMail.TextToMerge = mailing.HtmlMessage;
			    pvTestMail.SubsetID = 0;
				pvTestMail.MemberID = lm.CreateSingleMember("previewmailing@maileater.lyris.com", "Preview Mailing", "list1");
                pvTestMail.MemberIDSpecified = true;
                pvTestMail.SubsetIDSpecified = true;
				printToForm("Preview of mailing:", false);
				printToForm(lm.GetPreviewMailing(pvTestMail));
                string[] deleteArray = new string[2];
                deleteArray[0] = "EmailAddress like previewmailing%";
                deleteArray[1] = "ListName = list1";
                lm.DeleteMembers(deleteArray);
				
				printToForm("Sending mailing");
				// segmentID of 0 means whole list
				int inMailID = lm.SendMailing(0, mailing);
				printToForm("Got InMailID: " +  inMailID );

				//mailing status takes a while to change as the message is processed.
				printToForm("Running MailingStatus. (my not be complete)");
				printToForm(lm.MailingStatus(inMailID));
				
				/*
				*
				* SendMailingDirect
				*
				*/
				printToForm("Sending MailingDirect"); //this is how you send to someon not on any list on the server. does no list processing.
				
				String[] recipients = {"unknownUser@maileater.lyris.com", "Ben-Franklin@maileater.com", "Thomas-Aquinas@maileater.lyris.com"};
				int[] recipientsByID = new int[3] {1,2,3}; // also send to these memberID's
				
				int outMailID = lm.SendMailingDirect(recipients,recipientsByID, mailing);
				
				if (outMailID > 0) 				
					printToForm("Direct Mailing Sent successfully! outMailID " + outMailID);
				else 
					printToForm("Direct Mailing FAILED! " + outMailID);

			/*
				*
				* SendMessage
				*
				*/
			printToForm("Sending SendMessage"); //this is how you send to someon not on any list on the server. does no list processing.

			lmapiSoap.MessageStruct ms = new lmapiSoap.MessageStruct();

			String[] RecipientEmailsIn = {"unknownUser@maileater.lyris.com", "Ben-Franklin@maileater.com", "Thomas-Aquinas@maileater.lyris.com"};
			int[] RecipientMemberIDsIn = new int[3] {1,2,3}; // also send to these memberID's

			ms.RecipientEmailsIn = RecipientEmailsIn;
			ms.RecipientMemberIDsIn = RecipientMemberIDsIn;
				
			ms.Body = "This is a body of a message";
			lmapiSoap.KeyValueType[] HeadersIn = new lmapiSoap.KeyValueType[2];
			for (int i=0; i < 2; i++ ) HeadersIn[i] = new lmapiSoap.KeyValueType();	
			HeadersIn[0].Name = "Subject";
			HeadersIn[0].Value = "This is a SendMessage test subject";
			HeadersIn[1].Name = "From";
			HeadersIn[1].Value = "ApiClient@maileater.lyris.com";

			ms.HeadersIn = HeadersIn;
			ms.ListName = "list1";
			
			int outMessageID = lm.SendMessage(ms);
				
			if (outMessageID > 0) 				
				printToForm("SendMessage Sent successfully! messageID " + outMessageID);
			else 
				printToForm("SendMessage FAILED! " + outMessageID);

			
			/*
				*
				* Schedule Mailing
				*
				*/		
			bool moderated,undefined=false;
			int moderateID;
			printToForm("ScheduleMailing");							
			moderateID = lm.ScheduleMailing(0,new DateTime(2020,12,31), mailing);
			if (moderateID > 0) 				
				printToForm("Mailing Scheduled successfully! moderateID " + moderateID);
			else 
				printToForm("Schedule Mailing FAILED! " + moderateID);
			/*
				*
				* moderator approval
				*
				*/
			moderated = lm.ModerateMailing(moderateID, true, undefined); // Approve mailing	3rd parameter is undefined
			if (moderated) 				
				printToForm("Moderate Approve successful! moderateID " + moderateID );
			else 
				printToForm("Moderate Approve  FAILED! " + moderateID);




			/*
				*
				* Schedule Mailing
				*
				*/	
			printToForm("ScheduleMailing");							
			moderateID = lm.ScheduleMailing(0,new DateTime(2020,12,31), mailing);
			if (moderateID > 0) 				
				printToForm("Mailing Scheduled successfully! moderateID " + moderateID);
			else 
				printToForm("Schedule Mailing FAILED! " + moderateID);
			/*
				*
				* moderator approval
				*
				*/
			moderated = lm.ModerateMailing(moderateID, false, false); // rejection, no message 	
			if (moderated) 				
				printToForm("Moderate ejection, no message successful! moderateID " + moderateID );
			else 
				printToForm("Moderate ejection, no message  FAILED! " + moderateID);


			/*
				*
				* Schedule Mailing
				*
				*/	
			printToForm("ScheduleMailing");							
			moderateID = lm.ScheduleMailing(0,new DateTime(2020,12,31), mailing);
			if (moderateID > 0) 				
				printToForm("Mailing Scheduled successfully! moderateID " + moderateID);
			else 
				printToForm("Schedule Mailing FAILED! " + moderateID);
			/*
				*
				* moderator approval
				*
				*/
			/*
            moderated = lm.ModerateMailing(moderateID, false, true); // rejection, send rejection message 
			if (moderated) 				
				printToForm("Moderate rejection, send rejection message  successful! moderateID " + moderateID );
			else 
				printToForm("Moderate rejection, send rejection message   FAILED! " + moderateID);
            */

				/*
				 * 
				 * 
				 * TrackingSummary
				 * 
				 */
				printToForm("======= TrackingSummary =====");			
				lmapiSoap.TrackingSummaryStruct summary = lm.TrackingSummary(outMailID);
				
				printToForm("Active: " + summary.Active);
				printToForm("Clickstreams: " + summary.Clickstreams);
				printToForm("Clickthroughs: " + summary.Clickthroughs);
				printToForm("Created: " + summary.Created);
				printToForm("MailingID: " + summary.MailingID);
				printToForm("MailMergeAbort: " + summary.MailMergeAbort);
				printToForm("MailMergeSkipped: " + summary.MailMergeSkipped);
				printToForm("NotAttempted: " + summary.NotAttempted);
				printToForm("Opens: " + summary.Opens);
				printToForm("Paused: " + summary.Paused);
				printToForm("Pending: " + summary.Pending);
				printToForm("PermanentFailure: " + summary.PermanentFailure);
				printToForm("Retry: " + summary.Retry);
				printToForm("Success: " + summary.Success);
				printToForm("Title: " + summary.Title);
				printToForm("TotalRecipients: " + summary.TotalRecipients);
				printToForm("TotalUndelivered: " + summary.TotalUndelivered);
				printToForm("TransientFailure: " + summary.TransientFailure);
				printToForm("UniqueOpens: " + summary.UniqueOpens);
				printToForm("Urls: " + summary.Urls);			 
				
			}

			/* a real application would update the contstructor of the MailingStruct to take a simple mailing struct.... */
			private lmapiSoap.MailingStruct createMailingFromSimple(lmapiSoap.SimpleMailingStruct simple)
			{
				
				lmapiSoap.MailingStruct mailing  = new lmapiSoap.MailingStruct();

				mailing.Subject = simple.Subject;
				mailing.IsHtmlSectionEncoded = simple.IsHtmlSectionEncoded;
                mailing.IsHtmlSectionEncodedSpecified = true;
				mailing.HtmlSectionEncoding = simple.HtmlSectionEncoding;
                mailing.HtmlSectionEncodingSpecified = true;
				mailing.HtmlMessage = simple.HtmlMessage;
				mailing.To = simple.To;
				mailing.CharSetID = simple.CharSetID;
                mailing.CharSetIDSpecified = true;
				mailing.IsTextSectionEncoded = simple.IsTextSectionEncoded;
                mailing.IsTextSectionEncodedSpecified = true;
				mailing.TextSectionEncoding = simple.TextSectionEncoding;
                mailing.TextSectionEncodingSpecified = true;
				mailing.Title = simple.Title;
				mailing.TextMessage = simple.TextMessage;
				mailing.Attachments = simple.Attachments;
				mailing.From = simple.From;
				mailing.AdditionalHeaders = simple.AdditionalHeaders;
                
                

				return mailing;
				
			}

		private void s_member_perf_btn_Click(object sender, System.EventArgs e)
		{
			int memberID;
			
			int numMembers = System.Convert.ToInt32(NumMembers.Text);
			try 
			{
				initSoapVersion();
				printToForm("=======================  MEMBER PERFORMANCE TESTS " + NumMembers.Text + " + MEMBERS==============");
				printToForm("Deleting existing members");
				deleteMembers();

				DateTime t = DateTime.Now;
				printToForm("Sending Soap calls ");		
				for (int i=0 ; i < numMembers  ; i++)
				{	
					if ((i % 10) == 0) 
					{
						printToForm("\r\n" + i + " members @maileater.lyris.com created ", false);
					}

					try	{ memberID = lm.CreateSingleMember(i.ToString() + "@maileater.lyris.com", i.ToString() ,"list1"); }
					catch ( System.Web.Services.Protocols.SoapException ex ){printToForm("***FAULT: " + ex.Message, false);}

				}
				printToForm ("\r\nSeconds Elapsed: " + (DateTime.Now - t).ToString());
				
				t = DateTime.Now;
				selectSimpleMembers( false );
				printToForm ("\r\nSeconds Elapsed: " + (DateTime.Now - t).ToString());
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}
			printToForm(".... MEMBER PERFORMANCE TESTS COMPLETE....\r\n"); 		
		}

		private void member_perf_btn_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "This test has been disabled due to the API function being omitted");
			/*
			int numMembers = System.Convert.ToInt32(NumMembers.Text);
			try 
			{
				initSoapVersion();
				printToForm("\n=======================  CREATE MANY MEMBERS PERFORMANCE TESTS  " + NumMembers.Text + " + MEMBERS==============");
				printToForm("Deleting existing members");
				deleteMembers();
				lmapiSoap.TinyMemberStruct[] Member = new lmapiSoap.TinyMemberStruct[numMembers]; 
				for (int i=0 ; i < numMembers; i++) 
				{			
					Member[i] = new lmapiSoap.TinyMemberStruct();
					Member[i].EmailAddress = i + "icy@maileater.lyris.com";
					Member[i].FullName = "Icy Clone #"+i;
				}
				
				
				
				DateTime t = DateTime.Now;
				lmapiSoap.SimpleMemberStruct[] created = null; 
				printToForm("Sending Soap call");				
				lm.CreateManyMembers(Member, "list1", true);					
				printToForm ("Seconds Elapsed: " + (DateTime.Now - t).ToString());
				printToForm("Done..");

				if (created != null)
				{
					printToForm(created.Length + " members created.");;
					/*for (int i=0 ; i<created.Length ; i++ )
					{
						printToForm(created[i].EmailAddress + " created with member id "  + created[i].MemberID + " on list " + created[i].ListName);
				
					}//* /
				}
			} 
			catch (Exception ex)
			{
				MessageBox.Show(this, "ERROR\r\n"  + ex.Message);
			}

			*/
			
		}

		private void test_site_topic_btn_Click(object sender, System.EventArgs e)
		{
			try
			{
				initSoapVersion();

				printToForm("\n=======================  CREATE/UPDATE/DELETE Sites and Topics TEST==============");
				/* 
				 * Create test site and topic
				 * *************************/

				lmapiSoap.SiteStruct testSite  = new Lyris_Listmanager_SOAP_API_Tests.lmapiSoap.SiteStruct();
				testSite.HostName = "localhost";
				testSite.SiteDescription = "Test Description";
				testSite.SiteName = "test_site";
				testSite.SiteID = lm.CreateSite(testSite);			
				printToForm("Site created with id of " + testSite.SiteID,true);

				lmapiSoap.TopicStruct testTopic = new Lyris_Listmanager_SOAP_API_Tests.lmapiSoap.TopicStruct();
				testTopic.HiddenTopic = false;
				testTopic.SiteName = testSite.SiteName;
				testTopic.TopicDescription = "Test Description";
				testTopic.TopicName = "test_topic";
				lm.CreateTopic(testTopic);
				printToForm("Topic created", true);

				/* 
				 * Update test site and topic
				 * *************************/
				testSite.SiteDescription = "updated Description";
                testSite.SiteIDSpecified = true;
				lm.UpdateSite(testSite);
				printToForm("Site updated", true);

				testTopic.TopicDescription = "updated Description";
                testTopic.HiddenTopicSpecified = true;
				lm.UpdateTopic(testTopic);
				printToForm("Topic updated", true);

				/* 
				 * delete test site and topic
				 * *************************/
				lm.DeleteTopic(testTopic.TopicName);
				printToForm("Topic deleted", true);
				
				lm.DeleteSite((int)testSite.SiteID);
				printToForm("Site deleted", true);
				
				printToForm("\n=======================  END Sites and Topics TEST==============");

			}
			catch ( System.Web.Services.Protocols.SoapException ex )
			{
				printToForm("***FAULT: " + ex.Message, false);
			}


		}

	}
}
