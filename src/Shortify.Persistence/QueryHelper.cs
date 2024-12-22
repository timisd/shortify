using System.Linq.Expressions;
using Shortify.Persistence.Models;

namespace Shortify.Persistence;

public static class QueryHelper
{
    public static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter)
    {
        foreach (var expression in filter.FilterExpressions)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, expression.PropertyName);
            var value = Expression.Constant(expression.Value);
            Expression comparison = expression.Relation switch
            {
                RelationType.Equal => Expression.Equal(property, value),
                RelationType.NotEqual => Expression.NotEqual(property, value),
                RelationType.Larger => Expression.GreaterThan(property, value),
                RelationType.LargerOrEqual => Expression.GreaterThanOrEqual(property, value),
                RelationType.Smaller => Expression.LessThan(property, value),
                RelationType.SmallerOrEqual => Expression.LessThanOrEqual(property, value),
                _ => throw new NotSupportedException($"Relation type {expression.Relation} is not supported")
            };
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}