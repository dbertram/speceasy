using System;
using System.IO;
using Should;

namespace SpecEasy.Specs.GenericSpec.SupportingExamples
{
    [SupportingExample]
    internal class AccessMockingContainerOutsideOfContextSpec : Spec<object>
    {
        public void UseGetOutOfContext()
        {
            When("a spec calls Get<T> outside of a context action body", () => { });

            Get<IDisposable>().ShouldNotBeNull();
        }

        public void UseSetOutOfContext()
        {
            When("a spec calls Set<T> outside of a context action body", () => { });

            Set<IDisposable>(new MemoryStream());
        }

        public void UseSUTOutOfContext()
        {
            When("a spec accesses SUT outside of a context action body", () => { });

            SUT.ShouldNotBeNull();
        }
    }
}
