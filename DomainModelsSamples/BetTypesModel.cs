namespace DomainModelsSamples;

public class BetTypesModel
{
    private const int FirstHalfMinutes = 45;
    private readonly IBetTypesRepo _betTypesRepo;

    public BetTypesModel(IBetTypesRepo betTypesRepo)
    {
        _betTypesRepo = betTypesRepo;
        MajorBetTypes = _betTypesRepo.GetMajorBetTypes();
        HtBetTypes = _betTypesRepo.GetHtBetTypes();
    }

    public IEnumerable<int> FtBetTypes => MajorBetTypes.Except(HtBetTypes);
    public IEnumerable<int> HtBetTypes { get; }
    public IEnumerable<int> MajorBetTypes { get; }

    public IEnumerable<DbOdds> FilterOdds(IEnumerable<DbOdds> oddsList, MatchFull matchFull, int minutes)
    {
        var result = oddsList.Where(x => x.BetType.HasValue)
                             .IntersectBy(MajorBetTypes, odds => odds.BetType.Value);

        result = matchFull.IsFirstHalf() && IsOverTime(minutes)
            ? RemoveBetTypesOddsWhenNoOddsValue(result, HtBetTypes)
            : result.ExceptBy(HtBetTypes, odds => odds.BetType.Value);

        return RemoveBetTypesOddsWhenNoOddsValue(result, FtBetTypes);
    }

    private static bool IsOverTime(int minutes)
    {
        return minutes < (FirstHalfMinutes - 1);
    }

    private IEnumerable<DbOdds> RemoveBetTypesOddsWhenNoOddsValue(IEnumerable<DbOdds> result, IEnumerable<int> betTypes)
    {
        var hasNoOddsValue = result.IntersectBy(betTypes, odds => odds.BetType.Value)
                                   .All(x => x.Odds2A == 0m);

        return hasNoOddsValue ? result.ExceptBy(betTypes, odds => odds.BetType.Value) : result;
    }
}