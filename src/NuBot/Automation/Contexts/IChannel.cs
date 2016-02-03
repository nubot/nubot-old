using System.Threading.Tasks;

namespace NuBot.Automation.Contexts
{
    public interface IChannel
    {
        string Name { get; }

        Task SendAsync(string format, params object[] parameters);
    }
}