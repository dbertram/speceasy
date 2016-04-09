using System;

namespace SpecEasy
{
    public abstract class TestCaseCreationException : InvalidOperationException
    {
        private readonly string stackTrace;

        internal TestCaseCreationException(string description, string stackTrace) : base("Failed to generate test cases; " + description)
        {
            this.stackTrace = stackTrace;
        }

        public override string StackTrace
        {
            get
            {
                return stackTrace;
            }
        }
    }
}
