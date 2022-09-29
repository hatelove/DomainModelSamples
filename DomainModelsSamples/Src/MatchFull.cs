namespace DomainModelsSamples;

public class MatchFull
{
    public string GoalRecord { get; set; }
    public int LivePeriod { get; set; }

    public void ApplyEvent(SoccerEvent soccerEvent)
    {
        if (IsGoal(soccerEvent))
        {
            var goalRecord = GetGoalRecord();
            if (IsFirstHalf())
            {
                goalRecord.FirstHalf += GetGoalValue(soccerEvent);
                GoalRecord = goalRecord.FirstHalf;
            }
            else
            {
                goalRecord.SecondHalf += GetGoalValue(soccerEvent);
                GoalRecord = $"{goalRecord.FirstHalf};{goalRecord.SecondHalf}";
            }
        }
        
    }

    private static bool IsGoal(SoccerEvent soccerEvent)
    {
        return soccerEvent is SoccerEvent.HomeGoal or SoccerEvent.AwayGoal;
    }

    public GoalRecord GetGoalRecord()
    {
        return new GoalRecord(GoalRecord);
    }

    public bool IsFirstHalf()
    {
        return LivePeriod == 1;
    }

    private static string GetGoalValue(SoccerEvent soccerEvent)
    {
        switch (soccerEvent)
        {
            case SoccerEvent.HomeGoal:
                return "H";
            case SoccerEvent.AwayGoal:
                return "A";
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