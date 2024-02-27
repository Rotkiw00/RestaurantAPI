namespace RestaurantAPI.Controllers
{
	public class WeatherForecastService : IWeatherForecastService
	{
		private static readonly string[] Summaries =
		[
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		];

		public IEnumerable<WeatherForecast> Get(int n, int minC, int maxC)
		{
			return Enumerable.Range(1, n).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(minC, maxC),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}

	public interface IWeatherForecastService
	{
		IEnumerable<WeatherForecast> Get(int n, int minC, int maxC);
	}
}
