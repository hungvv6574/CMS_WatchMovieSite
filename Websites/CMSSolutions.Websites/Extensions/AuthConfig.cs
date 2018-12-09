using System.Configuration;
using Microsoft.Web.WebPages.OAuth;

namespace CMSSolutions.Websites.Extensions
{
    public static class AuthConfig
    {
        public static string FacebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
        public static string FacebookAppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];

        public static string GoogleAppId = ConfigurationManager.AppSettings["GoogleAppId"];
        public static string GoogleAppSecret = ConfigurationManager.AppSettings["GoogleAppSecret"];

        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterFacebookClient(FacebookAppId, FacebookAppSecret);
            OAuthWebSecurity.RegisterClient(new GoogleClient(GoogleAppId, GoogleAppSecret), "Google", null);
        }
    }
}