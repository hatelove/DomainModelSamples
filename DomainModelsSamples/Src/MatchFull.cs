namespace DomainModelsSamples;

public class MatchFull
{
    public int LivePeriod { get; set; }
    public string GoalRecord { get; set; }

    public bool IsFirstHalf()
    {
        return LivePeriod == 1;
    }

    public GoalRecord GetGoalRecord()
    {
        return new GoalRecord(GoalRecord);
    }

    public void ApplyEvent(SoccerEvent soccerEvent)
    {
        GoalRecord = GetGoalValue(soccerEvent);
    }

    private static string GetGoalValue(SoccerEvent soccerEvent)
    {
        switch (soccerEvent)
        {
            case SoccerEvent.HomeGoal:
                return "H";
            default:
                throw new ArgumentOutOfRangeException(nameof(soccerEvent), soccerEvent, null);
        }
    }
}

public class GoalRecord
{
    public GoalRecord(string goalRecord)
    {
        var scoreSections = goalRecord.Split(";");
        if (scoreSections.Length > 1)
        {
            SecondHalf = scoreSections[1];
        }

        FirstHalf = scoreSections[0];
    }

    public string FirstHalf { get; set; } = "";
    public string SecondHalf { get; set; } = "";
}