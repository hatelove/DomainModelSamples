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

    public IEnumerable<int> MajorBetTypes { get; set; }
    public IEnumerable<int> HtBetTypes { get; set; }
    public IEnumerable<int> FtBetTypes => MajorBetTypes.Except(HtBetTypes);

    public IEnumerable<DbOdds> FilterOdds(IEnumerable<DbOdds> oddsList, MatchFull matchFull, int minutes)
    {
        var result = oddsList.Where(x => x.BetType.HasValue)
                             .IntersectBy(MajorBetTypes, odds => odds.BetType.Value);
        
        return result;
    }
}