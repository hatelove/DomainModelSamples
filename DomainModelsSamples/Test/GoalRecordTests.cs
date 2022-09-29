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

    [Test]
    public void no_scores_when_full_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, ";");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    [Category("goal")]
    public void home_goal_when_first_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FirstHalf, "");
        matchFull.ApplyEvent(SoccerEvent.HomeGoal);

        matchFull.GoalRecord.Should().Be("H");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("H");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    [Category("goal")]
    public void away_goal_when_first_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FirstHalf, "");
        matchFull.ApplyEvent(SoccerEvent.AwayGoal);

        matchFull.GoalRecord.Should().Be("A");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("A");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    [Category("goal")]
    public void away_goal_when_first_half_with_score()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FirstHalf, "H");
        matchFull.ApplyEvent(SoccerEvent.AwayGoal);

        matchFull.GoalRecord.Should().Be("HA");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("HA");
        goalRecord.SecondHalf.Should().Be("");
    }

    [Test]
    [Category("goal")]
    public void home_goal_when_full_half()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, "A;");
        matchFull.ApplyEvent(SoccerEvent.HomeGoal);

        matchFull.GoalRecord.Should().Be("A;H");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("A");
        goalRecord.SecondHalf.Should().Be("H");
    }

    [Test]
    [Category("goal")]
    public void home_goal_score_when_full_half_and_no_scores()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, "");
        matchFull.ApplyEvent(SoccerEvent.HomeGoal);

        matchFull.GoalRecord.Should().Be(";H");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("");
        goalRecord.SecondHalf.Should().Be("H");
    }

    [Test]
    [Category("goal")]
    public void home_goal_when_full_half_and_both_have_scores()
    {
        var matchFull = GivenMatch(LivePeriodEnum.FullHalf, "AAH;HA");
        matchFull.ApplyEvent(SoccerEvent.HomeGoal);

        matchFull.GoalRecord.Should().Be("AAH;HAH");

        var goalRecord = matchFull.GetGoalRecord();
        goalRecord.FirstHalf.Should().Be("AAH");
        goalRecord.SecondHalf.Should().Be("HAH");
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

public enum SoccerEvent
{
    HomeGoal,
    AwayGoal
}