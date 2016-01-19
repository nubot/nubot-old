using System;
using NuBot.Brains;
using System.Collections.Generic;

namespace NuBot.Automation
{
    public interface IRobot
    {
        IBrain Brain { get; }

        void Listen(string pattern, Action<IContext> context);

        void Hear(string pattern, Action<IContext> context);

        T Random<T>(IEnumerable<T> collection);
    }
}
