using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;
using Xunit;

namespace Backend.Tests.Integration.Fixtures;

public class PostgresContainerFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; }

    public PostgresContainerFixture()
    {
        Container = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("moi-ivmiit-db")
            .WithUsername("postgres")
            .WithPassword("Postgres123!")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}