namespace Entities.Exceptions
{
    public sealed class OrderBooksDataNullOrEmptyException : NullOrEmptyException
    {
        public OrderBooksDataNullOrEmptyException()
            : base("OrderBooksData")
        {
        }
    }
}
