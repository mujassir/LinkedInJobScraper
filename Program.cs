using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using MimeKit;

namespace LinkedInJobScraper
{
    class Program
    {
        private static readonly ProcessedJobsManager _jobsManager = new ProcessedJobsManager();
        private static readonly LinkedInScraper _scraper;

        static Program()
        {
            _scraper = new LinkedInScraper(_jobsManager, Config.RelevantKeywords);
        }

        static async Task Main(string[] args)
        {
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    await RunLinkedInScraper();

                    // Random interval between MinDelaySeconds and MaxDelaySeconds
                    var delaySeconds = new Random().Next(Config.MinDelaySeconds, Config.MaxDelaySeconds);
                    Console.WriteLine($"\nSleeping for {delaySeconds} seconds...\n");
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cts.Token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Program stopped by user.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(60), cts.Token); // wait 1 min on error
                }
            }
        }

        static async Task RunLinkedInScraper()
        {
            var jobList = await _scraper.ScrapeJobs();

            if (jobList.Count == 0)
            {
                Console.WriteLine("No new jobs found matching the keyword criteria.");
                return;
            }

            // Print results
            Console.WriteLine($"Found {jobList.Count} new relevant job(s):\n");
            foreach (var job in jobList)
            {
                Console.WriteLine($"Title     : {job.Title}");
                Console.WriteLine($"Link      : {job.Link}");
                Console.WriteLine($"Location  : {job.Location}");
                Console.WriteLine($"ApplyMode : {job.ApplyMode}");
                Console.WriteLine(new string('-', 60));
            }

            // Optional: Send email
            // await SendEmail(jobList);
        }
    }
}
