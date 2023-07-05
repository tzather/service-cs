using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Tzather.Tools;

public class SecurityService : ISecurityService
{
  // https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Crypto.cs

  public string HashPassword(string password)
  {
    return Crypto.HashPassword(password);
  }

  public bool VerifyHashedPassword(string hashedPassword, string password)
  {
    return Crypto.VerifyHashedPassword(hashedPassword, password);
  }

  // public string HashString(string password, string salt) {
  //   // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0
  //   return Convert.ToBase64String(KeyDerivation.Pbkdf2(
  //     password: password,
  //     salt: Convert.FromBase64String(salt),
  //     prf: KeyDerivationPrf.HMACSHA256,
  //     iterationCount: 100000,
  //     numBytesRequested: 256 / 8)
  //   );
  // }

  public string[] TimeBasedToken(string secret, int digits, int validSeconds)
  {
    var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
    var counter = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / validSeconds;
    return new string[] {
      GetTotp(hmacsha1, digits, counter), // current 
      GetTotp(hmacsha1, digits, counter-1), // previous
    };
  }

  private static string GetTotp(HMACSHA1 hmacsha1, int digits, long counter)
  {
    var hash = hmacsha1.ComputeHash(BitConverter.GetBytes(counter));
    var offset = hash[hash.Length - 1] & 0xf;
    var password = (hash[offset] & 0x7f) << 24
      | (hash[offset + 1] & 0xff) << 16
      | (hash[offset + 2] & 0xff) << 8
      | (hash[offset + 3] & 0xff);
    return (password % (int)Math.Pow(10, digits)).ToString().PadLeft(digits, '0');
  }
}
