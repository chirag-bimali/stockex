using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Persistence;
using SE.Application.Common.Response;

namespace SE.Application.Features.Users.GetAllUser;

public class GetAllQueryHandler : IQueryHandler<GetAllUserQuery, GetAllUserResult>
{
    private readonly IUserRepository _userRepository;

    public GetAllQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetAllUserResult>> Handle(
        GetAllUserQuery query,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(
            query.PageNumber,
            query.PageSize,
            cancellationToken
        );
        return new GetAllUserResult(users);
    }
}
