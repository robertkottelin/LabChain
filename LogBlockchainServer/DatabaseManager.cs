using System;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS blockchain (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp DATETIME,
                    Data TEXT,
                    PreviousHash TEXT,
                    Hash TEXT
                )";
                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertBlock(Block block)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();
                string jsonData = JsonConvert.SerializeObject(block.Data);

                command.CommandText = @"
                INSERT INTO blockchain (Timestamp, Data, PreviousHash, Hash)
                VALUES (@Timestamp, @Data, @PreviousHash, @Hash)";
                
                command.Parameters.AddWithValue("@Timestamp", block.Timestamp);
                command.Parameters.AddWithValue("@Data", jsonData);
                command.Parameters.AddWithValue("@PreviousHash", block.PreviousHash);
                command.Parameters.AddWithValue("@Hash", block.Hash);

                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
