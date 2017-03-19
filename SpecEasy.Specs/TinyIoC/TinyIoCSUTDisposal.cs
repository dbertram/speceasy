using System;
using NUnit.Framework;

namespace SpecEasy.Specs.TinyIoC
{
    internal sealed class TinyIoCSUTDisposal : Spec<DisposableSubscriber>
    {
        public void SUTDisposal()
        {
            When("the updateable is updated", () => Raise<IUpdateable>(updateable => updateable.Updated += null, Get<IUpdateable>(), EventArgs.Empty));

            Given("SUT is constructed automatically", () => EnsureSUT()).Verify(() =>
                Then("the updated count should be 1", () => Assert.AreEqual(1, SUT.UpdatedCount)).
                Then("SUT has not been disposed", () => Assert.IsFalse(SUT.Disposed)));

            Given("SUT is constructed explicitly", () => SUT = new DisposableSubscriber(Get<IUpdateable>())).Verify(() =>
                Then("the updated count should be 1", () => Assert.AreEqual(1, SUT.UpdatedCount)).
                Then("SUT has not been disposed", () => Assert.IsFalse(SUT.Disposed)));
        }
    }
}