using System;
using System.Collections.Generic;
using System.Text;

namespace ZimmerBot.Core.Parser
{
    internal partial class ChatScanner
    {

    //    void GetNumber()
    //    {
    //        yylval.s = yytext;
    //        yylval.n = int.Parse(yytext);
    //    }


    //void GetString()
    //{
    //  yylval.s = yytext;
    //}


    public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);
			Console.WriteLine(format, args);
			Console.WriteLine();
		}
    }
}
