using System;
using NuBot.Factory.DI;
using NuBot.Factory.Registrations;

namespace NuBot.Factory
{
    internal sealed class DefaultContainer : Container<TinyIoCContainer>
    {
        protected override TinyIoCContainer CreateContainer()
        {
            return new TinyIoCContainer();
        }

        protected override T Resolve<T>(TinyIoCContainer container)
        {
            return container.Resolve<T>();
        }

        protected override void Register(TinyIoCContainer container, TypeRegistration registration)
        {
            switch (registration.Lifetime)
            {
                case Lifetime.Singleton:
                    container.Register(registration.RegistrationType, registration.ImplementationType).AsSingleton();
                    break;
                case Lifetime.InstancePerDependency:
                    container.Register(registration.RegistrationType, registration.ImplementationType).AsMultiInstance();
                    break;
                default:
                    throw new InvalidOperationException("Unknown life time.");
            }
        }

        protected override void Register(TinyIoCContainer container, InstanceRegistration registration)
        {
            container.Register(registration.RegistrationType, registration.Instance);
        }

        protected override void Register(TinyIoCContainer container, CollectionTypeRegistration registration)
        {
            switch (registration.Lifetime)
            {
                case Lifetime.Singleton:
                    container.RegisterMultiple(registration.RegistrationType, registration.ImplementationTypes).AsSingleton();
                    break;
                case Lifetime.InstancePerDependency:
                    container.RegisterMultiple(registration.RegistrationType, registration.ImplementationTypes).AsMultiInstance();
                    break;
                default:
                    throw new InvalidOperationException("Unknown life time.");
            }
        }
    }
}