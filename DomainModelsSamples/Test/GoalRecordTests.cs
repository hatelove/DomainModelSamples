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

    [Test]
    public void scores_when_full_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, "HA;A");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("HA");
        goalRecord.SecondHalf.Should().Be("A");
    }

    [Test]
    public void only_first_half_has_scores_when_full_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, "HA;");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("HA");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    public void only_second_half_has_scores_when_full_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, ";AA");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("AA");
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