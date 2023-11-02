using System.ComponentModel.DataAnnotations;

namespace ProfileService.Domain.DTOs;

public class MakerCreateDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Postcode { get; set; }
}