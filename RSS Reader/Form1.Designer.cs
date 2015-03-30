namespace RSS_Reader
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRead = new System.Windows.Forms.Button();
            this.lstNews = new System.Windows.Forms.ListView();
            this.colTitle = new System.Windows.Forms.ColumnHeader();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.pnlFeedInfo = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.Label();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.colLink = new System.Windows.Forms.ColumnHeader();
            this.pnlInput.SuspendLayout();
            this.pnlFeedInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(73, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(562, 20);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "http://www.geekpedia.com/gp_programming.xml";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Feed URL:";
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(641, 9);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "Read Feed";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lstNews
            // 
            this.lstNews.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTitle,
            this.colLink});
            this.lstNews.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstNews.FullRowSelect = true;
            this.lstNews.Location = new System.Drawing.Point(0, 82);
            this.lstNews.MultiSelect = false;
            this.lstNews.Name = "lstNews";
            this.lstNews.Size = new System.Drawing.Size(728, 209);
            this.lstNews.TabIndex = 3;
            this.lstNews.View = System.Windows.Forms.View.Details;
            this.lstNews.SelectedIndexChanged += new System.EventHandler(this.lstNews_SelectedIndexChanged);
            this.lstNews.DoubleClick += new System.EventHandler(this.lstNews_DoubleClick);
            // 
            // colTitle
            // 
            this.colTitle.Text = "Title";
            this.colTitle.Width = 300;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 291);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(728, 188);
            this.txtContent.TabIndex = 4;
            this.txtContent.Text = "";
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.txtUrl);
            this.pnlInput.Controls.Add(this.label1);
            this.pnlInput.Controls.Add(this.btnRead);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInput.Location = new System.Drawing.Point(0, 0);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(728, 44);
            this.pnlInput.TabIndex = 5;
            // 
            // pnlFeedInfo
            // 
            this.pnlFeedInfo.Controls.Add(this.lblDescription);
            this.pnlFeedInfo.Controls.Add(this.lblLanguage);
            this.pnlFeedInfo.Controls.Add(this.lblLink);
            this.pnlFeedInfo.Controls.Add(this.lblTitle);
            this.pnlFeedInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFeedInfo.Location = new System.Drawing.Point(0, 44);
            this.pnlFeedInfo.Name = "pnlFeedInfo";
            this.pnlFeedInfo.Size = new System.Drawing.Size(728, 38);
            this.pnlFeedInfo.TabIndex = 8;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(26, 13);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(216, 3);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(26, 13);
            this.lblLink.TabIndex = 1;
            this.lblLink.Text = "Link:";
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(542, 3);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(54, 13);
            this.lblLanguage.TabIndex = 2;
            this.lblLanguage.Text = "Language:";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(59, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description:";
            // 
            // colLink
            // 
            this.colLink.Text = "Link";
            this.colLink.Width = 378;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 479);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.lstNews);
            this.Controls.Add(this.pnlFeedInfo);
            this.Controls.Add(this.pnlInput);
            this.Name = "frmMain";
            this.Text = "RSS Reader";
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.pnlFeedInfo.ResumeLayout(false);
            this.pnlFeedInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ListView lstNews;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.Panel pnlFeedInfo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblLink;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ColumnHeader colLink;
    }
}

