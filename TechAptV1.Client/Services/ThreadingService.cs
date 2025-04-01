// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace TechAptV1.Client.Services;

/// <summary>
/// Default constructor providing DI Logger and Data Service
/// </summary>
/// <param name="logger"></param>
/// <param name="dataService"></param>
public sealed class ThreadingService(ILogger<ThreadingService> logger, IDataService dataService, IOptions<ThreadingServiceOptions> options)
: IThreadingService, IDisposable
{
    private ThreadingServiceOptions Options { get; } = options.Value;

    private readonly object _lock = new(); // This will be used for Thread-Safety lock on shared global variable.

    public IDataService DataService { get; } = dataService;

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
    public async Task SaveAsync()
    {
        logger.LogInformation(nameof(SaveAsync));

        await DataService.SaveAsync(_numbers);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Start the random number generation process
    /// </summary>
    public async Task StartAsync()
    {
        logger.LogInformation(nameof(StartAsync));
        logger.LogInformation($"Options MaxEntries: {Options.MaxEntries}");
        logger.LogInformation($"Options EvenThreadTriggerThreshold: {Options.EvenThreadTriggerThreshold}");


        Random random = new ();

        int maxEntries = 10_000_000; // TODO: Read using options patterns

        int evenThreadTriggerThreshold = 2_500_000; // TODO: Read using options patterns

        List<Task> tasks =
        [
            // Thread 1:  Generate Odd Numbers
            Task.Run(() =>
            {
                while (_numbers.Count < maxEntries)
                {
                    lock (_lock)
                    {
                        if(_numbers.Count < maxEntries)
                        {
                            var oddNumber = random.Next(1, 1000000) * 2 + 1; // odd = 2n+1, n in Integers

                            _numbers.Add(new Number { Value = oddNumber, IsPrime = false });
                        }
                    }
                }
            }),

            // Thread 2: Generate negative prime numbers.
            Task.Run(() =>
            {
                while (_numbers.Count < maxEntries)
                {
                    lock (_lock)
                    {
                        if (_numbers.Count < maxEntries)
                        {
                            int negativePrimeNumber = GeneratePrime(random) * -1;

                            _numbers.Add(new Number { Value = negativePrimeNumber, IsPrime = true });
                        }
                    }
                }
            }),

            // Thread 3: When Threadshold is reached, generate Even numbers.
            Task.Run(() =>
            {
                while (_numbers.Count < evenThreadTriggerThreshold)
                {
                    //Wait for the threshold to be reached
                    Thread.Sleep(100);
                }

                while (_numbers.Count < maxEntries)
                {
                    lock (_lock)
                    {
                        if(_numbers.Count < maxEntries)
                        {
                            var evenNumber = random.Next(1, 1000000) * 2; // even = 2n, n in Integers

                            _numbers.Add(new Number { Value = evenNumber, IsPrime = false });
                        }
                    }
                }
            }),
        ];

        // Wait for all threads to complete
        await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(Timeout.Infinite));

        // Sort the numbers
        lock (_lock)
        {
            _numbers.Sort((a, b) => a.Value.CompareTo(b.Value));
        }

        _totalNumbers = _numbers.Count;

        _oddNumbers = _numbers.Count(n => n.IsPrime == false && n.Value % 2 != 0 && n.Value > 0);

        _evenNumbers = _numbers.Count(n => n.IsPrime == false && n.Value % 2 == 0 && n.Value > 0);

        _primeNumbers = _numbers.Count(n => n.IsPrime == true); // Assumes prime checks have been done & stored in the numbers list.
    }

    private static int GeneratePrime(Random random)
        {
            int candidate = random.Next(2, 1000000);
            while (!IsPrime(candidate))
            {
                candidate = random.Next(2, 1000000);
            }
            return candidate;
        }

    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;

        if (number == 2) return true;

        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}


public interface IThreadingService
{
    Task StartAsync();

    Task SaveAsync();
}
