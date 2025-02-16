namespace clickdown.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void UpdateFrom(UserViewModel userVm)
    {
        Email = userVm.Email;
        Username = userVm.Username;
        Password = HashingService.GenerateHashedPassword(userVm.Password);
    }
}

public class UserViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public User ToUser()
    {
        return new User
        {
            Email = this.Email,
            Username = this.Username,
            Password = this.Password,
        };
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public static UserDto CreateUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username
        };
    }
}