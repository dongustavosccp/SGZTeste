﻿using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Infrastructure.Helpers
{
    public static class PassEncryptor
    {
        public static string Encrypt(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}