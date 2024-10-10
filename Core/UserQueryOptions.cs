namespace Core;
public class UserQueryOptions : BaseQueryOptions
{
    public string? PartialUserName { get; set; }
    public IEnumerable<int>? FilterRoleIds { get; set; }
    public bool? IsBlocked { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
