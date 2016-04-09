namespace SpecEasy
{
    public class DuplicateDescriptionException : TestCaseCreationException
    {
        internal DuplicateDescriptionException(string typeOfDuplicate, string description, string stackTrace)
            : base(string.Format("{0} description '{1}' has already been used", typeOfDuplicate, description), stackTrace)
        {
        }
    }
}