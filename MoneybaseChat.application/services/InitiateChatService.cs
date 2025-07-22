using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using MoneybaseChat.domain.entities;

namespace MoneybaseChat.application.services;

public class InitiateChatService(ITeamRepository teamRepository, IChatRequestRepository chatRequestRepository, IUnitOfWork unitOfWork, IQueueService queueService) : IInitiateChatService
{
    private readonly ITeamRepository teamRepository = teamRepository;
    private readonly IChatRequestRepository chatRequestRepository = chatRequestRepository;
    private readonly IQueueService queueService = queueService;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Guid?> Initiate(CancellationToken cancellationToken)
    {
        // get teams available during the current shift
        List<Team> availableTeams = await teamRepository.GetTeamWorkingInTheCurrentShift(cancellationToken);
        // get the team with the highest remaining capacity then put the overflow teams at the bottom
        var availableTeam = availableTeams.Select(async t => new
        {
            team = t,
            availableCapacity = t.TeamCapacity - (await queueService.GetQueueLength(t.Identifier, cancellationToken))
        })
        .Select(t => new
        {
            t.Result.team,
            t.Result.availableCapacity
        })
        .Where(t=>t.availableCapacity > 0)
        .OrderByDescending(t => t.availableCapacity).ThenBy(t => t.team.IsOverflowTeam).ToList();

        // if no team is available
        if (availableTeam.Count == 0) return null;

        availableTeam.ForEach(t=>
        {
            Console.WriteLine("[x] Team cap: {0} {1} {2}", t.team.Id, t.availableCapacity, t.team.TeamCapacity); 
        });

        // create and persist chat request
        var chatRequest = new ChatRequest(Guid.NewGuid(), DateTime.Now);
        await chatRequestRepository.Add(chatRequest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // assign to the first available team
        await queueService.Enqueue(availableTeam.First().team.Identifier, chatRequest.Identifier, cancellationToken);

        // return the identifier
        return chatRequest.Identifier;
    }
}
