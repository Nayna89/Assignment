using System;
using System.Collections.Generic;
using System.Linq;

namespace WebScrapperHiver
{
    public static class StringProcessor
    {
        public static string Html2Text(this string html)
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(@"<html><body>" + html + "</body></html>");

                //emoving script nodes
                var nodesToRemove = doc.DocumentNode.SelectNodes("//script").ToList();
                foreach (var node in nodesToRemove)
                    node.Remove();

                return doc.DocumentNode.SelectSingleNode("//body").InnerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        public static string ToNormalString(this string strText)
        {
            if (String.IsNullOrEmpty(strText)) return String.Empty;
            char chNormalKaf = (char)1603;
            char chNormalYah = (char)1610;
            char chNonNormalKaf = (char)1705;
            char chNonNormalYah = (char)1740;
            string result = strText.Replace(chNonNormalKaf, chNormalKaf);
            result = result.Replace(chNonNormalYah, chNormalYah);
            return result;
        }

        public static List<KeyValuePair<String, Int32>> Process(this String bodyText,
            List<String> blackListWords = null,
            int minimumWordLength = 3,
            char splitor = ' '
            )
        {
            string[] btArray = bodyText.ToNormalString().Split(splitor);
            long numberOfWords = btArray.LongLength;
            Dictionary<String, Int32> wordsDic = new Dictionary<String, Int32>(1);
            foreach (string word in btArray)
            {
                if (word != null)
                {
                    var normalWord = word.Replace(".", "").Replace("(", "").Replace(")", "")
                        .Replace("?", "").Replace("!", "").Replace(",", "")
                        .Replace("<br>", "").Replace(":", "").Replace(";", "")
                        .Replace("<", "").Replace(">", "").Replace("=", "")
                        .Replace("،", "").Replace("-", "").Replace("\n", "").Trim();
                    if ((normalWord.Length > minimumWordLength))
                    {
                        if (wordsDic.ContainsKey(normalWord))
                        {
                            var cnt = wordsDic[normalWord];
                            wordsDic[normalWord] = ++cnt;
                        }
                        else
                        {
                            wordsDic.Add(normalWord, 1);
                        }
                    }
                }
            }
            List<KeyValuePair<String, Int32>> keywords = wordsDic.ToList();
            return keywords;
        }

        public static List<KeyValuePair<String, Int32>> OrderByDescending(this List<KeyValuePair<String, Int32>> list)
        {
            List<KeyValuePair<String, Int32>> result = null;
            result = list.OrderByDescending(q => q.Value).ToList();
            return result;
        }

        public static List<KeyValuePair<String, Int32>> TakeTop(this List<KeyValuePair<String, Int32>> list, Int32 n)
        {
            List<KeyValuePair<String, Int32>> result = list.Take(n).ToList();
            return result;
        }
    }
}
