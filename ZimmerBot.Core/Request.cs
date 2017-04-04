using CuttingEdge.Conditions;

namespace ZimmerBot.Core
{
  public class Request
  {
    public enum EventEnum { Welcome }

    public string Input { get; set; }

    public object State { get; set; }

    public string SessionId { get; set; }

    public string UserId { get; set; }

    public string RuleId { get; set; }

    public string RuleLabel { get; set; }

    public EventEnum? EventType { get; set; }

    public string BotId { get; set; }

    public Request()
      : this("default", "default")
    {
    }


    public Request(string sessionId, string userId)
    {
      Condition.Requires(sessionId, nameof(sessionId)).IsNotNullOrEmpty();
      Condition.Requires(userId, nameof(userId)).IsNotNullOrEmpty();

      SessionId = sessionId;
      UserId = userId;
    }


    public Request(Request src, string input)
    {
      Input = input;
      State = src.State;
      SessionId = src.SessionId;
      UserId = src.UserId;
      State = src.State;
      RuleId = src.RuleId;
      RuleLabel = src.RuleLabel;
    }
  }
}
