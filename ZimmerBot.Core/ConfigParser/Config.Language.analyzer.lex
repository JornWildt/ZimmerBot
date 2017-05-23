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
\>\>        { return (int)Token.T_GTGT; }
\<          { return (int)Token.T_LT; }
:           { return (int)Token.T_COLON; }
;           { return (int)Token.T_SEMICOLON; }
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
-           { return (int)Token.T_MINUS; }
&           { return (int)Token.T_AMP; }
=           { return (int)Token.T_EQU; }
$           { return (int)Token.T_DOLLAR; }
\?          { return (int)Token.T_QUESTION; }
\!          { return (int)Token.T_EXCL; }
T\>         { return (int)Token.T_TOPICRULE; }

\"          { StringInput = new StringBuilder(); BEGIN(str); }

![ ]*concept     { return (int)Token.T_CONCEPT; }
![ ]*set         { return (int)Token.T_SET; }
![ ]*call        { return (int)Token.T_CALL; }
![ ]*weight      { return (int)Token.T_WEIGHT; }
![ ]*when        { return (int)Token.T_WHEN; }
![ ]*every       { return (int)Token.T_EVERY; }
![ ]*on          { return (int)Token.T_ON; }
![ ]*answer      { return (int)Token.T_ANSWER; }
![ ]*topic       { return (int)Token.T_TOPIC; }
![ ]*start_topic { return (int)Token.T_STARTTOPIC; }
![ ]*stop_output { return (int)Token.T_STOPOUTPUT; }
![ ]*repeatable        { return (int)Token.T_REPEATABLE; }
![ ]*not_repeatable    { return (int)Token.T_NOTREPEATABLE; }
![ ]*continue          { return (int)Token.T_CONTINUE; }
![ ]*continue[ ]+at    { return (int)Token.T_CONTINUE_AT; }
![ ]*continue[ ]+with  { return (int)Token.T_CONTINUE_WITH; }
![ ]*entities          { return (int)Token.T_ENTITIES; }
![ ]*define            { return (int)Token.T_DEFINE; }
![ ]*pattern           { return (int)Token.T_PATTERN; }
![ ]*rdf_import        { return (int)Token.T_RDF_IMPORT; }
![ ]*rdf_prefix        { return (int)Token.T_RDF_PREFIX; }
![ ]*rdf_entities      { return (int)Token.T_RDF_ENTITIES; }

at { return (int)Token.T_AT; }

{Number} { yylval.n = TryParseDouble(yytext); return (int)Token.T_NUMBER; }

\%{Word}  { yylval.s = yytext; return (int)Token.T_CWORD; }
{Word}    { yylval.s = yytext; return (int)Token.T_WORD; }

{Comment}   { BEGIN(comment); }

{Space}+		/* skip */


<str>[^\\"]*    { StringInput.Append(yytext); }
<str>\\\\       { StringInput.Append(yytext[1]); }
<str>\\\"       { StringInput.Append(yytext[1]); }
<str>\\n        { StringInput.Append("\n"); }
<str>\"         { BEGIN(INITIAL); yylval.s = StringInput.ToString(); return (int)Token.T_STRING; }

<comment>[^\r\n]* /* skip */
<comment>\r|\n    { BEGIN(INITIAL); }


<output>[^\r\n\\]*    { StringInput.Append(yytext); }
<output>\\(\r\n?|\n)  { StringInput.Append(yytext.Substring(1)); }
<output>\\[^\r\n]     { StringInput.Append(yytext.Substring(1)); }
<output>\r\n?|\n      { BEGIN(INITIAL); return (int)Token.T_OUTPUT; }

%%