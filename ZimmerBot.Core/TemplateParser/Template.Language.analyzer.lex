%namespace ZimmerBot.Core.TemplateParser
%scannertype TemplateScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [[:IsLetter:]_]+[[:IsLetter:]0-9_]*

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

<variant>\|          { return (int)Token.T_PIPE; }
<variant>\%{Word}    { yylval.s = yytext; return (int)Token.T_CWORD; }
<variant>[^\|\)\%]*  { yylval.s = yytext; return (int)Token.T_TEXT; }
<variant>\)\>        { return (int)Token.T_RVTAG; }

%%