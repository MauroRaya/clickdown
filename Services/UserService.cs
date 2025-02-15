namespace clickdown.Services;

public class UserService
{
    private AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public List<UserDto> Get()
    {
        var usersDb = _context.Users.ToList();

        var usersDto = new List<UserDto>();

        foreach (var user in usersDb)
        {
            usersDto.Add(new UserDto
            {
                Id = user.Id,
                Username = user.Username
            });
        }

        return usersDto;
    }
    
    public UserDto? GetById(int id)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = userDb.Id,
            Username = userDb.Username
        };
    }
    
    public User Add(UserViewModel user)
    {
        string hashedPassword = HashingService.GenerateHashedPassword(user.Password);

        var newUser = new User
        {
            Username = user.Username,
            Password = hashedPassword
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return newUser;
    }
    
    public User? Update(int id, UserViewModel user)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return null;
        }
        
        string hashedPassword = HashingService.GenerateHashedPassword(user.Password);

        userDb.Username = user.Username;
        userDb.Password = hashedPassword;
        _context.SaveChanges();

        return userDb;
    }
    
    public void Remove(int id)
    {
        var userDb = _context.Users.Find(id);

        if (userDb is null)
        {
            return;
        }

        _context.Users.Remove(userDb);
        _context.SaveChanges();
    }
}