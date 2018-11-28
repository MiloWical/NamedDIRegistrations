namespace NamedDIRegistrations.Exceptions
{
    using System;

    public class DuplicateRegistrationException : Exception, IRegistrationException
    {
        public string RegistrationName { get; }
        public Type RegistrationType { get; }

        private readonly string _message;

        public DuplicateRegistrationException(string registrationName, Type registrationType)
        {
            RegistrationName = registrationName ?? throw new ArgumentNullException(nameof(registrationName));
            RegistrationType = registrationType ?? throw new ArgumentNullException(nameof(registrationType));
            _message = base.Message;
        }

        public DuplicateRegistrationException(string registrationName, Type registrationType, string message)
        {
            RegistrationName = registrationName ?? throw new ArgumentNullException(nameof(registrationName));
            RegistrationType = registrationType ?? throw new ArgumentNullException(nameof(registrationType));
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public override string Message => $"Error registering type '{RegistrationType}' with name '{RegistrationName}' {Environment.NewLine} {_message}";
    }
}
