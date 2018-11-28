namespace NamedDIRegistrations.ServiceCollection
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Abstractions;
    using Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class NamedServiceCollectionServiceExtensions
    {
        private static readonly Type ProxyType = typeof(INamedRegistration);
        private static readonly object Lock;
        private static readonly IList<string> RegistrationNames;

        static NamedServiceCollectionServiceExtensions()
        {
            Lock = new object();
            RegistrationNames = new List<string>();
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        /// implementation of the type specified in <paramref name="implementationType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <param name="name">The registration name</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, Type serviceType, Type implementationType, string name)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (serviceType == (Type)null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == (Type)null)
                throw new ArgumentNullException(nameof(implementationType));
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            return NamedServiceCollectionServiceExtensions.AddNamedType(services, serviceType, implementationType, ServiceLifetime.Transient, name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <param name="name">The registration name.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory, string name)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (serviceType == (Type)null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            return NamedServiceCollectionServiceExtensions.AddNamedFactory(services, serviceType, implementationFactory, ServiceLifetime.Transient, name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="name">the registration name.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, string name) where TService : class where TImplementation : class, TService
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            return services.AddNamedTransient(typeof(TService), typeof(TImplementation), name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <param name="name">The registration name.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, Type serviceType, string name)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (serviceType == (Type)null)
                throw new ArgumentNullException(nameof(serviceType));
            return services.AddNamedTransient(serviceType, serviceType, name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="name">The registration name.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient<TService>(this IServiceCollection services, string name) where TService : class
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            return services.AddNamedTransient(typeof(TService), name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with a
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <param name="name"></param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, string name) where TService : class
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            return services.AddNamedTransient(typeof(TService), (Func<IServiceProvider, object>)implementationFactory, name);
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        /// implementation type specified in <typeparamref name="TImplementation" /> using the
        /// factory specified in <paramref name="implementationFactory" /> to the
        /// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <param name="name">The registration name.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory, string name) where TService : class where TImplementation : class, TService
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            return services.AddNamedTransient(typeof(TService), (Func<IServiceProvider, object>)implementationFactory, name);
        }

        ///// <summary>
        ///// Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        ///// implementation of the type specified in <paramref name="implementationType" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register.</param>
        ///// <param name="implementationType">The implementation type of the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, Type implementationType)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    if (implementationType == (Type)null)
        //        throw new ArgumentNullException(nameof(implementationType));
        //    //return NamedServiceCollectionServiceExtensions.Add(services, serviceType, implementationType, ServiceLifetime.Scoped);
        //    return null;
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    //return NamedServiceCollectionServiceExtensions.Add(services, serviceType, implementationFactory, ServiceLifetime.Scoped);
        //    return null;
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///// implementation type specified in <typeparamref name="TImplementation" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    return services.AddScoped(typeof(TService), typeof(TImplementation));
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped(this IServiceCollection services, Type serviceType)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    return services.AddScoped(serviceType, serviceType);
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped<TService>(this IServiceCollection services) where TService : class
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    return services.AddScoped(typeof(TService));
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    return services.AddScoped(typeof(TService), (Func<IServiceProvider, object>)implementationFactory);
        //}

        ///// <summary>
        ///// Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///// implementation type specified in <typeparamref name="TImplementation" /> using the
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        //public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    return services.AddScoped(typeof(TService), (Func<IServiceProvider, object>)implementationFactory);
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///// implementation of the type specified in <paramref name="implementationType" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register.</param>
        ///// <param name="implementationType">The implementation type of the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, Type implementationType)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    if (implementationType == (Type)null)
        //        throw new ArgumentNullException(nameof(implementationType));
        //    //return NamedServiceCollectionServiceExtensions.Add(services, serviceType, implementationType, ServiceLifetime.Singleton);
        //    return null;
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    //return NamedServiceCollectionServiceExtensions.Add(services, serviceType, implementationFactory, ServiceLifetime.Singleton);
        //    return null;
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///// implementation type specified in <typeparamref name="TImplementation" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    return NamedServiceCollectionServiceExtensions.AddSingleton(services, typeof(TService), typeof(TImplementation));
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    return NamedServiceCollectionServiceExtensions.AddSingleton(services, serviceType, serviceType);
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton<TService>(this IServiceCollection services) where TService : class
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    return services.AddSingleton(typeof(TService));
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    return NamedServiceCollectionServiceExtensions.AddSingleton(services, typeof(TService), (Func<IServiceProvider, object>)implementationFactory);
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///// implementation type specified in <typeparamref name="TImplementation" /> using the
        ///// factory specified in <paramref name="implementationFactory" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to add.</typeparam>
        ///// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="implementationFactory">The factory that creates the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (implementationFactory == null)
        //        throw new ArgumentNullException(nameof(implementationFactory));
        //    return NamedServiceCollectionServiceExtensions.AddSingleton(services, typeof(TService), (Func<IServiceProvider, object>)implementationFactory);
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///// instance specified in <paramref name="implementationInstance" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="serviceType">The type of the service to register.</param>
        ///// <param name="implementationInstance">The instance of the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton(this IServiceCollection services, Type serviceType, object implementationInstance)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (serviceType == (Type)null)
        //        throw new ArgumentNullException(nameof(serviceType));
        //    if (implementationInstance == null)
        //        throw new ArgumentNullException(nameof(implementationInstance));
        //    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(serviceType, implementationInstance);
        //    services.Add(serviceDescriptor);
        //    return services;
        //}

        ///// <summary>
        ///// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///// instance specified in <paramref name="implementationInstance" /> to the
        ///// specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        ///// </summary>
        ///// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        ///// <param name="implementationInstance">The instance of the service.</param>
        ///// <returns>A reference to this instance after the operation has completed.</returns>
        ///// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        //public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService implementationInstance) where TService : class
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if ((object)implementationInstance == null)
        //        throw new ArgumentNullException(nameof(implementationInstance));
        //    return services.AddSingleton(typeof(TService), (object)implementationInstance);
        //}

        private static IServiceCollection AddNamedType(IServiceCollection collection, Type serviceType, Type implementationType, ServiceLifetime lifetime, string name)
        {
            //lock (Lock)
            //{
            //    if (RegistrationNames.Contains(name))
            //        throw new DuplicateRegistrationException(name, implementationType);

            //    Func<IServiceProvider, object> namedImplementationFactory = provider => new NamedTypeRegistration(name, implementationType);

            //    var serviceDescriptor = new ServiceDescriptor(ProxyType, namedImplementationFactory, lifetime);
            //    collection.Add(serviceDescriptor);

            //    RegistrationNames.Add(name);
            //}

            object NamedImplementationFactory(IServiceProvider provider) => new NamedTypeRegistration(name, serviceType, implementationType);
            return AddNamedRegistration(collection, NamedImplementationFactory, lifetime, name);
        }

        private static IServiceCollection AddNamedFactory(IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, string name)
        {
            var namedFactory = new NamedFactoryRegistration(name, serviceType, implementationFactory);

            AddNamedRegistration(collection, provider => namedFactory, lifetime,
                name);
            return collection;
        }

        private static IServiceCollection AddNamedRegistration(IServiceCollection collection,
            Func<IServiceProvider, object> implementationFactory, ServiceLifetime lifetime, string name)
        {
            lock (Lock)
            {
                if (RegistrationNames.Contains(name))
                    throw new DuplicateRegistrationException(name, implementationFactory.GetType());

                var serviceDescriptor = new ServiceDescriptor(ProxyType, implementationFactory.Invoke, lifetime);
                collection.Add(serviceDescriptor);

                RegistrationNames.Add(name);
            }
            return collection;
        }
    }
}
