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
        throw new NotImplementedException();
    }
}

public class GoalRecord
{
    public string FirstHalf { get; set; }
    public string SecondHalf { get; set; }
}