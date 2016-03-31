%namespace ZimmerBot.Core.TemplateParser
%scannertype TemplateScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

%{
%}

%%

\{@[ \t]*  { Console.WriteLine("L: " + yytext); return (int)Token.T_LTAG; }
[ \t]*\}   { Console.WriteLine("R: " + yytext); return (int)Token.T_RTAG; }
[^{}@]+    { Console.WriteLine("T1: " + yytext); yylval.s = yytext; return (int)Token.T_TEXT; }
\{         { Console.WriteLine("T2: " + yytext); yylval.s = yytext; return (int)Token.T_TEXT; }



%%