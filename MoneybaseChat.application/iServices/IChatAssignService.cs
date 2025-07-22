namespace MoneybaseChat.application.iServices;

public interface IChatAssignService
{
    Task<bool> Assign(Guid teamIdentifier, Guid chatIdentifier, CancellationToken cancellationToken);
}