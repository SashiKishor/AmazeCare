namespace AmazeCareWebApi.Exceptions.UserException
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
