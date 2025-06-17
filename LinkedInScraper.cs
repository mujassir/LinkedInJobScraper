using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace LinkedInJobScraper
{
    public class LinkedInScraper
    {
        private readonly ProcessedJobsManager _jobsManager;
        private readonly string[] _relevantKeywords;

        public LinkedInScraper(ProcessedJobsManager jobsManager, string[] relevantKeywords)
        {
            _jobsManager = jobsManager;
            _relevantKeywords = relevantKeywords;
        }

        public async Task<List<(string Title, string Link, string Location, string ApplyMode)>> ScrapeJobs()
        {
            var jobList = new List<(string Title, string Link, string Location, string ApplyMode)>();

            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // LinkedIn session cookie
            await context.AddCookiesAsync(new[]
            {
                new Cookie
                {
                    Name = "li_at",
                    Value = Config.LinkedInSessionCookie,
                    Domain = ".linkedin.com",
                    Path = "/",
                    HttpOnly = true,
                    Secure = true
                }
            });

            // Construct search URL
            string keyword = Config.Keyword;
            string encodedKeyword = Uri.EscapeDataString(keyword);
            string geoId = Config.GeoId;
            string url = $"{Config.LinkedInSearchUrl}?" +
                          $"distance={Config.LinkedInDistance}&f_TPR={Config.LinkedInTimePosted}&geoId={geoId}" +
                          $"&keywords={encodedKeyword}" +
                          $"&origin=JOB_SEARCH_PAGE_SEARCH_BUTTON&refresh=true&sortBy={Config.LinkedInSortBy}";

            await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.DOMContentLoaded, Timeout = 60000 });
            await page.WaitForTimeoutAsync(2500);

            var jobCards = await page.QuerySelectorAllAsync(Config.JobCardSelector);

            foreach (var job in jobCards)
            {
                try
                {
                    var anchor = await job.QuerySelectorAsync(Config.JobLinkSelector);
                    if (anchor == null) continue;

                    var titleNode = await anchor.QuerySelectorAsync(Config.JobTitleSelector);
                    if (titleNode == null) continue;

                    string title = await titleNode.InnerTextAsync() ?? "";
                    string href = await anchor.GetAttributeAsync("href") ?? "";

                    var titleLower = title.ToLower();
                    if (!_relevantKeywords.Any(k => titleLower.Contains(k))) continue;

                    string fullLink = Config.LinkedInBaseUrl + href;

                    // Skip if job has been processed in the last hour
                    if (_jobsManager.IsJobProcessed(fullLink))
                    {
                        Console.WriteLine($"Skipping recently processed job: {title}");
                        continue;
                    }

                    var location = "Unknown";
                    var metadata = await job.QuerySelectorAsync(Config.JobMetadataSelector);
                    if (metadata != null)
                        location = (await metadata.InnerTextAsync())?.Trim() ?? "Unknown";

                    var applyMode = Config.DefaultApplyMode;
                    var footer = await job.QuerySelectorAsync(Config.JobFooterSelector);
                    if (footer != null)
                    {
                        var applyText = await footer.InnerTextAsync();
                        if (!string.IsNullOrWhiteSpace(applyText) && applyText.Contains(Config.EasyApplyKeyword, StringComparison.OrdinalIgnoreCase))
                            applyMode = Config.EasyApplyKeyword;
                    }

                    jobList.Add((title.Trim(), fullLink, location, applyMode));
                    _jobsManager.AddProcessedJob(fullLink);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Warning] Error while parsing a job card: {ex.Message}");
                    continue;
                }
            }

            return jobList;
        }
    }
} 