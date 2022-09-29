#region

using FluentAssertions;

#endregion

namespace DomainModelsSamples;

[TestFixture]
public class GoalRecordTests
{
    [Test]
    public void no_scores_when_first_half()
    {
        var matchFull = GivnMatch(LivePeriodEnum.FirstHalf, "");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("");
    }

    private static MatchFull GivnMatch(LivePeriodEnum livePeriod, string goalRecord)
    {
        return new MatchFull()
               {
                   LivePeriod = (int)livePeriod,
                   GoalRecord = goalRecord
               };
    }
}