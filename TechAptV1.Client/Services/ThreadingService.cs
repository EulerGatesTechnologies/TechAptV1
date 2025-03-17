// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

namespace TechAptV1.Client.Services;

/// <summary>
/// Default constructor providing DI Logger and Data Service
/// </summary>
/// <param name="logger"></param>
/// <param name="dataService"></param>
public sealed class ThreadingService(ILogger<ThreadingService> logger, IDataService dataService)
: IThreadingService
{
    private readonly object _lock = new(); // This will be used for Thead-Safety lock on shared global variable.
    private const int MaxEntries = 10_000_000; // TODO: We would like to have this read from the config file, rather then hardcoded.
    private const int ThresholdForEven = 2_500_000; // TODO: We would like to have this read from the config file, rather then hardcoded.

    private int _oddNumbers = 0;
    public int GetOddNumbers() => _oddNumbers;

    private int _evenNumbers = 0;
    public int GetEvenNumbers() => _evenNumbers;

    private int _primeNumbers = 0;
    public int GetPrimeNumbers() => _primeNumbers;

    private int _totalNumbers = 0;
    public int GetTotalNumbers() => _totalNumbers;

    private List<Number> _numbers = new();
    public List<Number> GetNumbers() => _numbers;

    /// <summary>
    /// Persist the results to the SQLite database
    /// </summary>
    public async Task Save()
    {
        logger.LogInformation("Save");
        // Implement the save logic here
        throw new NotImplementedException();
    }

    /// <summary>
    /// Start the random number generation process
    /// </summary>
    public async Task StartAsync()
    {
        logger.LogInformation(nameof(StartAsync));

        // Implement the threading logic here

        _numbers = new ();

        var random = new Random();

        var tasks = new List<Task>();

        // Thread 1:  Generate Odd Numbers
        tasks.Add(Task.Run(() =>
        {
            while (_numbers.Count < MaxEntries)
            {
                lock (_lock)
                {
                    var oddNumber = random.Next(1, 1000000) * 2 + 1; // odd = 2n+1, n in Integers

                    _numbers.Add(new Number { Value = oddNumber });
                }
            }
        }));


        _totalNumbers = _numbers.Count;

        _oddNumbers = _numbers.Count(n => n.Value % 2 != 0 && n.Value > 0);

        _evenNumbers = _numbers.Count(n => n.Value % 2 == 0 && n.Value > 0);

        _primeNumbers = _numbers.Count(n => n.IsPrime == 1); // Assumes prime checks have been done & stored in the numbers list.
    }
}

public interface IThreadingService
{
    Task StartAsync();
}
