using MoneybaseChat.domain.entities;
using MoneybaseChat.domain.enums;
using NUnit.Framework;

namespace MoneybaseChat.domain.tests;

public class TeamTests
{
    [Test]
    public void TeamCapacity_ReturnFullTeamCapacity_WhenMultipleAgents()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Junior));
        team.AddAgent(new("Jason", Seniority.Junior));

        Assert.That(team.TeamCapacity == 13, "Wrong team capacity");
    }

    [Test]
    public void TeamCapacity_ReturnFullTeamCapacity_WhenNoAgents()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);

        Assert.That(team.TeamCapacity == 0, "Wrong team capacity");
    }

    [Test]
    public void TeamCapacity_ReturnFullTeamCapacity_WhenOverflowTeam()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.MarkAsOverflowTeam();
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Junior));
        team.AddAgent(new("Jason", Seniority.Junior));
        team.AddAgent(new("Jason", Seniority.Mid));

        Assert.That(team.TeamCapacity == 0.4 * 10 * team.Agents.Count, "Wrong overflow team capacity");
    }

    [Test]
    public void AssignChat_AssignToJuniorFirst()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Junior));
        team.AddAgent(new("Jason", Seniority.Junior));

        var chatId = Guid.NewGuid();
        team.AssignChat(new ChatRequest(chatId, DateTime.Now));

        Assert.That(team.Agents.Where(a => a.Seniority == Seniority.Junior).Where(j => j.ChatRequests.Any(c => c.Identifier == chatId)).Any(), "new chat should be assigned to junior first");
    }

    [Test]
    public void AssignChat_AssignToMidFirst_WhenNoJuniors()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Lead));
        team.AddAgent(new("Jason", Seniority.Mid));

        var chatId = Guid.NewGuid();
        team.AssignChat(new ChatRequest(chatId, DateTime.Now));

        Assert.That(team.Agents.Where(a => a.Seniority == Seniority.Mid).Where(j => j.ChatRequests.Any(c => c.Identifier == chatId)).Any(), "new chat should be assigned to mids first when no juniors present");
    }

    [Test]
    public void AssignChat_AssignToSeniorFirst_WhenNoMids()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Lead));
        team.AddAgent(new("Jason", Seniority.Senior));

        var chatId = Guid.NewGuid();
        team.AssignChat(new ChatRequest(chatId, DateTime.Now));

        Assert.That(team.Agents.Where(a => a.Seniority == Seniority.Senior).Where(j => j.ChatRequests.Any(c => c.Identifier == chatId)).Any(), "new chat should be assigned to seniors first when no mids present");
    }
    
    [Test]
    public void AssignChat_AssignToLeadFirst_WhenNoSeniors()
    {
        var team =  new Team(Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        team.AddAgent(new("Matilda", Seniority.Lead));
        team.AddAgent(new("Eric", Seniority.Lead));

        var chatId = Guid.NewGuid();
        team.AssignChat(new ChatRequest(chatId, DateTime.Now));

        Assert.That(team.Agents.Where(a=>a.Seniority == Seniority.Lead).Where(j=>j.ChatRequests.Any(c=>c.Identifier == chatId)).Any(), "new chat should be assigned to seniors first when no mids present");
    }
}
