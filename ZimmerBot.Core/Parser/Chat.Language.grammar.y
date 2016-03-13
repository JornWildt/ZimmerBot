%namespace ZimmerBot.Core.Parser
%using ZimmerBot.Core.Language
%partial
%parsertype ChatParser
%visibility internal
%tokentype Token

%union { 
			public ZTokenString ts;
      public ZToken t;
	   }

%start main

%token T_NUMBER
%token T_WORD


%%

main 
  : itemSeq { Result = $1.ts; $$ = $1; }
  ;

itemSeq 
  : itemSeq item  { $1.ts.Add($2.t); $$ = $1; }
  | /* empty */   { $$.ts = new ZTokenString(); }
  ;

item   : T_NUMBER { $$.t = $1.t; }
       | T_WORD   { $$.t = $1.t; }
       ;

%%