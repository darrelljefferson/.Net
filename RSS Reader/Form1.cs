using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace RSS_Reader
{
    public partial class frmMain : Form
    {
        XmlTextReader rssReader;
        XmlDocument rssDoc;
        XmlNode nodeRss;
        XmlNode nodeChannel;
        XmlNode nodeItem;
        ListViewItem rowNews;
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            lstNews.Items.Clear();
            this.Cursor = Cursors.WaitCursor;
            // Create a new XmlTextReader from the specified URL (RSS feed)
            rssReader = new XmlTextReader(txtUrl.Text);
            rssDoc = new XmlDocument();
            // Load the XML content into a XmlDocument
            rssDoc.Load(rssReader);
            
            // Loop for the <rss> tag
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                // If it is the rss tag
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    // <rss> tag found
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }

            // Loop for the <channel> tag
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                // If it is the channel tag
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    // <channel> tag found
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }

            // Set the labels with information from inside the nodes
            lblTitle.Text = "Title: " + nodeChannel["title"].InnerText;
            lblLanguage.Text = "Language: " + nodeChannel["language"].InnerText;
            lblLink.Text = "Link: " + nodeChannel["link"].InnerText;
            lblDescription.Text = "Description: " + nodeChannel["description"].InnerText;

            // Loop for the <title>, <link>, <description> and all the other tags
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                // If it is the item tag, then it has children tags which we will add as items to the ListView
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];
                    
                    // Create a new row in the ListView containing information from inside the nodes
                    rowNews = new ListViewItem();
                    rowNews.Text = nodeItem["title"].InnerText;
                    rowNews.SubItems.Add(nodeItem["link"].InnerText);
                    lstNews.Items.Add(rowNews);
                }
            }

            this.Cursor = Cursors.Default;
        }

        private void lstNews_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When an items is selected
            if (lstNews.SelectedItems.Count == 1)
            {
                // Loop through all the nodes under <channel>
                for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
                {
                    // Until you find the <item> node
                    if (nodeChannel.ChildNodes[i].Name == "item")
                    {
                        // Store the item as a node
                        nodeItem = nodeChannel.ChildNodes[i];
                        // If the <title> tag matches the current selected item
                        if (nodeItem["title"].InnerText == lstNews.SelectedItems[0].Text)
                        {
                            // It's the item we were looking for, get the description
                            txtContent.Text = nodeItem["description"].InnerText;
                            // We don't need to loop anymore
                            break;
                        }
                    }
                }
            }
        }

        private void lstNews_DoubleClick(object sender, EventArgs e)
        {
            // When double clicked open the web page
            System.Diagnostics.Process.Start(lstNews.SelectedItems[0].SubItems[1].Text);   
        }
    }
}