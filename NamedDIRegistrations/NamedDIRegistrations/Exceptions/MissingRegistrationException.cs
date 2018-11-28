namespace NamedDIRegistrations.Exceptions
{
    using System;

    public class MissingRegistrationException : Exception, IRegistrationException
    {
        public string RegistrationName { get; }

        public Type RegistrationType { get; }

        private readonly string _message;

        public MissingRegistrationException(string registrationName, Type registrationType)
        {
            RegistrationName = registrationName ?? throw new ArgumentNullException(nameof(registrationName));
            RegistrationType = registrationType ?? throw new ArgumentNullException(nameof(registrationType));
            _message = base.Message;
        }

        public MissingRegistrationException(string registrationName, Type registrationType, string message)
        {
            RegistrationName = registrationName ?? throw new ArgumentNullException(nameof(registrationName));
            RegistrationType = registrationType ?? throw new ArgumentNullException(nameof(registrationType));
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }   

        public override string Message => $"Error resolving type '{RegistrationType}' with name '{RegistrationName}' {Environment.NewLine} {_message}";
    }
}
