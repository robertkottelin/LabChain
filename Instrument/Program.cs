using System;
using System.IO;

namespace Instrument
{
    class Program
    {
        static void Main(string[] args)
        {
            string logFilePath = "/home/robert/LabChain/instrument_log.csv";
            GenerateLogFile(logFilePath);
            Console.WriteLine($"Log file generated at {logFilePath}");
        }

        static void GenerateLogFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Timestamp, Temperature, Pressure"); // Header

                Random random = new Random();
                for (int i = 0; i < 1000; i++) // Generate 100 log entries
                {
                    string timestamp = DateTime.Now.AddMinutes(i * 5).ToString("yyyy-MM-dd HH:mm:ss");
                    // random temperature between 20 and 25
                    double temperature = 20 + random.NextDouble() * 5;
                    double pressure = random.NextDouble() * 2;
                    writer.WriteLine($"{timestamp}, {temperature:F2}, {pressure:F2}");
                }
            }
        }
    }
}
