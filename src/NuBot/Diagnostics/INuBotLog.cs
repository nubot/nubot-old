namespace NuBot.Diagnostics
{
    public interface INuBotLog
    {
        void Write(LogLevel level, string format, params object[] args);
    }
}
