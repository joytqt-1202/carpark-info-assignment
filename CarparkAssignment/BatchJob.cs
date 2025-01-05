using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using CarparkAssignment.Data;
using CarparkAssignment.Models;

namespace CarparkAssignment;

public class BatchJob {
    public static void ProcessFile(string filePath) {
        using var dbContext = new CarparkDbContext();
        using var transaction = dbContext.Database.BeginTransaction();

        try {
            Console.WriteLine("Loading data from file...");
            var carparks = ReadCsvFile(filePath);

            Console.WriteLine("Inserting records into the database...");
            foreach (var carpark in carparks) {
                var existing = dbContext.Carparks.Find(carpark.car_park_no);
                if (existing != null) {
                    // Update existing record
                    dbContext.Entry(existing).CurrentValues.SetValues(carpark);
                } else {
                    // Add new record
                    dbContext.Carparks.Add(carpark);
                }
            }

            dbContext.SaveChanges();
            transaction.Commit();
            Console.WriteLine("Batch job completed successfully.");
        } catch (Exception ex) {
            Console.WriteLine($"Error encountered: {ex.Message}. Rolling back...");
            transaction.Rollback();
        }
    }

    private static List<Carpark> ReadCsvFile(string filePath) {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<Carpark>().ToList();
        return records;
    }
}
