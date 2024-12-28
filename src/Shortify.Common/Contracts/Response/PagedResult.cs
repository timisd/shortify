namespace Shortify.Common.Contracts.Response;

public class PagedResult<T>
{
    public required IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}