namespace PostlyApp.Utilities
{
    class Constants
    {
#if ANDROID || IOS
        /// <summary>
        /// The api base url/path when on android/ios.
        /// </summary>
        public static readonly string API_BASE = "https://10.0.2.2:7128/api";
#else
        /// <summary>
        /// The api base url/path when on windows/mac.
        /// </summary>
        public static readonly string API_BASE = "https://localhost:7128/api";
#endif
    }
}
