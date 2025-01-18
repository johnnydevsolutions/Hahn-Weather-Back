# Weather Forecast Application

A weather forecast application built with .NET 8.0, implementing Clean Architecture and Domain-Driven Design patterns. The solution includes a WebAPI for data access and a WorkerService for automated weather updates.

## Architecture

The solution follows Clean Architecture principles with the following structure:

- **Domain**: Core business logic and entities
- **Application**: Application services and DTOs
- **Infrastructure**: Data access and external services implementation
- **WebAPI**: REST API endpoints
- **WorkerService**: Background job processing
- **Jobs**: Hangfire job definitions

## Technologies

- **.NET 8.0**
- **Entity Framework Core**
- **SQL Server**
- **Hangfire**: For background job processing
- **AutoMapper**: For object mapping
- **Open-Meteo API**: Weather data source

## Features

- Real-time weather data retrieval for multiple cities
- Automatic hourly weather updates
- Temperature in both Celsius and Fahrenheit
- Detailed weather conditions with human-readable descriptions
- Data persistence with historical records

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Database Setup

1. Update the connection string in `appsettings.json`
2. Run Entity Framework migrations:

cd Infrastructure
dotnet ef database update


### Running the Application

1. Start the WebAPI:
Root Foolder
dotnet watch run --project .\WebApi\WebApi.csproj

or

cd WebAPI
dotnet watch run


2. Start the WorkerService:

Root Foolder
dotnet watch run --project .\WebApi\WebApi.csproj

or

cd WorkerService
dotnet watch run 


### 4. Hangfire Dashboard

1. Access the Hangfire dashboard at: `http://localhost:5282/hangfire/`
2. You'll see the recurring job "MultiCityWeatherUpdateJob" scheduled to run every hour
3. To see immediate results:
   - Go to the "Recurring Jobs" tab
   - Find "MultiCityWeatherUpdateJob"
   - Click "Trigger now" to execute the job immediately
   - The job will:
     - Fetch current weather for all configured cities
     - Store the data in the database
     - Update every hour automatically

### 5. Verifying the Setup

1. Trigger the Hangfire job as described above
2. Use Swagger UI or make a GET request to `http://localhost:5282/api/weatherforecasts`
3. You should see weather data for all configured cities
4. The data will be automatically updated every hour

### Troubleshooting

1. If no data appears:
   - Check if the Hangfire job executed successfully in the dashboard
   - Verify your database connection string
   - Ensure all cities have correct coordinates in appsettings.json

2. If Hangfire dashboard isn't accessible:
   - Verify both WebAPI and WorkerService are running
   - Check if the database was created correctly
   - Look for any error messages in the console output

[... rest of the README ...]


### API Endpoints

- `GET /api/weatherforecasts`: Get all weather forecasts
- `GET /api/weatherforecasts/{id}`: Get forecast by ID
- `GET /api/weatherforecasts/test/{city}`: Get current weather for a city
- `POST /api/weatherforecasts`: Create new forecast
- `PUT /api/weatherforecasts/{id}`: Update existing forecast
- `DELETE /api/weatherforecasts/{id}`: Delete forecast

## Background Jobs

The application uses Hangfire to run automated weather updates every hour for configured cities. The job:
- Fetches current weather data from Open-Meteo API
- Updates the database with new readings
- Maintains historical weather data

## Configuration

Cities and their coordinates are configured in `appsettings.json`:

json
{
"WeatherSettings": {
"Cities": [
"São Paulo",
"Rio de Janeiro",
// ...
],
"LatitudeLongitude": {
"São Paulo": { "Latitude": "-23.5505", "Longitude": "-46.6333" },
// ...
}
}
}

