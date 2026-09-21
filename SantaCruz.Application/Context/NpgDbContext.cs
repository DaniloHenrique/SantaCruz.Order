using Microsoft.Extensions.Options;
using Npgsql;
using SantaCruz.Application.DI;
using SantaCruz.Domain.Repository;

namespace SantaCruz.Application.Context
{
    public class NpgDbContext(IOptions<ApiConfig> config) : DatabaseContext(
        new NpgsqlConnection(config.Value.ConnectionStrings)
        ), IDatabaseContext
    {
    }
}
