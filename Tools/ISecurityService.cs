namespace Zuhid.Tools;

public interface ISecurityService
{

  /// <summary>
  /// Uses the string and salt to return it as a hash 
  /// </summary>
  string HashString(string password, string salt);

  /// <summary>
  /// Returns the current totp as the first item in index[0], and the previous one in index[1]
  /// </summary>
  string[] TimeBasedToken(string secret, int digits, int validSeconds);
}
