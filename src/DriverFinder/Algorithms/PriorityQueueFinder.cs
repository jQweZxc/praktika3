namespace DriverFinderProject;
public class PriorityQueueFinder : IDriverFinder
{
    public string AlgorithmName => "PriorityQueue";
    
    public List<DriverSearchResult> FindNearestDrivers(List<Driver> drivers, int targetX, int targetY, int count)
    {
        // Используем MaxHeap для хранения ближайших водителей
        var pq = new PriorityQueue<DriverSearchResult, double>(Comparer<double>.Create((a, b) => b.CompareTo(a)));
        
        foreach (var driver in drivers)
        {
            double distance = driver.DistanceTo(targetX, targetY);
            var result = new DriverSearchResult(driver, distance);
            
            if (pq.Count < count)
            {
                pq.Enqueue(result, distance);
            }
            else if (distance < pq.Peek().Distance)
            {
                pq.Dequeue();
                pq.Enqueue(result, distance);
            }
        }
        
        // Извлекаем и сортируем по возрастанию расстояния
        var sortedResults = new List<DriverSearchResult>();
        while (pq.Count > 0)
        {
            sortedResults.Add(pq.Dequeue());
        }
        
        sortedResults.Reverse();
        return sortedResults;
    }
}