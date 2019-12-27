using System;
using System.Security.Cryptography;
using System.Text;

namespace ZipCodeApi.Authorization
{
    public static class SecurityManager
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh";

        public static string GenerateToken(string username, string password)
        {
            string hash = string.Join(":", new string[] { username });
            string hashLeft = "";
            string hashRight = "";

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { username });
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        public static string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));

                return Convert.ToBase64String(hmac.Hash);
            }
        }

        public static bool IsTokenValid(string token)
        {
            bool result = false;
            try
            {
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                string[] parts = key.Split(new char[] { ':' });

                if (parts.Length > 1)
                {
                    string hash = parts[0];
                    string username = parts[1];
                    string password = "am_pass";

                    string computedToken = GenerateToken(username, password);

                    result = token == computedToken;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return result;
        }
    }
}