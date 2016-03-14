%namespace ZimmerBot.Core.Parser
%using ZimmerBot.Core.Language
%partial
%parsertype ChatParser
%visibility internal
%tokentype Token

%union { 
  public ZStatementSequence stm;
  public ZTokenSequence ts;
  public ZToken t;
}

%start main

%token T_NUMBER
%token T_WORD
%token T_DELIMITER
%token T_EMAIL


%%

main 
  : statementSeq { Result = $1.stm; $$ = $1; }
  | /* empty */  { Result = new ZStatementSequence(); }
  ;

statementSeq
  : statementSeq statement { $1.stm.Statements.Add($2.ts); $$ = $1; }
  | /* empty */            { $$.stm = new ZStatementSequence(); }  
  ;

statement
  : itemSeq             { $$ = $1; }
  | itemSeq T_DELIMITER { $$ = $1; }
  ;

itemSeq 
  : itemSeq item  { $1.ts.Add($2.t); $$ = $1; }
  | /* empty */   { $$.ts = new ZTokenSequence(); }
  ;

item   : T_NUMBER    { $$.t = $1.t; }
       | T_WORD      { $$.t = $1.t; }
       ;

%%
