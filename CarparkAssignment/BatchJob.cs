using System.Globalization;
using CsvHelper;
using CarparkAssignment.Data;
using CarparkAssignment.Models;

namespace CarparkAssignment.BatchJobs;

public class BatchJob {
    private readonly CarparkDbContext _dbContext;

    public BatchJob(CarparkDbContext dbContext) {
        _dbContext = dbContext;
    }
    public void ProcessFile(string filePath) {
        using var transaction = _dbContext.Database.BeginTransaction();

        try {
            Console.WriteLine("Loading data from file...");
            var carparks = ReadCsvFile(filePath);

            Console.WriteLine("Inserting records into the database...");
            foreach (var carpark in carparks) {
                var existing = _dbContext.Carparks.Find(carpark.car_park_no);
                if (existing != null) {
                    // Update existing record
                    _dbContext.Entry(existing).CurrentValues.SetValues(carpark);
                } else {
                    // Add new record
                    _dbContext.Carparks.Add(carpark);
                }
            }

            _dbContext.SaveChanges();
            transaction.Commit();
            Console.WriteLine("Batch job completed successfully.");
        } catch (Exception ex) {
            Console.WriteLine($"Error encountered: {ex.Message}. Rolling back...");
            transaction.Rollback();
            throw new Exception(ex.Message);
        }
    }

    private static List<Carpark> ReadCsvFile(string filePath) {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<Carpark>().ToList();
        return records;
    }
}
