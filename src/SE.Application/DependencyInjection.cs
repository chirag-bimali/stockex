using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SE.Application;
using SE.Application.Abstractions.Behaviour;
using SE.Application.Abstractions.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationAssemblyMarker).Assembly;

        // register all command and query handlers
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(c =>
                    c.AssignableTo(typeof(ICommandHandler<,>))
                        .Where(t => !t.IsGenericTypeDefinition)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(c =>
                    c.AssignableTo(typeof(IQueryHandler<,>)).Where(t => !t.IsGenericTypeDefinition)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        // Add all validators from the assembly
        services.AddValidatorsFromAssembly(assembly);

        // services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingQueryDecorator<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(AuthorizationDecorator<,>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingCommandDecorator<,>));

        return services;
    }
}
