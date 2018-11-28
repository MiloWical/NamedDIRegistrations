namespace NamedDIRegistrations.Abstractions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public interface INamedRegistration
    {
        string Name { get; }
        Type ServiceType { get; }
        object GetInstance(IServiceProvider provider);
    }
}
