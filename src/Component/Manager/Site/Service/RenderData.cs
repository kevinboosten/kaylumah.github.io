using System.Collections.Generic;

namespace Kaylumah.Ssg.Manager.Site.Service
{
    class RenderData
    {
        public string Content { get;set; }
        public BuildData Build { get;set; }
        public SiteData Site { get;set; } = new SiteData();
        public PageData Page { get;set; } = new PageData();
    }

    class BuildData
    {
        public string Copyright { get;set; }
        public string SourceBaseUri { get;set; }
        public string GitHash { get;set; }
        public string ShortGitHash { get;set; }
    }

    class SiteData
    {
        public string Title { get;set; } = $"{nameof(SiteData)}{nameof(Title)}";
        public Dictionary<string, object> Data { get;set; }
    }

    class PageData
    {
        public string Title { get;set; } = $"{nameof(PageData)}{nameof(Title)}";
    }
}