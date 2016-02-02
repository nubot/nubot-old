using System;
using NuBot.Brains;
using System.Collections.Generic;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    public interface IRobot
    {
        IBrain Brain { get; }

        void Listen(string pattern, Action<IContext<TextMessage>> context);

        void Hear(string pattern, Action<IContext<TextMessage>> context);

        void OnJoin(Action<IContext<ChannelJoinMessage>> context);

        T Random<T>(IEnumerable<T> collection);
    }
}
