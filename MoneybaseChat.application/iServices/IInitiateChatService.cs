namespace MoneybaseChat.application.iServices;

public interface IInitiateChatService
{
    Task<Guid?> Initiate(CancellationToken cancellationToken);
}