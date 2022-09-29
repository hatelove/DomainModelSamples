namespace DomainModelsSamples;

public interface IBetTypesRepo
{
    IEnumerable<int> GetMajorBetTypes();
    IEnumerable<int> GetHtBetTypes();
}