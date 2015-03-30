using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ArrowElectronics
{
    class Rss
    {
        public static void GetRssElements(string url,
            DateTime date, List<string[]> elementItems)
        {

            Console.WriteLine(url);
            
            // load the RSS into an XDocument
            XDocument rss = XDocument.Load(url);

            var getItems = from item in rss.Descendants("item")
                           //where (string)item.Element("pubDate"). !=
                           //"Fri, 11 Mar 2011 16:37:57 GMT"
                           //where item.Element("category").

                           select new
                           {
                               title = item.Element("title").Value,
                               description = (string)item.Element("description") ?? "",
                               link = item.Element("link").Value,
                               category = (string) item.Element("category") ?? "",
                               guid = item.Element("guid").Value,
                               pubdate = item.Element("pubDate").Value,
                               


                           };

            var newQuery = from items in getItems
                              where (Convert.ToDateTime(items.pubdate)
                              > date)
                              select items;

            foreach(var item in newQuery)
            {
                
                
                // create the array
                string[] elements = {
                   item.title,item.description,item.link,
                   item.category,item.guid,item.pubdate
                                    };
                
                // add it to the list
                elementItems.Add(elements);
            }

            
        }
    }
}
