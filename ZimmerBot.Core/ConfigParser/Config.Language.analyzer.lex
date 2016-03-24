%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex;
%scannertype ConfigScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [[:IsLetter:]]+
Comment    #
Space      [ \t]
eol        (\r\n?|\n)
Other      .


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

\"          { StringInput = new StringBuilder(); BEGIN(str); }

$[0-9]+     { yylval.s = yytext; return (int)Token.T_WORD; } /* FIXME - not a word */

aggregate   { return (int)Token.T_AGGREGATE; }
call        { return (int)Token.T_CALL; }

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

/*
<output>[^\r\n]*    { yylval.s = yytext; }
<output>\r\n?|\n    { BEGIN(INITIAL); return (int)Token.T_OUTPUT; }
*/

%%