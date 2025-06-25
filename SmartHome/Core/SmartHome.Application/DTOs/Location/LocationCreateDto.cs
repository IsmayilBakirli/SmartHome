using System.ComponentModel.DataAnnotations;

public class LocationCreateDto
{
    [Required(ErrorMessage = "Location name is required")]
    [MaxLength(50, ErrorMessage = "Location name cannot exceed 50 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Zone name is required")]
    [MaxLength(50, ErrorMessage = "Zone name cannot exceed 50 characters.")]
    public string Zone { get; set; }

    [Required(ErrorMessage = "Floor is required")]
    [Range(0, 100, ErrorMessage = "Floor cannot be greater than 100")]
    public int Floor { get; set; }

    [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
    public string? Description { get; set; }
}
