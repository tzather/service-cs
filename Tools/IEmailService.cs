namespace Zuhid.Tools;

public interface IEmailService
{
  Task<bool> Send(string to, string subject, string message);
}
