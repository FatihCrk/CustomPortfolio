namespace Portfolio.Shared.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; protected set; }

    protected BaseException(string message) : base(message) { }
    protected BaseException(string message, Exception innerException) : base(message, innerException) { }
}

public class ValidationException : BaseException
{
    public List<string> Errors { get; }

    public ValidationException(List<string> errors) : base("Doğrulama hatası oluştu.")
    {
        Errors = errors;
        StatusCode = 400;
    }

    public ValidationException(string error) : base(error)
    {
        Errors = new List<string> { error };
        StatusCode = 400;
    }
}

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message) : base(message)
    {
        StatusCode = 401;
    }
}

public class ForbiddenException : BaseException
{
    public ForbiddenException(string message) : base(message)
    {
        StatusCode = 403;
    }
}

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message)
    {
        StatusCode = 404;
    }

    public NotFoundException(string entityName, object id) 
        : base($"{entityName} ({id}) bulunamadı.")
    {
        StatusCode = 404;
    }
}

public class BusinessException : BaseException
{
    public BusinessException(string message) : base(message)
    {
        StatusCode = 400;
    }
}

public class ConflictException : BaseException
{
    public ConflictException(string message) : base(message)
    {
        StatusCode = 409;
    }
}

public class RateLimitException : BaseException
{
    public RateLimitException(string message = "Çok fazla istek. Lütfen bekleyin.") : base(message)
    {
        StatusCode = 429;
    }
}
