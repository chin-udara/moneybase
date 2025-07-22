using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;

namespace MoneybaseChat.application.services;

public class ChatRequestPulseService(IChatRequestRepository chatRequestRepository, IUnitOfWork unitOfWork) : IChatRequestPulseService
{
    private readonly IChatRequestRepository chatRequestRepository = chatRequestRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task Pulsate(Guid chatRequestIdentifier, CancellationToken cancellationToken)
    {
        var chatRequest = await chatRequestRepository.GetByIdentifier(chatRequestIdentifier, cancellationToken);
        chatRequest?.PulseReceived();
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
