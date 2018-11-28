using System;
using System.Collections.Generic;
using System.Text;

namespace NamedDIRegistrations.Abstractions
{
    public class NamedRegistration : INamedRegistration
    {
        public string Name { get; }
        public Type ServiceType { get; }

        public object GetInstance(IServiceProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
