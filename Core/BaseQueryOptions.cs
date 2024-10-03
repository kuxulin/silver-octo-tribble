namespace Core;
public class BaseQueryOptions
{
    public int PageSize { get; set; } = 20;
    public int PageIndex { get; set; } = 0;
    public string? SortField { get; set; }
    public bool SortByDescending { get; set; } = false;

    public int GetStartIndex()
    {
        return PageIndex * PageSize;
    }
}