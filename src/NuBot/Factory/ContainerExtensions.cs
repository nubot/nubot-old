using System;
using System.Collections.Generic;
using NuBot.Factory.Registrations;

namespace NuBot.Factory
{
    public static class ContainerExtensions
    {
        public static IContainer RegisterType<TRegistration, TImplementation>(this IContainer container, Lifetime lifetime)
            where TImplementation : TRegistration
        {
            container.Register(new TypeRegistration(typeof(TRegistration), typeof(TImplementation), lifetime));
            return container;
        }

        public static IContainer RegisterInstance<TRegistration>(this IContainer container, TRegistration instance)
        {
            container.Register(new InstanceRegistration(typeof(TRegistration), instance));
            return container;
        }

        public static IContainer RegisterMultiple<TRegistration>(this IContainer container, IEnumerable<Type> types)
        {
            container.Register(new CollectionTypeRegistration(typeof(TRegistration), types));
            return container;
        }
    }
}
