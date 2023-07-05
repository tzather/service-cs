namespace Tzather.Tools;

public interface IEmailService
{
  Task<bool> Send(string to, string subject, string message);
}
