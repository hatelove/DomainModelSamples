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
        if (!goalRecord.Contains(";"))
        {
            FirstHalf = goalRecord;
        }
        else
        {
            var scoreSections = goalRecord.Split(";");
            FirstHalf = scoreSections[0];
            SecondHalf = scoreSections[1];
        }
        
    }

    public string FirstHalf { get; set; } = "";
    public string SecondHalf { get; set; } = "";
}