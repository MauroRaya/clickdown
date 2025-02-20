namespace clickdown.Services;

public class UserService
{
    private AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<UserDto>>> GetAsync()
    {
        try
        {
            List<UserDto> usersDto = await _context.Users
                .Select(user => User.CreateDto(user))
                .ToListAsync();

            return usersDto.Count == 0
                ? Result<List<UserDto>>.NewError("No users found")
                : Result<List<UserDto>>.NewSuccess(usersDto);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id)
    {
        try
        {
            User? userDb = await _context.Users.FindAsync(id);
        
            return userDb is null 
                ? Result<UserDto>.NewError("User not found") 
                : Result<UserDto>.NewSuccess(User.CreateDto(userDb));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<User> RegisterAsync(UserViewModel userVm)
    {
        try
        {
            byte[] salt = HashingUtil.GenerateSalt();
            string hash = HashingUtil.GenerateHash(userVm.Password, salt);
            User newUser = userVm.ToUser(salt, hash);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<string?>> LoginAsync(UserViewModel userVm)
    {
        try
        {
            User? userDb = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userVm.Email);

            if (userDb is null)
                return Result<string?>.NewError("User not found");

            byte[] salt = Convert.FromHexString(userDb.Salt);
            string hash = HashingUtil.GenerateHash(userVm.Password, salt);

            if (hash != userDb.Hash)
                return Result<string?>.NewError("Password does not match");
            
            return Result<string?>.NewSuccess(TokenUtil.GenerateToken(userDb));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<UserDto>> UpdateAsync(string userId, int targetId, UserViewModel userVm)
    {
        try
        {
            if (!userId.IsNullOrEmpty())
                return Result<UserDto>.NewError("User ID is required");
            
            if (Convert.ToInt32(userId) != targetId)
                return Result<UserDto>.NewError("You dont have permission to update this user");

            User? userDb = await _context.Users.FindAsync(targetId);

            if (userDb is null)
                return Result<UserDto>.NewError("User not found");

            byte[] salt = HashingUtil.GenerateSalt();
            string hash = HashingUtil.GenerateHash(userVm.Password, salt);

            userDb.UpdateFrom(userVm, salt, hash);
            await _context.SaveChangesAsync();

            return Result<UserDto>.NewSuccess(User.CreateDto(userDb));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<UserDto>> RemoveAsync(string? userId, int targetId)
    {
        try
        {
            if (!userId.IsNullOrEmpty())
                return Result<UserDto>.NewError("User ID is required");
            
            if (Convert.ToInt32(userId) != targetId)
                return Result<UserDto>.NewError("You don't have permission to delete this user");

            User? userDb = await _context.Users.FindAsync(targetId);

            if (userDb is null)
                return Result<UserDto>.NewError("User not found");

            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();

            return Result<UserDto>.NewSuccess(User.CreateDto(userDb));
        }
        catch (Exception)
        {
            throw;
        }
    }
}