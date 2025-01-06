using CarparkAssignment.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/carparks")]
public class CarparkController: ControllerBase {
    private CarparkDbContext _dbContext;
    public CarparkController(CarparkDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCarparks() {
        try {
            var carparks = await _dbContext.Carparks.ToListAsync();
            return Ok(carparks);
        } catch (Exception ex) {
            return StatusCode(500, $"Error encountered: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCarparkById(string id) {
        try {
            var carpark = await _dbContext.Carparks.FindAsync(id);
            if (carpark == null) {
                return NotFound($"Car park with ID {id} not found.");
            }
            return Ok(carpark);
        } catch (Exception ex) {
            return StatusCode(500, $"Error encountered: {ex.Message}");
        }


    }
}
