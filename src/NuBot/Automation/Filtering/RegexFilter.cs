using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Filtering
{
    public abstract class RegexFilter<T> : IFilter<T> where T : IMessage
    {
        private readonly Regex _regex;

        protected RegexFilter(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.IgnoreCase);
        }

        public IDictionary<string, string> GetOutput(T data)
        {
            var input = GetString(data);
            var match = _regex.Match(input);

            return _regex.GetGroupNames()
                .ToDictionary(
                    groupName => groupName,
                    groupName => match.Groups[groupName].Value);
        } 

        public bool IsMatch(T data)
        {
            var str = GetString(data);
            return _regex.IsMatch(str);
        }

        protected abstract string GetString(T data);
    }
}