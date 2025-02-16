namespace clickdown.Services;

public class TokenService
{
    public static string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        byte[] key = Encoding.UTF8.GetBytes(Configuration.PrivateKey);
        
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        { 
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
            Subject = GenerateClaims(user)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
    }

    public static ClaimsIdentity GenerateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(
            new Claim(ClaimTypes.Name, user.Username)
        );
        
        claimsIdentity.AddClaim(
            new Claim(ClaimTypes.Role, user.Role)
        );
        
        return claimsIdentity;
    }
}