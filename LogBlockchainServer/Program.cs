using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=blockchain.db;Version=3;";
        var databaseManager = new DatabaseManager(connectionString);
        Blockchain blockchain = new Blockchain();

        string logFilePath = "/home/robert/LabChain/instrument_log.csv";

        // Read logs from file
        List<string> logEntries = ReadLogEntries(logFilePath);

        // Add log entries to the blockchain and database
        foreach (var logEntry in logEntries)
        {
            AddLogEntryToBlockchain(logEntry, blockchain, databaseManager);
        }

        Console.WriteLine("Blockchain and Database updated with logs.");
    }

    static List<string> ReadLogEntries(string filePath)
    {
        List<string> logEntries = new List<string>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Log file not found.");
            return logEntries;
        }

        using (var reader = new StreamReader(filePath))
        {
            string headerLine = reader.ReadLine(); // Assuming the first line is a header

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                logEntries.Add(line);
            }
        }

        return logEntries;
    }

    static void AddLogEntryToBlockchain(string logEntry, Blockchain blockchain, DatabaseManager dbManager)
    {
        // Assuming logEntry is a CSV line, here we might process or validate the data

        // Create a new block with the log data
        Block newBlock = new Block(DateTime.Now, blockchain.GetLatestBlock().Hash, logEntry);

        // Add the block to the blockchain
        blockchain.AddBlock(newBlock);

        // Save the block to the database
        dbManager.InsertBlock(newBlock);
    }
}
