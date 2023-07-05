using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;

namespace Tzather.Tools;

public class ObjectHelper
{
  public T DeepClone<T>(T source)
  {
    return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));
  }

  public bool Equals<T>(T left, T right)
  {
    bool returnValue = true;
    if (left == null && right != null)
    {
      returnValue = LogMismatch(right.GetType().FullName, left, right); ;
    }
    else if (left != null && right == null)
    {
      returnValue = LogMismatch(left.GetType().FullName, left, right);
    }
    else if (left != null && right != null)
    {
      var properties = left.GetType().GetProperties();
      foreach (PropertyInfo property in properties)
      {
        var leftValue = property.GetValue(left);
        var rightValue = property.GetValue(right);
        if (leftValue?.GetType().FullName == "System.String[]")
        {
          return ListStringEquals(leftValue as string[], rightValue as string[]);
        }

        if (property.GetCustomAttribute<DatabaseGeneratedAttribute>() != null)
        {
          continue;
        }
        else if (property.Name == "ModifiedDate" || property.Name == "CurrentPassword" || property.Name == "NewPassword")
        {
          continue;
        }
        else if (leftValue == null && rightValue == null)
        {
          continue;
        }
        else if (leftValue == null && rightValue != null)
        {
          returnValue = LogMismatch(property.Name, leftValue, rightValue);
          break;
        }
        else if (leftValue != null && rightValue == null)
        {
          returnValue = LogMismatch(property.Name, leftValue, rightValue);
          break;
        }
        else if (!leftValue.Equals(rightValue))
        {
          returnValue = LogMismatch(property.Name, leftValue, rightValue);
          break;
        }
      }
    }
    return returnValue;
  }

  public bool ListEquals<T>(List<T> left, List<T> right)
  {
    bool returnValue = true;
    if (left == null && right != null)
    {
      returnValue = LogMismatch(right.GetType().Name, left, right);
    }
    else if (left != null && right == null)
    {
      returnValue = LogMismatch(left.GetType().Name, left, right);
    }
    else if (left != null && right != null)
    {
      if (left.Count != right.Count)
      {
        returnValue = LogMismatch(left.GetType().Name, left.Count, right.Count);
      }
      else
      {
        for (int i = 0; i < left.Count; i++)
        {
          if (left[i] is string && right[i] is string)
          {
            returnValue = left[i].Equals(right[i]);
          }
          else
          {
            returnValue = Equals(left[i], right[i]);
          }
          if (!returnValue)
          {
            break;
          }
        }
      }
    }
    return returnValue;
  }

  private bool ListStringEquals(string[] left, string[] right)
  {
    bool returnValue = true;
    if (left.Length != right.Length)
    {
      returnValue = LogMismatch(left.GetType().Name, left.Length, right.Length);
    }
    else
    {
      for (int i = 0; i < left.Length; i++)
      {
        returnValue = left[i].Equals(right[i], StringComparison.InvariantCulture);
        if (!returnValue)
        {
          break;
        }
      }
    }
    return returnValue;
  }

  private bool LogMismatch(string property, object left, object right)
  {
    Console.WriteLine($"{property}: '{left}' '{right}'");
    return false;
  }
}
