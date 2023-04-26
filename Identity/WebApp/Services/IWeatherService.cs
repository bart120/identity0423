using Refit;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IWeatherService
    {
        [Get("/weatherforecast")]
        Task<List<Wheather>> GetWheather();

    }
}
