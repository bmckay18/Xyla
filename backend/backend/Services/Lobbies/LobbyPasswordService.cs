using System.Security.Cryptography;
using System.Text;

namespace Backend.Services.Lobbies
{
    public static class LobbyPasswordService
    {
        public static byte[] GenerateSalt()
        {
            return RandomNumberGenerator.GetBytes(32);
        }

        public static string HashPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using var hmac = new HMACSHA256(salt);
            var hashBytes = hmac.ComputeHash(passwordBytes);

            var hashString = Convert.ToHexString(hashBytes);

            return hashString;
        }

        public static bool ValidatePassword(string inputPassword, string storedPassword, byte[] salt)
        {
            string inputtedPasswordHash = HashPassword(inputPassword, salt);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(inputtedPasswordHash), 
                Convert.FromHexString(storedPassword)
            );
        }
    }
}
