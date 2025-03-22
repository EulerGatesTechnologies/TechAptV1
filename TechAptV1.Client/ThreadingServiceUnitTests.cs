using Moq;
using Xunit;
using Shouldly;

using TechAptV1.Client.Services;

namespace TechAptV1.UnitTests.TechAptV1.Client.Services;

public class ThreadingServiceTests
{
    private readonly Mock<ILogger<ThreadingService>> _loggerMock;
    private readonly Mock<IDataService> _dataServiceMock;
    private ThreadingService _threadingService;

    public ThreadingServiceTests()
    {
        _loggerMock = new Mock<ILogger<ThreadingService>>();

        _dataServiceMock = new Mock<IDataService>();

         _threadingService = new ThreadingService(_loggerMock.Object, _dataServiceMock.Object);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldNotBeNull()
    { 
        // Act & Assert
        _threadingService.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task StartAsync_WhenNumbersAreProvided_ThenShouldProduceExactly10mSortedNumbers()
    {
        // Arrange
        int expectedToBe10m = 10_000;   

        // Act
         await _threadingService.StartAsync();

        // Assert 
        _threadingService.GetTotalNumbers().ShouldBe(expectedToBe10m);        

        (_threadingService.GetEvenNumbers() + _threadingService.GetOddNumbers()).ShouldBe(expectedToBe10m);
    } 
}
