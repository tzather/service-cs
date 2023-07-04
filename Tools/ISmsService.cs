namespace Zuhid.Tools;

public interface ISmsService
{
  Task<bool> Send(string phone, string message);
}
