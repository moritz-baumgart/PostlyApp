namespace PostlyApp.Services.Impl
{
    public class HttpsClientHandlerService
    {
        /// <summary>
        /// Createa a <see cref="HttpMessageHandler"/> with platform specific code for ignoring invalid ssl certs. Used for dev certs.
        /// </summary>
        /// <returns>The created <see cref="HttpMessageHandler"/> or null if not on the right plattform.</returns>
        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
#elif IOS
            var handler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl = IsHttpsLocalhost
            };
            return handler;
#else
            return null;
#endif
        }

#if IOS
        public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
        {
            if (url.StartsWith("https://localhost"))
                return true;
            return false;
        }
#endif
    }
}
