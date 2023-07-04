namespace Zuhid.Tools;

public static class RandomExtension
{
  public static string NextString(this Random random, int length)
  {
    var source = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~`!@#$%^&*()_-+={[}]|\\:;\"'<,>.?/";
    var result = Enumerable.Range(0, length).Select(s => source[random.Next(0, source.Length)]);
    return new string(result.ToArray());
  }

  public static string NextWord(this Random random)
  {
    var stringArray = new string[] {
      "able", "big", "child", "day", "early",
      "find", "give", "hand", "know", "last",
      "make", "next", "other", "place", "right",
      "same", "take", "want", "young",
    };
    return stringArray[random.Next(stringArray.Length)];
  }

  public static string NextListItem(this Random random, string[] stringArray)
  {
    return stringArray[random.Next(stringArray.Length)];
  }

  public static decimal NextDecimal(this Random random, int beforeDecimal, int afterDecimal)
  {
    int maxValue = (int)Math.Pow(10, beforeDecimal + afterDecimal) - 1;
    return (decimal)(random.Next(maxValue / 10, maxValue) / Math.Pow(10, afterDecimal));
  }

  public static DateTime NextDateTime(this Random random)
  {
    var maxMinutes = (int)(new DateTime(2050, 1, 1) - new DateTime(1950, 1, 1)).TotalMinutes;
    return new DateTime(1900, 1, 1).AddMinutes(random.Next(maxMinutes));
  }

  public static string NextDateTimeString(this Random random)
  {
    var dateTime = random.NextDateTime();
    return $"new DateTime({dateTime.Year}, {dateTime.Month}, {dateTime.Day}, {dateTime.Hour}, {dateTime.Minute}, {dateTime.Second}),";
  }
}
