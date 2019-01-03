using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using System.Xml;
using System.IO;

namespace WebScrapperHiver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Scraping Hiver website started... Please Wait...");
                // new xdoc instance 
                XmlDocument xDoc = new XmlDocument();
                //load up the xml from the location 
                xDoc.Load("https://hiverhq.com/sitemap.xml");

                string path = "SiteHtml.txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                Console.Write("Loading.");
                // cycle through each child node
                foreach (XmlNode node in xDoc.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode locNode in node)
                    {
                        if (locNode.Name == "loc")
                        {
                            GetHtmlDoc(locNode.InnerText, path);
                            Console.Write(".");
                        }
                    }
                }

                FindTop5Words(path);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                Console.WriteLine("Scraping Hiver website ended. Please press enter to end.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception Occured." + ex.Message);
            }
        }

        static void GetHtmlDoc(string uri, string filePath)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                using (var client = new WebClient())
                {
                    var html = client.DownloadString(uri);
                    htmlDocument.LoadHtml(html);
                    System.IO.File.AppendAllText(filePath, html.Html2Text());
                }
            }
            catch
            {
                //Logs
            }
        }

        static void FindTop5Words(string filePath)
        {
            try
            {
                string root = System.IO.File.ReadAllText(filePath);
                List<KeyValuePair<String, Int32>> list = root.ToString().Process().OrderByDescending().TakeTop(5);

                Console.WriteLine("");
                Console.WriteLine("Most commonly used words are : ");

                foreach (KeyValuePair<String, Int32> word in list)
                {
                    Console.WriteLine("\"" + word.Key + "\" : " + word.Value + " times.");
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Exception Occured." + ex.Message);
            }
        }
    }
}
