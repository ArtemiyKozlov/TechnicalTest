using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bds.TechTest.Builders;
using Bds.TechTest.Extensions;
using Bds.TechTest.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Bds.TechTest.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<EngineOption> _engines;

        public IndexModel(IHttpClientFactory httpClientFactory, IOptions<AppConfig> engineOptions)
        {
            _httpClientFactory = httpClientFactory;
            _engines = engineOptions.Value.Engines;
        }

        [BindProperty]
        public List<EngineOption> Engines { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        public HashSet<SearchResult> SearchResults { get; private set; } = new HashSet<SearchResult>();

        public async Task OnPost()
        {
            SearchResults.Clear();
            if (!string.IsNullOrEmpty(SearchString))
            {
                var results = await Task.WhenAll(Engines
                    .Where(e => e.IsSelected)
                    .Select(e => _engines.First(o => o.Name == e.Name))
                    .Select(GetResults));
                SearchResults = results.Length == 1
                    ? new HashSet<SearchResult>(results[0])
                    : new HashSet<SearchResult>(results.MergeOneByOne().Where(r => r.Title != null));
            }
        }

        public void OnGet()
        {
            Engines = _engines;
        }

        private async Task<IEnumerable<SearchResult>> GetResults(EngineOption engine)
        {
            var client = _httpClientFactory.CreateClient(engine.Name);
            var response = await client.GetAsync(string.Format(engine.SearchPath, SearchString, PageNumber * engine.PageMultiplier));
            var doc = new HtmlDocument();
            doc.LoadHtml(await response.Content.ReadAsStringAsync());
            var results = doc.DocumentNode.SelectNodes(engine.ResultPath);
            var builder = new ResultBuilder(engine);

            return results?.Select(builder.Create) ?? new SearchResult[] { };
        }
    }
}
