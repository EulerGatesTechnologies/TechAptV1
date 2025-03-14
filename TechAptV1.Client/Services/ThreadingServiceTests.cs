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
    private readonly ThreadingService _threadingService;

    public ThreadingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ThreadingService>>();
        _dataServiceMock = new Mock<IDataService>();
        _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object);
    }

    [Fact]
    public async Task StartAsync_WhenNoNumbersAreProvided_ShouldReturnEmptyNumbersListWithAllCountsZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        await _threadingService.StartAsync();

        // Assert
        _threadingService.GetNumbers().ShouldBeEmpty();
        _threadingService.GetTotalNumbers().ShouldBe(0);
        _threadingService.GetOddNumbers().ShouldBe(0);
        _threadingService.GetEvenNumbers().ShouldBe(0);
        _threadingService.GetPrimeNumbers().ShouldBe(0);
    }
}
