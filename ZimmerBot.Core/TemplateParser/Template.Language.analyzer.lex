%namespace ZimmerBot.Core.TemplateParser
%scannertype TemplateScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

%{
%}

%x variant

%%

\<\<[ \t]*   { return (int)Token.T_LRTAG; }
[ \t]*\>\>   { return (int)Token.T_RRTAG; }
[^<>]+       { yylval.s = yytext; return (int)Token.T_TEXT; }
\<\(         { return (int)Token.T_LVTAG; }
\<           { yylval.s = yytext; return (int)Token.T_TEXT; }
\>           { yylval.s = yytext; return (int)Token.T_TEXT; }

<variant>\|     { return (int)Token.T_PIPE; }
<variant>[^\|\)]* { yylval.s = yytext; return (int)Token.T_TEXT; }
<variant>\)\>   { return (int)Token.T_RVTAG; }

%%