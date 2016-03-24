using System;
using System.Linq;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.Utilities
{
  public class ParserException : Exception
  {
    public string Filename { get; protected set; }

    public ErrorCollection Errors { get; protected set; }


    public ParserException(string filename, ErrorCollection errors)
    {
      Condition.Requires(filename, nameof(filename)).IsNotNullOrEmpty();
      Condition.Requires(errors, nameof(errors)).IsNotNull();
      Filename = filename;
      Errors = errors;
    }


    public override string Message
    {
      get { return ToString(); }
    }


    public override string ToString()
    {
      if (Errors == null)
        return "<empty>";

      return Filename + "\n"
        + Errors.Select(e => $"  {e.Message}({e.LineNo},{e.Position})").Aggregate((a, b) => a + "\n" + b);
    }
  }
}
