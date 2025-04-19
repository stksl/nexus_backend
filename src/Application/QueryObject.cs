using System.Linq.Expressions;

namespace Nexus.Application;

public class QueryObject<T> where T : class
{
    public Func<T, bool>? Filter {get; set;}
    public LambdaExpression? SortBy {get; set;}
    public bool SortByAscending {get; set;} // the value is ignored when QueryObject`1.SortBy is null
    public int PageNumber {get; set;} = 1;
    public int PageSize {get; set;} = 20;
}