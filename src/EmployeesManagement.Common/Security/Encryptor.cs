using System.Security.Cryptography;
using EmployeesManagement.Common.Interfaces;

namespace EmployeesManagement.Common.Security;

public class Encryptor : IEncryptor
{
    private const int SaltSize = 40;
    private const int IterationsCount = 10000;
    

    public string GetHash(string value, string salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), IterationsCount);

        return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
    }

    private static byte[] GetBytes(string value)
    {
        var bytes = new byte[value.Length * sizeof(char)];
            
        Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

        return bytes;
    }
}