using Flashcard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flashcard.Infrastructure.Data.Configurations;

public class CardReviewLogConfiguration : IEntityTypeConfiguration<CardReviewLog>
{
    public void Configure(EntityTypeBuilder<CardReviewLog> builder)
    {
        builder.ToTable("CardReviewLogs");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Rating).HasConversion<byte>();
        builder.Property(x => x.State).HasConversion<byte>();
        builder.Property(x => x.ClientMutationId).HasMaxLength(100);

        builder.HasIndex(x => x.FlashcardId);
        builder.HasIndex(x => new { x.FlashcardId, x.ClientMutationId })
            .IsUnique()
            .HasFilter("\"ClientMutationId\" IS NOT NULL");

        builder.HasOne(x => x.Flashcard)
            .WithMany(x => x.ReviewLogs)
            .HasForeignKey(x => x.FlashcardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
