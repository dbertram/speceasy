using Should;

namespace SpecEasy.Specs.GenericSpec
{
    public class ManualSUTConstructionSpec : Spec<Mockable>
    {
        public void Run()
        {
            var ctorFuncCallCount = 0;

            When("testing a class with constructor dependencies that is being constructed manually", () => { }).BuildSUTUsing(() =>
            {
                ctorFuncCallCount++;
                return new Mockable(new Dependency1Impl("manually built"));
            });

            Given("a manually constructed dependency").Verify(() =>
            {
                Then("the constructed depenency is used to construct the SUT", () =>
                {
                    SUT.Dep1.ShouldBeType<Dependency1Impl>();
                    SUT.Dep1.Value.ShouldEqual("manually built");
                });

                Then("the func provided to construct the SUT is called exactly once", () => ctorFuncCallCount.ShouldEqual(1));

                Then("it constructs the SUT exactly once", () =>
                {
                    var sut1 = SUT;
                    var sut2 = SUT;
                    sut1.ShouldBeSameAs(sut2);
                });
            });
        }   
    }
}