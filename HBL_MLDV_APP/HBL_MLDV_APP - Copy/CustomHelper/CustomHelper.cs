using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Alfonick.CustomHelper
{
    public static class CustomHelper
    {
        public static IHtmlString File(this HtmlHelper helper, string id)
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("id", id);
            return new MvcHtmlString(tb.ToString());
        }

        public static IHtmlString GoogleCaptcha(this HtmlHelper helper)
        {
            const string publicSiteKey = SiteSettings.GoogleRecaptchaSiteKey;

            var mvcHtmlString = new TagBuilder("div")
            {
                Attributes =
            {
                new KeyValuePair<string, string>("class", "g-recaptcha"),
                new KeyValuePair<string, string>("data-sitekey", publicSiteKey)
            }
            };

            const string googleCaptchaScript = "<script src='https://www.google.com/recaptcha/api.js'></script>";
            var renderedCaptcha = mvcHtmlString.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create($"{googleCaptchaScript}{renderedCaptcha}");
        }


    }
}