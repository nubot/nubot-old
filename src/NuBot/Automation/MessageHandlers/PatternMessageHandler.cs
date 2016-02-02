using NuBot.Adapters;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NuBot.Automation.Messages;

namespace NuBot.Automation.MessageHandlers
{
    public class PatternMessageHandler : IMessageHandler<TextMessage>
    {
        private readonly Regex _regex;

        public PatternMessageHandler(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public bool CanHandle(TextMessage message)
        {
            return _regex.IsMatch(message.Content);
        }

        public IDictionary<string, string> Handle(TextMessage message)
        {
            var result = new Dictionary<string, string>();
            var match = _regex.Match(message.Content);

            foreach (var groupName in _regex.GetGroupNames())
            {
                result.Add(groupName, match.Groups[groupName].Value);
            }

            return result;
        }
    }
}
