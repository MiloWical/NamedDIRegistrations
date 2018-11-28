using System;
using System.Collections.Generic;
using System.Text;

namespace NamedDIRegistrations.Tests.TestObjects
{
    public class TestObjectB : ITestObject
    {
        public DateTime CreationTimestamp { get; }

        public string Message { get; }

        public TestObjectB(string message)
        {
            CreationTimestamp = DateTime.Now;
            Message = message;
        }

        public override string ToString()
        {
            return $"{Message} [{CreationTimestamp}]";
        }
    }
}
