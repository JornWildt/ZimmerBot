%namespace ZimmerBot.Core.Parser
%scannertype ChatScanner
%visibility internal
%tokentype Token
%using ZimmerBot.Core.Language;

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 


Eol             (\r\n?|\n)
NotWh           [^ \t\r\n]
Space           [ \t]
Number          [0-9]+
Word            [a-zA-ZÊ¯Â∆ÿ≈]+

%{
%}

%%

/* Scanner body */

{Number}		{ yylval.t = new ZToken(yytext); return (int)Token.T_NUMBER; }

{Word}		  { yylval.t = new ZToken(yytext); return (int)Token.T_WORD; }

{Space}+		/* skip */


%%