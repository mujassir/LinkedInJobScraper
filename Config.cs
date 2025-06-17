// Config.cs
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LinkedInJobScraper
{
    public static class Config
    {
        private static IConfiguration _configuration;

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
                }
                return _configuration;
            }
        }

        public static string[] RelevantKeywords => Configuration.GetSection("RelevantKeywords").Get<string[]>();
        public static string Keyword => Configuration["Keyword"];
        public static string GeoId => Configuration["GeoId"];
        public static string LinkedInSessionCookie => Configuration["LinkedInSessionCookie"];
        public static int MinDelaySeconds => Configuration.GetSection("RandomDelay:MinSeconds").Get<int>();
        public static int MaxDelaySeconds => Configuration.GetSection("RandomDelay:MaxSeconds").Get<int>();
        public static string LinkedInBaseUrl => Configuration["LinkedIn:BaseUrl"];
        public static string LinkedInSearchUrl => Configuration["LinkedIn:SearchUrl"];
        public static string LinkedInDistance => Configuration["LinkedIn:QueryParameters:Distance"];
        public static string LinkedInTimePosted => Configuration["LinkedIn:QueryParameters:TimePosted"];
        public static string LinkedInSortBy => Configuration["LinkedIn:QueryParameters:SortBy"];
        public static string JobCardSelector => Configuration["LinkedIn:Selectors:JobCard"];
        public static string JobLinkSelector => Configuration["LinkedIn:Selectors:JobLink"];
        public static string JobTitleSelector => Configuration["LinkedIn:Selectors:JobTitle"];
        public static string JobMetadataSelector => Configuration["LinkedIn:Selectors:JobMetadata"];
        public static string JobFooterSelector => Configuration["LinkedIn:Selectors:JobFooter"];
        public static string EasyApplyKeyword => Configuration["LinkedIn:EasyApply:Keyword"];
        public static string DefaultApplyMode => Configuration["LinkedIn:EasyApply:DefaultApplyMode"];
    }
} 