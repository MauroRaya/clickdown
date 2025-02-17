namespace clickdown.Utils;

public static class HashingUtil
{
    public static string GenerateHash(string password, byte[] salt)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = salt.Concat(passwordBytes).ToArray();
        byte[] hashed = SHA256.HashData(saltedPassword);
        
        return Convert.ToHexString(hashed);
    }

    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return salt;
    }
}