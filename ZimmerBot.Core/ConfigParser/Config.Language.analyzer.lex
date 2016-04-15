%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex;
%scannertype ConfigScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [[:IsLetter:]_]+[[:IsLetter:]0-9_]*
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

\>          { return (int)Token.T_GT; }
\<          { return (int)Token.T_LT; }
:           { return (int)Token.T_COLON; }
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
\?          { return (int)Token.T_QUESTION; }
\!          { return (int)Token.T_EXCL; }

\"          { StringInput = new StringBuilder(); BEGIN(str); }

![ ]*concept     { return (int)Token.T_CONCEPT; }
![ ]*set         { return (int)Token.T_SET; }
![ ]*call        { return (int)Token.T_CALL; }
![ ]*weight      { return (int)Token.T_WEIGHT; }
![ ]*when        { return (int)Token.T_WHEN; }
![ ]*every       { return (int)Token.T_EVERY; }
![ ]*on          { return (int)Token.T_ON; }
![ ]*answer      { return (int)Token.T_ANSWER; }
![ ]*continue          { return (int)Token.T_CONTINUE; }
![ ]*continue[ ]+at    { return (int)Token.T_CONTINUE_AT; }
![ ]*continue[ ]+with  { return (int)Token.T_CONTINUE_WITH; }
![ ]*rdf_import  { return (int)Token.T_RDF_IMPORT; }
![ ]*rdf_prefix  { return (int)Token.T_RDF_PREFIX; }

at { return (int)Token.T_AT; }

{Number} { yylval.n = TryParseDouble(yytext); return (int)Token.T_NUMBER; }

\%{Word}  { yylval.s = yytext; return (int)Token.T_CWORD; }
{Word}    { yylval.s = yytext; return (int)Token.T_WORD; }

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