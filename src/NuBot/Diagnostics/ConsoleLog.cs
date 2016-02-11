using System;

namespace NuBot.Diagnostics
{
    internal sealed class ConsoleLog : INuBotLog
    {
        private readonly object _lock;

        public ConsoleLog()
        {
            _lock = new object();
        }

        public void Write(LogLevel level, string format, params object[] args)
        {
            lock (_lock)
            {
                try
                {
                    Console.ForegroundColor = GetForegroundColor(level);
                    Console.WriteLine(format, args);
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }

        private ConsoleColor GetForegroundColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Fatal:
                    return ConsoleColor.Red;
                case LogLevel.Error:
                    return ConsoleColor.DarkRed;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Information:
                    return ConsoleColor.White;
                case LogLevel.Verbose:
                    return ConsoleColor.Gray;
                case LogLevel.Debug:
                    return ConsoleColor.DarkGray;
                default:
                    throw new InvalidOperationException("Unknown log level.");
            }
        }
    }
}
