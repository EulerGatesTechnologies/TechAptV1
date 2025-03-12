using TechAptV1.Client.Services;
using TechAptV1.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class DataServiceTests
{
    private readonly Mock<ILogger<DataService>> _loggerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly DataService _dataService;

    public DataServiceTests()
    {
        _loggerMock = new Mock<ILogger<DataService>>();
        _configurationMock = new Mock<IConfiguration>();
        _dataService = new DataService(_loggerMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task Save_ShouldPersistData()
    {
        // Arrange
        var dataList = new List<Number> { new Number { Value = 1, IsPrime = 1 } };

        // Act
        await _dataService.Save(dataList);

        // Assert
        // ...assert expected outcomes...
    }

    [Fact]
    public void Get_ShouldReturnSpecifiedNumberOfRecords()
    {
        // Arrange
        int count = 10;

        // Act
        var result = _dataService.Get(count);

        // Assert
        // ...assert expected outcomes...
    }

    [Fact]
    public void GetAll_ShouldReturnAllRecords()
    {
        // Arrange

        // Act
        var result = _dataService.GetAll();

        // Assert
        // ...assert expected outcomes...
    }
}
