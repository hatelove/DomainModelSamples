﻿#region

using FluentAssertions;

#endregion

namespace DomainModelsSamples;

[TestFixture]
public class GoalRecordTests
{
    [Test]
    public void no_scores_when_first_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FirstHalf, "");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    public void scores_when_first_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FirstHalf, "HA");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("HA");
        goalRecord.SecondHalf.Should().Be("");
    }

    private static MatchFull GivenMatch(LivePeriodEnum livePeriod, string goalRecord)
    {
        return new MatchFull()
               {
                   LivePeriod = (int)livePeriod,
                   GoalRecord = goalRecord
               };
    }
}