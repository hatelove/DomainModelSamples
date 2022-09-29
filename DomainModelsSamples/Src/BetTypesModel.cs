namespace DomainModelsSamples;

public class BetTypesModel
{
    public BetTypesModel(IBetTypesRepo betTypesRepo)
    {
        MajorBetTypes = betTypesRepo.GetMajorBetTypes();
        HtBetTypes = betTypesRepo.GetHtBetTypes();
    }

    public IEnumerable<int> FtBetTypes => MajorBetTypes.Except(HtBetTypes);
    public IEnumerable<int> HtBetTypes { get; }
    public IEnumerable<int> MajorBetTypes { get; }
}