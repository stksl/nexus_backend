namespace Nexus.Application;

public class Result 
{
    public bool Succeed {get; init;}
    protected Result()
    {
        
    }
    public static Result Success() => new Result() {Succeed = true};
    public static Result Success<TResult>(TResult resultValue) => new Result<TResult>() {Succeed = true, ResultValue = resultValue};
    public static Result Failed() => new Result() {Succeed = false};
    public static Result Failed<TResult>(TResult resultValue) => new Result<TResult>() {Succeed = false, ResultValue = resultValue};

} 
public class Result<TResult> : Result 
{
    public TResult? ResultValue {get; init;}
} 