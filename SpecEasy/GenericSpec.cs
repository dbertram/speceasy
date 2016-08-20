using System;
using System.Reflection;
using Rhino.Mocks;

namespace SpecEasy
{
    public class Spec<TUnit> : Spec
    {
        protected TUnit SUT
        {
            get { return GetSUTInstance(); }
            set
            {
                constructedSUTInstance = value;
                Set(value);
                alreadyConstructedSUT = true;
            }
        }

        private bool alreadyConstructedSUT;
        private TUnit constructedSUTInstance;

        protected virtual TUnit ConstructSUT()
        {
            return Get<TUnit>();
        }

        protected void EnsureSUT()
        {
            SUT = SUT;
        }

        internal override void BeforeEachInit()
        {
            base.BeforeEachInit();

            alreadyConstructedSUT = false;

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

        private TUnit GetSUTInstance()
        {
            if (!alreadyConstructedSUT)
            {
                constructedSUTInstance = ConstructSUT();

                if (constructedSUTInstance == null)
                {
                    throw new InvalidOperationException("Failed to construct SUT: ConstructSUT returned null");
                }

                Set(constructedSUTInstance);
                alreadyConstructedSUT = true;
            }

            return constructedSUTInstance;
        }
    }
}