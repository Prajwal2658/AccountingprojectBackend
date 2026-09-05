using AccountingSystem.Application.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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
                    u.Id,
                    u.Username,
                    u.PasswordHash,
                    u.FullName,
                    r.Name AS RoleName
                FROM Users u
                LEFT JOIN UserRoles ur
                    ON u.Id = ur.UserId
                LEFT JOIN Roles r
                    ON ur.RoleId = r.Id
                WHERE u.Username = @Username
                  AND u.IsActive = 1;
            ";

            AuthUser? user = null;

            await using var connection =
                new SqlConnection(_connectionString);

            await connection.OpenAsync();

            await using var command =
                new SqlCommand(sql, connection);

            command.Parameters.Add(
                new SqlParameter("@Username", SqlDbType.NVarChar, 100)
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
                            reader.GetOrdinal("Id")),

                        Username = reader.GetString(
                            reader.GetOrdinal("Username")),

                        PasswordHash = reader.GetString(
                            reader.GetOrdinal("PasswordHash")),

                        FullName = reader.IsDBNull(
                            reader.GetOrdinal("FullName"))
                            ? string.Empty
                            : reader.GetString(
                                reader.GetOrdinal("FullName"))
                    };
                }

                if (!reader.IsDBNull(
                        reader.GetOrdinal("RoleName")))
                {
                    user.Roles.Add(
                        reader.GetString(
                            reader.GetOrdinal("RoleName")));
                }
            }

            return user;
        }
    }
}