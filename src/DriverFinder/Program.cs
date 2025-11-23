using System.Diagnostics;

namespace DriverFinderProject;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🚗 Driver Finder System");
        Console.WriteLine("=======================\n");

        if (args.Length > 0 && args[0] == "benchmark")
        {
            RunBenchmarks();
        }
        else
        {
            ShowUsage();
            RunSimpleDemo();
        }
    }

    static void ShowUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run              - Show demo");
        Console.WriteLine("  dotnet run benchmark    - Run performance benchmarks");
        Console.WriteLine("  dotnet test             - Run unit tests");
        Console.WriteLine("\nAlgorithms implemented:");
        Console.WriteLine("  • LINQ OrderBy");
        Console.WriteLine("  • PriorityQueue");
        Console.WriteLine("  • Partial Sort");
    }

    static void RunSimpleDemo()
    {
        Console.WriteLine("\n🧪 Demo - Finding nearest drivers to position (0,0):");

        var drivers = new List<Driver>
        {
            new Driver(1, 0, 0),
            new Driver(2, 3, 4),
            new Driver(3, 1, 1),
            new Driver(4, 5, 5),
            new Driver(5, 2, 2),
            new Driver(6, 10, 10)
        };

        var finders = new List<IDriverFinder>
        {
            new LinqOrderByFinder(),
            new PriorityQueueFinder(),
            new PartialSortFinder()
        };

        foreach (var finder in finders)
        {
            Console.WriteLine($"\n🔍 {finder.AlgorithmName}:");
            var results = finder.FindNearestDrivers(drivers, 0, 0, 3);

            foreach (var result in results)
            {
                Console.WriteLine($"   Driver {result.Driver.Id} at ({result.Driver.X},{result.Driver.Y}) - distance: {result.Distance:F2}");
            }
        }

        Console.WriteLine("\n✅ Demo completed successfully!");
    }

    static void RunBenchmarks()
    {
        Console.WriteLine("Running performance benchmarks...\n");

        var drivers = GenerateDrivers(10000);
        var finders = new List<IDriverFinder>
        {
            new LinqOrderByFinder(),
            new PriorityQueueFinder(),
            new PartialSortFinder()
        };

        int iterations = 100;
        var results = new Dictionary<string, long>();

        foreach (var finder in finders)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var result = finder.FindNearestDrivers(drivers, 500, 500, 5);
            }
            sw.Stop();
            results[finder.AlgorithmName] = sw.ElapsedMilliseconds;
        }

        Console.WriteLine($"Results for {drivers.Count} drivers, {iterations} iterations:");
        Console.WriteLine("===============================================");
        foreach (var result in results.OrderBy(r => r.Value))
        {
            Console.WriteLine($"{result.Key,-15}: {result.Value,6} ms");
        }
    }

    static List<Driver> GenerateDrivers(int count)
    {
        var random = new Random();
        var drivers = new List<Driver>();
        for (int i = 0; i < count; i++)
        {
            drivers.Add(new Driver(i, random.Next(0, 1000), random.Next(0, 1000)));
        }
        return drivers;
    }
}