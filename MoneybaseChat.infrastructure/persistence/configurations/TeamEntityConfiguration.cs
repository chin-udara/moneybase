using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.infrastructure.persistence.configurations;

public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("team");
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.HasIndex(e => e.Identifier).IsUnique();
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Identifier).HasColumnName("identifier");
        builder.Property(e => e.IsOverflowTeam).HasColumnName("is_overflow_team");
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
        builder.Property(e => e.ShiftDurationInMinutes).HasColumnName("shift_duration_minutes");
        builder.Property(e => e.ShiftStartTime).HasColumnName("shift_start_time").HasConversion(new ValueConverter<TimeOnly, TimeSpan>(
            v => v.ToTimeSpan(),
            v => TimeOnly.FromTimeSpan(v)
        )).HasColumnType("TIME");

        builder.Navigation(e => e.Agents).AutoInclude();
        builder.Ignore(e => e.TeamCapacity);
    }
}
