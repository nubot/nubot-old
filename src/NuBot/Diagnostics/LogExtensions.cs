namespace NuBot.Diagnostics
{
    public static class LogExtensions
    {
        public static void Fatal(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Fatal, format, args);
        }

        public static void Error(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Error, format, args);
        }

        public static void Warning(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Warning, format, args);
        }

        public static void Information(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Information, format, args);
        }

        public static void Verbose(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Verbose, format, args);
        }

        public static void Debug(this INuBotLog log, string format, params object[] args)
        {
            log.Write(LogLevel.Debug, format, args);
        }
    }
}
