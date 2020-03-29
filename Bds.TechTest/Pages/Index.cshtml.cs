using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bds.TechTest.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Bds.TechTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<EngineOption> _engines;

        public IndexModel(IHttpClientFactory httpClientFactory, IOptionsMonitor<AppConfig> optionsMonitor)
        {
            _httpClientFactory = httpClientFactory;
            _engines = optionsMonitor.CurrentValue.Engines;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public SelectList Engines { get; set; }

        public List<string> Results { get; private set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string SelectedEngine { get; set; }

        public async Task OnGet()
        {
            Results.Clear();

            if (!string.IsNullOrEmpty(SearchString))
            {
                var results = await Task.WhenAll(_engines.Select(GetResults));
                foreach (var res in results)
                {
                    Results.AddRange(res);
                }

            }
        }

        private async Task<string[]> GetResults(EngineOption engine)
        {
            var page = 0;
            var client = _httpClientFactory.CreateClient(engine.Name);
            var response = await client.GetAsync(string.Format(engine.SearchPath, SearchString, page * engine.PageMultiplier));
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            var results = doc.DocumentNode.SelectNodes(engine.ResultXPath);

            return results?.Select(r => r?.OuterHtml).ToArray() ?? new string[] { };
        }
    }
}
