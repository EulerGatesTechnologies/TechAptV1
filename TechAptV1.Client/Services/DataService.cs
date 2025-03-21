﻿// Copyright © 2025 Always Active Technologies PTY Ltd

using TechAptV1.Client.Models;

using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;
using TechAptV1.Client.Data;
using System.Text;
using Microsoft.Data.Sqlite;

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
    public string ConnectionString {get;  } = configuration?.GetConnectionString("Default");

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task SaveAsync(List<Number> dataList)
    {
        if (dataList == null)
        {
            string message = $"Numbers list is null in {nameof(SaveAsync)}";

            _logger.LogError(message, nameof(SaveAsync));

            throw new ArgumentNullException(message, nameof(dataList));
        }

         _logger.LogInformation(nameof(SaveAsync));

        // Clear existing data
        await DataContext.Database.ExecuteSqlRawAsync("DELETE FROM Number");

        // Update: use raw SQL for inserts, batching the records to improve performance.
        using (var connection = new SqliteConnection(ConnectionString))
        {
            await connection.OpenAsync();
            // Ensure that the table exists.
             // This is more robust & durable as we have witnessed with some tests
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ""Number"" (
                    ""Value"" INTEGER NOT NULL,
                    ""IsPrime"" INTEGER NOT NULL DEFAULT 0
                );
            ";

            tableCmd.ExecuteNonQuery();
            // Begin a transaction to efficiently insert many records.
            using (var transaction = connection.BeginTransaction())
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO \"Number\" (Value, IsPrime) VALUES (@value, @isPrime)";

                // Create parameters to avoid re-creating them on each insert.
                SqliteParameter valueParam = insertCmd.CreateParameter();
                valueParam.ParameterName = "@value";
                insertCmd.Parameters.Add(valueParam);

                SqliteParameter isPrimeParam = insertCmd.CreateParameter();
                isPrimeParam.ParameterName = "@isPrime";
                insertCmd.Parameters.Add(isPrimeParam);

                foreach (Number number in dataList)
                {
                    valueParam.Value = number.Value;

                    isPrimeParam.Value = number.IsPrime ? 1 : 0;

                    insertCmd.ExecuteNonQuery();
                }

                await transaction.CommitAsync();
            }
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

    Task SaveAsync(List<Number> List);
}
