using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Shouldly;

using TechAptV1.Client.Services;
using TechAptV1.Client.Models;
using TechAptV1.Client.Data;

namespace TechAptV1.IntegrationTests.TechAptV1.Client.Services;

public class DataServiceTests
{
    private readonly Mock<ILogger<DataService>> _loggerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly DataContext _dataContext;
    private readonly DataService _dataService;

    public DataServiceTests()
    {
        _loggerMock = new Mock<ILogger<DataService>>();

        _configurationMock = new Mock<IConfiguration>();

        _dataContext = CreateInMemoryDatabase();

        _dataService = new DataService(_loggerMock.Object, _configurationMock.Object, _dataContext);
    }
    // Create a fresh in-memory database for each test.
    private DataContext CreateInMemoryDatabase()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DataContext(options);
    }

    [Fact]
    public async Task SaveAsync_WhenNumbersAreProvided_ThenShouldSaveToSQLiteDb()
    {
        // Arrange
        var numbers = new List<Number>
        {
            new Number { Value = 9, IsPrime = 0 },
            new Number { Value = -5, IsPrime = 1 },
            new Number { Value = 8, IsPrime = 0 }
        };

        // Act
        await _dataService.SaveAsync(numbers);

        var retrieved = await _dataService.GetAllAsync();

        // Assert
        retrieved.Count().ShouldBe(3);
        retrieved.ShouldContain(numbers[0]);
        retrieved.ShouldContain(numbers[1]);
        retrieved.ShouldContain(numbers[2]);
    }    
}

