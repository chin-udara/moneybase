using MoneybaseChat.domain.enums;

namespace MoneybaseChat.domain.entities;

public class Agent(int id, string name, Seniority seniority)
{
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public Seniority Seniority { get; private set; } = seniority;
    public int Team { get; private set; }

    public virtual Team TeamNavigation { get; private set; } = null!;
    public virtual List<ChatRequest> ChatRequests { get; private set; } = [];

    public int Capacity
    {
        get
        {
            switch (Seniority)
            {
                case Seniority.Junior:
                    return (int)(0.4 * 10);
                case Seniority.Mid:
                    return (int)(0.6 * 10);
                case Seniority.Senior:
                    return (int)(0.8 * 10);
                case Seniority.Lead:
                    return (int)(0.5 * 10);
                default:
                    return 0;
            }
        }
    }

    public int AvailableCapacity => Capacity - ChatRequests.Where(a => a.IsActive).Count();

    public void AssignChat(ChatRequest chatRequest)
    {
        ChatRequests.Add(chatRequest);
    }

    public void SetTeam(Team team)
    {
        Team = team.Id;
    }
}