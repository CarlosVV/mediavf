using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace MediaVF.Services.Polling.Content
{
    [ContentType("text/html")]
    public class HtmlContent : IContent<HtmlDocument>
    {
        public HtmlDocument Content { get; set; }

        public bool IsLoaded { get; set; }

        public void Load(object content)
        {
            Content = new HtmlDocument();
            Content.LoadHtml(content as string);
        }
    }
}
