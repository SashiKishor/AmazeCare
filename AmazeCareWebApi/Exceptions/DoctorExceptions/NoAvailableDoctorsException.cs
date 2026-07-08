namespace AmazeCareWebApi.Exceptions.DoctorExceptions
{
    public class NoAvailableDoctorsException:Exception
    {
        public NoAvailableDoctorsException(string message):base(message)
        {
            
        }
    }
}
