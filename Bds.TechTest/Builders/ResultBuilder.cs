using System.Xml.XPath;
using Bds.TechTest.Models;
using HtmlAgilityPack;

namespace Bds.TechTest.Builders
{
    public class ResultBuilder
    {
        public ResultBuilder(EngineOption engine)
        {
            var parsingOption = engine.ParsingOption;

            EngineName = engine.Name;
            TitleXPath = XPathExpression.Compile(parsingOption.TitlePath);
            UrlXPath = XPathExpression.Compile(parsingOption.UrlPath);
            UrlViewXPath = XPathExpression.Compile(parsingOption.UrlViewPath);
            ParagraphXPath = XPathExpression.Compile(parsingOption.ParagraphPath);
        }

        private XPathExpression TitleXPath { get; }
        private XPathExpression UrlXPath { get; }
        private XPathExpression UrlViewXPath { get; }
        private XPathExpression ParagraphXPath { get; }
        private string EngineName { get; }

        public SearchResult Create(HtmlNode node)
        {
            if (node == null) return null;

            return new SearchResult
            {
                Title = node.SelectSingleNode(TitleXPath)?.InnerHtml,
                Url = node.SelectSingleNode(UrlXPath)?.GetAttributeValue("href", null),
                UrlView = node.SelectSingleNode(UrlViewXPath)?.InnerHtml,
                Paragraph = node.SelectSingleNode(ParagraphXPath)?.InnerHtml,
                EngineName = EngineName
            };
        }
    }
}
