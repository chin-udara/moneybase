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

        // seed initial data
        var teamA = new Team(1, Guid.NewGuid(), "Team A", new(0, 0, 0), 480, false);
        var teamB = new Team(2, Guid.NewGuid(), "Team B", new(8, 0, 0), 480, false);
        var teamC = new Team(3, Guid.NewGuid(), "Team C", new(16, 0, 0), 480, false);
        modelBuilder.Entity<Team>().HasData(
            teamA, teamB, teamC
        );
        var agent_Erica = new Agent(1, "Erica", domain.enums.Seniority.Lead);
        var agent_James = new Agent(2, "James", domain.enums.Seniority.Senior);
        var agent_Michelle = new Agent(3, "Michelle", domain.enums.Seniority.Mid);
        var agent_Tim = new Agent(4, "Tim", domain.enums.Seniority.Junior);
        agent_Erica.SetTeam(teamA);
        agent_James.SetTeam(teamA);
        agent_Michelle.SetTeam(teamB);
        agent_Tim.SetTeam(teamC);

        modelBuilder.Entity<Agent>().HasData(
            agent_Erica, agent_James, agent_Michelle, agent_Tim
        );
    }

    public MoneybaseChatContext()
    {
    }

    public MoneybaseChatContext(DbContextOptions<MoneybaseChatContext> options)
        : base(options)
    {
    }
}
