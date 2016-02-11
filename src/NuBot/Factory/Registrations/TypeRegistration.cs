using System;

namespace NuBot.Factory.Registrations
{
    public sealed class TypeRegistration : ContainerRegistration
    {
        public Type ImplementationType { get; }

        public TypeRegistration(Type registrationType, Lifetime lifetime)
            : this(registrationType, registrationType, lifetime)
        {
        }

        public TypeRegistration(Type registrationType, Type implementationType, Lifetime lifetime)
            : base(registrationType, lifetime)
        {
            if (!registrationType.IsAssignableFrom(implementationType))
            {
                const string format = "The type '{0}' is not assignable from '{1}'.";
                throw new InvalidOperationException(string.Format(format, implementationType.FullName, registrationType.FullName));
            }

            ImplementationType = implementationType;
        }
    }
}