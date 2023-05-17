using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using AutoMapper;
using Microsoft.Data.Sqlite;
using WorkPlanner.Data;
using WorkPlanner.Domain.Profiles;

namespace WorkPlanner.Tests;

public class TestBase
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;

    protected readonly IMapper Mapper;

    public TestBase()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new AppDbContext(_contextOptions);

        if (!context.Database.EnsureCreated())
        {
            throw new Exception("Database could be not created.");
        }

        Mapper = new Mapper(new MapperConfiguration(conf => conf.AddMaps(typeof(WorkerProfile))));
    }

    protected AppDbContext CreateContext() => new(_contextOptions);

    protected void Dispose() => _connection.Dispose();
}