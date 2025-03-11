namespace Nexus.Application;

public class Result 
{
    public bool Succeed {get; init;}
    protected Result()
    {
        
    }
    /// <summary>
    /// Returns a plain success result
    /// </summary>
    /// <returns></returns>
    public static Result Success() => new Result() {Succeed = true};
    /// <summary>
    /// Returns a succeeded result with given <paramref name="resultValue"/>
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="resultValue"></param>
    /// <returns></returns>
    public static Result Success<TResult>(TResult resultValue) => new Result<TResult>() {Succeed = true, ResultValue = resultValue};
    /// <summary>
    /// Returns a plain failed result
    /// </summary>
    /// <returns></returns>
    public static Result Failed() => new Result() {Succeed = false};
    /// <summary>
    /// Returns a failed result with given <paramref name="resultValue"/>
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="resultValue"></param>
    /// <returns></returns>
    public static Result Failed<TResult>(TResult resultValue) => new Result<TResult>() {Succeed = false, ResultValue = resultValue};

} 
public class Result<TResult> : Result 
{
    public TResult? ResultValue {get; init;}
} 