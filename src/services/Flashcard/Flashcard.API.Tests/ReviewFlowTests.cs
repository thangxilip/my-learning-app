using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Flashcard.API.Tests.TestSupport;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Flashcard.API.Tests;

public class ReviewFlowTests : IClassFixture<FlashcardApiFactory>
{
    private readonly FlashcardApiFactory _factory;

    public ReviewFlowTests(FlashcardApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateDeckCardAndSubmitReview_Works()
    {
        var userId = Guid.NewGuid();
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateJwt(userId));

        var createDeckResponse = await client.PostAsJsonAsync("/api/v1/decks", new
        {
            name = "Spanish A1",
            description = "Verb cards",
            sortOrder = 1
        });
        createDeckResponse.EnsureSuccessStatusCode();

        var deck = await createDeckResponse.Content.ReadFromJsonAsync<DeckResponse>();
        Assert.NotNull(deck);

        var now = DateTime.UtcNow;
        var createCardResponse = await client.PostAsJsonAsync($"/api/v1/decks/{deck!.Id}/flashcards", new
        {
            front = "ser",
            back = "to be",
            card = new
            {
                due = now,
                stability = 0.0,
                difficulty = 0.0,
                elapsed_days = 0.0,
                scheduled_days = 0.0,
                reps = 0,
                lapses = 0,
                state = 0,
                last_review = (DateTime?)null
            }
        });
        createCardResponse.EnsureSuccessStatusCode();

        var flashcard = await createCardResponse.Content.ReadFromJsonAsync<FlashcardResponse>();
        Assert.NotNull(flashcard);
        Assert.False(string.IsNullOrWhiteSpace(flashcard!.RowVersion));

        var reviewAt = now.AddMinutes(1);
        var submitReviewResponse = await client.PostAsJsonAsync($"/api/v1/flashcards/{flashcard.Id}/reviews", new
        {
            card = new
            {
                due = now.AddDays(1),
                stability = 1.2,
                difficulty = 4.7,
                elapsed_days = 1.0,
                scheduled_days = 1.0,
                reps = 1,
                lapses = 0,
                state = 1,
                last_review = reviewAt
            },
            log = new
            {
                rating = 3,
                state = 0,
                scheduled_days = 1.0,
                review = reviewAt
            },
            expectedRowVersion = flashcard.RowVersion,
            clientMutationId = Guid.NewGuid().ToString("N")
        });
        submitReviewResponse.EnsureSuccessStatusCode();

        var reviewResult = await submitReviewResponse.Content.ReadFromJsonAsync<ReviewSyncResponse>();
        Assert.NotNull(reviewResult);
        Assert.Equal(1, reviewResult!.Flashcard.Card.Reps);
        Assert.Equal(3, reviewResult.ReviewLog.Rating);
    }

    private static string CreateJwt(Guid userId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("integration-tests-signing-key-at-least-32")),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private sealed record DeckResponse(Guid Id, string Name, string? Description, int SortOrder);

    private sealed record FlashcardResponse(Guid Id, string RowVersion, FsrsCardResponse Card);

    private sealed record FsrsCardResponse(int Reps);

    private sealed record ReviewLogResponse(byte Rating);

    private sealed record ReviewSyncResponse(FlashcardResponse Flashcard, ReviewLogResponse ReviewLog);
}
