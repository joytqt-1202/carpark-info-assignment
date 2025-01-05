using CarparkAssignment;
using CarparkAssignment.Data;

class Program {
    static void Main(string[] args) {
        if (args.Length == 0) {
            Console.WriteLine("Please provide the path to the CSV file.");
            return;
        }

        string filePath = args[0];

        // Ensure the database is created
        using (var dbContext = new CarparkDbContext()) {
            dbContext.Database.EnsureCreated();
        }

        // Run the batch job
        BatchJob.ProcessFile(filePath);
    }
}
