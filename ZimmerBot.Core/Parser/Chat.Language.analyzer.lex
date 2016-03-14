%namespace ZimmerBot.Core.Parser
%scannertype ChatScanner
%visibility internal
%tokentype Token
%using ZimmerBot.Core.Language;

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [a-zA-ZÊ¯Â∆ÿ≈]+
Number     [0-9]+(\.[0-9]+)?
Delimiter  (\r\n?|\n|\.|\?)
NotWh      [^ \t\r\n]
Space      [ \t]

%{
%}

%%

/* Scanner body */

{Word}		  { yylval.t = new ZToken(yytext); return (int)Token.T_WORD; }

{Number}		{ yylval.t = new ZToken(yytext); return (int)Token.T_NUMBER; }

{Delimiter}	{ yylval.t = new ZToken(yytext); return (int)Token.T_DELIMITER; }

{Space}+		/* skip */


%%