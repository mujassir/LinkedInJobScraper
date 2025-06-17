using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkedInJobScraper
{
    public class ProcessedJobsManager
    {
        private readonly Dictionary<string, DateTime> _processedJobs = new Dictionary<string, DateTime>();
        private readonly TimeSpan _jobRetentionPeriod = TimeSpan.FromHours(1);

        public bool IsJobProcessed(string jobUrl)
        {
            CleanupOldJobs();
            return _processedJobs.ContainsKey(jobUrl);
        }

        public void AddProcessedJob(string jobUrl)
        {
            _processedJobs[jobUrl] = DateTime.UtcNow;
        }

        private void CleanupOldJobs()
        {
            var now = DateTime.UtcNow;
            var oldJobs = _processedJobs
                .Where(kvp => (now - kvp.Value) >= _jobRetentionPeriod)
                .ToList();

            foreach (var job in oldJobs)
            {
                _processedJobs.Remove(job.Key);
            }
        }
    }
} 