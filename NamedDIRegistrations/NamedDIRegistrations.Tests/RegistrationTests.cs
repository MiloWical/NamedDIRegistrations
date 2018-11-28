using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NamedDIRegistrations.Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Abstractions;
    using Exceptions;
    using NamedDIRegistrations.ServiceCollection;
    using NamedDIRegistrations.ServiceProvider;
    using Microsoft.Extensions.DependencyInjection;
    using TestObjects;

    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddTransient<ITestObject>(_ => new TestObjectA());
            collection.AddSingleton<ITestObject>(new TestObjectB("Hello from Test Object B!"));

            var provider = collection.BuildServiceProvider();

            Debug.WriteLine(provider.GetService(typeof(TestObjectA)));
            Debug.WriteLine(provider.GetService(typeof(TestObjectB)));

            Thread.Sleep(1200);

            Debug.WriteLine(provider.GetService(typeof(TestObjectA)));
            Debug.WriteLine(provider.GetService(typeof(TestObjectB)));

            Thread.Sleep(1200);

            Debug.WriteLine(provider.GetService<ITestObject>());
        }

        [TestMethod]
        public void TestMethod2()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddTransient<ITestObject, TestObjectA>();

            //This doesn't work because there's no default constructor
            //collection.AddTransient<ITestObject, TestObjectB>();

            var provider = collection.BuildServiceProvider();

            Debug.WriteLine(provider.GetService(typeof(ITestObject)));
        }

        [TestMethod]
        public void TestMethod3()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient(typeof(ITestObject), typeof(TestObjectA), "NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient(typeof(ITestObject), typeof(TestObjectB), "NumeroDos");

            var provider = collection.BuildServiceProvider();

            var registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroDos");

            //This registration can't be found because it doesn't have a valid default constructor.
            Assert.IsNull(registration);
        }

        [TestMethod]
        public void TestMethod4()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient(typeof(ITestObject), _ => new TestObjectA(), "NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient(typeof(ITestObject), _ => new TestObjectB("Dynamically generated object!"), "NumeroDos");

            var provider = collection.BuildServiceProvider();

            var registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroDos");

            //This registration can be found because of the constructor call in the factory method.
            Assert.IsNotNull(registration);

            Debug.WriteLine(registration);
        }

        [TestMethod]
        public void TestMethod5()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient<ITestObject,TestObjectA>("NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient<ITestObject, TestObjectB>("NumeroDos");

            var provider = collection.BuildServiceProvider();

            var registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroDos");

            //This registration can't be found because it doesn't have a valid default constructor.
            Assert.IsNull(registration);
        }

        [TestMethod]
        public void TestMethod6()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient(typeof(TestObjectA), "NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient(typeof(TestObjectB), "NumeroDos");

            var provider = collection.BuildServiceProvider();

            object registration;

            try
            {
                registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");
            }
            catch (MissingRegistrationException mre)
            {
                Assert.AreEqual(typeof(ITestObject), mre.RegistrationType);
                Assert.AreEqual("NumeroUno", mre.RegistrationName);
            } 

            registration = provider.GetNamedService(typeof(TestObjectA), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(TestObjectA), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(TestObjectB), "NumeroDos");

            //This registration can't be found because it doesn't have a valid default constructor.
            Assert.IsNull(registration);
        }

        [TestMethod]
        public void TestMethod7()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient<TestObjectA>("NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient<TestObjectB>("NumeroDos");

            var provider = collection.BuildServiceProvider();

            object registration;

            try
            {
                registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");
            }
            catch (MissingRegistrationException mre)
            {
                Assert.AreEqual(typeof(ITestObject), mre.RegistrationType);
                Assert.AreEqual("NumeroUno", mre.RegistrationName);
            }

            registration = provider.GetNamedService(typeof(TestObjectA), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(TestObjectA), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(TestObjectB), "NumeroDos");

            //This registration can't be found because it doesn't have a valid default constructor.
            Assert.IsNull(registration);
        }

        [TestMethod]
        public void TestMethod8()
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddNamedTransient<ITestObject>(_ => new TestObjectA(), "NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient<ITestObject>(_ => new TestObjectB("Dynamically generated object!"), "NumeroDos");

            var provider = collection.BuildServiceProvider();

            var registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroDos");

            //This registration can be found because of the constructor call in the factory method.
            Assert.IsNotNull(registration);

            Debug.WriteLine(registration);
        }

        [TestMethod]
        public void TestMethod9()
        {
            IServiceCollection collection = new ServiceCollection();
        
            collection.AddNamedTransient<ITestObject, TestObjectA>(_ => new TestObjectA(), "NumeroUno");
            Thread.Sleep(1200);
            collection.AddNamedTransient<ITestObject, TestObjectB>(_ => new TestObjectB("Dynamically generated object!"), "NumeroDos");

            var provider = collection.BuildServiceProvider();

            var registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno"); //TODO: Implement typed lookups

            Assert.IsInstanceOfType(registration, typeof(ITestObject));

            Debug.WriteLine(registration);

            Thread.Sleep(1200);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroUno");

            Debug.WriteLine(registration);

            registration = provider.GetNamedService(typeof(ITestObject), "NumeroDos");

            //This registration can be found because of the constructor call in the factory method.
            Assert.IsNotNull(registration);

            Debug.WriteLine(registration);
        }
    }
}
