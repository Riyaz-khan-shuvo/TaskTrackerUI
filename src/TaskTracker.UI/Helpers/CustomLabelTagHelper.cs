using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;

namespace TaskTracker.UI.Helpers
{
    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class CustomLabelTagHelper : TagHelper
    {
        public override int Order => 1;

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (For == null) return;

            output.Content.Clear();

            var name = For.Name.Split('.').Last();
            if (name.EndsWith("Id"))
            {
                name = name.Substring(0, name.Length - 2) + "Name";
            }
            var readable = Regex.Replace(name, "(\\B[A-Z])", " $1");

            output.Content.SetContent(readable);
        }
    }
}