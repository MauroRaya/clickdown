namespace clickdown.Utils;

public class TokenUtil
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
        
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
        
        return claimsIdentity;
    }

    public static ClaimsPrincipal? ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(Configuration.PrivateKey);

        if (key.IsNullOrEmpty()) return null;

        try
        {
            var validation = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(token, validation, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}