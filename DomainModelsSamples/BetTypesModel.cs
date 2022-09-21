namespace DomainModelsSamples;

public class BetTypesModel
{
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

        if (matchFull.IsFirstHalf())
        {
            result = RemoveBetTypesOddsWhenNoOddsValue(result, HtBetTypes); 
            result = RemoveFtBetTypesOddsWhenNoOddsValue(result);
        }
        else
        {
            result = result.ExceptBy(HtBetTypes, odds => odds.BetType.Value); 
            result = RemoveFtBetTypesOddsWhenNoOddsValue(result);
        }

        return result;
    }

    private IEnumerable<DbOdds> RemoveBetTypesOddsWhenNoOddsValue(IEnumerable<DbOdds> result, IEnumerable<int> betTypes)
    {
        var hasNoOddsValue = result.IntersectBy(betTypes, odds => odds.BetType.Value)
                                .All(x => x.Odds2A == 0m);

        return hasNoOddsValue ? result.ExceptBy(betTypes, odds => odds.BetType.Value) : result;
    }

    private IEnumerable<DbOdds> RemoveFtBetTypesOddsWhenNoOddsValue(IEnumerable<DbOdds> result)
    {
        var hasNoFtOdds = result.IntersectBy(FtBetTypes, odds => odds.BetType.Value)
                                .All(x => x.Odds2A == 0m);

        if (hasNoFtOdds)
        {
            result = result.ExceptBy(FtBetTypes, odds => odds.BetType.Value);
        }

        return result;
    }
}