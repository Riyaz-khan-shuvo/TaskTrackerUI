using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskTracker.UI.Helpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class RequiredLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var metadata = For.Metadata;
            var modelType = For.ModelExplorer.Container.ModelType;
            var propertyName = metadata.PropertyName;

            // Find the actual property in the model using reflection
            var propInfo = modelType?.GetProperty(propertyName ?? "");
            bool isExplicitlyRequired = propInfo?.GetCustomAttribute<RequiredAttribute>() != null;

            var labelText = metadata.DisplayName ?? metadata.PropertyName ?? string.Empty;

            if (isExplicitlyRequired)
            {
                labelText += " <span class='text-danger'>*</span>";
            }

            output.Content.SetHtmlContent(labelText);
        }
    }
}
