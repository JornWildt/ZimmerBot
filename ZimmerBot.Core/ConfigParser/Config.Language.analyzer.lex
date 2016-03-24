%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex;
%scannertype ConfigScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [[:IsLetter:]]+
Number     [0-9]+(\.[0-9]+)?
Comment    #
Space      [ \t]


/* No. 1-3 */
%x str
%x output
%x comment

%{
 internal StringBuilder StringInput = null;
%}


%%

/* Scanner body */

>           { return (int)Token.T_GT; }
:           { return (int)Token.T_COLON; }
!           { return (int)Token.T_EXCL; }
=>          { return (int)Token.T_IMPLIES; }
,           { return (int)Token.T_COMMA; }
\.          { return (int)Token.T_DOT; }
\|          { return (int)Token.T_PIPE; }
\(          { return (int)Token.T_LPAR; }
\)          { return (int)Token.T_RPAR; }
\{          { return (int)Token.T_LBRACE; }
\}          { return (int)Token.T_RBRACE; }
\*          { return (int)Token.T_STAR; }
\+          { return (int)Token.T_PLUS; }
&           { return (int)Token.T_AMP; }
=           { return (int)Token.T_EQU; }
$           { return (int)Token.T_DOLLAR; }

\"          { StringInput = new StringBuilder(); BEGIN(str); }

abstraction { return (int)Token.T_ABSTRACTION; }
call        { return (int)Token.T_CALL; }

{Number}    { yylval.s = yytext; return (int)Token.T_NUMBER; }

{Word}		  { yylval.s = yytext; return (int)Token.T_WORD; }

{Comment}   { BEGIN(comment); }

{Space}+		/* skip */


<str>[^\n\"]* { StringInput.Append(yytext); }
<str>\"       { BEGIN(INITIAL); return (int)Token.T_STRING; }

<comment>[^\r\n]* /* skip */
<comment>\r|\n    { BEGIN(INITIAL); }


<output>[^\r\n\\]*    { StringInput.Append(yytext); }
<output>\\(\r\n?|\n)  { StringInput.Append(yytext.Substring(1)); }
<output>\\[^\r\n]     { StringInput.Append(yytext.Substring(1)); }
<output>\r\n?|\n      { BEGIN(INITIAL); return (int)Token.T_OUTPUT; }

%%