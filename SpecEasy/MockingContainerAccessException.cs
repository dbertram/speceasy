namespace SpecEasy
{
    public class MockingContainerAccessException : TestCaseCreationException
    {
        internal MockingContainerAccessException(string caller, string stackTrace)
            : base(string.Format("the {0} method can only be called from within a spec context (Given, When, Then, etc.)", caller), stackTrace)
        {
        }
    }
}
