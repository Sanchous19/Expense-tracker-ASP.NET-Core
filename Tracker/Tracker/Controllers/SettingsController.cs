using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;


namespace Tracker.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult SelectLanguage(string culture, string returnUrl)
        {
            //string s = context.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            //Response.Cookies.Append(".AspNetCore.Culture", "c=" + culture + "|uic=" + culture);
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            //var p = HttpContext.Response.Cookies;
            //var pathBase = HttpContext.Request.Cookies.Keys;
            //var cul = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            return LocalRedirect(returnUrl);
        }
    }
}