using System;
using System.Data.SQLite;
using Newtonsoft.Json;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;

        // Initialize the database when the manager is constructed.
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            // Create a command to execute the SQL statements.
            using (var command = new SQLiteCommand(connection))
            {
                // SQL statement to create a 'blockchain' table if it doesn't exist.
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS blockchain (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Timestamp DATETIME NOT NULL,
                    Data TEXT NOT NULL,
                    PreviousHash TEXT NOT NULL,
                    Hash TEXT NOT NULL
                )";
                command.ExecuteNonQuery();
            }
        }
    }

    public void InsertBlock(Block block)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                var command = connection.CreateCommand();

                // Serialize block data to JSON for storage
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

    // Add more database operations as needed...
}

// A simple Block class for demonstration purposes
public class Block
{
    public DateTime Timestamp { get; set; }
    public object Data { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
}
