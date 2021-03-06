﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NamedDIRegistrations.Tests.TestObjects
{
    public class TestObjectA : ITestObject
    {
        public DateTime CreationTimestamp { get; }

        public TestObjectA()
        {
            CreationTimestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return GetType().FullName + " " + CreationTimestamp;
        }
    }
}
