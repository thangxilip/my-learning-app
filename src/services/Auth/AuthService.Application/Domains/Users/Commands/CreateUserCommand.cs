using AuthService.Application.Common.Exceptions;
using AuthService.Application.Contracts;
using AuthService.Application.Security;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AuthService.Application.Domains.Users.Commands;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<UserDto>;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(256)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(128)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
    }
}

public sealed class CreateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await unitOfWork.Users.ExistsAsync(
            user => user.Email == email,
            cancellationToken).ConfigureAwait(false);

        if (emailExists)
        {
            throw new ConflictException("A user with this email already exists.");
        }

        var passwordHash = passwordHasher.HashPassword(request.Password);
        var user = new User
        {
            Email = email,
            Salt = passwordHash.Salt,
            PasswordHash = passwordHash.PasswordHash,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            IsActive = true
        };

        await unitOfWork.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.IsActive,
            user.CreatedAt);
    }
}
