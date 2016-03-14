%namespace ZimmerBot.Core.Parser
%scannertype ChatScanner
%visibility internal
%tokentype Token
%using ZimmerBot.Core.Language;

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [a-zA-ZÊ¯Â∆ÿ≈]+
Number     [0-9]+(\.[0-9]+)?
EMail      [a-zA-Z0-9+-_]+(\.[a-zA-Z0-9+-_])*@([a-zA-Z0-9]([a-zA-Z0-9\-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9]([a-z0-9\-]*[a-z0-9])?
Delimiter  (\r\n?|\n|\.|\?|;|:)
NotWh      [^ \t\r\n]
Space      [ \t]
Other      .

%{
%}

%%

/* Scanner body */

{Word}		  { yylval.t = new ZToken(yytext, ZToken.TokenType.Word); return (int)Token.T_WORD; }

{Number}		{ yylval.t = new ZToken(yytext, ZToken.TokenType.Number); return (int)Token.T_NUMBER; }

{EMail}		  { yylval.t = new ZToken(yytext, ZToken.TokenType.EMail); return (int)Token.T_EMAIL; }

{Delimiter}	{ yylval.t = new ZToken(yytext); return (int)Token.T_DELIMITER; }

{Space}+		/* skip */

{Other}     /* skip */


%%