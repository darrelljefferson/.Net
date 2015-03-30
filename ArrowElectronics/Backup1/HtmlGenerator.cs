using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ArrowElectronics
{
    class HtmlGenerator
    {
        public static string ReturnArrowNewsValueConditionalValue(
            string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate
            
            string conditionalContentBlock = "";
            
            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";            
            string staticHtmlClosingTag1 = "</p>";

            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
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
                    "(%%Alternative Energy%% = \"All\") || ",
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


                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += newProductArray[2];

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // fill in the a href part
                newsItem = string.Concat(newsItem,
                    staticHtmlOpeningTag5,
                    newProductArray[2], "\">", "More &gt;&gt;",
                    staticHtmlClosingTag5);

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");


                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }
            // only thing left should be the specific demographic
            else
            {
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"),"%% = \"New Products\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Featured Products\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Solutions\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Design Tools\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Training and Events\") || ",
                    "(%%", newProductArray[3].Replace("&","and"),"%% = \"All\") %%] "
                    );

                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += newProductArray[2];

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description plus a space
                newsItem += newProductArray[1]+" ";

                // fill in the a href part
                newsItem = string.Concat(newsItem,
                    staticHtmlOpeningTag5,
                    newProductArray[2], "\">", "More &gt;&gt;",
                    staticHtmlClosingTag5);

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");


                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;
        }


        public static string ReturnDesignToolSingleValueConditionalValue(
            string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate

            string conditionalContentBlock = "";

            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";
            string staticHtmlClosingTag1 = "</p>";

            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                    "(%%Categories%% = \"Alternative Energy\") ||",
                    "(%%Categories%% = \"Lighting\") ||",
                    "(%%Categories%% = \"Medical\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Aerospace and Defense eResource%% = \"Design Tools\") || ",
                    "(%%Aerospace and Defense eResource%% = \"All\") || ",
                    "(%%Alternative Energy%% = \"Design Tools\") || ",
                    "(%%Alternative Energy%% = \"All\") || ",
                    "(%%Lighting%% = \"Design Tools\") || ",
                    "(%%Lighting%% = \"All\")) || ",
                    "(%%Medical%% = \"Design Tools\")) || ",
                    "(%%Medical%% = \"All\") %%]");
                 */

                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") %%]"
                    );


                
                // build the appropriate section
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // close the link
                newsItem += "\">";


                // add in the strong tag
                // use the title as the display name
                // then close the a tag
                // add the br tag
                newsItem = string.Concat(newsItem,
                    staticHtmlOpeningTag3,
                    newProductArray[0],
                    staticHtmlClosingTag3,
                    staticHtmlClosingTag2,
                    staticHtmlOpeningTag4
                    );


                // fill in the description
                newsItem += newProductArray[1];

                // close the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag
                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");


            }
            // only thing left should be the specific demographic
            else
            {
                
                
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3], "%% = \"Design Tools\") || ",
                    "(%%", newProductArray[3], "%% = \"All\") %%] "
                    );
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                // also need to change & character to a value of and
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Design Tools\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"All\") %%] "
                    );


                // build the appropriate section
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // close the link
                newsItem += "\">";


                // add in the strong tag
                // use the title as the display name
                // then close the a tag
                // add the br tag
                newsItem = string.Concat(newsItem,
                    staticHtmlOpeningTag3,
                    newProductArray[0],
                    staticHtmlClosingTag3,
                    staticHtmlClosingTag2,
                    staticHtmlOpeningTag4
                    );


                // fill in the description
                newsItem += newProductArray[1];

                // close the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag
                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;

        }

        public static string ReturnFeaturedPromotionSingleValueConditionalValue(
            string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate

            string conditionalContentBlock = "";


            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";
            string staticHtmlClosingTag1 = "</p>";

            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                    "(%%Categories%% = \"Alternative Energy\") ||",
                    "(%%Categories%% = \"Lighting\") ||",
                    "(%%Categories%% = \"Medical\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Aerospace and Defense eResource%% = \"Featured Promotions\") || ",
                    "(%%Aerospace and Defense eResource%% = \"All\") || ",
                    "(%%Alternative Energy%% = \"Featured Promotions\") || ",
                    "(%%Alternative Energy%% = \"All\") || ",
                    "(%%Lighting%% = \"Featured Promotions\") || ",
                    "(%%Lighting%% = \"All\")) || ",
                    "(%%Medical%% = \"Featured Promotions\")) || ",
                    "(%%Medical%% = \"All\") %%]");
                 */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") %%]"
                    );

                // build the appropriate section
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // close the open a tag
                newsItem += "\">";


                // add in the strong tag
                // use the title as the display name
                // then close the a tag
                newsItem = string.Concat(newsItem, 
                    staticHtmlOpeningTag3,
                    newProductArray[0],
                    staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );


                // fill in the description
                newsItem += " "+newProductArray[1];

                // close the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag
                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");

            }
            // only thing left should be the specific demographic
            else
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3], "%% = \"Featured Promotions\") || ",
                    "(%%", newProductArray[3], "%% = \"All\") %%] "
                    );
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Featured Promotions\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"All\") %%] "
                    );

                // build the appropriate section
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // close the open a tag
                newsItem += "\">";


                // add in the strong tag
                // use the title as the display name
                // then close the a tag
                newsItem = string.Concat(newsItem,
                    staticHtmlOpeningTag3,
                    newProductArray[0],
                    staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );


                // fill in the description
                newsItem += " " + newProductArray[1];

                // close the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag
                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;

        }

        public static string ReturnNewProductSingleValueConditionalValue(
           string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate

            string conditionalContentBlock = "";

            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";
            string staticHtmlClosingTag1 = "</p>";

            Log.WriteLine(newProductArray[3]);
            Console.WriteLine(newProductArray[3].Contains('&'));
            
            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                    "(%%Categories%% = \"Alternative Energy\") ||",
                    "(%%Categories%% = \"Lighting\") ||",
                    "(%%Categories%% = \"Medical\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Aerospace and Defense eResource%% = \"New Products\") || ",
                    "(%%Aerospace and Defense eResource%% = \"All\") || ",
                    "(%%Alternative Energy%% = \"New Products\") || ",
                    "(%%Alternative Energy%% = \"All\") || ",
                    "(%%Lighting%% = \"New Products\") || ",
                    "(%%Lighting%% = \"All\")) || ",
                    "(%%Medical%% = \"New Products\")) || ",
                    "(%%Medical%% = \"All\") %%]");
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") %%]"
                   );

                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");


                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }
                
            // only thing left should be the specific demographic            
            else
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3], "%% = \"New Products\") || ",
                    "(%%", newProductArray[3], "%% = \"All\") %%] "
                    );
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"New Products\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"All\") %%] "
                    );
                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;
        }

        public static string ReturnSolutionsSingleValueConditionalValue(
            string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate

            string conditionalContentBlock = "";

            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";
            string staticHtmlClosingTag1 = "</p>";

            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                    "(%%Categories%% = \"Alternative Energy\") ||",
                    "(%%Categories%% = \"Lighting\") ||",
                    "(%%Categories%% = \"Medical\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Aerospace and Defense eResource%% = \"Solutions\") || ",
                    "(%%Aerospace and Defense eResource%% = \"All\") || ",
                    "(%%Alternative Energy%% = \"Solutions\") || ",
                    "(%%Alternative Energy%% = \"All\") || ",
                    "(%%Lighting%% = \"Solutions\") || ",
                    "(%%Lighting%% = \"All\")) || ",
                    "(%%Medical%% = \"Solutions\")) || ",
                    "(%%Medical%% = \"All\") %%]");
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") %%]"
                    );

                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");

            }
            // only thing left should be the specific demographic
            else
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3], "%% = \"Solutions\") || ",
                    "(%%", newProductArray[3], "%% = \"All\") %%] "
                    );
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Solutions\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"All\") %%] "
                    );

                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;

        }

        public static string ReturnTrainingEventsSingleValueConditionalValue(
            string[] newProductArray)
        {
            // array layout
            // 0 = title
            // 1 = description
            // 2 = link
            // 3 = category
            // 4 = guid
            // 5 = pubdate

            string conditionalContentBlock = "";

            // start by defining the static html values
            string staticHtmlOpeningTag1 = "<p>";
            string staticHtmlOpeningTag2 = "<a href=\"";
            string staticHtmlOpeningTag3 = "<strong>";
            string staticHtmlClosingTag3 = "</strong>";
            string staticHtmlClosingTag2 = "</a>";

            string staticHtmlOpeningTag4 = "<br />";
            string staticHtmlOpeningTag5 = "<a href=\"";
            string staticHtmlClosingTag5 = "</a>";
            string staticHtmlClosingTag1 = "</p>";

            // deal with multiple first or if desciption is null
            if (newProductArray[3].ToLower() == "multiple" ||
                newProductArray[3].ToLower() == "")
            {
                /*
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%Categories%% = \"Aerospace and Defense eResource\") ||",
                    "(%%Categories%% = \"Alternative Energy\") ||",
                    "(%%Categories%% = \"Lighting\") ||",
                    "(%%Categories%% = \"Medical\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Aerospace and Defense eResource%% = \"Training and Events\") || ",
                    "(%%Aerospace and Defense eResource%% = \"All\") || ",
                    "(%%Alternative Energy%% = \"Training and Events\") || ",
                    "(%%Alternative Energy%% = \"All\") || ",
                    "(%%Lighting%% = \"Training and Events\") || ",
                    "(%%Lighting%% = \"All\")) || ",
                    "(%%Medical%% = \"Training and Events\")) || ",
                    "(%%Medical%% = \"All\") %%]");
                */
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") %%]"
                    );

                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");

            }
            // only thing left should be the specific demographic
            else
            {
                // define the conditional logic
                // start with the if block
                // still need to add in the content and close it
                string conditionalClause = string.Concat(
                    "[%% ",
                    "if ",
                    "(%%Categories%% = \"All\") ||",
                    "(%%All Arrow Publications%% = \"on\") ||",
                    "(%%Categories%% = \"", newProductArray[3], "\") ||",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"Training and Events\") || ",
                    "(%%", newProductArray[3].Replace("&","and"), "%% = \"All\") %%] "
                    );


                // build the appropriate section
                // opening p and a tags
                string newsItem = string.Concat(
                    staticHtmlOpeningTag1, staticHtmlOpeningTag2
                    );

                // add in the link
                newsItem += HttpUtility.HtmlDecode(newProductArray[2]);

                // now finish the quotes
                // add the span tag
                // and add in title as the name
                // and complet the <span> and <p> tags
                newsItem = string.Concat(newsItem, "\">",
                    staticHtmlOpeningTag3,
                    newProductArray[0], staticHtmlClosingTag3,
                    staticHtmlClosingTag2
                    );

                // build the next section BR tag
                newsItem += staticHtmlOpeningTag4;

                // fill in the description
                newsItem += newProductArray[1];

                // now close out the p tag
                newsItem += staticHtmlClosingTag1;

                // make sure there are not CR or LF and replace with a <br /> tag

                newsItem = newsItem.Replace("\n", "<br />");

                conditionalContentBlock = string.Concat(
                    conditionalClause, newsItem,
                    "[%% endif %%]");
            }

            return conditionalContentBlock;

        }
    }
}
