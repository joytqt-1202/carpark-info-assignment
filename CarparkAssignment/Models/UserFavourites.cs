namespace CarparkAssignment.Models;

public class UserFavourites {
    public int id { get; set; }
    public required int user_id { get; set; }
    public required string car_park_no { get; set; }

    // TODO: add and link user table
    /* public required User user { get; set; } */
    public Carpark? carpark { get; set; }
}
