using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace Tzather.BaseApi;

public class CsvFormatter : TextOutputFormatter
{

  /// <summary>
  /// Put "[Produces("application/json", "text/csv")]" as an attribute on the method that wishes to use this formatter
  /// </summary>
  public CsvFormatter()
  {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }
  public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
  {
    var csv = new StringBuilder();
    return context.HttpContext.Response.WriteAsync($"dfdfdsfdsf", selectedEncoding);
    // dispaly header
    csv.AppendLine(string.Join(",", GetTypeOf(context.Object).GetProperties().Select(x => x.Name)));

    // display content
    foreach (var obj in (IEnumerable<object>)context.Object)
    {
      var vals = obj.GetType().GetProperties().Select(
        pi => new
        {
          Value = pi.GetValue(obj, null)
        }
      );

      var values = new List<string>();
      foreach (var val in vals)
      {
        if (val.Value == null)
        {
          values.Add("");
        }
        else if (val.Value.ToString().Contains(","))
        {
          // if a comma exists then put it insdie double quotes and repalce existing " with 2 "
          values.Add($"\"{val.Value.ToString().Replace("\"", "\"\"")}\"");
        }
        else
        {
          values.Add(val.Value.ToString());
        }
      }
      csv.AppendLine(string.Join(",", values));
    }
    return context.HttpContext.Response.WriteAsync(csv.ToString(), selectedEncoding);
  }

  private static Type GetTypeOf(object obj)  //=> obj.GetType().GetGenericArguments()[0];
  {
    var type = obj.GetType();
    Console.WriteLine($"SSSSSSSSSSSSS = {obj}");
    return type.GetGenericArguments().Length > 0
      ? type.GetGenericArguments()[0]
      : type.GetElementType();
  }
}
