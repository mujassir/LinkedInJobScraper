// Config.cs
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LinkedInJobScraper
{
    public static class Config
    {
        private static IConfiguration? _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder().SetBasePath(
                        Directory.GetCurrentDirectory()
                    );

#if DEBUG
                    Console.WriteLine("Loading appsettings.local.json (DEBUG)");
                    builder.AddJsonFile(
                        "appsettings.local.json",
                        optional: false,
                        reloadOnChange: true
                    );
#else
                    Console.WriteLine("Loading appsettings.json (RELEASE)");
                    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
#endif

                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        public static string[] RelevantKeywords =>
            Configuration.GetSection("RelevantKeywords").Get<string[]>();
        public static string Keyword =>
            Configuration["Keyword"]
            ?? throw new InvalidOperationException("Keyword is not configured");
        public static string GeoId =>
            Configuration["GeoId"]
            ?? throw new InvalidOperationException("GeoId is not configured");
        public static string LinkedInSessionCookie =>
            Configuration["LinkedInSessionCookie"]
            ?? throw new InvalidOperationException("LinkedInSessionCookie is not configured");
        public static int MinDelaySeconds =>
            Configuration.GetSection("RandomDelay:MinSeconds").Get<int>();
        public static int MaxDelaySeconds =>
            Configuration.GetSection("RandomDelay:MaxSeconds").Get<int>();
        public static string LinkedInBaseUrl =>
            Configuration["LinkedIn:BaseUrl"]
            ?? throw new InvalidOperationException("LinkedInBaseUrl is not configured");
        public static string LinkedInSearchUrl =>
            Configuration["LinkedIn:SearchUrl"]
            ?? throw new InvalidOperationException("LinkedInSearchUrl is not configured");
        public static int LinkedInDistance =>
            int.Parse(
                Configuration["LinkedIn:QueryParameters:Distance"]
                    ?? throw new InvalidOperationException("LinkedInDistance is not configured")
            );
        public static string LinkedInTimePosted =>
            Configuration["LinkedIn:QueryParameters:TimePosted"]
            ?? throw new InvalidOperationException("LinkedInTimePosted is not configured");
        public static string LinkedInSortBy =>
            Configuration["LinkedIn:QueryParameters:SortBy"]
            ?? throw new InvalidOperationException("LinkedInSortBy is not configured");
        public static string JobCardSelector =>
            Configuration["LinkedIn:Selectors:JobCard"]
            ?? throw new InvalidOperationException("JobCardSelector is not configured");
        public static string JobLinkSelector =>
            Configuration["LinkedIn:Selectors:JobLink"]
            ?? throw new InvalidOperationException("JobLinkSelector is not configured");
        public static string JobTitleSelector =>
            Configuration["LinkedIn:Selectors:JobTitle"]
            ?? throw new InvalidOperationException("JobTitleSelector is not configured");
        public static string JobMetadataSelector =>
            Configuration["LinkedIn:Selectors:JobMetadata"]
            ?? throw new InvalidOperationException("JobMetadataSelector is not configured");
        public static string JobFooterSelector =>
            Configuration["LinkedIn:Selectors:JobFooter"]
            ?? throw new InvalidOperationException("JobFooterSelector is not configured");
        public static string EasyApplyKeyword =>
            Configuration["LinkedIn:EasyApply:Keyword"]
            ?? throw new InvalidOperationException("EasyApplyKeyword is not configured");
        public static string DefaultApplyMode =>
            Configuration["LinkedIn:EasyApply:DefaultApplyMode"]
            ?? throw new InvalidOperationException("DefaultApplyMode is not configured");
        public static bool CheckRelevancy => bool.Parse(Configuration["CheckRelevancy"] ?? "false");
        public static int PageLoadTimeout => 
            int.Parse(Configuration["PageLoadTimeout"] ?? "60000"); // Default to 60 seconds if not configured
        public static bool Headless => 
            bool.Parse(Configuration["Headless"] ?? "true"); // Default to true if not configured
    }
}
