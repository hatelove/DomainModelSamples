namespace DomainModelsSamples;

public class MatchFull
{
    public int LivePeriod { get; set; }

    public bool IsFirstHalf()
    {
        return LivePeriod == 1;
    }
}