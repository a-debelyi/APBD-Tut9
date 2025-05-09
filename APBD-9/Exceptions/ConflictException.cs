namespace APBD_8.Exceptions;

public class ConflictException : ApiException
{
    public ConflictException(string message) : base(StatusCodes.Status409Conflict, message)
    {
    }
}