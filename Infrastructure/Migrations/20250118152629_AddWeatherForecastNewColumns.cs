using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeatherForecastNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FeelsLike",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Humidity",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TemperatureAverage",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TemperatureFahrenheit",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TemperatureMax",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TemperatureMin",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WindSpeed",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeelsLike",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "Humidity",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "TemperatureAverage",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "TemperatureFahrenheit",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "TemperatureMax",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "TemperatureMin",
                table: "WeatherForecasts");

            migrationBuilder.DropColumn(
                name: "WindSpeed",
                table: "WeatherForecasts");
        }
    }
}
