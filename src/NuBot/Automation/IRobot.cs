using System;
using NuBot.Brains;
using System.Collections.Generic;
using NuBot.Automation.Contexts;
using NuBot.Automation.Messages;
using NuBot.Automation.WebHooks;

namespace NuBot.Automation
{
    public interface IRobot
    {
        IBrain Brain { get; }

        void Listen(string pattern, Action<ISourcedContext<ITextMessage>> context);

        void Hear(string pattern, Action<ISourcedContext<ITextMessage>> context);

        void OnChannelJoin(Action<ISourcedContext<IChannelJoinMessage>> context);

        void OnChannelLeave(Action<ISourcedContext<IChannelLeaveMessage>> context);

        void OnConnected(Action<IContext> context);

        void OnDisconnected(Action<IContext> context);

        T Random<T>(IEnumerable<T> collection);

        void WebHook(string method, string pattern, Action<IWebHookContext> context);
    }
}
