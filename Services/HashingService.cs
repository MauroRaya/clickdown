using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace clickdown.Services;

public static class HashingService
{
    public static string GenerateHashedPassword(string password)
    {
        byte[] bytes  = Encoding.UTF8.GetBytes(password);
        byte[] hashed = SHA256.HashData(bytes);
        return Convert.ToHexString(hashed);
    }
}