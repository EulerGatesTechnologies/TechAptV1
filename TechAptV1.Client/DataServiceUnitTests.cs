using Moq;
using Xunit;
using Shouldly;

using TechAptV1.Client.Services;
using TechAptV1.Client.Data;

namespace TechAptV1.UnitTests.TechAptV1.Client.Services;

public class DataServiceUnitTests
{
    private readonly Mock<ILogger<DataService>> _loggerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly Mock<DataContext> _dataContextMock;
    private DataService _dataService;

    public DataServiceUnitTests()
    {
       _loggerMock = new Mock<ILogger<DataService>>();

        _configMock = new Mock<IConfiguration>();

        _dataContextMock = new Mock<DataContext>();
    }

    [Fact]
    public void ConnectionString_ShouldNotBeNullOrWhitespace()
    {
        // Arrange
        _configMock
            .Setup(x => x.GetConnectionString("Default"))
            .Returns("Data Source=test.db");

        _dataService = new DataService(_loggerMock.Object, _configMock.Object, _dataContextMock.Object);

        // Act
        string connectionString = _dataService.ConnectionString;

        // Assert
        connectionString.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void ConnectionString_WhenConfigIsNull_TheneShouldThrowArgumentNullException()
    {
        // Arrange
        IConfiguration nullConfig = null;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => new DataService(_loggerMock.Object, nullConfig, _dataContextMock.Object));

    }

    [Fact]
    public void ConnectionString_WhenConnectionStringIsMissing_ShouldThrowException()
    {
        // Arrange
        _configMock
            .Setup(x => x.GetConnectionString("Default"))
            .Returns((string)null);

        _dataService = new DataService(_loggerMock.Object, _configMock.Object, _dataContextMock.Object);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _dataService.ConnectionString);
    }
}

