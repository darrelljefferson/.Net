using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrowElectronics
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Init(@"c:\tmp\arrow\log_");
            
            // create the rss feeds to parse
            string arrowNewsRSS = "http://web.arrownac.com/arrow-news-feed";
            string eventsAndTrainingRSS = 
                "http://web.arrownac.com/arrow-events-feed";
            string designAndToolsRSS =
                "http://web.arrownac.com/arrow-design-feed ";
            string newProductsRSS =
                "http://web.arrownac.com/arrow-npi-feed";
            string solutionsRSS =
                "http://web.arrownac.com/arrow-solutions-feed ";
            string featuredPromotionsRSS =
                "http://web.arrownac.com/arrow-featured-promos-feed";

            // create the lists to store the xml elements
            List<string[]> arrowNewsList = new List<string[]> { };
            List<string[]> eventsAndTrainingList = new List<string[]> { };
            List<string[]> designAndToolsList = new List<string[]> { };
            List<string[]> newProductsList = new List<string[]> { };
            List<string[]> solutionsList = new List<string[]> { };
            List<string[]> featuredPromotionsList = new List<string[]> { };

            // create the DateTime values to be used to figure out
            // quartly, monthly, and bi-montly elements
            
            // we always get everything for this feed
               DateTime arrowNewsStartDate = Convert.ToDateTime("2000-01-01");
            // DateTime arrowNewsStartDate = Convert.ToDateTime("2011-05-11");

            //DateTime startDate = DateTime.Now.AddDays(-45);
            //DateTime startDate = Convert.ToDateTime("2011-04-26");
            DateTime startDate = Convert.ToDateTime("2011-05-25");

            DateTime quarterlyDate = DateTime.Now.AddMonths(-3);
            DateTime montlyDate = DateTime.Now.AddDays(-1);
            DateTime biMontlyDate = DateTime.Now.AddDays(-1);

            
            // placeholders for the html conditional content
            string arrowNewsConditionalContent = "";
            string designToolsConditionalContent = "";
            string featuredPromotionsConditionalContent = "";
            string newProductsConditionalContent = "";
            string solutionsConditionalContent = "";
            string eventsAndTrainingConditionalContent = "";

            // placeholders for the text conditional content
            string arrowNewsTextConditionalContent = "";
            string designToolsTextConditionalContent = "";
            string featuredPromotionsTextConditionalContent = "";
            string newProductsTextConditionalContent = "";
            string solutionsTextConditionalContent = "";
            string eventsAndTrainingTextConditionalContent = "";
            


            // parse and populate in the rss info
            // use the old start date at first
            Rss.GetRssElements(arrowNewsRSS, arrowNewsStartDate,
                arrowNewsList);
            Rss.GetRssElements(designAndToolsRSS, startDate,
                designAndToolsList);
            Rss.GetRssElements(featuredPromotionsRSS, startDate,
                featuredPromotionsList);
            Rss.GetRssElements(newProductsRSS, startDate,
                newProductsList);
            Rss.GetRssElements(solutionsRSS, startDate,
                solutionsList);
            Rss.GetRssElements(eventsAndTrainingRSS, arrowNewsStartDate,
                eventsAndTrainingList);
            
           

            // get the counts to confirm
            Console.WriteLine(arrowNewsList.Count());
            Console.WriteLine(eventsAndTrainingList.Count());
            Console.WriteLine(designAndToolsList.Count());
            Console.WriteLine(newProductsList.Count());
            Console.WriteLine(solutionsList.Count());
            Console.WriteLine(arrowNewsList.Count());


            if (arrowNewsList.Count() > 0)
            {
                // arrow news is first
                // Create the conditional content
                arrowNewsConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                        "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"New Products\") || ",
                        "(%%Aerospace and Defense%% = \"Featured Promotions\") || ",
                        "(%%Aerospace and Defense%% = \"Solutions\") || ",
                        "(%%Aerospace and Defense%% = \"Design Tools\") || ",
                        "(%%Aerospace and Defense%% = \"Training and Events\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"New Products\") || ",
                        "(%%Alternative Energy%% = \"Featured Promotions\") || ",
                        "(%%Alternative Energy%% = \"Solutions\") || ",
                        "(%%Alternative Energy%% = \"Design Tools\") || ",
                        "(%%Alternative Energy%% = \"Training and Events\") || ",
                        "(%%AAlternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"New Products\") || ",
                        "(%%Lighting%% = \"Featured Promotions\") || ",
                        "(%%Lighting%% = \"Solutions\") || ",
                        "(%%Lighting%% = \"Design Tools\") || ",
                        "(%%Lighting%% = \"Training and Events\") || ",
                        "(%%Lighting%% = \"All\") || ",
                        "(%%Medical%% = \"New Products\") || ",
                        "(%%Medical%% = \"Featured Promotions\") || ",
                        "(%%Medical%% = \"Solutions\") || ",
                        "(%%Medical%% = \"Design Tools\") || ",
                        "(%%Medical%% = \"Training and Events\") || ",
                        "(%%Medical%% = \"All\") %%]");
                // 

                // create the string we'll use to store news part
                string arrowNewsContent = "<div id =\"news\"><br />";
                // append in the first headline
                arrowNewsContent += "<h2>Arrow News ";
                // span class
                arrowNewsContent += "<span class=\"pipe\">|</span>";
                // span opening
                arrowNewsContent += " <span class=\"sub_text\">";
                // text
                arrowNewsContent += "Find  out what's new at Arrow ";
                // open a tag
                arrowNewsContent += "<a href = \"http://www.arrownac.com/about/news/\">";
                // the a tag descpription
                arrowNewsContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                arrowNewsContent += "</a></span> </h2>";

                // now we'll append in the the RSS items
                foreach (string[] item in arrowNewsList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnNewProductSingleValueConditionalValue(
                        item));
                    arrowNewsContent += HtmlGenerator.
                        ReturnArrowNewsValueConditionalValue(
                        item);
                }

                // Add in the go to the top part
                // start with the opening p tag
                arrowNewsContent += "<p class=\"top\">";

                // add in the href and close the p
                arrowNewsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                arrowNewsContent += "</div>";

                // now append the new content to the condtional area
                // and close out the conditional tag
                arrowNewsConditionalContent = string.Concat(
                    arrowNewsConditionalContent, arrowNewsContent,
                    "[%% endif %%]");

                //Log.WriteLine(arrowNewsConditionalContent);
            }


            if (designAndToolsList.Count() > 0)
            {
                // this is the middle part of the dynamic content
                // create the string we'll use to store design tools part
                string designToolsContent = "<div id=\"tools\"><br />";

                // append in the first headline
                designToolsContent += "<h2>Design Tools ";
                // span class
                designToolsContent += "<span class=\"pipe\">|</span>";
                // span opening
                designToolsContent += " <span class=\"sub_text\">";
                // text
                designToolsContent += "Tools  that help get you to market fast ";
                // open a tag
                designToolsContent += "<a href = \"http://www.arrownac.com/services-tools/design/\">";
                // the a tag descpription
                designToolsContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                designToolsContent += "</a></span> </h2>";
                
                // set this to a static value, so we can put it together late
                string staticHtml1 = designToolsContent;

                
                // this will come after we build the blocks
                // first reset the value
                designToolsContent = "";

                // now rebuild it

                // Add in the go to the top part
                // start with the opening p tag
                designToolsContent += "<p class=\"top\">";

                // add in the href and close the p
                designToolsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                designToolsContent += "</div>";

                // and set it another static value
                string staticHtml2 = designToolsContent;

                
                
                // we now have to deal with the values found inside of the
                // RSS and if we should include the header or not

                // set the boolean values
                Boolean hasMultiple = true;
                Boolean hasAerospaceDefence = true;
                Boolean hasAlternateEnergy = true;
                Boolean hasLighting = true;
                Boolean hasMedical = true;

                // iterate through the elements and set the boolean
                // values to true if they have the values
                foreach (string[] row in designAndToolsList)
                {
                    if (row[3].ToLower() == "multiple" &&
                        hasMultiple == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"All\") ||",
                            "(%%All Arrow Publications%% = \"on\") %%]"
                        );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in designAndToolsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        designToolsConditionalContent += "\n" +
                            conditionalContent;

                        // set the flag to false, so we don't add in any more line
                        hasMultiple = false;

                    }
                    // return the specific values
                    else if (row[3].ToLower() == "aerospace and defense"
                        && hasAerospaceDefence == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&","and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Design Tools\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in designAndToolsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        designToolsConditionalContent += "\n" +
                            conditionalContent;

                        // set the flag to false, so avoid repeated headings
                        hasAerospaceDefence = false;
                    }
                    else if (row[3].ToLower() == "alternative energy"
                        && hasAlternateEnergy == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Design Tools\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in designAndToolsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        designToolsConditionalContent += "\n" +
                            conditionalContent;

                        // set the flag to false, so avoid repeated headings
                        hasAlternateEnergy = false;
                    }
                    else if (row[3].ToLower() == "lighting"
                        && hasLighting == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Design Tools\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in designAndToolsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        designToolsConditionalContent += "\n" +
                            conditionalContent;

                        // set the flag to false, so avoid repeated headings
                        hasLighting = false;
                    }
                    else if (row[3].ToLower() == "medical"
                        && hasMedical == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Design Tools\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in designAndToolsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnDesignToolSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        designToolsConditionalContent += "\n" +
                            conditionalContent;

                        // set the flag to false, so avoid repeated headings
                        hasMedical = false;
                    }
                }
                
                /*
                // design tools is next
                // create the conditional content
                designToolsConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                        "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"Design Tools\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"Design Tools\") || ",
                        "(%%Alternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"Design Tools\") || ",
                        "(%%Lighting%% = \"All\")) || ",
                        "(%%Medical%% = \"Design Tools\")) || ",
                        "(%%Medical%% = \"All\") %%]");
                */
                
                /*
                // now we'll append in the the RSS items
                foreach (string[] item in designAndToolsList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnDesignToolSingleValueConditionalValue(
                        item));
                    designToolsContent += HtmlGenerator.
                        ReturnDesignToolSingleValueConditionalValue(
                        item);
                }
                */
                
                /*
                // now append the new content to the condtional area
                // and close out the conditional tag
                designToolsConditionalContent = string.Concat(
                    designToolsConditionalContent,
                    designToolsContent,
                    "[%% endif %%]");
                */
                //Log.WriteLine(designToolsConditionalContent);
            }


            if (featuredPromotionsList.Count() > 0)
            {
                // set the boolean values
                Boolean hasMultiple = true;
                Boolean hasAerospaceDefence = true;
                Boolean hasAlternateEnergy = true;
                Boolean hasLighting = true;
                Boolean hasMedical = true;
                
                // this is the middle part of the dynamic content
                // create the string we'll use to store featured promotions part
                string featuredPromotionsContent = "<div id=\"promotions\"><br />";

                // append in the first headline
                featuredPromotionsContent += "<h2>Featured Promotions ";
                // span class
                featuredPromotionsContent += "<span class=\"pipe\">|</span>";
                // span opening
                featuredPromotionsContent += " <span class=\"sub_text\">";
                // text
                featuredPromotionsContent += "Great deals from Arrow ";
                // close the a, span, and h2 tags
                featuredPromotionsContent += "</span> </h2>";

                string staticHtml1 = featuredPromotionsConditionalContent;

                // this will come after we build the blocks
                // first reset the value
                featuredPromotionsContent = "";

                // now rebuild it
                
                // Add in the go to the top part
                // start with the opening p tag
                featuredPromotionsContent += "<p class=\"top\">";

                // add in the href and close the p
                featuredPromotionsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                featuredPromotionsContent += "</div>";

                // set the next html area
                string staticHtml2 = featuredPromotionsContent;



                // iterate through the elements and set the boolean
                // values to true if they have the values
                foreach (string[] row in featuredPromotionsList)
                {
                    if (row[3].ToLower() == "multiple" &&
                        hasMultiple == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"All\") ||",
                            "(%%All Arrow Publications%% = \"on\") %%]"
                        );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in featuredPromotionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        featuredPromotionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid duplicate headers
                        hasMultiple = false;

                    }
                    // return the specific values
                    else if (row[3].ToLower() == "aerospace & defense" &&
                        hasAerospaceDefence == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Featured Promotions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in featuredPromotionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        featuredPromotionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid duplicate headers
                        hasAerospaceDefence = false;
                    }
                    else if (row[3].ToLower() == "alternative energy" &&
                        hasAlternateEnergy == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Featured Promotions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in featuredPromotionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        featuredPromotionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid duplicate headers
                        hasAlternateEnergy = false;
                    }
                    else if (row[3].ToLower() == "lighting" &&
                        hasLighting == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Featured Promotions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in featuredPromotionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        featuredPromotionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid duplicate headers
                        hasLighting = false;
                    }

                    else if (row[3].ToLower() == "medical" &&
                        hasMedical == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Featured Promotions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in featuredPromotionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnFeaturedPromotionSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        featuredPromotionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid duplicate headers
                        hasMedical = false;
                    }


                }


                
                /*
                // Featured Promotions is next
                // create the conditional content
                featuredPromotionsConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                       "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"Featured Promotions\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"Featured Promotions\") || ",
                        "(%%Alternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"Featured Promotions\") || ",
                        "(%%Lighting%% = \"All\")) || ",
                        "(%%Medical%% = \"Featured Promotions\")) || ",
                        "(%%Medical%% = \"All\") %%]");

                // create the string we'll use to store featured promotions part
                string featuredPromotionsContent = "<div id=\"promotions\"><br />";

                // append in the first headline
                featuredPromotionsContent += "<h2>Featured Promotions ";
                // span class
                featuredPromotionsContent += "<span class=\"pipe\">|</span>";
                // span opening
                featuredPromotionsContent += " <span class=\"sub_text\">";
                // text
                featuredPromotionsContent += "Great deals from Arrow ";
                // close the a, span, and h2 tags
                featuredPromotionsContent += "</span> </h2>";

                // // now we'll append in the the RSS items
                foreach (string[] item in featuredPromotionsList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnFeaturedPromotionSingleValueConditionalValue(
                        item));
                    featuredPromotionsContent += HtmlGenerator.
                        ReturnFeaturedPromotionSingleValueConditionalValue(
                        item);
                }

                // Add in the go to the top part
                // start with the opening p tag
                featuredPromotionsContent += "<p class=\"top\">";

                // add in the href and close the p
                featuredPromotionsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                featuredPromotionsContent += "</div>";

                // now append the new content to the condtional area
                // and close out the conditional tag
                featuredPromotionsConditionalContent = string.Concat(
                    featuredPromotionsConditionalContent,
                    featuredPromotionsContent,
                    "[%% endif %%]");

                //Log.WriteLine(featuredPromotionsConditionalContent);
                */
            }

            if (newProductsList.Count() > 0)
            {

                // set the boolean values
                Boolean hasMultiple = true;
                Boolean hasOther = true;
                Boolean hasAerospaceDefence = true;
                Boolean hasAlternateEnergy = true;
                Boolean hasLighting = true;
                Boolean hasMedical = true;

                // counts to keep track of the # of items
                int aerospaceDefenseCount = 0;
                int alternativeEneryCount = 0;
                int lightingCount = 0;
                int medicalCount = 0;

                // strings value to use if logic with
                string aerospaceDefense = "Aerospace & Defense";
                string alternativeEnergy = "Alternative Energy";
                string lighting = "Lighting";
                string medical = "Medical";

                // create the string we'll use to store featured promotions part
                string newProductContent = "<div id=\"products\"><br />";

                // append in the first headline
                newProductContent += "<h2>New Products ";
                // span class
                newProductContent += "<span class=\"pipe\">|</span>";
                // span opening
                newProductContent += " <span class=\"sub_text\">";
                // text
                newProductContent += "The latest products from the industry's innovators ";
                // open a tag
                newProductContent += "<a href = \"http://www.arrownac.com/mktg/new-products/\">";
                // the a tag descpription
                newProductContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                newProductContent += "</a></span> </h2>";

                // set it to the html value
                string staticHtml1 = newProductContent;

                // reset the value
                newProductContent = "";

                // Add in the go to the top part
                // start with the opening p tag
                newProductContent += "<p class=\"top\">";

                // add in the href and close the p
                newProductContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                newProductContent += "</div>";

                // set it to the html value
                string staticHtml2 = newProductContent;

                // 2011-04-26 new requirement to have 1 heading for all selections
                // but now they want to include everything, not just multiple
                // since multiple comes first, then we'll cut it off after the first iteration
                

                // iterate through the elements and set the boolean
                // values to true if they have the values
                foreach (string[] row in newProductsList)
                {
                    if (row[3].ToLower() == "multiple" &&
                        hasMultiple == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"All\") ||",
                            "(%%All Arrow Publications%% = \"on\") %%]"
                        );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        foreach (string[] item in newProductsList)
                        {
                            /*
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                            */
                            Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                            conditionalContent += HtmlGenerator.
                                ReturnNewProductSingleValueConditionalValue(
                                item);

                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        newProductsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid duplicate headers
                        hasMultiple = false;

                    }
                    // will return just 1 headline
                    // so need to search for them all
                    // then build all the RSS under it

                    // first looks for the other 4 cateogories
                    else if (
                        (row[3] == aerospaceDefense ||
                        row[3] == alternativeEnergy ||
                        row[3] == lighting ||
                        row[3] == medical) 
                        && hasOther == true)
                    {
                        // variables to build the conditional stuff wtih
                        string aerospaceDefenceCondition = "";
                        string alternativeEnergyCondition = "";
                        string lightingCondition = "";
                        string medicalCondition = "";
                        


                        // loop through elements and increment the counts if
                        // we find the matching element
                        // since each section determines the conditional block,
                        // then set it to the value
                        foreach (string[] element in newProductsList)
                        {
                            
                            // increment the aerospace count if we find a match
                            if (element[3] == aerospaceDefense)
                            {
                                aerospaceDefenseCount++;
                            }
                            // increment the alernative count if we find a match
                            else if (element[3] == alternativeEnergy)
                            {
                                alternativeEneryCount++;
                            }
                            // increment the lighting count if we find a match
                            else if (element[3] == lighting)
                            {
                                lightingCount++;
                            }
                            // increment the medical count if we find a match
                            else if (element[3] == medical)
                            {
                                medicalCount++;
                            }
                        }

                        Console.WriteLine(aerospaceDefenseCount +
                            alternativeEneryCount + lightingCount +
                            medicalCount);

                        // build the middle part if the count is > 1
                        if (aerospaceDefenseCount > 0)
                        {
                            aerospaceDefenceCondition = string.Concat(
                                "(%%Categories%% = \"", aerospaceDefense.Replace("&","and"), "\") ||",
                                "(%%", aerospaceDefense.Replace("&", "and"), "%% = \"New Products\") || ",
                                "(%%", aerospaceDefense.Replace("&", "and"), "%% = \"All\") ||");
                        }
                        if (alternativeEneryCount > 0)
                        {
                            alternativeEnergyCondition = string.Concat(
                                "(%%Categories%% = \"", alternativeEnergy, "\") ||",
                                "(%%", alternativeEnergy, "%% = \"New Products\") || ",
                                "(%%", alternativeEnergy, "%% = \"All\") ||");
                        }
                        if (lightingCount > 0)
                        {
                            lightingCondition = string.Concat(
                                "(%%Categories%% = \"", lighting, "\") ||",
                                "(%%", lighting, "%% = \"New Products\") || ",
                                "(%%", lighting, "%% = \"All\") ||");
                        }
                        if (medicalCount > 0)
                        {
                            medicalCondition = string.Concat(
                                "(%%Categories%% = \"", medical, "\") ||",
                                "(%%", medical, "%% = \"New Products\") || ",
                                "(%%", medical, "%% = \"All\") ||");
                        }

                        // concat the values to together to create the middle part
                        string conditionalMiddle = string.Concat(
                            aerospaceDefenceCondition,
                            alternativeEnergyCondition,
                            lightingCondition,
                            medicalCondition
                            );

                        Console.WriteLine(conditionalMiddle.Length);
                        // find the length
                        int stringLength = conditionalMiddle.Length;
                        // want to remove the last 2 characters
                        stringLength--;
                        stringLength--;
                        // remove the last 2 characters
                        conditionalMiddle = conditionalMiddle.Remove(
                            stringLength, 2);
                        // now append in the closing if part
                        conditionalMiddle += "%%]";


                        // change this to put in the new block
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            conditionalMiddle
                            );

                        conditionalContent += staticHtml1;
                        
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // if we find a match to item and the count is > 0
                        foreach (string[] item in newProductsList)
                        {
                            // only bring in rss elements that have counts > 0
                            // and they match the category

                            // aerospace & defense
                            if (aerospaceDefenseCount > 0 
                                && item[3] == aerospaceDefense)
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                            // alternative engery
                            else if (alternativeEneryCount > 0
                                && item[3] == alternativeEnergy)
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                            // lighting
                            else if (lightingCount > 0 &&
                                item[3] == lighting)
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                            // medical 
                            else if (medicalCount > 0 &&
                                item[3] == medical)
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        newProductsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid duplicate headers
                        hasAerospaceDefence = false;

                        // set the flag to false
                        hasOther = false;
                    }
                    /*
                    else if (row[3].ToLower() == "alternative energy" &&
                        hasAlternateEnergy == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"New Products\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in newProductsList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        newProductsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid duplicate headers
                        hasAlternateEnergy = false;
                    }
                    else if (row[3].ToLower() == "lighting" &&
                        hasLighting == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"New Products\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in newProductsList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        newProductsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid duplicate headers
                        hasLighting = false;
                    }
                    else if (row[3].ToLower() == "medical" &&
                        hasMedical == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"New Products\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in newProductsList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnNewProductSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        newProductsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid duplicate headers
                        hasMedical = false;
                    }
                    */
                }
                

                 
                /* 
                // New Products is next
                // create the conditional content
                newProductsConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                        "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"New Products\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"New Products\") || ",
                        "(%%Alternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"New Products\") || ",
                        "(%%Lighting%% = \"All\")) || ",
                        "(%%Medical%% = \"New Products\")) || ",
                        "(%%Medical%% = \"All\") %%]");

                // create the string we'll use to store featured promotions part
                string newProductContent = "<div id=\"products\"><br />";

                // append in the first headline
                newProductContent += "<h2>New Products ";
                // span class
                newProductContent += "<span class=\"pipe\">|</span>";
                // span opening
                newProductContent += " <span class=\"sub_text\">";
                // text
                newProductContent += "The latest products from the industry's innovators ";
                // open a tag
                newProductContent += "<a href = \"http://www.arrownac.com/mktg/new-products/\">";
                // the a tag descpription
                newProductContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                newProductContent += "</a></span> </h2>";

                // // now we'll append in the the RSS items
                foreach (string[] item in newProductsList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnNewProductSingleValueConditionalValue(
                        item));
                    newProductContent += HtmlGenerator.
                        ReturnNewProductSingleValueConditionalValue(
                        item);
                }

                // Add in the go to the top part
                // start with the opening p tag
                newProductContent += "<p class=\"top\">";

                // add in the href and close the p
                newProductContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                newProductContent += "</div>";

                // now append the new content to the condtional area
                // and close out the conditional tag
                newProductsConditionalContent = string.Concat(
                    newProductsConditionalContent,
                    newProductContent,
                    "[%% endif %%]");

                //Log.WriteLine(newProductsConditionalContent);
                */
            }


            
            if (solutionsList.Count() > 0)
            {

                // set the boolean values
                Boolean hasMultiple = true;
                Boolean hasAerospaceDefence = true;
                Boolean hasAlternateEnergy = true;
                Boolean hasLighting = true;
                Boolean hasMedical = true;




                // create the string we'll use to store featured promotions part
                string solutionsContent = "<div id=\"solutions\"><br />";

                // append in the first headline
                solutionsContent += "<h2>Solutions ";
                // span class
                solutionsContent += "<span class=\"pipe\">|</span>";
                // span opening
                solutionsContent += " <span class=\"sub_text\">";
                // text
                solutionsContent += "Technologies brought together to solve your design needs ";
                // close the span, and h2 tags
                solutionsContent += "</span> </h2>";

                string staticHtml1 = solutionsContent;

                solutionsContent = "";

                // Add in the go to the top part
                // start with the opening p tag
                solutionsContent += "<p class=\"top\">";

                // add in the href and close the p
                solutionsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                solutionsContent += "</div>";

                string staticHtml2 = solutionsContent;


                                // iterate through the elements and set the boolean
                // values to true if they have the values
                foreach (string[] row in solutionsList)
                {
                    if ((row[3].ToLower() == "multiple" || row[3] == "")
                        && hasMultiple == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"All\") ||",
                            "(%%All Arrow Publications%% = \"on\") %%]"
                        );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // // now we'll append in the the RSS items
                        foreach (string[] item in solutionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        solutionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid a duplicate header
                        hasMultiple = false;

                    }
                    // return the specific values
                    else if (row[3].ToLower() == "aerospace & defense"
                        && hasAerospaceDefence == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Solutions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in solutionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        solutionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid a duplicate header
                        hasAerospaceDefence = false;                               
                    }

                    else if (row[3].ToLower() == "alternative energy"
                        && hasAlternateEnergy == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Solutions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in solutionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        solutionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid a duplicate header
                        hasAlternateEnergy = false;
                    }
                    else if (row[3].ToLower() == "lighting"
                        && hasLighting == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Solutions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in solutionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        solutionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid a duplicate header
                        hasLighting = false;
                    }

                    else if (row[3].ToLower() == "medical"
                        && hasMedical == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Solutions\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items
                        // // now we'll append in the the RSS items
                        foreach (string[] item in solutionsList)
                        {
                            Log.WriteLine(HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item));
                            conditionalContent += HtmlGenerator.
                                ReturnSolutionsSingleValueConditionalValue(
                                item);
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        solutionsConditionalContent += "\n" +
                            conditionalContent;

                        // reset the value to avoid a duplicate header
                        hasMedical = false;
                    }
                }

                
                /*
                // Featured Solutions is next
                // create the conditional content
                solutionsConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                        "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"Solutions\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"Solutions\") || ",
                        "(%%Alternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"Solutions\") || ",
                        "(%%Lighting%% = \"All\")) || ",
                        "(%%Medical%% = \"Solutions\")) || ",
                        "(%%Medical%% = \"All\") %%]");

                // create the string we'll use to store featured promotions part
                string solutionsContent = "<div id=\"solutions\"><br />";

                // append in the first headline
                solutionsContent += "<h2>Solutions ";
                // span class
                solutionsContent += "<span class=\"pipe\">|</span>";
                // span opening
                solutionsContent += " <span class=\"sub_text\">";
                // text
                solutionsContent += "Technologies brought together to solve your design needs ";
                // close the span, and h2 tags
                solutionsContent += "</span> </h2>";

                // // now we'll append in the the RSS items
                foreach (string[] item in solutionsList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnSolutionsSingleValueConditionalValue(
                        item));
                    solutionsContent += HtmlGenerator.
                        ReturnSolutionsSingleValueConditionalValue(
                        item);
                }

                // Add in the go to the top part
                // start with the opening p tag
                solutionsContent += "<p class=\"top\">";

                // add in the href and close the p
                solutionsContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                solutionsContent += "</div>";

                // now append the new content to the condtional area
                // and close out the conditional tag
                solutionsConditionalContent = string.Concat(
                    solutionsConditionalContent,
                    solutionsContent,
                    "[%% endif %%]");

                //Log.WriteLine(solutionsConditionalContent);
                */
            }



            if (eventsAndTrainingList.Count() > 0)
            {
                // set the boolean values
                Boolean hasMultiple = true;
                Boolean hasAerospaceDefence = true;
                Boolean hasAlternateEnergy = true;
                Boolean hasLighting = true;
                Boolean hasMedical = true;
                
                // create the string we'll use to store featured promotions part
                string eventsAndTrainingContent = "<div id=\"events\"><br />";

                // append in the first headline
                eventsAndTrainingContent += "<h2>Training and Events ";
                // span class
                eventsAndTrainingContent += "<span class=\"pipe\">|</span>";
                // span opening
                eventsAndTrainingContent += " <span class=\"sub_text\">";
                // text
                eventsAndTrainingContent += "<a href = \"http://www.arrownac.com/events-training/training/\">";
                // the a tag descpription
                eventsAndTrainingContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                eventsAndTrainingContent += "</a></span> </h2>";

                string staticHtml1 = eventsAndTrainingContent;

                // reset the value
                eventsAndTrainingContent = "";

                
                // Add in the go to the top part
                // start with the opening p tag
                eventsAndTrainingContent += "<p class=\"top\">";

                // add in the href and close the p
                eventsAndTrainingContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                eventsAndTrainingContent += "</div>";

                string staticHtml2 = eventsAndTrainingContent;


                // iterate through the elements and set the boolean
                // values to true if they have the values
                foreach (string[] row in eventsAndTrainingList)
                {
                    if (row[3].ToLower() == "multiple"
                        && hasMultiple == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"All\") ||",
                            "(%%All Arrow Publications%% = \"on\") %%]"
                        );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part

                        // // now we'll append in the the RSS items
                        foreach (string[] item in eventsAndTrainingList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        eventsAndTrainingConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid a duplicate header
                        hasMultiple = false;

                    }
                    // return the specific values
                    else if (row[3].ToLower() == "aerospace & defense"
                        && hasAerospaceDefence == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Training and Events\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items

                        // // now we'll append in the the RSS items
                        foreach (string[] item in eventsAndTrainingList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        eventsAndTrainingConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid a duplicate header
                        hasAerospaceDefence = false;
                    }
                    else if (row[3].ToLower() == "alternative energy"
                        && hasAlternateEnergy == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Training and Events\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items

                        // // now we'll append in the the RSS items
                        foreach (string[] item in eventsAndTrainingList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        eventsAndTrainingConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid a duplicate header
                        hasAlternateEnergy = false;
                    }
                    else if (row[3].ToLower() == "lighting"
                        && hasLighting == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Training and Events\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items

                        // // now we'll append in the the RSS items
                        foreach (string[] item in eventsAndTrainingList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        eventsAndTrainingConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid a duplicate header
                        hasLighting = false;
                    }

                    else if (row[3].ToLower() == "medical"
                        && hasMedical == true)
                    {
                        string conditionalContent = string.Concat(
                            "[%% ",
                            "if ",
                            "(%%Categories%% = \"", row[3].Replace("&", "and"), "\") ||",
                            "(%%", row[3].Replace("&", "and"), "%% = \"Training and Events\") || ",
                            "(%%", row[3].Replace("&", "and"), "%% = \"All\") %%] "
                            );

                        conditionalContent += staticHtml1;
                        // do a foreach to build the next part
                        // now we'll append in the the RSS items

                        // // now we'll append in the the RSS items
                        foreach (string[] item in eventsAndTrainingList)
                        {
                            // only bring in rss elements that match
                            // the category we are searching on
                            if (row[3] == item[3])
                            {
                                Log.WriteLine(HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item));
                                conditionalContent += HtmlGenerator.
                                    ReturnTrainingEventsSingleValueConditionalValue(
                                    item);
                            }
                        }

                        // add in the next section
                        conditionalContent += staticHtml2;
                        // finish it out
                        conditionalContent += "[%% endif %%]";

                        // append to the design tools conditional content
                        eventsAndTrainingConditionalContent += "\n" +
                            conditionalContent;

                        // reset the flag to avoid a duplicate header
                        hasMedical = false;
                    }
                }



                /*
                // Training and events is last
                // create the conditional content
                eventsAndTrainingConditionalContent = string.Concat(
                        "[%% ",
                        "if ",
                       "(%%Categories%% = \"All\") ||",
                        "(%%Categories%% = \"Aerospace and Defense\") ||",
                        "(%%Categories%% = \"Alternative Energy\") ||",
                        "(%%Categories%% = \"Lighting\") ||",
                        "(%%Categories%% = \"Medical\") ||",
                        "(%%All Arrow Publications%% = \"on\") ||",
                        "(%%Aerospace and Defense%% = \"Training and Events\") || ",
                        "(%%Aerospace and Defense%% = \"All\") || ",
                        "(%%Alternative Energy%% = \"Training and Events\") || ",
                        "(%%Alternative Energy%% = \"All\") || ",
                        "(%%Lighting%% = \"Training and Events\") || ",
                        "(%%Lighting%% = \"All\")) || ",
                        "(%%Medical%% = \"Training and Events\")) || ",
                        "(%%Medical%% = \"All\") %%]");

                // create the string we'll use to store featured promotions part
                string eventsAndTrainingContent = "<div id=\"events\"><br />";

                // append in the first headline
                eventsAndTrainingContent += "<h2>Training and Events ";
                // span class
                eventsAndTrainingContent += "<span class=\"pipe\">|</span>";
                // span opening
                eventsAndTrainingContent += " <span class=\"sub_text\">";
                // text
                eventsAndTrainingContent += "<a href = \"http://www.arrownac.com/events-training/training/\">";
                // the a tag descpription
                eventsAndTrainingContent += "View all &gt;&gt;";
                // close the a, span, and h2 tags
                eventsAndTrainingContent += "</a></span> </h2>";

                // // now we'll append in the the RSS items
                foreach (string[] item in eventsAndTrainingList)
                {
                    Log.WriteLine(HtmlGenerator.
                        ReturnTrainingEventsSingleValueConditionalValue(
                        item));
                    eventsAndTrainingContent += HtmlGenerator.
                        ReturnTrainingEventsSingleValueConditionalValue(
                        item);
                }

                // Add in the go to the top part
                // start with the opening p tag
                eventsAndTrainingContent += "<p class=\"top\">";

                // add in the href and close the p
                eventsAndTrainingContent += "<a href=\"#\">Back to top</a></p>";

                // close out the div tag to complete this section
                eventsAndTrainingContent += "</div>";

                // now append the new content to the condtional area
                // and close out the conditional tag
                eventsAndTrainingConditionalContent = string.Concat(
                    eventsAndTrainingConditionalContent,
                    eventsAndTrainingContent,
                    "[%% endif %%]");

                //Log.WriteLine(eventsAndTrainingConditionalContent);
               */

            }

            Log.WriteLine("\n" + arrowNewsConditionalContent +
                "\n" + designToolsConditionalContent +
                "\n" + featuredPromotionsConditionalContent +
                "\n" + newProductsConditionalContent +
                "\n" + solutionsConditionalContent +
                "\n" + eventsAndTrainingConditionalContent);
            
            
            
            
            
            

            


           
        }
    }
}
