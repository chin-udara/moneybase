using System.Linq.Expressions;

namespace MoneybaseChat.domain.entities;

public class ChatRequest(Guid identifier, DateTime createdOn)
{
    public int Id { get; private set; }
    public Guid Identifier { get; private set; } = identifier;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastPulse { get; private set; }
    public DateTime CreatedOn { get; private set; } = createdOn;
    public int? Agent { get; private set; }

    public virtual Agent? AgentNavigation { get; set; }

    public void PulseReceived() => LastPulse = DateTime.Now;
}