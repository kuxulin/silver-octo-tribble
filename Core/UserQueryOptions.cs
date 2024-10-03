namespace Core;
public class UserQueryOptions : BaseQueryOptions
{
    public string? PartialUserName { get; set; }
    public string[] FilterRoles { get; set; } = [];
    public bool? IsBlocked { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
