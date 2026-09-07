using AccountingSystem.Application.Authentication;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace AccountingSystem.Infrastructure.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "DefaultConnection is not configured.");
        }

        public async Task<AuthUser?> GetUserAsync(string username)
        {
            //var hash = BCrypt.Net.BCrypt.HashPassword("123456");
            const string sql = @"
                SELECT
                    u.id,
                    u.username,
                    u.password_hash,
                    u.full_name,
                    r.name AS role_name
                FROM users u
                LEFT JOIN user_roles ur
                    ON u.id = ur.user_id
                LEFT JOIN roles r
                    ON ur.role_id = r.id
                WHERE u.username = @Username
                  AND u.is_active = TRUE;
            ";

            AuthUser? user = null;

            await using var connection =
                new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var command =
                new NpgsqlCommand(sql, connection);

            command.Parameters.Add(
                new NpgsqlParameter("@Username", NpgsqlDbType.Varchar, 100)
                {
                    Value = username
                });

            await using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (user == null)
                {
                    user = new AuthUser
                    {
                        Id = reader.GetInt32(
                            reader.GetOrdinal("id")),

                        Username = reader.GetString(
                            reader.GetOrdinal("username")),

                        PasswordHash = reader.GetString(
                            reader.GetOrdinal("password_hash")),

                        FullName = reader.IsDBNull(
                            reader.GetOrdinal("full_name"))
                            ? string.Empty
                            : reader.GetString(
                                reader.GetOrdinal("full_name"))
                    };
                }

                if (!reader.IsDBNull(
                        reader.GetOrdinal("role_name")))
                {
                    user.Roles.Add(
                        reader.GetString(
                            reader.GetOrdinal("role_name")));
                }
            }

            return user;
        }
    }
}