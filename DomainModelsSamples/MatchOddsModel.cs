namespace DomainModelsSamples;

public class MatchOddsModel
{
    private const int FirstHalfMinutes = 45;
    public int MatchId { get; set; } // it should be MatchFull reference in real project
    public IEnumerable<DbOdds> OddsList { get; set; }

    // if replace MatchId property to MatchFull, it can reduce 1 parameter: matchFull
    public IEnumerable<DbOdds> FilterOdds(MatchFull matchFull, int minutes, BetTypesModel betTypesModel)
    {
        var result = OddsList.Where(x => x.BetType.HasValue)
                             .IntersectBy(betTypesModel.MajorBetTypes, odds => odds.BetType!.Value);

        result = matchFull.IsFirstHalf() && !IsOverTime(minutes)
            ? RemoveBetTypesOddsWhenNoOddsValue(result, betTypesModel.HtBetTypes)
            : result.ExceptBy(betTypesModel.HtBetTypes, odds => odds.BetType!.Value);

        return RemoveBetTypesOddsWhenNoOddsValue(result, betTypesModel.FtBetTypes);
    }

    private static bool IsOverTime(int minutes)
    {
        return minutes >= (FirstHalfMinutes - 1);
    }

    private static IEnumerable<DbOdds> RemoveBetTypesOddsWhenNoOddsValue(IEnumerable<DbOdds> result, IEnumerable<int> betTypes)
    {
        var hasNoOddsValue = result.IntersectBy(betTypes, odds => odds.BetType!.Value)
                                   .All(x => x.Odds2A == 0m);

        return hasNoOddsValue ? result.ExceptBy(betTypes, odds => odds.BetType!.Value) : result;
    }
}