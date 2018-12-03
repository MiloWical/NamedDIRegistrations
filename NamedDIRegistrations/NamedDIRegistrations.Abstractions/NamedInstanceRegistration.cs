namespace NamedDIRegistrations.Abstractions
{
    using System;

    public class NamedInstanceRegistration : INamedRegistration
    {
        public string Name { get; }

        public Type ServiceType { get; }

        private object _instance;

        private readonly Func<IServiceProvider, object> _instanceFactory;

        private NamedInstanceRegistration(string name, Type serviceType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        }

        public NamedInstanceRegistration(string name, Type serviceType, object instance)
            : this(name, serviceType)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            _instanceFactory = _ => null;
        }

        public NamedInstanceRegistration(string name, Type serviceType, Func<IServiceProvider, object> factory)
            : this(name, serviceType)
        {
            _instanceFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            _instance = null;
        }

        public object GetInstance(IServiceProvider provider)
        {
            return _instance ?? (_instance = _instanceFactory.Invoke(provider));
        }
    }
}
