%namespace ZimmerBot.Core.TemplateParser
%scannertype TemplateScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

%{
%}

%%

\{@[ \t]*  { return (int)Token.T_LTAG; }
[ \t]*\}   { return (int)Token.T_RTAG; }
[^{}@]+    { yylval.s = yytext; return (int)Token.T_TEXT; }


%%