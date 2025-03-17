// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TechAptV1.Client.Services;

/// <summary>
/// Data Access Service for interfacing with the SQLite Database
/// </summary>
public sealed class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly IConfiguration _configuration;
    private readonly DataContext _context;

    /// <summary>
    /// Default constructor providing DI Logger and Configuration
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public DataService(ILogger<DataService> logger, IConfiguration configuration, DataContext context)
    {
        _logger = logger;
        _configuration = configuration;
        _context = context;
    }

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task SaveAsync(List<Number> numbersList)
    {
        _logger.LogInformation(nameof(SaveAsync));

        await _context.AddRangeAsync(numbersList);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Fetch N records from the SQLite Database where N is specified by the count parameter
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Number>> GetAsync(int count)
    {
        _logger.LogInformation(nameof(GetAsync));

        return await _context
            .Numbers
            .Take(count)
            .ToListAsync();
    }

    /// <summary>
    /// Fetch All the records from the SQLite Database
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Number>> GetAllAsync()
    {
        _logger.LogInformation(nameof(GetAllAsync));

        return await _context.Numbers.ToListAsync();
    }

    public async Task<string> SerializeToXmlAsync()
    {
        _logger.LogInformation(nameof(SerializeToXmlAsync));

        var numbers = await _context.Numbers.ToListAsync();

        var xmlSerializer = new XmlSerializer(typeof(List<Number>));

        using var stringWriter = new StringWriter();

        xmlSerializer.Serialize(stringWriter, numbers);

        return stringWriter.ToString();
    }
}

public interface IDataService
{
    Task<string> SerializeToXmlAsync();

    Task<IEnumerable<Number>> GetAsync(int count);

    Task<IEnumerable<Number>> GetAllAsync();

    Task SaveAsync(List<Number> numbersList);
}
