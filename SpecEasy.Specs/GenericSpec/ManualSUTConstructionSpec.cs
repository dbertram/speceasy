using Should;

namespace SpecEasy.Specs.GenericSpec
{
    public class ManualSUTConstructionSpec : Spec<Mockable>
    {
        private int constructSutCallCount;

        public void Run()
        {
            When("testing a class with constructor dependencies that is being constructed manually", () => { });

            Given("the SUT is never accessed").Verify(() =>
                Then("the method to construct the SUT should never be called", () => constructSutCallCount.ShouldEqual(0)));

            Given("the SUT is accessed", () => EnsureSUT()).Verify(() =>
            {
                Then("the constructed depenency is used to construct the SUT", () =>
                {
                    SUT.Dep1.ShouldBeType<Dependency1Impl>();
                    SUT.Dep1.Value.ShouldEqual("manually built");
                });

                Then("any call to get the SUT refers to the same instance", () =>
                {
                    var sut1 = SUT;
                    var sut2 = SUT;
                    sut1.ShouldBeSameAs(sut2);
                });

                Then("the func provided to construct the SUT is called exactly once", () => constructSutCallCount.ShouldEqual(1));
            });
        }

        protected override Mockable ConstructSUT()
        {
            constructSutCallCount++;
            return new Mockable(new Dependency1Impl("manually built"));
        }
    }
}