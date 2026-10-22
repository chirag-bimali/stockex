using System.Reflection;
using Application.Abstractions.Security;
using SE.Application.Abstractions.Messaging;
using SE.Application.Abstractions.Security;
using SE.Application.Common.Response;
using SE.Application.Common.Security;

internal sealed class AuthorizationDecorator<TCommand, TResponse>(
    ICommandHandler<TCommand, TResponse> inner,
    IPermissionService permissionService,
    ICurrentUser currentUser
) : ICommandHandler<TCommand, TResponse>
{
    private static readonly string[] Required = typeof(TCommand)
        .GetCustomAttributes<RequirePermissionAttribute>()
        .Select(a => a.Permission)
        .ToArray();

    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
    {
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine($"AuthorizationDecorator: Handling command of type {typeof(TCommand).Name}");
        Console.WriteLine($"Required permissions: {string.Join(", ", Required)}");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("----------------------------------------------");
        if (Required.Length == 0)
            return await inner.Handle(command, ct);

        if (!currentUser.IsAuthenticated)
            return Error.Unauthorized("auth.unauthenticated", "You must be logged in");
        


        foreach (var permission in Required)
        {
            if (
                !await permissionService.HasPermissionAsync(
                    currentUser.Id ?? throw new Exception("Something went wrong."),
                    permission,
                    ct
                )
            )
                return Error.Forbidden(
                    "auth.forbidden",
                    "You do not have permission for this action."
                );
        }

        return await inner.Handle(command, ct);
    }
}
