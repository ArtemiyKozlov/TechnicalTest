using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bds.TechTest.Builders;
using Bds.TechTest.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Bds.TechTest.Tests.Builders
{
    [TestFixture]
    public class ResultBuilderTest
    {
        private static readonly List<EngineOption> _engines;

        private int SEARCH_RESULTS_COUNT = 10;

        static ResultBuilderTest()
        {
            var collection = new ServiceCollection();
            collection.AddOptions();


            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            collection.Configure<AppConfig>(config);

            var services = collection.BuildServiceProvider();

            var options = services.GetService<IOptions<AppConfig>>();
            _engines = options.Value.Engines;
        }

        public static IEnumerable  TestCases
        {
            get
            {
                foreach (var engine in _engines)
                {
                    yield return new TestCaseData(engine).SetName(engine.Name);
                }
            }
        }


        [TestCaseSource(typeof(ResultBuilderTest), "TestCases")]
        public void ShouldBuild(EngineOption engine)
        {
            var resultNodes = GetResultNodes(engine);

            var builder = new ResultBuilder(engine);
            var result = resultNodes.Select(builder.Create).Distinct().ToArray();

            Assert.AreEqual(SEARCH_RESULTS_COUNT, result.Length);
            foreach (var item in result)
            {
                Assert.IsNotNull(item.Title);
                Assert.IsNotNull(item.Paragraph);
                Assert.IsNotNull(item.Url);
                Assert.IsNotNull(item.UrlView);
            }
        }


        private HtmlNodeCollection GetResultNodes(EngineOption engine)
        {
            var path = $"./TestData/{engine.Name}.htm";

            var doc = new HtmlDocument();
            doc.Load(path);

            return doc.DocumentNode.SelectNodes(engine.ResultPath);
        }
    }
}
