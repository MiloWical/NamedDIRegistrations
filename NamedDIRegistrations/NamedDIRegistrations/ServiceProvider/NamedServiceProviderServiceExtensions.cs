namespace NamedDIRegistrations.ServiceProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using IServiceScope = Microsoft.Extensions.DependencyInjection.IServiceScope;
    using IServiceScopeFactory = Microsoft.Extensions.DependencyInjection.IServiceScopeFactory;
    using ISupportRequiredService = Microsoft.Extensions.DependencyInjection.ISupportRequiredService;

    /// <summary>
    /// Extension methods for getting services from an <see cref="T:System.IServiceProvider" />.
    /// </summary>
    public static class NamedServiceProviderServiceExtensions
    {
        /// <summary>Gets the named service object of the specified type.</summary>
        /// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="name">The registration name</param>
        /// <returns>A service object of type <paramref name="serviceType">serviceType</paramref>.   -or-  null if there is no service object of type <paramref name="serviceType">serviceType</paramref>.</returns>
        public static object GetNamedService(this IServiceProvider provider, Type serviceType, string name)
        {
            var registrations = provider.GetServices<INamedRegistration>();

            var registration = registrations.FirstOrDefault(reg => reg.Name == name && reg.ServiceType == serviceType);

            if (registration != null)
                return registration.GetInstance(provider);

            throw new MissingRegistrationException(name, serviceType);
        }

        ///// <summary>
        ///// Get named service of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        ///// </summary>
        ///// <typeparam name="T">The type of service object to get.</typeparam>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        ///// <param name="name">The registration name</param>
        ///// <returns>A service object of type <typeparamref name="T" /> or null if there is no such service.</returns>
        //public static T GetNamedService<T>(this IServiceProvider provider, string name)
        //{
        //    if (provider == null)
        //        throw new ArgumentNullException(nameof(provider));
        //    return (T) provider.GetService(typeof(T));
        //}

        ///// <summary>
        ///// Get service of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" />.
        ///// </summary>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        ///// <param name="serviceType">An object that specifies the type of service object to get.</param>
        ///// <returns>A service object of type <paramref name="serviceType" />.</returns>
        ///// <exception cref="T:System.InvalidOperationException">There is no service of type <paramref name="serviceType" />.</exception>
        //public static object GetRequiredService(this IServiceProvider provider, Type serviceType)
        //{
        //    if (provider == null)
        //        throw new ArgumentNullException(nameof(provider));
        //    if (serviceType == (Type) null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    ISupportRequiredService supportRequiredService = provider as ISupportRequiredService;
        //    if (supportRequiredService != null)
        //        return supportRequiredService.GetRequiredService(serviceType);
        //    object service = provider.GetService(serviceType);
        //    if (service != null)
        //        return service;
        //    throw new InvalidOperationException(Resources.FormatNoServiceRegistered((object) serviceType));
        //}

        ///// <summary>
        ///// Get service of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        ///// </summary>
        ///// <typeparam name="T">The type of service object to get.</typeparam>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the service object from.</param>
        ///// <returns>A service object of type <typeparamref name="T" />.</returns>
        ///// <exception cref="T:System.InvalidOperationException">There is no service of type <typeparamref name="T" />.</exception>
        //public static T GetRequiredService<T>(this IServiceProvider provider)
        //{
        //    if (provider == null)
        //        throw new ArgumentNullException(nameof(provider));
        //    return (T) provider.GetRequiredService(typeof(T));
        //}

        ///// <summary>
        ///// Get an enumeration of services of type <typeparamref name="T" /> from the <see cref="T:System.IServiceProvider" />.
        ///// </summary>
        ///// <typeparam name="T">The type of service object to get.</typeparam>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the services from.</param>
        ///// <returns>An enumeration of services of type <typeparamref name="T" />.</returns>
        //public static IEnumerable<T> GetServices<T>(this IServiceProvider provider)
        //{
        //    if (provider == null)
        //        throw new ArgumentNullException(nameof(provider));
        //    return provider.GetRequiredService<IEnumerable<T>>();
        //}

        ///// <summary>
        ///// Get an enumeration of services of type <paramref name="serviceType" /> from the <see cref="T:System.IServiceProvider" />.
        ///// </summary>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to retrieve the services from.</param>
        ///// <param name="serviceType">An object that specifies the type of service object to get.</param>
        ///// <returns>An enumeration of services of type <paramref name="serviceType" />.</returns>
        //public static IEnumerable<object> GetServices(this IServiceProvider provider, Type serviceType)
        //{
        //    if (provider == null)
        //        throw new ArgumentNullException(nameof(provider));
        //    if (serviceType == (Type) null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    Type serviceType1 = typeof(IEnumerable<>).MakeGenericType(serviceType);
        //    return (IEnumerable<object>) provider.GetRequiredService(serviceType1);
        //}

        ///// <summary>
        ///// Creates a new <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> that can be used to resolve scoped services.
        ///// </summary>
        ///// <param name="provider">The <see cref="T:System.IServiceProvider" /> to create the scope from.</param>
        ///// <returns>A <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceScope" /> that can be used to resolve scoped services.</returns>
        //public static IServiceScope CreateScope(this IServiceProvider provider)
        //{
        //    return provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        //}
    }
}
