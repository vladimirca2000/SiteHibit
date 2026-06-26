using Hibit.Domain.Common;

namespace Hibit.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Role { get; set; } = "Application";
}
