namespace Entities.Exceptions
{
    public abstract class NullOrEmptyException : Exception
    {
        protected NullOrEmptyException(string varName)
            : base($"{varName} is null or empty.")
        {
        }
    }
}
