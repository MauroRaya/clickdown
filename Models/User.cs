using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clickdown.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    
    public static UserDto CreateDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
        };
    }

    public void UpdateFrom(UserViewModel userVm, byte[] salt, string hash)
    {
        Email = userVm.Email;
        Username = userVm.Username;
        Salt = Convert.ToHexString(salt);
        Hash = hash;
    }
}

public class UserViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    public User ToUser(byte[] salt, string hash)
    {
        return new User
        {
            Email = this.Email,
            Username = this.Username,
            Salt = Convert.ToHexString(salt),
            Hash = hash,
        };
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}