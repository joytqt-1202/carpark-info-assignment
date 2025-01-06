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

    [HttpGet("search")]
    public async Task<IActionResult> SearchCarpark([FromBody] CarparkSearchRequest request) {
        var query = _dbContext.Carparks.AsQueryable();

        try {
            // Apply filters dynamically  
            // all filters in request body build on each other
            
            // for user to see if their vehicle height fits within the gantry height limit
            if (request.min_gantry_height.HasValue)
                query = query.Where(cp => cp.gantry_height >= request.min_gantry_height 
                                        || cp.gantry_height == 0); // gantry_height = 0 for surface carparks -- no height limit

            // for user to search whether they can park for free at a particular timing
            // options: NO, 1PM-10.30PM, 7AM-10.30PM
            // NO > 1PM-10.30PM > 7AM-10.30PM
            if (!string.IsNullOrEmpty(request.free_parking)) {
                if (request.free_parking == "SUN & PH FR 1PM-10.30PM") {
                    query = query.Where(cp => cp.free_parking != "NO");
                } else if (request.free_parking == "SUN & PH FR 7AM-10.30PM") {
                    query = query.Where(cp => cp.free_parking == request.free_parking);
                }
            }

            // for user to search whether they can park at night
            // options: YES, NO
            // NO would also include YES records
            if (!string.IsNullOrEmpty(request.night_parking) && request.night_parking == "YES")
                query = query.Where(cp => cp.night_parking == request.night_parking);


            // Additional Search conditions
            // for user to search for which carpark has their preferred payment method
            if (!string.IsNullOrEmpty(request.type_of_parking_system)) 
                query = query.Where(cp => cp.type_of_parking_system == request.type_of_parking_system);

            // TODO: add more search conditions

            // Execute the query and get the results
            var filteredCarParks = await query.ToListAsync();

            return Ok(filteredCarParks);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
