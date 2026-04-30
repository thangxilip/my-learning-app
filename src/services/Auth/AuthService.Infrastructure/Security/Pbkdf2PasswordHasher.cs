using System.Security.Cryptography;
using AuthService.Application.Security;

namespace AuthService.Infrastructure.Security;

public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSizeInBytes = 16;
    private const int KeySizeInBytes = 32;
    private const int Iterations = 210_000;

    public PasswordHashResult HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var salt = RandomNumberGenerator.GetBytes(SaltSizeInBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySizeInBytes);

        return new PasswordHashResult(salt, Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string password, byte[] salt, string storedHashBase64)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentNullException.ThrowIfNull(salt);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedHashBase64);

        byte[] expectedHash;
        try
        {
            expectedHash = Convert.FromBase64String(storedHashBase64);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySizeInBytes);

        return expectedHash.Length == actualHash.Length
               && CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}
