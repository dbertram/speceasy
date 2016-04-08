using Rhino.Mocks;
using Should;
using Should.Core.Assertions;

namespace SpecEasy.Specs.GenericSpec
{
    public class ManualSUTConstructionSpec : Spec<Mockable>
    {
        private const string ManualConstructedDependencyValue = "Manually Constructed";
        private const string MockedDependencyValue = "Mocked";

        private int constructSUTCallCount;
        private bool shouldManauallyConstructSUT;

        public void ManuallyConstructSUT()
        {
            When("testing a class that might need to be constructed manually", () => { });

            Given("the SUT is never accessed").Verify(() =>
                Then("the method to construct the SUT should never be called", () => constructSUTCallCount.ShouldEqual(0)));

            Given("the SUT needs to be constructed manually", () => shouldManauallyConstructSUT = true).Verify(() =>
            {
                Given("the SUT is accessed", () => EnsureSUT()).Verify(() =>
                {
                    Then("the dependency should be the one manually constructed", () => SUT.Dep1.Value.ShouldEqual(ManualConstructedDependencyValue));

                    Then("any call to get the SUT refers to the same instance", () =>
                    {
                        var instanceA = SUT;
                        var instanceB = SUT;
                        instanceA.ShouldBeSameAs(instanceB);
                    });

                    Then("the method to construct the SUT is called exactly once", () => constructSUTCallCount.ShouldEqual(1));

                    Then("any SUT instance created using Get<T> should refer to the same instance as SUT", () =>
                    {
                        var instanceA = SUT;
                        var instanceB = Get<Mockable>();
                        var instanceC = Get<Mockable>();
                        instanceA.ShouldBeSameAs(instanceB);
                        instanceB.ShouldBeSameAs(instanceC);
                    });
                });
            });

            

            
        }

        protected override Mockable ConstructSUT()
        {
            constructSUTCallCount++;

            if (!shouldManauallyConstructSUT)
            {
                return base.ConstructSUT();
            }

            return new Mockable(new Dependency1Impl(ManualConstructedDependencyValue));
        }

        protected override void BeforeEachExample()
        {
            base.BeforeEachExample();
            constructSUTCallCount = 0;
        }
    }
}