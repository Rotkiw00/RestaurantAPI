using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IWeatherForecastService _service;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpPost("generate")]
		public ActionResult<IEnumerable<WeatherForecast>> GeneratePost([FromQuery]int recordsCount, [FromBody]TemperatureRequestModel requestModel)
		{
			if (recordsCount <= 0 || requestModel.MaximumTempC < requestModel.MinimumTempC)
			{
				return BadRequest("Error: n should be grater than 0 and maximum temp should be grater than minimum");
			}
			else
			{
				var results = _service.Get(recordsCount, requestModel.MinimumTempC, requestModel.MaximumTempC);
				return Ok(results);
			}
		}
	}

	public class TemperatureRequestModel
	{
        public int MinimumTempC { get; set; }
        public int MaximumTempC { get; set; }
    }
}
 