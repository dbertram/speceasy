using System;

namespace SpecEasy
{
    internal class TestSetup<TUnit> : ITestSetup<TUnit>
    {
        private Func<TUnit> overrideBuildSUTFunc;
        private TUnit manuallyBuiltSUT;
        private readonly Func<TUnit> defaultBuildSUTFunc;  

        internal TestSetup(Func<TUnit> defaultBuildSUTFunc)
        {
            this.defaultBuildSUTFunc = defaultBuildSUTFunc;
        }

        public void BuildSUTUsing(Func<TUnit> buildSUTFunc)
        {
            overrideBuildSUTFunc = buildSUTFunc;
        }

        public TUnit BuildSUT()
        {
            if (overrideBuildSUTFunc == null)
            {
                return defaultBuildSUTFunc();
            }
            
            if (manuallyBuiltSUT == null)
            {
                manuallyBuiltSUT = overrideBuildSUTFunc();
            }

            return manuallyBuiltSUT;
        }
    }
}