using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bds.TechTest.Builders;
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

        public IndexModel(IHttpClientFactory httpClientFactory, IOptions<AppConfig> optionsMonitor)
        {
            _httpClientFactory = httpClientFactory;
            _engines = optionsMonitor.Value.Engines;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        public SelectList Engines { get; set; }

        public List<SearchResult> Results { get; private set; } = new List<SearchResult>();

        [BindProperty(SupportsGet = true)]
        public string SelectedEngine { get; set; }

        public async Task OnGet()
        {
            Results.Clear();

            Engines = new SelectList(_engines.Select(e => e.Name));

            if (!string.IsNullOrEmpty(SearchString))
            {
                var activeEngines =
                    string.IsNullOrEmpty(SelectedEngine)
                    ? _engines
                    : _engines.Where(e => e.Name == SelectedEngine).ToList();

                var results = await Task.WhenAll(activeEngines.Select(GetResults));
                foreach (var res in results)
                {
                    Results.AddRange(res);
                }

            }
        }

        private async Task<SearchResult[]> GetResults(EngineOption engine)
        {
            var client = _httpClientFactory.CreateClient(engine.Name);
            var response = await client.GetAsync(string.Format(engine.SearchPath, SearchString, PageNumber * engine.PageMultiplier));
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            var results = doc.DocumentNode.SelectNodes(engine.ResultPath);
            var builder = new ResultBuilder(engine.ParsingOption);

            return results?.Select(builder.Create).ToArray() ?? new SearchResult[] { };
        }
    }
}
