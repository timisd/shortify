namespace Shortify.Common.Contracts.Requests;

public class Filter
{
    public int StartPage { get; set; } = -1;
    public int ItemsPerPage { get; set; } = -1;
    public string? OrderBy { get; set; }

    public List<FilterExpression> FilterExpressions { get; set; }
        = [];
}

public class FilterExpression
{
    public required string PropertyName { get; set; }
    public FilterOperator Operator { get; set; }
    public required string Value { get; set; }
}

public enum FilterOperator
{
    Equal = 0,
    NotEqual = 1,
    GreaterThan = 2,
    GreaterThanOrEqual = 3,
    LessThan = 4,
    LessThanOrEqual = 5
}