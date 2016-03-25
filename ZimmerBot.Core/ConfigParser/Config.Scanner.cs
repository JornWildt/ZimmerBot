using System.Globalization;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigScanner
  {
    internal ErrorCollection Errors { get; set; }

    internal int TryParseInt(string s)
    {
      int result;
      if (!int.TryParse(s, out result))
        yyerror("The value '{0}' is not a valid integer", s);
      return result;
    }


    internal double TryParseDouble(string s)
    {
      double result;
      if (!double.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
        yyerror("The value '{0}' is not a valid number", s);
      return result;
    }


    public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
      if (Errors == null)
        Errors = new ErrorCollection();
      Errors.Add(string.Format(format, args), yyline, yycol);
		}
  }
}
