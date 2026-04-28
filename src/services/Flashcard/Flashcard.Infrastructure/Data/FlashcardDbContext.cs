using Microsoft.EntityFrameworkCore;
using Flashcard.Domain.Entities;

namespace Flashcard.Infrastructure.Data;

public class FlashcardDbContext(DbContextOptions<FlashcardDbContext> options) : DbContext(options)
{
    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<Domain.Entities.Flashcard> Flashcards => Set<Domain.Entities.Flashcard>();
    public DbSet<CardReviewLog> CardReviewLogs => Set<CardReviewLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlashcardDbContext).Assembly);
    }
}
