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
            .UseInMemoryDatabase($"InMemoryNumbersTestDb_{Guid.NewGuid().ToString()}")
            .Options);

    [Fact]
    public async Task SaveAsync_WhenNumbersListIsNotEmpty_ThenShouldSaveToSqliteDatabase()
    {
        // Arrange
        var random = new Random();

        var numbersList = new List<Number>();

        // Create a list of 1000 random numbers with IsPrime flag set randomly 0 or 1
        for (int i = 0; i < 1000; i++)
        {
            numbersList.Add(new Number
            {
                Id = i + 1,
                Value = random.Next(1, 1000000),
                IsPrime = random.Next(0, 2)
            });
        }

        // Act
        await _dataService.SaveAsync(numbersList);

        // Assert
        var savedNumbers = await _dataContext.Numbers.ToListAsync();

        numbersList.Count.ShouldBe(savedNumbers.Count());

        // Verify that each number in the list is saved to the database
        foreach (var number in numbersList)
        {
            savedNumbers.ShouldContain(n => n.Id == number.Id && n.Value == number.Value && n.IsPrime == number.IsPrime);
        }
    }

    [Fact]
    public async Task SaveAsync_WhenNumbersAreNull_ThenShouldThrowAnArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(() => _dataService.SaveAsync(null));
    }
}

