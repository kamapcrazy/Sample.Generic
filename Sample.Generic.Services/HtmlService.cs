using System;
using HtmlAgilityPack;
using Sample.Generic.Services.Interfaces;

namespace Sample.Generic.Services
{
    public class HtmlService : IHtmlService
    {
        public string RemoveHtmlTag()
        {
            var input = "This is test text with html tag: <a href=\"http://google.com\">google.com</a>.";

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(input);

            var result = htmlDoc.DocumentNode.InnerText;

            return result;
        }
    }
}
