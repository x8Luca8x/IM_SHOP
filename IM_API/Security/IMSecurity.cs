using System.Security.Cryptography;
using System.Text;

namespace IM_API.Security
{
    public static class IMSecurity
    {
        public static string SHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }

        public static string IMHash(string input)
        {
            string salt = "";
            for(int i = 0; i < 5; ++i)
                salt += (char)Random.Shared.Next(33, 126);

            return SHA256Hash(input + salt) + salt;
        }

        public static bool ComparePassword(string Password, string HashedPassword)
        {
            string salt = HashedPassword.Substring(64);
            return (SHA256Hash(Password + salt) + salt) == HashedPassword;
        }
    }
}
