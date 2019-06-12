using PAIS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace PAIS.Infrastructure
{
    public class SortHeaderTagHelper : TagHelper
    {
        public SortEnum Property { get; set; }
        public SortEnum Current { get; set; }
        public string Action { get; set; }
        public bool Up { get; set; }

        private IUrlHelperFactory urlHelperFactory;
        public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            PageUrlValues["sortOrder"] = Property;
            string url = urlHelper.Action(Action, PageUrlValues);
            output.Attributes.SetAttribute("href", url);

            switch (Current)
            {
                case SortEnum.NAME_DESC:
                    Current = SortEnum.NAME_ASC;
                    break;
                case SortEnum.NAME_ASC:
                    Current = SortEnum.NAME_DESC;
                    break;
                case SortEnum.YEAR_ASC:
                    Current = SortEnum.YEAR_DESC;
                    break;
                case SortEnum.YEAR_DESC:
                    Current = SortEnum.YEAR_ASC;
                    break;
                case SortEnum.RATE_ASC:
                    Current = SortEnum.RATE_DESC;
                    break;
                case SortEnum.RATE_DESC:
                    Current = SortEnum.RATE_ASC;
                    break;
                case SortEnum.PRICE_ASC:
                    Current = SortEnum.PRICE_DESC;
                    break;
                case SortEnum.PRICE_DESC:
                    Current = SortEnum.PRICE_ASC;
                    break;
            }

            if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");

                if (Up == true)
                    tag.AddCssClass("fas fa-chevron-up");
                else
                    tag.AddCssClass("fas fa-chevron-down");
                output.PreContent.AppendHtml(tag);
            }
        }
    }
}