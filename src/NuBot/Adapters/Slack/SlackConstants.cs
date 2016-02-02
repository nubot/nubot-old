namespace NuBot.Adapters.Slack
{
    internal static class SlackConstants
    {
        internal static class Events
        {
            public const string Message = "message";
            public const string TeamJoin = "team_join";
            // TODO: lots of missing events
        }

        internal static class MessageSubTypes
        {
            public const string ChannelJoin = "channel_join";
        }
    }
}