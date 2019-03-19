using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using log4net;

namespace ZimmerBot.Core.Utilities
{
  // Avoid error output to console
  // - Especially relevant when allowing using of undefined attributes in templates.
  public class StringTemplateErrorHandler : ITemplateErrorListener
  {
    private static ILog Logger = LogManager.GetLogger(typeof(StringTemplateErrorHandler));


    public void CompiletimeError(TemplateMessage msg)
    {
      Logger.Warn(msg);
    }

    public void InternalError(TemplateMessage msg)
    {
      Logger.Warn(msg);
    }

    public void IOError(TemplateMessage msg)
    {
      Logger.Warn(msg);
    }

    public void RuntimeError(TemplateMessage msg)
    {
      Logger.Warn(msg);
    }
  }
}
