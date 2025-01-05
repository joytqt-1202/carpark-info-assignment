namespace CarparkAssignment.Models;

public class Carpark {
    public required string car_park_no { get; set; }
    public required string address { get; set; }
    public required double x_coord { get; set; }
    public required double y_coord { get; set; }
    public int car_park_decks { get; set; }
    public double gantry_height { get; set; }
    public required string car_park_basement { get; set; }
    public required string car_park_type { get; set; }
    public required string type_of_parking_system { get; set; }
    public required string short_term_parking { get; set; }
    public required string free_parking { get; set; }
    public required string night_parking { get; set; }
}
