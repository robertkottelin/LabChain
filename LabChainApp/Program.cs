using System;
using System.IO;

namespace LabChainApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // read and display the sqlite database named blochchain.db
            string connectionString = "Data Source=blockchain.db;Version=3;";
            var databaseManager = new DatabaseManager(connectionString);
            
        }
    }
}