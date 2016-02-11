using NuBot.Factory.Registrations;

namespace NuBot.Factory
{
    public abstract class Container<TContainer> : IContainer
    {
        private TContainer _container;

        public void Register(TypeRegistration registration)
        {
            Initialize();
            Register(_container, registration);
        }

        public void Register(InstanceRegistration registration)
        {
            Initialize();
            Register(_container, registration);
        }

        public void Register(CollectionTypeRegistration registration)
        {
            Initialize();
            Register(_container, registration);
        }

        public T Resolve<T>()
            where T : class
        {
            Initialize();
            return Resolve<T>(_container);
        }

        protected abstract TContainer CreateContainer();
        protected abstract T Resolve<T>(TContainer container) where T : class;

        protected abstract void Register(TContainer container, TypeRegistration registration);
        protected abstract void Register(TContainer container, InstanceRegistration registration);
        protected abstract void Register(TContainer container, CollectionTypeRegistration registration);

        private void Initialize()
        {
            if (_container == null)
            {
                _container = CreateContainer();
            }
        }
    }
}
