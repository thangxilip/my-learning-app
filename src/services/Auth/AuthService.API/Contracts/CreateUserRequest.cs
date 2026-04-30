namespace AuthService.API.Contracts;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName);
