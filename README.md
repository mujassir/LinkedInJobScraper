# LinkedIn Job Scraper

A .NET application that scrapes job listings from LinkedIn based on specified criteria and sends them via email.

## Features

- Scrapes LinkedIn job listings based on keywords and location
- Filters jobs based on relevancy
- Sends job listings via email
- Configurable through environment-based settings
- Supports both headless and visible browser modes
- Handles rate limiting and timeouts gracefully
- Tracks processed jobs to avoid duplicates
- Configurable email notifications

## Configuration

The application uses two configuration files:

1. `appsettings.json` - Default configuration (used in production)
2. `appsettings.local.json` - Local overrides (used in development)

### Environment-based Configuration

The application automatically selects the appropriate configuration file based on the environment:

- In development mode (when `DOTNET_ENVIRONMENT=Development`), it uses `appsettings.local.json`
- In production mode, it uses `appsettings.json`

To set the environment:
```bash
# For development
export DOTNET_ENVIRONMENT=Development

# For production
export DOTNET_ENVIRONMENT=Production
```

### Configuration Settings

```json
{
  "LinkedInSessionCookie": "YOUR_LINKEDIN_SESSION_COOKIE",
  "Keyword": "C#, .NET",
  "GeoId": "104305776",
  "LinkedIn": {
    "BaseUrl": "https://www.linkedin.com",
    "SearchUrl": "https://www.linkedin.com/jobs/search",
    "QueryParameters": {
      "Distance": "25",
      "TimePosted": "r86400",
      "SortBy": "DD"
    },
    "Selectors": {
      "JobCard": ".job-card-container",
      "JobLink": "a.job-card-list__title",
      "JobTitle": ".job-card-list__title",
      "JobMetadata": ".job-card-container__metadata-item",
      "JobFooter": ".job-card-container__footer-item"
    },
    "EasyApply": {
      "Keyword": "Easy Apply",
      "DefaultApplyMode": "Apply on LinkedIn"
    }
  },
  "RandomDelay": {
    "MinSeconds": 2,
    "MaxSeconds": 5
  },
  "CheckRelevancy": true,
  "RelevantKeywords": [
    "C#",
    ".NET",
    "ASP.NET",
    "Core",
    "Developer",
    "Engineer",
    "Software"
  ],
  "PageLoadTimeout": 60000,
  "Headless": true,
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-specific-password",
    "FromAddress": "your-email@gmail.com",
    "ToAddress": "recipient@example.com",
    "Subject": "New LinkedIn Job Listings"
  }
}
```

### Key Configuration Options

#### LinkedIn Settings
- `LinkedInSessionCookie`: Your LinkedIn session cookie (required for authentication)
- `Keyword`: The job search keyword
- `GeoId`: LinkedIn location ID
- `CheckRelevancy`: Enable/disable job relevancy filtering
- `RelevantKeywords`: List of keywords to filter jobs
- `PageLoadTimeout`: Timeout in milliseconds for page loading (default: 60000)
- `Headless`: Run browser in headless mode (default: true)

#### Email Settings
- `SmtpServer`: SMTP server address (e.g., smtp.gmail.com)
- `SmtpPort`: SMTP server port (e.g., 587 for TLS)
- `Username`: Email account username
- `Password`: Email account password or app-specific password
- `FromAddress`: Sender email address
- `ToAddress`: Recipient email address
- `Subject`: Email subject line

## Security Notes

- Never commit your `appsettings.local.json` file to version control
- Keep your LinkedIn session cookie secure
- The application is configured to use `appsettings.local.json` only in development mode
- For Gmail, use an App Password instead of your regular password
- Store sensitive information only in `appsettings.local.json`

## Getting Started

1. Clone the repository
2. Create `appsettings.local.json` based on `appsettings.json`
3. Set your LinkedIn session cookie in `appsettings.local.json`
4. Configure your email settings in `appsettings.local.json`
5. Set the environment to development:
   ```bash
   export DOTNET_ENVIRONMENT=Development
   ```
6. Run the application:
   ```bash
   dotnet run
   ```

## Development Tips

- Set `Headless: false` in `appsettings.local.json` to see the browser in action
- Adjust `PageLoadTimeout` if you experience timeout issues
- Use `CheckRelevancy: false` to see all jobs without filtering
- Monitor the console output for detailed logging
- For Gmail, enable 2FA and generate an App Password for the email configuration

## Troubleshooting

If you encounter issues:

