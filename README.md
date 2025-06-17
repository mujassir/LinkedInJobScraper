# LinkedIn Job Scraper

A .NET-based automated tool that scrapes LinkedIn job listings based on specified keywords and criteria. The tool uses Playwright for web automation and can be configured to run continuously with random delays between searches.

## Features

- Automated LinkedIn job search based on configurable keywords
- Filters jobs based on relevance using customizable keyword matching
- Tracks processed jobs to avoid duplicates
- Configurable search parameters (location, time posted, distance)
- Supports Easy Apply job detection
- Random delay between searches to avoid rate limiting
- Graceful error handling and recovery
- Optional email notifications (configured but disabled by default)

## Prerequisites

- .NET 9.0 SDK or later
- A valid LinkedIn account
- LinkedIn session cookie (li_at)

## Setup

1. Clone the repository:
```bash
git clone [your-repository-url]
cd LinkedInJobScraper
```

2. Install dependencies:
```bash
dotnet restore
```

3. Install Playwright browsers:
```bash
dotnet tool install --global Microsoft.Playwright.CLI
playwright install
```

4. Configure the application:
   - Copy `appsettings.json` and update the following settings:
     - `LinkedInSessionCookie`: Your LinkedIn session cookie (li_at)
     - `RelevantKeywords`: Array of keywords to filter jobs
     - `Keyword`: Main search keyword
     - `GeoId`: LinkedIn location ID
     - Adjust other parameters as needed

## Configuration

The `appsettings.json` file contains all configurable parameters:

```json
{
  "RelevantKeywords": [
    "software engineer", "full stack", "dotnet", ".net", "c#", "backend developer",
    "fullstack", "asp.net", "microsoft developer"
  ],
  "Keyword": "C#, .NET",
  "GeoId": "104305776",
  "LinkedInSessionCookie": "your-session-cookie",
  "RandomDelay": {
    "MinSeconds": 60,
    "MaxSeconds": 200
  },
  "LinkedIn": {
    "BaseUrl": "https://www.linkedin.com",
    "SearchUrl": "https://www.linkedin.com/jobs/search/",
    "QueryParameters": {
      "Distance": "25",
      "TimePosted": "r86400",
      "SortBy": "DD"
    }
  }
}
```

### Key Configuration Parameters

- `RelevantKeywords`: Keywords to filter job listings
- `Keyword`: Main search term for LinkedIn
- `GeoId`: LinkedIn location ID (find this in LinkedIn URL when searching by location)
- `LinkedInSessionCookie`: Your LinkedIn authentication cookie
- `RandomDelay`: Minimum and maximum seconds between searches
- `LinkedIn.QueryParameters`:
  - `Distance`: Search radius in miles
  - `TimePosted`: Time filter (r86400 = last 24 hours)
  - `SortBy`: Sort order (DD = date posted)

## Usage

Run the application:

```bash
dotnet run
```

The program will:
1. Start searching for jobs based on configured parameters
2. Filter results based on relevant keywords
3. Display new job listings in the console
4. Wait for a random interval before the next search
5. Continue running until manually stopped (Ctrl+C)

## Project Structure

- `Program.cs`: Main entry point and program loop
- `LinkedInScraper.cs`: Core scraping functionality
- `ProcessedJobsManager.cs`: Manages job history and deduplication
- `Config.cs`: Configuration management
- `appsettings.json`: Application settings

## Dependencies

- Microsoft.Playwright (1.52.0): Web automation
- MailKit (4.12.1): Email functionality
- MimeKit (4.12.0): Email message handling

## Security Notes

- Never commit your `appsettings.json` with your LinkedIn session cookie
- Keep your session cookie secure and rotate it periodically
- Consider using environment variables for sensitive data

## Contributing

Feel free to submit issues and enhancement requests!

## License

[Your chosen license] 