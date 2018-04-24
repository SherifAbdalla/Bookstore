using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bookstore.WebUI.Models;

namespace Bookstore.WebUI.HTMLHelper
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pageinfo, Func<int, string> PageUrl)
        {
            StringBuilder result = new StringBuilder();
            for(int i=0; i<=pageinfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", PageUrl(i));
                tag.InnerHtml = i.ToString();
                if(i == pageinfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                 tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}