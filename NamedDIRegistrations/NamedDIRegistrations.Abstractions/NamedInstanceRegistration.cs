namespace NamedDIRegistrations.Abstractions
{
    using System;

    public class NamedInstanceRegistration<T> : INamedRegistration
    {
        public string Name { get; }

        public Type ServiceType { get; }
        
        private T Instance { get; }

        public NamedInstanceRegistration(string name, Type serviceType, T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            Name = name ?? throw new ArgumentNullException(nameof(name));
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            Instance = instance;
        }

        public object GetInstance(IServiceProvider provider)
        {
            return Instance;
        }
    }
}
