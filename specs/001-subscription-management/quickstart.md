# Quickstart: MVP Subscription Management

Use this guide to validate the add/list flow after the API and UI projects are implemented.

## Prerequisites

- A supported .NET SDK installed on Windows, macOS, or Linux.
- A modern browser.
- The API and UI origins configured consistently. The documented local defaults are API
  `http://localhost:5151` and UI `http://localhost:5213`.
- CORS configured to allow the actual UI origin.

## Automated checks

From the repository root:

```powershell
dotnet test backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj
dotnet build backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj
dotnet build frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj
```

Run the test and build commands in any terminal supported by the installed .NET SDK. They
should complete successfully.

## Run the application

In separate terminals:

```powershell
dotnet run --project backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj
```

```powershell
dotnet run --project frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj
```

Open the UI URL reported by the frontend (expected default: `http://localhost:5213`). Confirm
the subscription page loads and an empty collection is shown.

## Verify the user journey

1. Enter `https://example.com/feed.xml` and submit it.
2. Confirm the exact submitted value appears in the list.
3. Submit the same URL again and confirm two entries appear.
4. Submit whitespace-only input and confirm no entry is added.
5. Submit a non-empty value that is not a valid URL and confirm it is listed without feed
   validation or a feed request.
6. Stop and restart the API, reload the UI, and confirm the list is empty.
7. Confirm the browser console has no failed local API requests or CORS errors.

External internet access is not needed for these scenarios. The browser must be able to reach
the local API.
