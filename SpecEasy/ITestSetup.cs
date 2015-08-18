using System;

namespace SpecEasy
{
    public interface ITestSetup<TUnit>
    {
        void BuildSUTUsing(Func<TUnit> buildSUTFunc);
    }
}