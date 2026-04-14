using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace QuanLyKhachSanWeb.DAL
{
    public class DBConnect
    {
        private readonly string _connectionString;

        public DBConnect()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["QuanLyKhachSanConnection"]?.ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new InvalidOperationException("Không tìm thấy connection string 'QuanLyKhachSanConnection' trong App.config.");
            }

            var errors = new List<string>();
            string databaseName = new SqlConnectionStringBuilder(_connectionString).InitialCatalog;

            foreach (string candidate in BuildCandidateConnectionStrings())
            {
                SqlConnectionStringBuilder probeBuilder = new SqlConnectionStringBuilder(candidate)
                {
                    InitialCatalog = "master"
                };

                try
                {
                    using (var probeConnection = new SqlConnection(probeBuilder.ConnectionString))
                    {
                        probeConnection.Open();

                        using (var command = new SqlCommand("SELECT COUNT(1) FROM sys.databases WHERE name = @DatabaseName", probeConnection))
                        {
                            command.Parameters.AddWithValue("@DatabaseName", databaseName);
                            int exists = Convert.ToInt32(command.ExecuteScalar());

                            if (exists == 0)
                            {
                                errors.Add($"[{probeBuilder.DataSource}] Không tìm thấy database '{databaseName}'.");
                                continue;
                            }
                        }
                    }

                    var connection = new SqlConnection(candidate);
                    connection.Open();
                    return connection;
                }
                catch (Exception ex)
                {
                    errors.Add($"[{probeBuilder.DataSource}] {ex.Message}");
                }
            }

            throw new InvalidOperationException(
                "Không thể kết nối tới cơ sở dữ liệu QuanLyKhachSan. " +
                "Hãy kiểm tra quyền truy cập SQL Server và Data Source trong App.config.\n" +
                string.Join("\n", errors));
        }

        private IEnumerable<string> BuildCandidateConnectionStrings()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = string.IsNullOrWhiteSpace(new SqlConnectionStringBuilder(_connectionString).InitialCatalog)
                    ? "QuanLyKhachSan"
                    : new SqlConnectionStringBuilder(_connectionString).InitialCatalog,
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                Encrypt = false
            };

            var dataSources = new[]
            {
                builder.DataSource,
                @".\SQLEXPRESS",
                ".",
                @"(local)\SQLEXPRESS",
                "(local)",
                @"localhost\SQLEXPRESS",
                "localhost",
                $@"{Environment.MachineName}\SQLEXPRESS",
                Environment.MachineName
            }
            .Where(source => !string.IsNullOrWhiteSpace(source))
            .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (string dataSource in dataSources)
            {
                builder.DataSource = dataSource;
                yield return builder.ConnectionString;
            }
        }

        private static string GetDataSource(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString).DataSource;
        }
    }
}