1. Check your LinkedIn session cookie is valid
2. Verify your environment is set correctly
3. Adjust the `PageLoadTimeout` if pages are loading slowly
4. Try running with `Headless: false` to debug browser issues
5. Check the console output for detailed error messages
6. For email issues:
   - Verify SMTP settings
   - Check if using correct port for TLS/SSL
   - Ensure using App Password for Gmail
   - Check spam folder for test emails

## Prerequisites

- .NET 9.0 SDK or later
- A valid LinkedIn account
- For email notifications:
  - Gmail account (or other SMTP server)
  - App Password if using Gmail
  - Valid SMTP server configuration

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

   ### Development Environment
   ```bash
   # Set environment to Development
   export DOTNET_ENVIRONMENT=Development
   
   # Copy appsettings.json to appsettings.local.json
   cp appsettings.json appsettings.local.json
   ```
   
   Update your `appsettings.local.json`:
   ```json
   {
     "LinkedInSessionCookie": "YOUR_ACTUAL_LINKEDIN_SESSION_COOKIE"
   }
   ```

   ### Production Environment
   ```bash
   # Set environment to Production (default)
   export DOTNET_ENVIRONMENT=Production
   ```
   
   Update `appsettings.json` with your production settings.

### Getting Your LinkedIn Session Cookie

1. Log in to LinkedIn in your web browser
2. Open Developer Tools (F12 or right-click -> Inspect)
3. Go to the "Application" tab
4. Under "Storage" -> "Cookies" -> "https://www.linkedin.com"
5. Find the cookie named "li_at" and copy its value
6. Add the cookie value to your configuration file based on environment

⚠️ **Important Security Notes:**
- Replace the placeholder cookie value with your actual LinkedIn session cookie
- Never commit your `appsettings.local.json` to version control
- The repository includes `appsettings.json` with a placeholder cookie value
- Consider using environment variables or a secure secret management system for production
- Rotate your session cookie periodically for security

## Configuration

The application uses different configuration files based on the environment:

- Development Environment (`DOTNET_ENVIRONMENT=Development`):
  - Uses `appsettings.local.json` (gitignored)
  - Contains development-specific settings and sensitive data

- Production Environment (`DOTNET_ENVIRONMENT=Production`):
  - Uses `appsettings.json` (committed)
  - Contains production settings

Here's the configuration structure:

```json
{
  "RelevantKeywords": [
    "software engineer", "full stack", "dotnet", ".net", "c#", "backend developer",
    "fullstack", "asp.net", "microsoft developer"
  ],
  "Keyword": "C#, .NET",
  "GeoId": "104305776",
  "LinkedInSessionCookie": "YOUR_LINKEDIN_SESSION_COOKIE_HERE",
  "CheckRelevancy": true,
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
- `CheckRelevancy`: Enable/disable keyword-based filtering (true by default)
- `RandomDelay`: Minimum and maximum seconds between searches
- `LinkedIn.QueryParameters`:
  - `Distance`: Search radius in miles
  - `TimePosted`: Time filter (r86400 = last 24 hours)
  - `SortBy`: Sort order (DD = date posted)

## Usage

Run the application:

```bash
# Development environment
export DOTNET_ENVIRONMENT=Development
dotnet run

# Production environment
export DOTNET_ENVIRONMENT=Production
dotnet run
```

The program will:
1. Start searching for jobs based on configured parameters
2. Filter results based on relevant keywords (if CheckRelevancy is true)
3. Display new job listings in the console
4. Wait for a random interval before the next search
5. Continue running until manually stopped (Ctrl+C)

## Project Structure

- `Program.cs`: Main entry point and program loop
- `LinkedInScraper.cs`: Core scraping functionality
- `ProcessedJobsManager.cs`: Manages job history and deduplication
- `Config.cs`: Configuration management
- `appsettings.json`: Production configuration
- `appsettings.local.json`: Development configuration (gitignored)

## Dependencies

- Microsoft.Playwright (1.52.0): Web automation
- MailKit (4.12.1): Email functionality
- MimeKit (4.12.0): Email message handling

## Security Notes

- Replace the placeholder cookie value with your actual LinkedIn session cookie
- Never commit your `appsettings.local.json` to version control
- Keep your session cookie secure and rotate it periodically
- Consider using environment variables for sensitive data
- The repository includes default settings in `appsettings.json` with placeholder values
- For production environments, use a secure secret management system

## Contributing

Feel free to submit issues and enhancement requests!

## License

[Your chosen license] 