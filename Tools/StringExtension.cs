using System.Text;

namespace Zuhid.Tools;

public static class StringExtension
{
  public static string ToCamelCase(this string str)
  {
    return str == null ? null : str.Substring(0, 1).ToLower() + str.Substring(1);
  }

  public static string ToUpperCaseFirst(this string str)
  {
    return str == null ? null : str.Substring(0, 1).ToUpper() + str.Substring(1);
  }

  public static string RemoveSpecialCharacters(this string str)
  {
    // return null if null is sent in
    if (str == null)
    {
      return str;
    }

    var stringBuilder = new StringBuilder();
    foreach (char c in str)
    {
      if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z')
      {
        stringBuilder.Append(c);
      }
    }
    return stringBuilder.ToString();
  }
}
