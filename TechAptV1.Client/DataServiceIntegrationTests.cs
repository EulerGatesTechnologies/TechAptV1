using Microsoft.EntityFrameworkCore;
using TechAptV1.Client.Data;
using TechAptV1.Client.Services;
using Xunit;
using Shouldly;

using TechAptV1.Client.Models;

namespace TechAptV1.IntegrationTests.TechAptV1.Client.Services;

public class DataServiceIntegrationTests
{
    private readonly ILogger<DataService> _logger;
    private readonly IConfiguration _configuration;
    private readonly DataContext _dataContext;
    private readonly DataService _dataService;

    public DataServiceIntegrationTests()
    {
        _logger = new LoggerFactory().CreateLogger<DataService>();

        _configuration = new ConfigurationBuilder().Build();

        _dataContext = CreateInMemoryDatabase();

        _dataService = new DataService(_logger, _configuration, _dataContext);
    }

    // Create a fresh in-memory database for each test.
    private DataContext CreateInMemoryDatabase() => new DataContext(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"InMemoryNumbersTestDb_{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task SaveAsync_WhenNumbersListIsNotEmpty_ThenShouldSaveToSqliteDatabase()
    {
        // Arrange
        var numbersList = new List<Number>();

        const int NUMBER_COUNT = 10_000;
        // Create a list of sequential numbers with alternating IsPrime flag
        for (int i = 1; i <= NUMBER_COUNT; i++)
        {
            numbersList.Add(new Number
            {
                Value = i,
                IsPrime = i % 2 == 0  // Alternating true/false for simplicity
            });
        }

        // Act
        await _dataService.SaveAsync(numbersList);

        // Assert
        var savedNumbers = await _dataService.GetAllAsync();

        numbersList.Count.ShouldBe(savedNumbers.Count());

        // Verify that each number in the list is saved to the database
        foreach (var number in numbersList)
        {
            savedNumbers.ShouldContain(n => n.Value == number.Value && n.IsPrime == number.IsPrime);
        }
    }

    [Fact]
    public async Task SaveAsync_WhenNumbersAreNull_ThenShouldThrowAnArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(() => _dataService.SaveAsync(null));
    }
}

