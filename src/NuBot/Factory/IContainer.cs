using NuBot.Factory.Registrations;

namespace NuBot.Factory
{
    public interface IContainer
    {
        void Register(TypeRegistration registration);
        void Register(InstanceRegistration registration);
        void Register(CollectionTypeRegistration registration);

        T Resolve<T>() where T : class;
    }
}