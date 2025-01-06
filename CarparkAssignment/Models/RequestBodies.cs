public class BatchJobRequest {
    public required string filePath { get; set; }
}

public class CarparkSearchRequest {
    public double? min_gantry_height { get; set; }
    public string? type_of_parking_system { get; set; }
    public string? free_parking { get; set; }
    public string? night_parking { get; set; }
}

public class UserFavouritesRequest {
    public required string car_park_no { get; set; }
    public required int user_id { get; set; }
}
