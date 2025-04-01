using System.Collections.Generic;
using Moq;
using Xunit;
using Shouldly;
using Microsoft.Extensions.Options;

using TechAptV1.Client.Services;
using TechAptV1.Client.Models;

namespace TechAptV1.UnitTests.TechAptV1.Client.Services;

public class ThreadingServiceTests
{
    private readonly Mock<ILogger<ThreadingService>> _loggerMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private readonly IOptions<ThreadingServiceOptions> _options;
    private ThreadingService _threadingService;

    public ThreadingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ThreadingService>>();

        _dataServiceMock = new Mock<IDataService>();

        // Configure options for testing
        _options = Options.Create(new ThreadingServiceOptions
        {
            MaxEntries = 10_000, // Using the same value as before
            EvenThreadTriggerThreshold = 2_500 // Set a reasonable threshold
        });

         _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object, _options);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldNotBeNull()
    {
        // Act & Assert
        _threadingService.ShouldNotBeNull();
    }

    [Fact]
    public async Task StartAsync_WhenNumbersAreProvided_ThenShouldProduceExpectedResults()
    {
        // Arrange
        int expectedResult = 10_000;

        // Act
         await _threadingService.StartAsync();

        // Assert
        _threadingService.GetTotalNumbers().ShouldBe(expectedResult);

        // Lets test the total numbers of each section generated.
        // We do know it's generating primes and negitive primes, as well as odds, and evens.
         (_threadingService.GetEvenNumbers() + _threadingService.GetOddNumbers() + _threadingService.GetPrimeNumbers()).ShouldBeLessThanOrEqualTo(expectedResult);
         (_threadingService.GetEvenNumbers() + _threadingService.GetOddNumbers() + _threadingService.GetPrimeNumbers()).ShouldBeGreaterThan(expectedResult - 500);
    }
}
