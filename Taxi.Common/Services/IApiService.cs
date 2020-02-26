using Taxi.Common.Models;
using System.Threading.Tasks;

namespace Taxi.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetTaxiAsync(
            string Plaque,
            string urlBase,
            string servicePrefix,
            string controller);

        Task<bool> CheckConnectionAsync(string url);
    }
}