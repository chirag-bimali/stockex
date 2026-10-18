namespace SE.Application.Features.Authentication.Login;
public record LoginResult(string AccessToken, DateTime ExpiresAtUtc);
