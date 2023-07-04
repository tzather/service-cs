using System.Reflection;

namespace Zuhid.Tools;

public class TestSmsService : ISmsService
{
  private readonly string basePath;

  public TestSmsService(string basePath)
  {
    this.basePath = basePath;
  }

  public async Task<bool> Send(string phone, string message)
  {
    Directory.CreateDirectory(basePath);
    await File.WriteAllTextAsync(Path.Join(basePath, $"{phone}.txt"), message);
    return true;
  }
}
