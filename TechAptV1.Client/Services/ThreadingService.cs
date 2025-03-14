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
    private int _evenNumbers = 0;
    private int _primeNumbers = 0;
    private int _totalNumbers = 0;

    public int GetOddNumbers() => _oddNumbers;
    public int GetEvenNumbers() => _evenNumbers;
    public int GetPrimeNumbers() => _primeNumbers;
    public int GetTotalNumbers() => _totalNumbers;

    /// <summary>
    /// Start the random number generation process
    /// </summary>
    public async Task Start()
    {
        logger.LogInformation("Start");
        // Implement the threading logic here
        throw new NotImplementedException();
    }

    /// <summary>
    /// Persist the results to the SQLite database
    /// </summary>
    public async Task Save()
    {
        logger.LogInformation("Save");
        // Implement the save logic here
        throw new NotImplementedException();
    }

    public Task<(List<Number> Numbers, int TotalCount, int OddCount, int EvenCount)> ComputeNumbersAsync(CancellationToken cancellationToken)
    {
        var numbers = new List<Number>();

         return Task.FromResult((numbers, _totalNumbers, _oddNumbers, _evenNumbers));
    }
}

public interface IThreadingService
{
    Task<(List<Number> Numbers, int TotalCount, int OddCount, int EvenCount)> ComputeNumbersAsync(CancellationToken cancellationToken);
}
