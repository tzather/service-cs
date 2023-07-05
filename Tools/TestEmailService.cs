namespace Tzather.Tools;

public class TestEmailService : IEmailService
{
  private readonly string basePath;

  public TestEmailService(string basePath)
  {
    this.basePath = basePath;
  }


  public async Task<bool> Send(string to, string subject, string message)
  {
    Directory.CreateDirectory(basePath);
    await File.WriteAllTextAsync(Path.Join(basePath, $"{to}.txt"), $"{subject}{Environment.NewLine}{message}");
    return true;
  }
}
