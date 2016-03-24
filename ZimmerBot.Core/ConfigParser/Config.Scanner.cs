using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigScanner
  {
    internal ErrorCollection Errors { get; set; }


		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
      if (Errors == null)
        Errors = new ErrorCollection();
      Errors.Add(string.Format(format, args), yyline, yycol);
		}
  }
}
