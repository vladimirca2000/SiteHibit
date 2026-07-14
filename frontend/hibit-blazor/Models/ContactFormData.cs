using System.ComponentModel.DataAnnotations;

namespace Hibit.Web.Models;

public class ContactFormData
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = "É necessário concordar com a política de privacidade")]
    public bool ConsentGiven { get; set; }
}