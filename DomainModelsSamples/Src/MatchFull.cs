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
}

public class GoalRecord
{
    public GoalRecord(string goalRecord)
    {
        var scoreSections = goalRecord.Split(";");
        if (scoreSections.Length > 1)
        {
            FirstHalf = scoreSections[0];
            SecondHalf = scoreSections[1];
        }
        else
        {
            FirstHalf = scoreSections[0];
        }
    }

    public string FirstHalf { get; set; } = "";
    public string SecondHalf { get; set; } = "";
}