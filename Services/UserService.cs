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
            .Select(User.CreateDto)
            .ToList();
    }
    
    public UserDto? GetById(int id)
    {
        User? userDb = _context.Users.Find(id);
        return userDb is null ?
            null : User.CreateDto(userDb);
    }
    
    public User Register(UserViewModel userVm)
    {
        byte[] salt = HashingService.GenerateSalt();
        string hash = HashingService.HashPassword(userVm.Password, salt);
        User newUser = userVm.ToUser(salt, hash);

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return newUser;
    }
    
    public string? Login(UserViewModel userVm)
    {
        User? userDb = _context
            .Users
            .FirstOrDefault(u => u.Email == userVm.Email);

        if (userDb is null) return null;

        byte[] salt = Convert.FromHexString(userDb.Salt);
        string hash = HashingService.HashPassword(userVm.Password, salt);

        if (hash != userDb.Hash) return null;
        
        return TokenService.GenerateToken(userDb);
    }
    
    public User? Update(int id, UserViewModel userVm)
    {
        User? userDb = _context.Users.Find(id);
        if (userDb is null) return null;
        
        byte[] salt = HashingService.GenerateSalt();
        string hash = HashingService.HashPassword(userVm.Password, salt);
        
        userDb.UpdateFrom(userVm, salt, hash);
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