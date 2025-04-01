// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
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

    public DataContext DataContext { get; } = dataContext;

    // Update: Use the configuration to get the connection string.
    public string ConnectionString { get;  } = configuration?.GetConnectionString("Default");
       

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task SaveAsync(List<Number> dataList, IProgress<int> progress = null)
    {
        if (dataList == null)
        {
            string message = $"Numbers list is null in {nameof(SaveAsync)}";

            _logger.LogError(message, nameof(SaveAsync));

            throw new ArgumentNullException(message, nameof(dataList));
        }

        const int batchSize = 1000;
        int totalRecords = dataList.Count;
        int processedRecords = 0;

        _logger.LogInformation($"{nameof(SaveAsync)} - Starting to save '{totalRecords}' records, with batch size '{batchSize}', completed '{processedRecords}'...");

        // Ensure we have a valid connection string
        if (string.IsNullOrEmpty(ConnectionString))
        {
            throw new InvalidOperationException("Database connection string is not properly configured.");
        }

        // Clear existing data
        await DataContext.Database.ExecuteSqlRawAsync("DELETE FROM Number");

        // Use System.Data.SQLite for efficient bulk inserts
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();

        // Begin transaction
        using var transaction = connection.BeginTransaction();

        try
        {
            for (int start = 0; start < totalRecords; start += batchSize)
            {
                int currentBatchSize = Math.Min(batchSize, totalRecords - start);

                var batch = dataList.Skip(start).Take(currentBatchSize).ToList();

                // Dynamically build the INSERT statement for this batch
                var sb = new StringBuilder("INSERT OR IGNORE INTO Number (Value, IsPrime) VALUES ");

                for (int i = 0; i < currentBatchSize; i++)
                {
                    if (i > 0) sb.Append(",");
                    sb.Append($"(@value{i}, @isPrime{i})");
                }

                using var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = sb.ToString();

                // Add parameters for this batch
                for (int i = 0; i < currentBatchSize; i++)
                {
                    command.Parameters.AddWithValue($"@value{i}", batch[i].Value);
                    command.Parameters.AddWithValue($"@isPrime{i}", batch[i].IsPrime);
                }

                await command.ExecuteNonQueryAsync();

                processedRecords += currentBatchSize;

                if (progress != null)
                {
                    int percentage = (int)((double)processedRecords / totalRecords * 100);
                    progress.Report(percentage);
                }
            }

            // Commit the transaction
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    private bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;
        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
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
            .OrderByDescending(n => n.Value)
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

    Task SaveAsync(List<Number> List, IProgress<int> progress = null);
}
