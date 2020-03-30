using System.Xml.XPath;
using Bds.TechTest.Models;
using HtmlAgilityPack;

namespace Bds.TechTest.Builders
{
    public class ResultBuilder
    {
        public ResultBuilder(ParsingOption parsingOption)
        {
            TitleXPath = XPathExpression.Compile(parsingOption.TitlePath);
            UrlXPath = XPathExpression.Compile(parsingOption.UrlPath);
            UrlViewXPath = XPathExpression.Compile(parsingOption.UrlViewPath);
            ParagraphXPath = XPathExpression.Compile(parsingOption.ParagraphPath);
        }

        public XPathExpression TitleXPath { get; }
        public XPathExpression UrlXPath { get; }
        public XPathExpression UrlViewXPath { get; }
        public XPathExpression ParagraphXPath { get; }

        public SearchResult Create(HtmlNode node)
        {
            return new SearchResult
            {
                Title = node.SelectSingleNode(TitleXPath)?.InnerHtml,
                Url = node.SelectSingleNode(UrlXPath)?.GetAttributeValue("href", null),
                UrlView = node.SelectSingleNode(UrlViewXPath)?.InnerHtml,
                Paragraph = node.SelectSingleNode(ParagraphXPath)?.InnerHtml
            };
        }
    }
}
