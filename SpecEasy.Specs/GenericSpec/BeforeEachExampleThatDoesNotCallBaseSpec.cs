using Should;

namespace SpecEasy.Specs.GenericSpec
{
    public class BeforeEachExampleThatDoesNotCallBaseSpec : Spec<BeforeEachExampleThatDoesNotCallBaseSpec.MySUT>
    {
        public void Run()
        {
            When("running a spec that doesn't call base.BeforeEachExample", () => {});

            Then("using Get<T> should not throw", () => Get<IBaz>().ShouldNotBeNull());

            Then("using Set<T> should not throw", () => Set<IQux>(new Qux()));
        }

        protected override void BeforeEachExample()
        {
            // intentionally omit calling base.BeforeEachExample()

            Get<IFoo>().ShouldNotBeNull(); // using Get<T> should not throw

            Set<IBar>(new Bar()); // using Set<T> should not throw

            SUT.ShouldNotBeNull(); // accessing SUT should not throw
        }

        public interface IFoo { }

        public interface IBar { }

        public interface IBaz { }

        public interface IQux { }

        public class Bar : IBar { }

        public class Qux : IQux { }

        public class MySUT { }
    }
}
