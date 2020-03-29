using System.Diagnostics;

namespace Bds.TechTest.Models
{
    [DebuggerDisplay("{Name}")]
    public class EngineOption
    {
        public string Name { get; set; }
        public string UrlBase { get; set; }
        public string SearchPath { get; set; }
        public int PageMultiplier { get; set; }
        public string ResultXPath { get; set; }
    }
}
