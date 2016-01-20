using System.Threading.Tasks;

namespace NuBot.Brains
{
    public interface IBrain
    {
        Task<T> GetAsync<T>(string key);
        Task<T> GetAsync<T>(string key, T defaultValue);
        Task SetAsync<T>(string key, T value);
    }
}
