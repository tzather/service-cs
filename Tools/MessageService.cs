namespace Zuhid.Tools;

public class MessageService : IMessageService
{
  private readonly string basePath;

  public MessageService(string basePath) => this.basePath = basePath;

  public async Task<bool> SendEmail(string to, string subject, string message) => await WriteToFile(basePath, $"{to}.txt", $"{subject}{Environment.NewLine}{message}");

  public async Task<bool> SendSms(string phone, string message) => await WriteToFile(basePath, $"{phone}.txt", message);

  private async Task<bool> WriteToFile(string basePath, string fileName, string message)
  {
    Directory.CreateDirectory(basePath);
    await File.WriteAllTextAsync(Path.Join(basePath, fileName), message);
    return true;
  }

}
