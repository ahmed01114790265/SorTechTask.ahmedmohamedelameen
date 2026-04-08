# SorTechTask.ahmedmohamedelameen
IP Geolocation & Country Blocking System
A robust ASP.NET Core Web API designed to manage country-based access control. 
The system allows administrators to block specific countries,
perform IP lookups using third-party services, 
and automatically manage temporal blocks using background services.

## Features
-Country Management: CRUD operations to block/unblock countries with duplicate prevention.
-IP Lookup: Integration with IPGeolocation.io to fetch detailed geographical data for any IP.
-Automated Blocking:
                      1-Permanent Block: Static blocking by country code.
                      2-Temporal Block: Block countries for a specific duration (1-1440 minutes).
-Background Service: A scheduled service that runs every 5 minutes to automatically unblock expired temporal blocks.
-Audit Logging: Every access attempt is logged with IP, Timestamp, Country, and User-Agent details.
-Advanced Queries: Support for Pagination and Search across blocked lists and logs.
-Thread-Safety: Implementation uses ConcurrentDictionary and ConcurrentBag for safe in-memory data management.

## Technologies Used
-ASP.NET Core Web API. .NET 8.0.
-BackgroundService for scheduled tasks
-Concurrent Collections for thread-safe data handling
-External API: IPGeolocation.io.
-Documentation: Swagger.

## Configuration
-Get your API Key from IPGeolocation.io.
-API Key: Obtain an API key from IPGeolocation.io and set it in the appsettings.json file.

## API Endpoints
-POST /api/countries/block: Block a country by its ISO code with an optional duration for temporal blocks.
-DELETE /api/countries/unblock/{countryCode}: Unblock a country by its ISO code.
-GET /api/countries/blocked: Retrieve a list of all currently blocked countries.
-GET /api/iplookup/{ipAddress}: Fetch geolocation data for a specific IP address.
-GET /api/logs: Retrieve access logs with support for pagination and search.
-POST /api/countries/temporal-block - Block a country for a specific duration (1-1440 minutes).


## Developed by: Ahmed Mohamed Alameen.


