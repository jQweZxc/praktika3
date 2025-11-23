namespace DriverFinderProject;
public interface IDriverFinder
{
    string AlgorithmName { get; }
    List<DriverSearchResult> FindNearestDrivers(List<Driver> drivers, int targetX, int targetY, int count);
}