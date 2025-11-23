namespace DriverFinderProject;
public class PartialSortFinder : IDriverFinder
{
    public string AlgorithmName => "Partial Sort";
    
    public List<DriverSearchResult> FindNearestDrivers(List<Driver> drivers, int targetX, int targetY, int count)
    {
        if (drivers.Count == 0) return new List<DriverSearchResult>();
        
        var results = drivers
            .Select(d => new DriverSearchResult(d, d.DistanceTo(targetX, targetY)))
            .ToList();
        
        // Находим count наименьших расстояний
        for (int i = 0; i < Math.Min(count, results.Count); i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < results.Count; j++)
            {
                if (results[j].Distance < results[minIndex].Distance)
                {
                    minIndex = j;
                }
            }
            
            // Меняем местами
            if (minIndex != i)
            {
                var temp = results[i];
                results[i] = results[minIndex];
                results[minIndex] = temp;
            }
        }
        
        return results.Take(count).OrderBy(r => r.Distance).ToList();
    }
}