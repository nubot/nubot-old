using NuBot.Adapters;
using NuBot.Adapters.Gitter;
using NuBot.Adapters.Slack;
using NuBot.Factory;
using NuBot.Parts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Hosts.CommandLine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.TreatControlCAsInput = true;
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            var resetEvent = new ManualResetEvent(false);
            var tokenSource = new CancellationTokenSource();

            var thread = new Thread(() =>
            {
                try
                {
                    IAdapter adapter;

                    switch (args[0])
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

                    new RobotFactory()
                        .AddPart<Echo>()
                        .AddPart<EchoWebHook>()
                        .AddPart<HelloGoodbye>()
                        .AddPart<IllBeBack>()
                        .UseAdapter(adapter)
                        .UseHttpServer(1337)
                        .RunAsync(tokenSource.Token)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception);
                    Console.ResetColor();
                }
                finally
                {
                    resetEvent.Set();
                }
            });

            thread.Start();

            while (true)
            {
                var key = Console.ReadKey(true);

                if ((key.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control && key.Key == ConsoleKey.C)
                {
                    break;
                }
            }

            tokenSource.Cancel();
            resetEvent.WaitOne();
            thread.Join();

            Console.WriteLine("NuBot has stopped.");
            Console.ReadKey();
        }
    }
}
