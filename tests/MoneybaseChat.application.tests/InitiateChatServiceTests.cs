using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using MoneybaseChat.application.services;
using MoneybaseChat.domain.entities;
using MoneybaseChat.domain.enums;
using Moq;
using NUnit.Framework;

namespace MoneybaseChat.application.tests;

public class InitiateChatServiceTests
{
    [Test]
    public async Task InitiateChat_SuccessfulInitiation_WhenCapacityAvailable()
    {
        var teamRepository = new Mock<ITeamRepository>();
        var chatRequestRepository = new Mock<IChatRequestRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var queueService = new Mock<IQueueService>();

        var teamA = new Team(1, Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        teamA.AddAgent(new(1,"Matilda", Seniority.Lead));
        teamA.AddAgent(new(2,"Eric", Seniority.Junior));

        var teamB = new Team(2, Guid.NewGuid(), "TeamB", new TimeOnly(0,0,0), 480, false);
        teamB.AddAgent(new(3,"Jason", Seniority.Lead));
        teamB.AddAgent(new(4,"Michelle", Seniority.Mid));

        var allTeams = new List<Team> { teamA, teamB };


        // moq empty queues
        queueService.Setup(r => r.GetQueueLength(teamA.Identifier, CancellationToken.None)).ReturnsAsync(0u);
        queueService.Setup(r => r.GetQueueLength(teamB.Identifier, CancellationToken.None)).ReturnsAsync(0u);

        teamRepository.Setup(r => r.GetTeamWorkingInTheCurrentShift(CancellationToken.None)).ReturnsAsync(allTeams);

        var initiateChatService = new InitiateChatService(teamRepository.Object, chatRequestRepository.Object, unitOfWork.Object, queueService.Object);
        Assert.That((await initiateChatService.Initiate(CancellationToken.None)) != null, "Chat request must return the identifier when capacity is available");
    }

    [Test]
    public async Task InitiateChat_SuccessfulInitiation_WhenCapacityNotAvailable()
    {
        var teamRepository = new Mock<ITeamRepository>();
        var chatRequestRepository = new Mock<IChatRequestRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var queueService = new Mock<IQueueService>();

        var teamA = new Team(1, Guid.NewGuid(), "TeamA", new TimeOnly(0,0,0), 480, false);
        teamA.AddAgent(new(1,"Matilda", Seniority.Lead));
        teamA.AddAgent(new(2,"Eric", Seniority.Junior));

        var teamB = new Team(2, Guid.NewGuid(), "TeamB", new TimeOnly(0,0,0), 480, false);
        teamB.AddAgent(new(3,"Jason", Seniority.Lead));
        teamB.AddAgent(new(4,"Michelle", Seniority.Mid));

        var allTeams = new List<Team> { teamA, teamB };


        // moq empty queues
        queueService.Setup(r => r.GetQueueLength(teamA.Identifier, CancellationToken.None)).ReturnsAsync(100u);
        queueService.Setup(r => r.GetQueueLength(teamB.Identifier, CancellationToken.None)).ReturnsAsync(100u);

        teamRepository.Setup(r => r.GetTeamWorkingInTheCurrentShift(CancellationToken.None)).ReturnsAsync(allTeams);

        var initiateChatService = new InitiateChatService(teamRepository.Object, chatRequestRepository.Object, unitOfWork.Object, queueService.Object);
        Assert.That((await initiateChatService.Initiate(CancellationToken.None)) == null, "Chat request must return null when capacity is unavailable");
    }
}
