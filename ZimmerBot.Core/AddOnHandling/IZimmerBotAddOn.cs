namespace ZimmerBot.Core.AddOnHandling
{
  public interface IZimmerBotAddOn
  {
    void Initialize();

    void Shutdown();
  }
}
