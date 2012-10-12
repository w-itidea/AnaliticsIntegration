using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Analytics;
using Google.GData.Client;

namespace MvcApplication1.Models
{
    // Summary: Oauth2 athentication helper

    public class ApiAuth
    {
        public OAuth2Parameters Parameters { get; set; }

        private HttpContextBase context;

        public ApiAuth(HttpContextBase context)
        {
            this.Parameters = GetParameters(context);
            this.context = context;
        }

        public static OAuth2Parameters GetParameters(HttpContextBase context)
        {
            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "788104354832-4f8hbomo43ga2mm92htpcj3i35mni3al.apps.googleusercontent.com",
                ClientSecret = "9fhl9Xa2dM9UbbsAum5ROb9_",
                RedirectUri = "http://localhost:52690/Analytics/Auth",
                Scope = "https://www.googleapis.com/auth/analytics.readonly"
            };
            if (context.Session["code"] != null)
            {
                parameters.AccessCode = context.Session["code"].ToString();
            }
            if (context.Session["token"] != null)
            {
                parameters.AccessToken = context.Session["token"].ToString();
            }
            return parameters;
        }

        public void SetTokenCode(string code)
        {
            if (code != null)
            {
                context.Session["code"] = code;
                Parameters.AccessCode = code;
            }

            if (context.Session["code"] != null)// && this.HttpContext.Session["token"] == null)
            {
                OAuthUtil.GetAccessToken(Parameters);
                context.Session["token"] = Parameters.AccessToken;
            }
        }
    }
}