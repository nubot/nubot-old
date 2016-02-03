using System.Threading.Tasks;

namespace NuBot.Automation
{
    public interface IChannel
    {
        string Name { get; }

        Task RespondAsync(IUser user, string format, object[] parameters);

        Task SendAsync(string format, params object[] parameters);
    }
}