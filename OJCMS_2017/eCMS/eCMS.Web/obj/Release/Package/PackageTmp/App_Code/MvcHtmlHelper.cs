using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace eCMS.Web
{
    public static class MvcHtmlHelper
    {
        //public static MvcHtmlString LabelRequiredFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        //{
        //    return LabelRequiredFor(html, expression, new RouteValueDictionary(htmlAttributes));
        //}
        public static MvcHtmlString LabelRequiredFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            //tag.MergeAttributes(htmlAttributes);
            tag.AddCssClass("control-label");
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            TagBuilder span = new TagBuilder("span");
            span.AddCssClass("required");
            span.SetInnerText("*");

            // assign <span> to <label> inner html
            tag.InnerHtml = labelText + span.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString CustomValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            //TagBuilder containerDivBuilder = new TagBuilder("div");
            //containerDivBuilder.AddCssClass("field-error-box");

            //TagBuilder topDivBuilder = new TagBuilder("div");
            //topDivBuilder.AddCssClass("top");

            //TagBuilder midDivBuilder = new TagBuilder("div");
            //midDivBuilder.AddCssClass("mid");
            //midDivBuilder.InnerHtml = helper.ValidationMessageFor(expression).ToString();

            //containerDivBuilder.InnerHtml += topDivBuilder.ToString(TagRenderMode.Normal);
            //containerDivBuilder.InnerHtml += midDivBuilder.ToString(TagRenderMode.Normal);
            //<span class=\"field-validation-error\" data-valmsg-for=\"Name\" data-valmsg-replace=\"true\">Name is required</span>
            string html = helper.ValidationMessageFor(expression).ToString();
            html = html.Replace("field-validation-error", "help-inline required");
            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString Image(this HtmlHelper html, string imgFileName, UrlHelper url)
        {
            TagBuilder tag = new TagBuilder("img");
            tag.Attributes.Add("src", url.Content("~/Images/" + imgFileName));
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}