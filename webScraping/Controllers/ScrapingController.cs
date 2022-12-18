using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace webScraping.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScrapingController : Controller
    {
        [HttpGet("webscraping")]
        public List<string> Index()
        {
            string url = "https://www.goc.gov.tr";
            var response = CallUrl(url).Result;
            var linkList = ParseHtml(response);
            return linkList;
        }

        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        private List<string> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var programmerLinks = htmlDoc.DocumentNode.Descendants("ul")
                    .Where(node => node.GetAttributeValue("class", "").Contains("dropdown-menu-right")).ToList();

            List<string> wikiLink = new List<string>();

            foreach (var link in programmerLinks[0].ChildNodes)
            {
                if(link.Name == "li")
                {
                    wikiLink.Add(link.InnerText);
                }
            }

            return wikiLink;

        }
    }
}
