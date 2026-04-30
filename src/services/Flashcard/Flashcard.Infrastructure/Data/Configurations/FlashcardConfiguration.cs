using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashcard.Infrastructure.Data.Configurations;

public class FlashcardConfiguration : IEntityTypeConfiguration<Domain.Entities.Flashcard>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Flashcard> builder)
    {
        builder.ToTable("Flashcards");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Front).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Back).HasMaxLength(8000).IsRequired();

        builder.Property(x => x.State).HasConversion<byte>();
        builder.Property(x => x.RowVersion).IsConcurrencyToken().IsRequired();

        builder.HasIndex(x => new { x.UserId, x.DeckId });
        builder.HasIndex(x => x.Due);
        builder.HasIndex(x => new { x.State, x.Due });

        builder.HasOne(x => x.Deck)
            .WithMany(x => x.Flashcards)
            .HasForeignKey(x => x.DeckId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
