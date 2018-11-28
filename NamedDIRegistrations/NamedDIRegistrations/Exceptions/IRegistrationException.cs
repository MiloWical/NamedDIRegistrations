namespace NamedDIRegistrations.Exceptions
{
    using System;

    public interface IRegistrationException
    {
        string RegistrationName { get; }
        Type RegistrationType { get; }
    }
}