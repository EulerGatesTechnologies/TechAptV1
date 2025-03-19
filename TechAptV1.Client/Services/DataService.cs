// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using TechAptV1.Client.Data;
using System.Text;

namespace TechAptV1.Client.Services;

/// <summary>
/// Data Access Service for interfacing with the SQLite Database
/// </summary>
/// <remarks>
/// Default constructor providing DI Logger and Configuration
/// </remarks>
/// <param name="logger"></param>
/// <param name="configuration"></param>
public sealed class DataService(ILogger<DataService> logger, IConfiguration configuration, DataContext dataContext) : IDataService
{
    private readonly ILogger<DataService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    public DataContext DataContext { get; } = dataContext;

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task SaveAsync(List<Number> numbersList)
    {
        _logger.LogInformation(nameof(SaveAsync));

        if (numbersList == null)
        {
            _logger.LogError(nameof(numbersList));

            throw new ArgumentNullException(nameof(numbersList));
        }

        await DataContext.AddRangeAsync(numbersList);

        await DataContext.SaveChangesAsync();
    }

    /// <summary>
    /// Fetch N records from the SQLite Database where N is specified by the count parameter
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Number>> GetAsync(int count)
    {
        _logger.LogInformation(nameof(GetAsync));

        return await DataContext.Numbers
            .Take(count)
            .OrderBy(n => n.Value)
            .ToListAsync();
    }

    /// <summary>
    /// Fetch All the records from the SQLite Database
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Number>> GetAllAsync()
    {
        _logger.LogInformation(nameof(GetAllAsync));

        return await DataContext.Numbers.ToListAsync();
    }

    public async Task<string> SerializeToXmlAsync()
    {
        _logger.LogInformation(nameof(SerializeToXmlAsync));

        var numbers = await DataContext.Numbers.ToListAsync();

        var xmlSerializer = new XmlSerializer(typeof(List<Number>));

        using var stringWriter = new StringWriter();

        xmlSerializer.Serialize(stringWriter, numbers);

        return stringWriter.ToString();
    }

    public async Task<byte[]> SerializeToBinaryAsync()
    {
        _logger.LogInformation(nameof(SerializeToBinaryAsync));

        var numbers = await DataContext.Numbers.ToListAsync();

        using var stream = new MemoryStream();

        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);

        foreach (var number in numbers)
        {
            writer.Write(number.Value);

            writer.Write(number.IsPrime);
        }

        return stream.ToArray();
    }
}

public interface IDataService
{
    Task<string> SerializeToXmlAsync();

    Task<byte[]> SerializeToBinaryAsync();

    Task<IEnumerable<Number>> GetAsync(int count);

    Task<IEnumerable<Number>> GetAllAsync();

    Task SaveAsync(List<Number> numbersList);
}
