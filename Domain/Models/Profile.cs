using System.ComponentModel.DataAnnotations;

namespace ProfileService.Domain.Models;

public class Profile
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Postcode { get; set; }
}