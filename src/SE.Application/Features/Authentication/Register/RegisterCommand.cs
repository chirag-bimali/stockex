namespace SE.Application.Features.Authentication.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName);
