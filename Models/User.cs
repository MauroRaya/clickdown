namespace clickdown.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    
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
        Role = "Funcionario";
    }
}

public class UserViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public User ToUser(byte[] salt, string hash)
    {
        return new User
        {
            Email = this.Email,
            Username = this.Username,
            Salt = Convert.ToHexString(salt),
            Hash = hash,
            Role = "Funcionario"
        };
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}