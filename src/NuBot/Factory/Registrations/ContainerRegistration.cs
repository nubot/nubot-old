using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuBot.Factory.Registrations
{
    public abstract class ContainerRegistration
    {
        public Type RegistrationType { get; }
        public Lifetime Lifetime { get; }

        protected ContainerRegistration(Type registrationType, Lifetime lifetime)
        {
            Lifetime = lifetime;
            RegistrationType = registrationType;
        }
    }
}
