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
        return _context.Users
            .Select(UserDto.CreateUserDto)
            .ToList();
    }
    
    public UserDto? GetById(int id)
    {
        User? userDb = _context.Users.Find(id);
        return userDb is null ?
            null : UserDto.CreateUserDto(userDb);
    }
    
    public User Add(UserViewModel userVm)
    {
        User newUser = userVm.ToUser();
        newUser.Password = HashingService.GenerateHashedPassword(userVm.Password);

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return newUser;
    }
    
    public User? Update(int id, UserViewModel userVm)
    {
        User? userDb = _context.Users.Find(id);
        if (userDb is null) return null;
        
        userDb.UpdateFrom(userVm);
        _context.SaveChanges();
        
        return userDb;
    }
    
    public User? Remove(int id)
    {
        User? userDb = _context.Users.Find(id);
        if (userDb is null) return null;

        _context.Users.Remove(userDb);
        _context.SaveChanges();
        
        return userDb;
    }
}