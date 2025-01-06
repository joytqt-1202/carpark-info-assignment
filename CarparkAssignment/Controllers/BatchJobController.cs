using CarparkAssignment.BatchJobs;
using CarparkAssignment.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarparkAssignment.Controllers;

[ApiController]
[Route("/batch-job")]
public class BatchJobController: ControllerBase {
    private readonly BatchJob _batchJob;

    public BatchJobController(CarparkDbContext dbContext) {
        _batchJob = new BatchJob(dbContext);
    }

    /// <summary>
    /// Runs a batch job to process and add all records into the database.
    /// </summary>
    /// <param name="request">The request containing file path to .csv file</param>
    /// <response code="200">Batch Job processed successfully.</response>
    [HttpPost("process")] // POST: /batch-job/process
    public IActionResult ProcessFile([FromBody] BatchJobRequest request) {
    if (request == null || string.IsNullOrEmpty(request.filePath)) {
        return BadRequest("File path is required.");
    }

    try {
        _batchJob.ProcessFile(request.filePath);
        return Ok("Batch Job processed successfully.");
    } catch (Exception ex) {
        return StatusCode(500, $"Error processing batch job: {ex.Message}");
    }
    }
}
