using System;
using Rhino.Mocks;
using TinyIoC;

namespace SpecEasy
{
    public partial class Spec
    {
        private ResolveOptions resolveOptions;

        internal ResolveOptions ResolveOptions
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

        internal TinyIoCContainer MockingContainer;

        protected T Get<T>()
        {
            RequireMockingContainer();
            return (T)MockingContainer.Resolve(typeof(T), ResolveOptions);
            //Preferred, but only allows reference types: return MockingContainer.Resolve<T>();
        }

        protected void Set<T>(T item)
        {
            RequireMockingContainer();
            MockingContainer.Register(typeof(T), item);
        }

        protected T Mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        private void InitializeMockingContainer()
        {
            MockingContainer = new TinyIoCContainer();
        }

        private void RequireMockingContainer()
        {
            if (MockingContainer == null)
            {
                throw new InvalidOperationException("This method cannot be called before the test context is initialized.");
            }
        }

        private object TryAutoMock(TinyIoCContainer.TypeRegistration registration, TinyIoCContainer container)
        {
            var type = registration.Type;
            return type.IsInterface || type.IsAbstract ? MockRepository.GenerateMock(type, new Type[0]) : null;
        }
    }
}
