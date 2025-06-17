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
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = Config.Headless });
            var context = await browser.NewContextAsync(new() 
            { 
                ViewportSize = new() { Width = 1920, Height = 1080 },
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
            });
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

            try
            {
                Console.WriteLine($"Navigating to LinkedIn jobs page...");
                var response = await page.GotoAsync(url, new() 
                { 
                    Timeout = Config.PageLoadTimeout
                });
                await page.WaitForTimeoutAsync(5000);
                
                if (response == null)
                {
                    throw new Exception("Failed to load LinkedIn jobs page - no response received");
                }

                // Check for redirect loops or authentication issues
                if (response.Status == 302 || response.Status == 301)
                {
                    throw new Exception("Session cookie appears to be invalid or expired. Please update your LinkedIn session cookie.");
                }

                if (!response.Ok)
                {
                    throw new Exception($"Failed to load LinkedIn jobs page. Status: {response.Status}");
                }

                Console.WriteLine("Waiting for job cards to load...");
                await page.WaitForSelectorAsync(Config.JobCardSelector, new() 
                { 
                    State = WaitForSelectorState.Visible,
                    Timeout = Config.PageLoadTimeout
                });
                
                // Add a small delay to ensure all content is loaded
                await page.WaitForTimeoutAsync(5000);

                var jobCards = await page.QuerySelectorAllAsync(Config.JobCardSelector);
                Console.WriteLine($"Found {jobCards.Count} job cards");

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
                        
                        // Only check relevancy if the flag is enabled
                        if (Config.CheckRelevancy && !_relevantKeywords.Any(k => titleLower.Contains(k))) 
                        {
                            Console.WriteLine($"Skipping job due to relevancy check: {title}");
                            continue;
                        }

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
            }
            catch (TimeoutException)
            {
                Console.WriteLine("[Error] Page load timed out. This could be due to:");
                Console.WriteLine("1. Slow internet connection");
                Console.WriteLine("2. LinkedIn rate limiting");
                Console.WriteLine("3. Invalid session cookie");
                Console.WriteLine("Please try again in a few minutes or update your session cookie.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to scrape LinkedIn jobs: {ex.Message}");
                if (ex.Message.Contains("session cookie"))
                {
                    Console.WriteLine("Please update your LinkedIn session cookie in the configuration file.");
                }
                throw;
            }
            finally
            {
                await browser.CloseAsync();
            }

            return jobList;
        }
    }
} 