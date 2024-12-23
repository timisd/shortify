using System.Linq.Expressions;
using Shortify.Persistence.Models;

namespace Shortify.Persistence;

public static class QueryHelper
{
    public static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter)
    {
        // Apply filters
        query = (from expression in filter.FilterExpressions
                let parameter = Expression.Parameter(typeof(T), "x")
                let property = Expression.Property(parameter, expression.PropertyName)
                let value = Expression.Constant(expression.Value)
                let comparison = expression.Relation switch
                {
                    RelationType.Equal => Expression.Equal(property, value),
                    RelationType.NotEqual => Expression.NotEqual(property, value),
                    RelationType.Larger => Expression.GreaterThan(property, value),
                    RelationType.LargerOrEqual => Expression.GreaterThanOrEqual(property, value),
                    RelationType.Smaller => Expression.LessThan(property, value),
                    RelationType.SmallerOrEqual => Expression.LessThanOrEqual(property, value),
                    _ => throw new NotSupportedException($"Relation type {expression.Relation} is not supported")
                }
                select Expression.Lambda<Func<T, bool>>(comparison, parameter))
            .Aggregate(query, (current, lambda) => current.Where(lambda));

        // Apply ordering
        if (string.IsNullOrEmpty(filter.OrderBy)) return query;
        {
            var orderByParts = filter.OrderBy.Split(' ');
            var propertyName = orderByParts[0];
            var isDescending = orderByParts.Length > 1 && orderByParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            query = isDescending ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
        }

        return query;
    }
}