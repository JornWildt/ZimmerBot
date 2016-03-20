using System;
using System.Collections.Generic;
using System.Text;

namespace ZimmerBot.Core.ConfigParser
{
  internal partial class ConfigScanner
  {

		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
			Console.WriteLine(format, args);
			Console.WriteLine();
		}
  }
}
