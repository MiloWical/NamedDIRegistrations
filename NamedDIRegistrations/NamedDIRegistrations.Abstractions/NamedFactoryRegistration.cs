namespace NamedDIRegistrations.Abstractions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class NamedFactoryRegistration : INamedRegistration
    {
        public string Name { get; }

        public Type ServiceType { get; }

        public Func<IServiceProvider, object> Factory { get; }

        public NamedFactoryRegistration(string name, Type serviceType, Func<IServiceProvider, object> factory)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public object GetInstance(IServiceProvider provider)
        {
            return Factory.Invoke(provider);
        }
    }
}
