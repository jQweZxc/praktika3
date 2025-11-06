public class LinqOrderByFinder : IDriverFinder
{
    public string AlgorithmName => "LINQ OrderBy";
    
    public List<DriverSearchResult> FindNearestDrivers(List<Driver> drivers, int targetX, int targetY, int count)
    {
        return drivers
            .Select(d => new DriverSearchResult(d, d.DistanceTo(targetX, targetY)))
            .OrderBy(r => r.Distance)
            .Take(count)
            .ToList();
    }
}