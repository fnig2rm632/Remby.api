namespace ECommerce.Domain.Common;

public class Result
{
    public bool IsSuccess { get;}
    public Error Error { get;}
    
    private Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }
    
    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Success() => new();
    private static Result Failure(Error error) => new(error);
    
    public static implicit operator Result(Error error) => Failure(error);
    
}