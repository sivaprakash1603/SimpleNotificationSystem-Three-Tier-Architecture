using Npgsql;

namespace NotificationDALLibrary.Infrastructure
{
    public sealed class PostgresConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be empty.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}