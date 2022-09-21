#region

using FluentAssertions;
using NSubstitute;

#endregion

namespace DomainModelsSamples;

[TestFixture]
public class BetTypesModelTests
{
    private BetTypesModel _betTypesModel;
    private IBetTypesRepo _betTypesRepo;

    [SetUp]
    public void SetUp()
    {
        _betTypesRepo = Substitute.For<IBetTypesRepo>();
    }

    [Test]
    public void major_ht_ft_betTypes()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        _betTypesModel.MajorBetTypes.Should().BeEquivalentTo(new[] { 1, 3, 7, 8 });
        _betTypesModel.HtBetTypes.Should().BeEquivalentTo(new[] { 8, 7 });
        _betTypesModel.FtBetTypes.Should().BeEquivalentTo(new[] { 1, 3 });
    }

    [Test]
    public void filter_odds_when_first_half()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = null, Odds2A = 0.1m }, //filter by null
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m }, //filter by major
                                           new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                           new DbOdds() { BetType = 4, Odds2A = 0.4m }, //filter by major
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 1 } //first half
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                      new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                  });
    }

    [Test]
    public void filter_odds_when_full_time()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0.1m },
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m }, //filter by major
                                           new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m }, //remove by ft 
                                           new DbOdds() { BetType = 8, Odds2A = 0.8m }, //remove by ft
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 2 } //full time
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 1, Odds2A = 0.1m },
                                      new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                  });
    }

    [Test]
    public void filter_odds_when_full_time_and_ft_types_odds_all_0()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0m }, // remove, because betType 1 & 3 were 0
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m },
                                           new DbOdds() { BetType = 3, Odds2A = 0m }, // remove, because betType 1 & 3 were 0
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                           new DbOdds() { BetType = 8, Odds2A = 0.8m }
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 2 } //full time
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new DbOdds[] { });
    }

    [Test]
    public void filter_odds_when_full_time_and_ft_types_odds_not_all_0()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0.1m }, // keep, because betType 1 & 3 have some odds value
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m },
                                           new DbOdds() { BetType = 3, Odds2A = 0m }, // keep, because betType 1 & 3 have some odds value
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                           new DbOdds() { BetType = 8, Odds2A = 0.8m }
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 2 } //full time
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 1, Odds2A = 0.1m }, // keep, because betType 1 & 3 have some odds value
                                      new DbOdds() { BetType = 3, Odds2A = 0m }, // keep, because betType 1 & 3 have some odds value
                                  });
    }

    [Test]
    public void filter_odds_when_first_half_and_ft_types_odds_all_0()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0m }, // remove, because betType 1 & 3 all 0
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m },
                                           new DbOdds() { BetType = 3, Odds2A = 0m }, // remove, because betType 1 & 3 all 0
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                           new DbOdds() { BetType = 8, Odds2A = 0m }
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 1 } // first half
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 7, Odds2A = 0.7m },
                                      new DbOdds() { BetType = 8, Odds2A = 0m }
                                  });
    }

    [Test]
    public void filter_odds_when_first_half_and_ht_types_odds_all_0()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0m },
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m },
                                           new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                           new DbOdds() { BetType = 7, Odds2A = 0m }, // remove, because betType 7 & 8 all 0
                                           new DbOdds() { BetType = 8, Odds2A = 0m }, // remove, because betType 7 & 8 all 0
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 1 } //first half
                                                   , 40, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 1, Odds2A = 0m },
                                      new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                  });
    }

    [Test]
    public void filter_odds_when_first_half_but_over_time()
    {
        GivenMajorBetTypesFromRepo(new[] { 1, 7, 8, 3 });
        GivenHtBetTypesFromRepo(new[] { 7, 8 });
        _betTypesModel = new BetTypesModel(_betTypesRepo);

        IEnumerable<DbOdds> oddsList = new[]
                                       {
                                           new DbOdds() { BetType = 1, Odds2A = 0m },
                                           new DbOdds() { BetType = 2, Odds2A = 0.2m },
                                           new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                           new DbOdds() { BetType = 7, Odds2A = 0.7m },//remove, because of over time 
                                           new DbOdds() { BetType = 8, Odds2A = 0m }, //remove, because of over time
                                       };
        var filterOdds = _betTypesModel.FilterOdds(new MatchFull() { LivePeriod = 1 } //first half
                                                   , 44, new MatchOddsModel{MatchId=91, OddsList = oddsList});

        filterOdds.Should()
                  .BeEquivalentTo(new[]
                                  {
                                      new DbOdds() { BetType = 1, Odds2A = 0m },
                                      new DbOdds() { BetType = 3, Odds2A = 0.3m },
                                  });
    }

    private void GivenHtBetTypesFromRepo(IEnumerable<int> htBetTypes)
    {
        _betTypesRepo.GetHtBetTypes().Returns(htBetTypes);
    }

    private void GivenMajorBetTypesFromRepo(IEnumerable<int> majorBetTypes)
    {
        _betTypesRepo.GetMajorBetTypes().Returns(majorBetTypes);
    }
}