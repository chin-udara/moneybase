using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MoneybaseChat.domain.entities;
using MoneybaseChat.infrastructure.persistence.configurations;

namespace MoneybaseChat.infrastructure.persistence;

public class MoneybaseChatContext : DbContext
{
    public DbSet<Agent> Agents { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ChatRequest> ChatRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AgentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ChatRequestEntityConfiguration());
    }

    public MoneybaseChatContext()
    {
    }

    public MoneybaseChatContext(DbContextOptions<MoneybaseChatContext> options)
        : base(options)
    {
    }
}
