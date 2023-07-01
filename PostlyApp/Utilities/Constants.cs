namespace PostlyApp.Utilities
{
    class Constants
    {
#if ANDROID || IOS
        public static readonly string API_BASE = "https://10.0.2.2:7128/api";
#else
        public static readonly string API_BASE = "https://localhost:7128/api";
#endif
    }
}
