using System;
using System.Collections.Generic;

namespace NuBot.Factory.Registrations
{
    public sealed class CollectionTypeRegistration : ContainerRegistration
    {
        public IEnumerable<Type> ImplementationTypes { get; }

        public CollectionTypeRegistration(Type registrationType, IEnumerable<Type> implementationTypes)
            : base(registrationType, Lifetime.Singleton)
        {
            ImplementationTypes = implementationTypes;
        }
    }
}