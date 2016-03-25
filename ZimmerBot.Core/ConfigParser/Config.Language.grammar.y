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
  public KeyValuePair<string,string> template;
  public List<Expression> exprList;
  public List<string> stringList;
  public string s;
  public double n;
}

%start main

%token T_EXCL, T_GT, T_COLON

%token T_ABSTRACTION
%token T_CALL

%token T_IMPLIES
%token T_COMMA
%token T_LPAR
%token T_RPAR
%token T_LBRACE
%token T_RBRACE
%token T_AMP
%token T_OUTPUT
%token T_WORD
%token T_STRING
%token T_NUMBER
%token T_EOL

%left T_EQU
%left T_PLUS, T_STAR
%left T_PIPE
%left T_DOT

%token T_DOLLAR

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
  | T_EOL
  ;

configuration
  : T_EXCL T_ABSTRACTION wordSeq T_IMPLIES wordSeq T_EOL { RegisterAbstractions($3.stringList, $5.stringList); }
  ;

rule
  : input ruleModifier outputSeq
    { 
      Knowledge.Rule r = Domain.AddRule($1.regex);
      if ($2.expr != null)
        r.WithCondition($2.expr);
      r.WithWeight($2.n);
      if ($3.outputList != null)
        r.WithOutputStatements($3.outputList);
    }
  ;

/******************************************************************************
  INPUT
******************************************************************************/

input
  : T_GT inputPattern T_EOL { $$.regex = $2.regex; }
  | T_GT T_EOL              { $$.regex = null; }
  | T_EOL                   { $$.regex = null; }
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
      { $$.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
  ;


/******************************************************************************
  MODIFIERS
******************************************************************************/

/* Cheating slightly - should return some sort of typed Condition and Weight, but so far the general expr and n suffices */

ruleModifier
  : condition weight { $$.expr = $1.expr; $$.n = $2.n; }
  | weight condition { $$.expr = $2.expr; $$.n = $1.n; }
  ;

condition
  : T_AMP expr T_EOL { $$.expr = $2.expr; }
  | /* empty */      { $$.expr = null; }
  ;

weight
  : T_STAR T_NUMBER T_EOL { $$.n = $2.n; }
  | /* empty */           { $$.n = 1.0; }
  ;

/******************************************************************************
  OUTPUT
******************************************************************************/

outputSeq
  : outputSeq output  { $1.outputList.Add($2.output); $$ = $1; }
  | /* empty */       { $$.outputList = new List<OutputStatement>(); }
  ;

output
  : outputPattern { $$.output = new TemplateOutputStatement($1.template); }
  | call          { $$.output = new CallOutputStatment($1.expr as FunctionCallExpr); }
  ;

outputPattern
  : T_COLON  
      { ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT 
      { $$.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
  | T_LBRACE T_WORD T_RBRACE T_COLON 
      { ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT 
      { $$.template = new KeyValuePair<string,string>($2.s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
  ;

call
  : T_EXCL T_CALL exprReference T_LPAR exprSeq T_RPAR T_EOL { $$.expr = new FunctionCallExpr($3.s, $5.exprList); }
  ;


/******************************************************************************
  EXPRESSION
******************************************************************************/

exprSeq
  : exprSeq2    { $$ = $1; }
  | /* empty */ { $$.exprList = new List<Expression>(); }
  ;

exprSeq2
  : exprSeq2 T_COMMA expr { $1.exprList.Add($3.expr); $$ = $1; }
  | expr                 { $$.exprList = new List<Expression>(); $$.exprList.Add($1.expr); }
  ;

expr
  : exprBinary { $$ = $1; }
  | exprUnary  { $$ = $1; }
  ;

exprBinary
  : expr T_EQU expr { $$.expr = new BinaryOperatorExpr($1.expr, $3.expr, BinaryOperatorExpr.OperatorType.Equals); }
  ;

exprUnary
  : T_LPAR expr T_RPAR { $$ = $2; }
  | exprIdentifier     { $$ = $1; }
  | T_STRING           { $$.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
  | T_NUMBER           { $$.expr = new ConstantValueExpr($1.n); }
  ;

exprIdentifier
  : exprReference     { $$.expr = new IdentifierExpr($1.s); }
  | T_DOLLAR T_NUMBER { $$.expr = new IdentifierExpr("$"+$2.n); }
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