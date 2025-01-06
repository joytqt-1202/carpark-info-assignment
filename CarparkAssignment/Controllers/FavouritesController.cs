using CarparkAssignment.Data;
using CarparkAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/favourites")]
public class FavouritesController: ControllerBase {
    private CarparkDbContext _dbContext;
    public FavouritesController(CarparkDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserFavourites(int id) {
        try {
            var favourites = _dbContext.UserFavourites.AsQueryable();

            favourites = favourites.Where(f => f.user_id == id);
            var result = await favourites.ToListAsync();

            return Ok(result);
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddUserFavourites (UserFavourites request) {
        try {
            // Check if the carpark exists
            var carpark = await _dbContext.Carparks.FindAsync(request.car_park_no);
            if (carpark == null) 
                return BadRequest($"Unable to favourite unknown car park {request.car_park_no}.");

            // TODO: Check if the user exists after adding User table

            // Check if favourites already exists
            var existing_favourite = await _dbContext.UserFavourites
                    .FirstOrDefaultAsync(
                        uf => uf.user_id == request.user_id 
                            && uf.car_park_no == request.car_park_no 
                    );

            if (existing_favourite != null)
                return BadRequest($"Car park {request.car_park_no} is already user's favourite.");

            _dbContext.UserFavourites.Add(request);
            await _dbContext.SaveChangesAsync();
            
            return Ok($"Car park {request.car_park_no} added to favourites.");
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveUserFavourite (UserFavourites request) {
        try {
            // Check if favourites already exists
            var existing_favourite = await _dbContext.UserFavourites
                    .FirstOrDefaultAsync(
                        uf => uf.user_id == request.user_id 
                            && uf.car_park_no == request.car_park_no 
                    );

            if (existing_favourite == null)
                return BadRequest($"Car park {request.car_park_no} is not user's favourite.");

            _dbContext.UserFavourites.Remove(existing_favourite);
            await _dbContext.SaveChangesAsync();
            
            return Ok($"Car park {request.car_park_no} removed from favourites.");
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
