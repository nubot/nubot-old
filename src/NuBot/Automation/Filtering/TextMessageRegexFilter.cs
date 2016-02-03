using NuBot.Automation.Messages;

namespace NuBot.Automation.Filtering
{
    internal sealed class TextMessageRegexFilter : RegexFilter<ITextMessage>
    {
        public TextMessageRegexFilter(string pattern) : base(pattern)
        {
        }

        protected override string GetString(ITextMessage data)
        {
            return data.Content;
        }
    }
}