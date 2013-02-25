using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Analytics;
using Google.GData.Client;

namespace MvcApplication1.Models
{
    /// <summary>
    /// 
    /// Ta klasa służy do uzyskania połączenia z Google Analytics. Dane dostępowe (token oraz access code) zapisywane są w sesji.
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>

    public class ApiAuth
    {
        public OAuth2Parameters Parameters { get; set; }

        private HttpContextBase context;

        public ApiAuth(HttpContextBase context)
        {
            this.Parameters = GetParameters(context);
            this.context = context;
        }

        /// <summary>
        /// 
        /// Zwraca parametry połączenia z GA.
        /// UWAGA: dane dostępowe do API GA są aktualnie zharcodowane.
        /// 
        /// </summary>
        /// <returns>OAuth2Parameters</returns>

        public static OAuth2Parameters GetParameters(HttpContextBase context)
        {
            OAuth2Parameters parameters = new OAuth2Parameters()
            {
                ClientId = "788104354832-4f8hbomo43ga2mm92htpcj3i35mni3al.apps.googleusercontent.com", // Dane API Google
                ClientSecret = "9fhl9Xa2dM9UbbsAum5ROb9_", // Dane API Google
                // Adres url naszej aplikacji musi być dodany do ustawień naszego Google API https://code.google.com/apis/console
                RedirectUri = "http://localhost:52690/Analytics/Auth", // Adres na który przeikieruje google po udanej autoryzacji
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

        /// <summary>
        /// 
        /// Zapisuje token i access code w sesji.
        /// 
        /// </summary>

        public void SetTokenCode(string code)
        {
            if (code != null)
            {
                context.Session["code"] = code;
                Parameters.AccessCode = code;
            }

            if (context.Session["code"] != null)// && this.HttpContext.Session["token"] == null) // Jeśli posiadamy access code, to pobieramy toekn
            {
                OAuthUtil.GetAccessToken(Parameters); // Pobieranie tokenu
                context.Session["token"] = Parameters.AccessToken; // Zapisanie go w sesji
            }
        }
    }
}