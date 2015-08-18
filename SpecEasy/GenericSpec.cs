using System;
using System.Reflection;
using System.Threading.Tasks;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using TinyIoC;

namespace SpecEasy
{
    public class Spec<TUnit> : Spec
    {
        internal TinyIoCContainer MockingContainer;
        private Func<TUnit> sutBuilderFunc;
        private TestSetup<TUnit> testSetup;  
        private TUnit constructedSUTInstance;

        protected T Mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        private void RequireMockingContainer()
        {
            if (MockingContainer == null)
                throw new InvalidOperationException(
                    "This method cannot be called before the test context is initialized.");
        }

        private ResolveOptions resolveOptions;
        private ResolveOptions ResolveOptions
        {
            get
            {
                return resolveOptions ?? (resolveOptions = new ResolveOptions
                {
                    UnregisteredResolutionRegistrationOption = UnregisteredResolutionRegistrationOptions.RegisterAsSingleton,
                    FallbackResolutionAction = TryAutoMock
                });
            }
        }

        protected T Get<T>()
        {
            RequireMockingContainer();
            return (T)MockingContainer.Resolve(typeof(T), ResolveOptions);
            //Preferred, but only allows reference types: return MockingContainer.Resolve<T>();
        }

        private object TryAutoMock(TinyIoCContainer.TypeRegistration registration, TinyIoCContainer container)
        {
            var type = registration.Type;
            return type.IsInterface || type.IsAbstract ? MockRepository.GenerateMock(type, new Type[0]) : null;
        }

        private TUnit GetSUTInstance()
        {
            if (testSetup == null)
            {
                return Get<TUnit>();
            }

            return testSetup.BuildSUT();
        }

        protected void Set<T>(T item)
        {
            RequireMockingContainer();
            MockingContainer.Register(typeof(T), item);
        }

        protected void Raise<T>(Action<T> eventSubscription, params object[] args) where T : class
        {
            var mock = Get<T>();
            mock.Raise(eventSubscription, args);
        }

        protected new ITestSetup<TUnit> When(string description, Action task)
        {
            testSetup = new TestSetup<TUnit>(Get<TUnit>);
            base.When(description, task);
            return testSetup;
        }

        protected new ITestSetup<TUnit> When(string description, Func<Task> func)
        {
            testSetup = new TestSetup<TUnit>(Get<TUnit>);
            base.When(description, func);
            return testSetup;
        } 

        protected void AssertWasCalled<T>(Action<T> action)
        {
            var mock = Get<T>();
            mock.AssertWasCalled(action);
        }

        protected void AssertWasCalled<T>(Action<T> action, Action<IMethodOptions<object>> methodOptions)
        {
            var mock = Get<T>();
            mock.AssertWasCalled(action, methodOptions);
        }

        protected void AssertWasNotCalled<T>(Action<T> action)
        {
            var mock = Get<T>();
            mock.AssertWasNotCalled(action);
        }

        protected void AssertWasNotCalled<T>(Action<T> action, Action<IMethodOptions<object>> methodOptions)
        {
            var mock = Get<T>();
            mock.AssertWasNotCalled(action, methodOptions);
        }

        protected override void BeforeEachExample()
        {
            base.BeforeEachExample();
            MockingContainer = new TinyIoCContainer();

            if (typeof (TUnit).IsAbstract)
            {
                MockingContainer.Register(typeof(TUnit), (ioc, namedParameterOverloads) =>
                {
                    var constructor = ioc.GetConstructor(typeof (TUnit), namedParameterOverloads, ResolveOptions, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    var args = ioc.ResolveConstructorParameters(constructor, namedParameterOverloads, ResolveOptions);
                    return MockRepository.GeneratePartialMock<TUnit>(args);
                }).AsSingleton();
            }
            else
            {
                MockingContainer.Register(typeof(TUnit)).AsSingleton();
            }
        }

        protected void EnsureSUT()
        {
            Get<TUnit>();
        }

        protected TUnit SUT
        {
            get { return GetSUTInstance(); }
            set { Set(value); }
        }
    }
}