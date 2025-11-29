using BenchmarkDotNet.Attributes;
using DriverFinderProject;

namespace DriverFinderProject.Tests.Benchmarks;

[MemoryDiagnoser]
public class DriverFinderBenchmarks
{
    private List<Driver>? _drivers;
    private List<IDriverFinder>? _finders;
    private readonly Random _random = new Random();
    
    [Params(100, 1000, 10000)]
    public int DriverCount { get; set; }
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _drivers = GenerateDrivers(DriverCount);
        _finders = new List<IDriverFinder>
        {
            new LinqOrderByFinder(),
            new PriorityQueueFinder(),
            new PartialSortFinder()
        };
    }
    
    [Benchmark]
    public void BenchmarkLinqOrderBy() => RunAlgorithm(0);
    
    [Benchmark]
    public void BenchmarkPriorityQueue() => RunAlgorithm(1);
    
    [Benchmark]
    public void BenchmarkPartialSort() => RunAlgorithm(2);
    
    private void RunAlgorithm(int finderIndex)
    {
        if (_finders == null || _drivers == null) return;
        
        var finder = _finders[finderIndex];
        int targetX = _random.Next(0, 1000);
        int targetY = _random.Next(0, 1000);
        
        var result = finder.FindNearestDrivers(_drivers, targetX, targetY, 5);
    }
    
    private List<Driver> GenerateDrivers(int count)
    {
        var drivers = new List<Driver>();
        for (int i = 0; i < count; i++)
        {
            drivers.Add(new Driver(
                i,
                _random.Next(0, 1000),
                _random.Next(0, 1000)
            ));
        }
        return drivers;
    }
}
