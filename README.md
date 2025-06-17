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
   - Copy `appsettings.json` to `appsettings.local.json`:
     ```bash
     cp appsettings.json appsettings.local.json
     ```
   - Replace the placeholder in `appsettings.local.json` with your actual LinkedIn session cookie:
     ```json
     {
       "LinkedInSessionCookie": "YOUR_ACTUAL_LINKEDIN_SESSION_COOKIE"
     }
     ```
   - Optionally update other settings in your `appsettings.local.json`:
     - `RelevantKeywords`: Array of keywords to filter jobs
     - `Keyword`: Main search keyword
     - `GeoId`: LinkedIn location ID
     - Adjust other parameters as needed

### Getting Your LinkedIn Session Cookie

1. Log in to LinkedIn in your web browser
2. Open Developer Tools (F12 or right-click -> Inspect)
3. Go to the "Application" tab
4. Under "Storage" -> "Cookies" -> "https://www.linkedin.com"
5. Find the cookie named "li_at" and copy its value
6. Replace "YOUR_LINKEDIN_SESSION_COOKIE_HERE" in your `appsettings.local.json` with your actual cookie value

⚠️ **Important Security Notes:**
- Replace the placeholder cookie value in `appsettings.local.json` with your actual LinkedIn session cookie
- Never commit your `appsettings.local.json` to version control
- The repository includes `appsettings.json` with a placeholder cookie value
- Consider using environment variables or a secure secret management system for production
- Rotate your session cookie periodically for security

## Configuration

The application uses two configuration files:
- `appsettings.json`: Default configuration (committed to repository, contains placeholder values)
- `appsettings.local.json`: Local overrides (gitignored, contains actual values)

Values in `appsettings.local.json` will override those in `appsettings.json`. Here's the structure:

```json
{
  "RelevantKeywords": [
    "software engineer", "full stack", "dotnet", ".net", "c#", "backend developer",
    "fullstack", "asp.net", "microsoft developer"
  ],
  "Keyword": "C#, .NET",
  "GeoId": "104305776",
  "LinkedInSessionCookie": "YOUR_LINKEDIN_SESSION_COOKIE_HERE",
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
- `LinkedInSessionCookie`: Your LinkedIn authentication cookie (replace placeholder in local config)
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
- `appsettings.json`: Default configuration (committed, contains placeholder values)
- `appsettings.local.json`: Local configuration overrides (gitignored, contains actual values)

## Dependencies

- Microsoft.Playwright (1.52.0): Web automation
- MailKit (4.12.1): Email functionality
- MimeKit (4.12.0): Email message handling

## Security Notes

- The LinkedIn session cookie should ONLY be stored in `appsettings.local.json`
- Never commit your `appsettings.local.json` to version control
- Keep your session cookie secure and rotate it periodically
- Consider using environment variables for sensitive data
- The repository includes default settings in `appsettings.json` (no sensitive data)
- For production environments, use a secure secret management system

## Contributing

Feel free to submit issues and enhancement requests!

## License

[Your chosen license] 