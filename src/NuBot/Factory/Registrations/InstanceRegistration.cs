using System;

namespace NuBot.Factory.Registrations
{
    public sealed class InstanceRegistration : ContainerRegistration
    {
        public object Instance { get; }

        public InstanceRegistration(Type registrationType, object instance)
            : base(registrationType, Lifetime.Singleton)
        {
            Instance = instance;
        }
    }
}