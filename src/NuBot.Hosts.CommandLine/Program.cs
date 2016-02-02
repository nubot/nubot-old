using NuBot.Adapters;
using NuBot.Adapters.Gitter;
using NuBot.Adapters.Slack;
using NuBot.Factory;
using NuBot.Parts;
using System;
using System.Threading;

namespace NuBot.Hosts.CommandLine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IAdapter adapter = null;

            switch(args[0])
            {
                case "gitter":
                    adapter = new GitterAdapter(args[1]);
                    break;

                case "slack":
                    adapter = new SlackAdapter(args[1]);
                    break;

                default:
                    throw new NotImplementedException("Unknown NuBot adapter.");
            }

            var cancellationToken = CancellationToken.None;

            new RobotFactory()
                .AddPart<Echo>()
                .AddPart<Greeter>()
                .UseAdapter(adapter)
                .RunAsync(cancellationToken)
                .GetAwaiter()
                .GetResult();
        }
    }
}
