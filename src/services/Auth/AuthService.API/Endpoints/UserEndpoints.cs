using AuthService.API.Contracts;
using AuthService.Application.Common.Exceptions;
using AuthService.Application.Contracts;
using AuthService.Application.Domains.Users.Commands;
using FluentValidation;
using MediatR;

namespace AuthService.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/v1/users")
            .WithTags("Users");

        group.MapPost("/", CreateAsync)
            .WithName("CreateUser")
            .Produces<UserDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        return endpoints;
    }

    private static async Task<IResult> CreateAsync(
        CreateUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await sender.Send(
                new CreateUserCommand(request.Email, request.Password, request.FirstName, request.LastName),
                cancellationToken).ConfigureAwait(false);

            return Results.Created($"/api/v1/users/{user.Id}", user);
        }
        catch (ValidationException exception)
        {
            return Results.ValidationProblem(ToValidationErrors(exception));
        }
        catch (ConflictException exception)
        {
            return Results.Problem(
                title: "Conflict",
                detail: exception.Message,
                statusCode: StatusCodes.Status409Conflict);
        }
    }

    private static Dictionary<string, string[]> ToValidationErrors(ValidationException exception)
    {
        return exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
