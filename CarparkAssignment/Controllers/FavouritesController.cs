using CarparkAssignment.Data;
using CarparkAssignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/favourites")]
public class FavouritesController: ControllerBase {
    private CarparkDbContext _dbContext;
    public FavouritesController(CarparkDbContext dbContext) {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Find user favourite by user id
    /// </summary>
    /// <param name="id">The ID of the user</param>
    /// <response code="200">Lists the ids of favourite car parks of the user</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(string[]))]
    public async Task<IActionResult> GetUserFavourites(int id) {
        try {
            var favourites = _dbContext.UserFavourites.AsQueryable();

            favourites = favourites.Where(f => f.user_id == id);
            var result = await favourites.ToListAsync();
            var user_favourites = new List<string>();
            foreach (var res in result) {
                user_favourites.Add(res.car_park_no);
            }
            return Ok(user_favourites);

        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Add car park as one of user's favourites
    /// </summary>
    /// <param name="request">The request containing user id and car park id to add as favourite</param>
    /// <response code="200">Car park {car_park_no} added to user's favourites</response>
    [HttpPost("add")]
    public async Task<IActionResult> AddUserFavourites (UserFavouritesRequest request) {
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

            var record = new UserFavourites {
                car_park_no = request.car_park_no,
                user_id = request.user_id
            };

            _dbContext.UserFavourites.Add(record);
            await _dbContext.SaveChangesAsync();
            
            return Ok($"Car park {request.car_park_no} added to favourites.");
        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Remove car park from one of user's favourite
    /// </summary>
    /// <param name="request">The request containing user if and carpark id to remove from favourites</param>
    /// <response code="200">Car park {car_park_no} removed from user's favourites</response>
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveUserFavourite (UserFavouritesRequest request) {
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
