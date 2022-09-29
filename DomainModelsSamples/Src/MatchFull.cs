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
    private readonly string _goalRecord;

    public GoalRecord(string goalRecord)
    {
        _goalRecord = goalRecord;
    }

    public string FirstHalf { get; set; } = "";
    public string SecondHalf { get; set; } = "";
}