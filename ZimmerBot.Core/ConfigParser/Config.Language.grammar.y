%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex
%using ZimmerBot.Core.Expressions
%partial
%parsertype ConfigParser
%visibility internal
%tokentype Token

%union { 
  public OutputStatement output;
  public List<OutputStatement> outputList;
  public WRegex regex;
  public Expression expr;
  public List<Expression> exprList;
  public List<string> stringList;
  public string s;
}

%start main

%token T_AGGREGATE
%token T_CALL
%token T_GT
%token T_COLON
%token T_EXCL
%token T_DOT
%token T_IMPLIES
%token T_COMMA
%token T_PIPE
%token T_LPAR
%token T_RPAR
%token T_STAR
%token T_PLUS
%token T_OUTPUT
%token T_WORD
%token T_STRING

%%

main
  : statementSeq
  ;

statementSeq
  : statementSeq statement
  | /* empty */
  ;

statement
  : configuration
  | rule
  ;

configuration
  : T_EXCL T_AGGREGATE wordSeq T_IMPLIES wordSeq { RegisterAggregates($3.stringList, $5.stringList); }
  ;

rule
  : input outputSeq
    { 
      Knowledge.Rule r = Domain.AddRule($1.regex);
      if ($2.stringList != null)
        r.WithResponses($2.stringList);
      if ($2.outputList != null)
        r.WithOutputStatements($2.outputList);
    }
  ;

/******************************************************************************
  INPUT
******************************************************************************/

inputSeq
  : inputSeq input
  | /* empty */
  ;

input
  : T_GT inputPattern { $$.regex = $2.regex; }
  ;

inputPatternSeq
  : inputPatternSeq inputPattern { ((SequenceWRegex)$1.regex).Add($2.regex); }
  | /* empty */                  { $$.regex = new SequenceWRegex(); }
  ;

inputPattern
  : inputPattern inputPattern 
      { $$.regex = CombineSequence($1.regex, $2.regex); }
  | inputPattern T_PIPE inputPattern
      { $$.regex = new ChoiceWRegex($1.regex, $3.regex); }
  | T_LPAR inputPattern T_RPAR
      { $$.regex = new GroupWRegex($2.regex); }
  | T_WORD     
      { $$.regex = new WordWRegex($1.s); }
  | T_STAR
      { $$.regex = new RepetitionWRegex(new WildcardWRegex()); }
  | T_PLUS
      { $$.regex =  new SequenceWRegex().Add(new WildcardWRegex()).Add(new RepetitionWRegex(new WildcardWRegex())); }
  ;


/******************************************************************************
  OUTPUT
******************************************************************************/

outputSeq
  : outputSeq output  { $1.outputList.Add($2.output); $$ = $1; }
  | /* empty */       { $$.outputList = new List<OutputStatement>(); }
  ;

output
  : outputPattern { $$.output = new TemplateOutputStatement($1.s); }
  | call          { $$.output = new CallOutputStatment($1.expr as FunctionCallExpr); }
  ;

outputPattern
  : T_COLON  
      { ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT { $$.s = $3.s; }
  ;

call
  : T_EXCL T_CALL exprReference T_LPAR exprSeq T_RPAR { $$.expr = new FunctionCallExpr($3.s, $5.exprList); }
  ;


/******************************************************************************
  EXPRESSION
******************************************************************************/

exprSeq
  : exprSeq T_COMMA expr { $1.exprList.Add($3.expr); $$ = $1; }
  | expr                 { $$.exprList = new List<Expression>(); $$.exprList.Add($1.expr); }
  ;

expr
  : exprIdentifier { $$ = $1; }
  | T_STRING       { $$.expr = new ConstantValueExpr($1.s); }
  ;

exprIdentifier
  : exprReference { $$.expr = new IdentifierExpression($1.s); }
  ;

exprReference
  : exprReference T_DOT T_WORD { $$.s = $1.s + "." + $3.s; }
  | T_WORD                     { $$.s = $1.s; }
  ;

/******************************************************************************
  OTHER
******************************************************************************/

wordSeq
  : wordSeq T_COMMA T_WORD { $$.stringList.Add($3.s); }
  | T_WORD                 { $$.stringList = new List<string>(); $$.stringList.Add($1.s); }
  ;

%%