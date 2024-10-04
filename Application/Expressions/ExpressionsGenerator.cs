using System.Linq.Expressions;

namespace Application.Expressions;
internal static class ExpressionsGenerator<T>
{
    public static Expression<Func<T, object>> CreateSortExpression(string sortField)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, sortField);
        var sortFunction = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);
        return sortFunction;
    }
}
