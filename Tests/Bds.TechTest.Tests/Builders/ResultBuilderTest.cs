using System;
using System.Collections;
using System.Collections.Generic;
using Bds.TechTest.Models;
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
        public void Test1(EngineOption engine)
        {
            Console.WriteLine(engine.Name);
        }
    }
}
