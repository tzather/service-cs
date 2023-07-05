namespace Zuhid.Tools;

public interface IMessageService
{
  Task<bool> SendEmail(string to, string subject, string message);
  Task<bool> SendSms(string phone, string message);
}
