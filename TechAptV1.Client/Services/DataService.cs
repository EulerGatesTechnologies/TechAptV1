// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TechAptV1.Client.Services;

public interface IDataService
{
    Task<string> SerializeToXmlAsync();

    Task<IEnumerable<Number>> GetAsync(int count);

    Task<IEnumerable<Number>> GetAllAsync();
}
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
        this._logger = logger;
        this._configuration = configuration;
        this._context = context;
    }

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task Save(List<Number> dataList)
    {
        this._logger.LogInformation("Save");

        await _context.AddRangeAsync(dataList);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Fetch N records from the SQLite Database where N is specified by the count parameter
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Number>> GetAsync(int count)
    {
        this._logger.LogInformation("Get");

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
        this._logger.LogInformation("GetAll");

        return await _context.Numbers.ToListAsync();
    }

    public async Task<string> SerializeToXmlAsync()
    {
        var numbers = await _context.Numbers.ToListAsync();

        var xmlSerializer = new XmlSerializer(typeof(List<Number>));     

        using var stringWriter = new StringWriter();
        
        xmlSerializer.Serialize(stringWriter, numbers);
        
        return stringWriter.ToString();
    }
}