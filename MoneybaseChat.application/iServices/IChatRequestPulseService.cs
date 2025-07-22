namespace MoneybaseChat.application.iServices;

public interface IChatRequestPulseService
{
    Task Pulsate(Guid chatRequestIdentifier, CancellationToken cancellationToken);
}