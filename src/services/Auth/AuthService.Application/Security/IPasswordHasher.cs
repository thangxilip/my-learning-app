namespace AuthService.Application.Security;

public interface IPasswordHasher
{
    PasswordHashResult HashPassword(string password);

    bool VerifyPassword(string password, byte[] salt, string storedHashBase64);
}

public sealed record PasswordHashResult(byte[] Salt, string PasswordHash);
