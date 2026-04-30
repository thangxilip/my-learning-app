namespace AuthService.Application.Security;

public interface IPasswordHasher
{
    PasswordHashResult HashPassword(string password);
}

public sealed record PasswordHashResult(byte[] Salt, string PasswordHash);
