namespace SE.Application.Features.Users.CreateUser;

public record CreateUserCommand(string Email, string Password, string FirstName, string LastName);
