using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class Session
  {
    public NullValueDictionary<string,dynamic> Store { get; protected set; }


    public Session()
    {
      Store = new NullValueDictionary<string, object>();

      Store[Constants.ResponseCountKey] = 0;
    }
  }
}
