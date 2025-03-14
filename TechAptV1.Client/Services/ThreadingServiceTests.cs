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
    private readonly IThreadingService _threadingService;

    public ThreadingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ThreadingService>>();
        _dataServiceMock = new Mock<IDataService>();
        _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object);
    }

    [Fact]
    public async Task ComputeNumbersAsync_WhenNoNumbersAreProvided_ShouldReturnEmptyNumbersListWithAllCountsZero()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        var actualResult = await _threadingService.ComputeNumbersAsync(cancellationToken);

        // Assert
        actualResult.Numbers.ShouldBeEmpty();
        actualResult.TotalNumbersCount.ShouldBe(0);
        actualResult.OddNumbersCount.ShouldBe(0);
        actualResult.EvenNumbersCount.ShouldBe(0);
        actualResult.PrimeNumbersCount.ShouldBe(0);
    }
}
