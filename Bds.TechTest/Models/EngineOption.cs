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
        public string ResultPath { get; set; }
        public ParsingOption ParsingOption { get; set; }
        public bool Selected { get; set; }
    }
}
