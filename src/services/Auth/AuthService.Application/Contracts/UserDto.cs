namespace AuthService.Application.Contracts;

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt);
