var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
seeder.Seed();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
