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
                let comparison = expression.Operator switch
                {
                    FilterOperator.Equal => CreateComparison(ExpressionType.Equal, property, value),
                    FilterOperator.NotEqual => CreateComparison(ExpressionType.NotEqual, property, value),
                    FilterOperator.GreaterThan => Expression.GreaterThan(property, value),
                    FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(property, value),
                    FilterOperator.LessThan => Expression.LessThan(property, value),
                    FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(property, value),
                    _ => throw new NotSupportedException($"Relation type {expression.Operator} is not supported")
                }
                select Expression.Lambda<Func<T, bool>>(comparison, parameter))
            .Aggregate(query, (current, lambda) => current.Where(lambda));

        // Apply ordering
        if (string.IsNullOrEmpty(filter.OrderBy)) return query;
        {
            var orderByParts = filter.OrderBy.Split(' ');
            var propertyName = orderByParts[0];
            var isDescending = orderByParts.Length > 1 &&
                               orderByParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            query = isDescending ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
        }

        return query;
    }

    private static Expression CreateComparison(ExpressionType comparisonType, MemberExpression property,
        ConstantExpression value)
    {
        if (property.Type != typeof(Guid?) || value.Type != typeof(string))
            return Expression.MakeBinary(comparisonType, property, value);

        if (value.Value == null)
            return Expression.MakeBinary(comparisonType, property, Expression.Constant(null, typeof(Guid?)));

        var guidValue = Guid.Parse((string)value.Value);
        var guidConstant = Expression.Constant(guidValue, typeof(Guid?));
        return Expression.MakeBinary(comparisonType, property, guidConstant);
    }
}