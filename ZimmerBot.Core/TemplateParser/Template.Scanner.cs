using System;
using System.Collections.Generic;
using System.Text;

namespace ZimmerBot.Core.TemplateParser
{
  internal partial class TemplateScanner
  {
		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
			Console.WriteLine(format, args);
			Console.WriteLine();
		}
  }
}
