namespace NamedDIRegistrations.Abstractions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class NamedTypeRegistration : INamedRegistration
    {
        public string Name { get; }
        public Type ServiceType { get; }
        public Type Type { get; }

        public Func<object> Constructor { get; }

        private Lazy<object> _instance;

        public NamedTypeRegistration(string name, Type serviceType, Type type)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ServiceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Constructor = () => Type.GetConstructor(new Type[0])?.Invoke(new object[0]);

            _instance = null;
        }
        public object GetInstance(IServiceProvider provider)
        {
            if(_instance == null)
                _instance = new Lazy<object>(Constructor);

            return _instance.Value;
        }
    }
}
