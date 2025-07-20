using System.Linq.Expressions;
using MoneybaseChat.domain.enums;

namespace MoneybaseChat.domain.entities;

public class Team(Guid identifier, string Name, TimeOnly ShiftStartTime, int shiftDurationInMinutes, bool isOverflowTeam)
{
    public int Id { get; private set; }
    public Guid Identifier { get; private set; } = identifier;
    public string Name { get; private set; } = Name;
    public TimeOnly ShiftStartTime { get; private set; } = ShiftStartTime;
    public int ShiftDurationInMinutes { get; private set; } = shiftDurationInMinutes;
    public bool IsOverflowTeam { get; private set; } = isOverflowTeam;
    public List<Agent> Agents { get; private set; } = [];

    public void AddAgent(Agent agent)
    {
        /*  
        only add if a similar agent does not exist
        following is a simple validation to demonstrate domain level validation.
        ideally the agent would be linked to an employee via a foreign key (eg: employee_id)
        */
        if (!Agents.Exists(a => a.Name == agent.Name))
            Agents.Add(agent);
    }

    public void MarkAsOverflowTeam()
    {
        IsOverflowTeam = true;
    }


    public int TeamCapacity
    {
        get
        {
            // if this team is an overflow team
            if (IsOverflowTeam)
                return (int)(Agents.Count * 0.4 * 10);

            // if this is a normal team
            return Agents.Sum(a => a.Capacity);
        }
    }

    public bool AssignChat(ChatRequest chatRequest)
    {
        // get juniors with highest available capacity
        var junior = Agents.Where(a => a.Seniority == Seniority.Junior && a.AvailableCapacity > 0).OrderByDescending(a => a.AvailableCapacity).FirstOrDefault();
        if (junior is null)
        {
            var mid = Agents.Where(a => a.Seniority == Seniority.Mid && a.AvailableCapacity > 0).OrderByDescending(a => a.AvailableCapacity).FirstOrDefault();
            if (mid is null)
            {
                var senior = Agents.Where(a => a.Seniority == Seniority.Senior && a.AvailableCapacity > 0).OrderByDescending(a => a.AvailableCapacity).FirstOrDefault();
                if (senior is null)
                {
                    var lead = Agents.Where(a => a.Seniority == Seniority.Lead && a.AvailableCapacity > 0).OrderByDescending(a => a.AvailableCapacity).FirstOrDefault();
                    if (lead is null) return false;
                    else
                        lead.AssignChat(chatRequest);
                }

                else
                    senior.AssignChat(chatRequest);
            }

            else
                mid.AssignChat(chatRequest);
        }

        else
            junior.AssignChat(chatRequest);

        return true;
    }
}