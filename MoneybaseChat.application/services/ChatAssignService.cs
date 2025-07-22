using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;

namespace MoneybaseChat.application.services;

public class ChatAssignService(IChatRequestRepository chatRequestRepository, ITeamRepository teamRepository, IUnitOfWork unitOfWork) : IChatAssignService
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ITeamRepository teamRepository = teamRepository;
    private readonly IChatRequestRepository chatRequestRepository = chatRequestRepository;

    public async Task<bool> Assign(Guid teamIdentifier, Guid chatIdentifier, CancellationToken cancellationToken)
    {
        var team = await teamRepository.GetTeamWithActiveChatsOnly(teamIdentifier, cancellationToken);
        var chatRequest = await chatRequestRepository.GetByIdentifier(chatIdentifier, cancellationToken);

        // if either the team or the chat request does not exists or is inactive => simply acknowledge as processed
        if (team is null || chatRequest is null || !chatRequest.IsActive)
        {
            Console.WriteLine("[x] Invalid Chat request: Skipped");
            // TODO: send to a dead-letter exchange
            return true;
        }
        else if (team.AssignChat(chatRequest))
        {
            // persist assigned chat
            await unitOfWork.CommitAsync(cancellationToken);
            Console.WriteLine("[x] Chat request assigned to agent");
            // acknowledge the message if successfully assigned
            return true;
        }

        return false;
    }
}
