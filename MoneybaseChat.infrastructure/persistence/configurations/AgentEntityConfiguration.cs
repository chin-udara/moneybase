using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.infrastructure.persistence.configurations;

public class AgentEntityConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.ToTable("agent");
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
        builder.Property(e => e.Seniority).HasColumnName("seniority");
        builder.Property(e => e.Team).HasColumnName("team");
        builder.HasOne(e => e.TeamNavigation).WithMany(e => e.Agents).HasForeignKey(e => e.Team);

        builder.Ignore(e => e.Capacity);
        builder.Ignore(e => e.AvailableCapacity);
    }
}
