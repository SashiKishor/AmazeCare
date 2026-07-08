namespace AmazeCareWebApi.Exceptions.UserException
{
    public class InvalidPasswordException:Exception
    {
        public InvalidPasswordException(string message):base(message)
        {
            
        }
    }
}
