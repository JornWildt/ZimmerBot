%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex;
%scannertype ConfigScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

Word       [a-zA-Z_]+
Comment    #
Space      [ \t]
eol        (\r\n?|\n)
Other      .


/* No. 1-3 */
%x str
%x output
%x comment

%{
 StringBuilder StringInput = null;
%}


%%

/* Scanner body */

>           { return (int)Token.T_GT; }
:           { return (int)Token.T_COLON; }
\|          { return (int)Token.T_PIPE; }
\(          { return (int)Token.T_LPAR; }
\)          { return (int)Token.T_RPAR; }
\*          { return (int)Token.T_STAR; }
\+          { return (int)Token.T_PLUS; }


\"          { StringInput = new StringBuilder(); BEGIN(str); }

{Word}		  { yylval.s = yytext; return (int)Token.T_WORD; }

{Comment}   { BEGIN(comment); }

{Space}+		/* skip */


<str>[^\n\"]* { StringInput.Append(yytext); }
<str>\"       { 
                BEGIN(INITIAL); 
                /*yylval.t = new ZToken(StringInput.ToString(), ZToken.TokenType.Word); */
                return (int)Token.T_STRING; 
              }

<comment>[^\r\n]* /* skip */
<comment>\r|\n    { BEGIN(INITIAL); }

<output>[^\r\n]*    { yylval.s = yytext; Console.WriteLine("OOO: " + yytext); }
<output>\r\n?|\n    { Console.WriteLine("O-end"); BEGIN(INITIAL); return (int)Token.T_OUTPUT; }

%%