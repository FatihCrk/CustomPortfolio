using MediatR;

namespace Portfolio.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    Guid? RoleId = null,
    bool? IsActive = null
) : IRequest<PagedResult<UserDto>>;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
