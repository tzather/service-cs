namespace Tzather.Tools;

public interface ISmsService
{
  Task<bool> Send(string phone, string message);
}
