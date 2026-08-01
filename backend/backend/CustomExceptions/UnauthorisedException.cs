namespace Backend.CustomExceptions
{
    public class UnauthorisedException : Exception
    {
        public UnauthorisedException(string message) : base(message) { }
    }
}
