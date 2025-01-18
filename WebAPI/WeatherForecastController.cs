using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI
{
    [ApiController]
    [Route("api/weatherforecasts")]
    public class WeatherForecastController : ControllerBase
    {
       private readonly IWeatherService _weatherService;
        private readonly IMapper _mapper;

        public WeatherForecastController(IWeatherService weatherService, IMapper mapper)
        {
            _weatherService = weatherService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> GetAllForecasts()
        {
            var forecasts = await _weatherService.GetAllForecastsAsync();
            var forecastDtos = _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts);
            return Ok(forecastDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecastDto>> GetForecastById(int id)
        {
            var forecast = await _weatherService.GetForecastByIdAsync(id);
            if (forecast == null)
            {
                return NotFound();
            }
            var forecastDto = _mapper.Map<WeatherForecastDto>(forecast);
            return Ok(forecastDto);
        }

        [HttpGet("test/{city}")]
public async Task<ActionResult<WeatherForecastDto>> TestGetForecast(string city)
{
    try
    {
        var forecast = await _weatherService.GetForecastByCityAsync(city);
        var forecastDto = _mapper.Map<WeatherForecastDto>(forecast);
        return Ok(forecastDto);
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}


        [HttpPost]
        public async Task<ActionResult<WeatherForecastDto>> CreateForecast([FromBody] WeatherForecastDto forecastDto)
        {
            if (forecastDto == null)
            {
                return BadRequest();
            }

            var forecast = _mapper.Map<WeatherForecast>(forecastDto);
            await _weatherService.UpsertForecastAsync(forecast);
            var createdDto = _mapper.Map<WeatherForecastDto>(forecast);

            return CreatedAtAction(nameof(GetForecastById), new { id = forecast.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateForecast(int id, [FromBody] WeatherForecastDto forecastDto)
        {
            if (forecastDto == null || id != forecastDto.Id)
            {
                return BadRequest();
            }

            var existingForecast = await _weatherService.GetForecastByIdAsync(id);
            if (existingForecast == null)
            {
                return NotFound();
            }

            var forecast = _mapper.Map<WeatherForecast>(forecastDto);
            await _weatherService.UpsertForecastAsync(forecast);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteForecast(int id)
        {
            var existingForecast = await _weatherService.GetForecastByIdAsync(id);
            if (existingForecast == null)
            {
                return NotFound();
            }

            await _weatherService.DeleteForecastAsync(id);
            return NoContent();
        }
    }
}