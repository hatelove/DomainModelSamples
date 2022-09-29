using FluentAssertions;
using NUnit.Framework;

namespace DomainModelsSamples;

[TestFixture]
public class GoalRecordTests
{
    [Test]
    public void no_scores_when_first_half()
    {
        var matchFull = new MatchFull()
                        {
                            LivePeriod = (int)LivePeriodEnum.FirstHalf,
                            GoalRecord = ""
                        };

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("");
    }
}