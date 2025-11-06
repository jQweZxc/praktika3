using NUnit.Framework;

[TestFixture]
public class DriverFinderTests
{
    private List<Driver> _testDrivers;
    private List<IDriverFinder> _finders;
    
    [SetUp]
    public void Setup()
    {
        _testDrivers = new List<Driver>
        {
            new Driver(1, 0, 0),    // расстояние до (0,0) = 0
            new Driver(2, 3, 4),    // расстояние до (0,0) = 5
            new Driver(3, 10, 10),  // расстояние до (0,0) = 14.14
            new Driver(4, 1, 1),    // расстояние до (0,0) = 1.41
            new Driver(5, 5, 5),    // расстояние до (0,0) = 7.07
            new Driver(6, 2, 2)     // расстояние до (0,0) = 2.83
        };
        
        _finders = new List<IDriverFinder>
        {
            new LinqOrderByFinder(),
            new PriorityQueueFinder(),
            new PartialSortFinder()
        };
    }
    
    [Test]
    public void FindNearestDrivers_ShouldReturnCorrectCount()
    {
        foreach (var finder in _finders)
        {
            var result = finder.FindNearestDrivers(_testDrivers, 0, 0, 3);
            Assert.That(result.Count, Is.EqualTo(3));
        }
    }
    
    [Test]
    public void FindNearestDrivers_ShouldReturnCorrectOrder()
    {
        foreach (var finder in _finders)
        {
            var result = finder.FindNearestDrivers(_testDrivers, 0, 0, 3);
            
            Console.WriteLine($"Algorithm: {finder.AlgorithmName}");
            foreach (var r in result)
            {
                Console.WriteLine($"  Driver {r.Driver.Id}: ({r.Driver.X},{r.Driver.Y}) -> {r.Distance}");
            }
            
            for (int i = 0; i < result.Count - 1; i++)
            {
                Assert.That(result[i].Distance, Is.LessThanOrEqualTo(result[i + 1].Distance),
                    $"Algorithm {finder.AlgorithmName}: Distance {result[i].Distance} should be <= {result[i + 1].Distance}");
            }
        }
    }
    
    [Test]
    public void FindNearestDrivers_ShouldFindClosestDriver()
    {
        foreach (var finder in _finders)
        {
            var result = finder.FindNearestDrivers(_testDrivers, 0, 0, 1);
            Console.WriteLine($"Algorithm: {finder.AlgorithmName} - Closest driver: {result[0].Driver.Id}");
            Assert.That(result[0].Driver.Id, Is.EqualTo(1), 
                $"Algorithm {finder.AlgorithmName}: Expected driver 1, but got driver {result[0].Driver.Id}");
            Assert.That(result[0].Distance, Is.EqualTo(0));
        }
    }
    
    [Test]
    public void FindNearestDrivers_WithEmptyList_ShouldReturnEmpty()
    {
        foreach (var finder in _finders)
        {
            var result = finder.FindNearestDrivers(new List<Driver>(), 0, 0, 5);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
    
    [Test]
    public void FindNearestDrivers_RequestMoreThanAvailable_ShouldReturnAll()
    {
        foreach (var finder in _finders)
        {
            var result = finder.FindNearestDrivers(_testDrivers, 0, 0, 10);
            Assert.That(result.Count, Is.EqualTo(6));
        }
    }
}