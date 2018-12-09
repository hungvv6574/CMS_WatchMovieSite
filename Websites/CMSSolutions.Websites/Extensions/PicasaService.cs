using System.Collections.Generic;
using System.Xml;
using CMSSolutions.Websites.Entities;

namespace CMSSolutions.Websites.Extensions
{
    public class PicasaService
    {
        public List<PicasaInfo> ParseRssFile(string albumLink,string currentLink)
        {
            var rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load(albumLink);
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");
            var list = new List<PicasaInfo>();
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("link");
                if (rssSubNode == null)
                {
                    continue;
                }

                string link = rssSubNode.InnerText;
                if (link != currentLink)
                {
                    continue;
                }

                rssSubNode = rssNode.LastChild;
                XmlNodeList listUrl = rssSubNode.ChildNodes;
                foreach (XmlNode item in listUrl)
                {
                    if (item == null || item.Attributes == null || item.Attributes.Count <= 0)
                    {
                        continue;
                    }

                    if (item.Attributes["medium"] == null)
                    {
                        continue;
                    }

                    string medium = item.Attributes["medium"].Value;
                    if (!string.IsNullOrEmpty(medium) && medium == "video")
                    {
                        var info = new PicasaInfo();
                        info.Medium = medium;
                        string url = item.Attributes["url"].Value;
                        info.Url = url.Replace("&amp;", "&");
                        string width = item.Attributes["width"].Value;
                        info.Width = int.Parse(width);
                        string height = item.Attributes["height"].Value;
                        info.Height = int.Parse(height);
                        string type = item.Attributes["type"].Value;
                        info.Type = type;

                        list.Add(info);
                    }
                }
            }

            return list;
        }
    }
}