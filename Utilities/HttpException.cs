
namespace SSO_Backend.Utilities;

public class HttpException : Exception
{
    public int StatusCode { get; }

    public HttpException(int statusCode)
    {
        StatusCode = statusCode;
    }

    public HttpException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpException(int statusCode, string message, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}

public class UserNotFoundException : HttpException
{
    public UserNotFoundException() : base(401, "User not found")
    {
    }
}

public class UserInActiveException : HttpException
{
    public UserInActiveException() : base(403, "User is inactive")
    {
    }
}

public class UnAuthorizedException : HttpException
{
    public UnAuthorizedException() : base(403, "You are unauthorized to access this resource.")
    {
    }
}

public class PermissionsNotFoundException : HttpException
{
    public PermissionsNotFoundException() : base(403, "Access to this resource is forbidden. Please contact your administrator for assistance.")
    {
    }
}