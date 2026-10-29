namespace SE.Application.Features.Users.GetAllUser;

public sealed record GetAllUserQuery(int PageNumber = 1, int PageSize = 10);
