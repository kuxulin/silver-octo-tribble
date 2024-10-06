namespace Core;
public sealed class PagedResult<TResult>
{
    public IEnumerable<TResult> Items { get; set; }
    public int TotalCount { get; set; }

    public PagedResult(IEnumerable<TResult> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }
}
