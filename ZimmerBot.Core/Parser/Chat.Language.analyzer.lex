%namespace ZimmerBot.Core.Parser
%scannertype ChatScanner
%visibility internal
%tokentype Token
%using ZimmerBot.Core.Parser;

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [[:IsLetter:]]+
Number     [0-9]+(\.[0-9]+)?
EMail      [a-zA-Z0-9+-_]+(\.[a-zA-Z0-9+-_])*@([a-zA-Z0-9]([a-zA-Z0-9\-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9]([a-z0-9\-]*[a-z0-9])?
URL        (https?:\/\/|www\.)[a-zA-Z+&@#/%=~_?!:;,.]*[a-zA-Z+&@#/%=~_?!:;]+
Delimiter  (\r\n?|\n|\.|\?|;|:)
NotWh      [^ \t\r\n]
Space      [ \t]
Other      .

%x str

%{
 StringBuilder StringInput = null;
%}

%%

/* Scanner body */

\"          { StringInput = new StringBuilder(); BEGIN(str); }

{Word}		  { yylval.t = new ZToken(yytext, ZToken.TokenType.Word); return (int)Token.T_WORD; }

{Number}		{ yylval.t = new ZToken(yytext, ZToken.TokenType.Number); return (int)Token.T_NUMBER; }

{EMail}		  { yylval.t = new ZToken(yytext, ZToken.TokenType.EMail); return (int)Token.T_EMAIL; }

{URL}		    { yylval.t = new ZToken(yytext, ZToken.TokenType.EMail); return (int)Token.T_URL; }

{Delimiter}	{ yylval.t = new ZToken(yytext); return (int)Token.T_DELIMITER; }

{Space}+		/* skip */

{Other}     /* skip */

<str>[^\n\"]* { StringInput.Append(yytext); }
<str>\"       { 
                BEGIN(INITIAL); 
                yylval.t = new ZToken(StringInput.ToString(), ZToken.TokenType.Word); 
                return (int)Token.T_STRING; 
              }

%%