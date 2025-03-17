using TechAptV1.Client.Models;
using TechAptV1.Client.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;

public class ThreadingServiceTests
{
    private readonly Mock<ILogger<ThreadingService>> _loggerMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private ThreadingService _threadingService;

    public ThreadingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ThreadingService>>();
        _dataServiceMock = new Mock<IDataService>();
    }

    [Fact]
    public async Task StartAsync_WhenNoNumbersAreProvided_ThenShouldReturnEmptyNumbersListWithAllCountsZero()
    {
        // Arrange
        int expectedToBeZero = 0;

        _dataServiceMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Number>());

        _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object);

        // Act
        await _threadingService.StartAsync();
        var globalNumbers = _threadingService.GetNumbers();

        // Assert
        globalNumbers.ShouldBeEmpty();
        globalNumbers.Count.ShouldBe(expectedToBeZero);
        _threadingService.GetTotalNumbers().ShouldBe(expectedToBeZero);
        _threadingService.GetOddNumbers().ShouldBe(expectedToBeZero);
        _threadingService.GetEvenNumbers().ShouldBe(expectedToBeZero);
        _threadingService.GetPrimeNumbers().ShouldBe(expectedToBeZero);
    }

    [Fact]
    public async Task StartAsync_WhenNumbersAreProvided_ThenShouldProduceExactly10mSortedNumbers()
    {
        // Arrange
        int expectedToBe10m = 10_000_000;

         _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object);

        // Act
         await _threadingService.StartAsync();
        var globalNumbers = _threadingService.GetNumbers();

        // Assert
        globalNumbers.ShouldNotBeEmpty();
        globalNumbers.Count.ShouldBe(expectedToBe10m);
        _threadingService.GetTotalNumbers().ShouldBe(expectedToBe10m);
        // Ensure list is sorted
        for (int i = 1; i < globalNumbers.Count; i++)
        {
            globalNumbers[i - 1].Value.ShouldBeLessThan(globalNumbers[i].Value);
        }
        // Even if we don’t know exact odd/prime/even counts, we can check totals.
        int oddCount = globalNumbers.Count(n => n.Value % 2 != 0);
        int evenCount = globalNumbers.Count(n => n.Value % 2 == 0);
        (oddCount + evenCount).ShouldBe(expectedToBe10m);
    }
}
