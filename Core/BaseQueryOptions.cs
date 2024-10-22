namespace Core;
public class BaseQueryOptions
{
    public int? PageSize { get; set; }
    public int PageIndex { get; set; } = 0;
    public string? SortField { get; set; }
    public bool SortByDescending { get; set; } = false;

    public int GetStartIndex()
    {
        var pageSize = PageSize.HasValue ? PageSize.Value : 1;
        return PageIndex * pageSize;
    }
}