using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.infrastructure.persistence.configurations;

public class ChatRequestEntityConfiguration : IEntityTypeConfiguration<ChatRequest>
{
    public void Configure(EntityTypeBuilder<ChatRequest> builder)
    {
        builder.ToTable("chat_request");
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.HasIndex(e => e.Identifier, "unique_chat_request_identifier").IsUnique();
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Identifier).HasColumnName("identifier");
        builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(e => e.LastPulse).HasColumnName("last_pulse").IsRequired(false);
        builder.Property(e => e.CreatedOn).HasColumnName("created_on");
        builder.Property(e => e.Agent).HasColumnName("agent").IsRequired(false);
        builder.HasOne(e => e.AgentNavigation).WithMany(e => e.ChatRequests).IsRequired(false).HasForeignKey(e => e.Agent);
    }
}
