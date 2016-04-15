%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex
%using ZimmerBot.Core.Expressions
%using ZimmerBot.Core.Statements
%partial
%parsertype ConfigParser
%visibility internal
%tokentype Token

%union { 
  public Statement statement;
  public List<Statement> statementList;
  public WRegex regex;
  public Expression expr;
  public List<Expression> exprList;
  public KeyValuePair<string,string> template;
  public RuleModifier ruleModifier;
  public List<RuleModifier> ruleModifierList;
  public Knowledge.Rule rule;
  public List<Knowledge.Rule> ruleList;
  public List<string> stringList;
  public List<List<string>> patternList;
  public string s;
  public double n;
}

%start main

%token T_COLON

%token T_CONCEPT, T_CALL, T_SET, T_WEIGHT, T_EVERY, T_ANSWER, T_RDF_IMPORT, T_RDF_PREFIX, T_WHEN
%token T_CONTINUE, T_CONTINUE_AT, T_CONTINUE_WITH, T_ON

%token T_IMPLIES
%token T_COMMA
%token T_LPAR
%token T_RPAR
%token T_LBRACE
%token T_RBRACE
%token T_AMP
%token T_OUTPUT
%token T_WORD
%token T_CWORD
%token T_STRING
%token T_NUMBER

%left T_QUESTION
%left T_EQU, T_LT, T_GT
%left T_PLUS, T_STAR
%left T_PIPE
%left T_EXCL
%left T_DOT

%token T_DOLLAR

%%

/* "item" aint the best word ... waiting for a better to popup ("statement" is already used) */

main
  : itemSeq
  ;

itemSeq
  : itemSeq item
  | /* empty */
  ;

item
  : configuration
  | rule
  ;

configuration
  : T_CONCEPT T_WORD T_EQU conceptPatternSeq 
      { RegisterConcept($2.s, $4.patternList); }
  | T_ON T_LPAR T_WORD T_RPAR T_LBRACE statementSeq T_RBRACE
      { RegisterEventHandler($3.s, $6.statementList); }
  | T_RDF_IMPORT T_STRING            
      { RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
  | T_RDF_PREFIX T_WORD T_STRING     
      { RDFPrefix($2.s, ((ConfigScanner)Scanner).StringInput.ToString()); }
  ;

conceptPatternSeq
  : conceptPatternSeq T_COMMA cwordSeq  { $1.patternList.Add($3.stringList); $$.patternList = $1.patternList; }
  | cwordSeq                            { $$.patternList = new List<List<string>>(); $$.patternList.Add($1.stringList); }
  ;

ruleSeq
  : ruleSeq rule  { $1.ruleList.Add($2.rule); $$.ruleList = $1.ruleList; }
  | /* empty */   { $$.ruleList = new List<Knowledge.Rule>(); }
  ;

rule
  : ruleId input ruleModifierSeq statementSeq
    { 
      $$.rule = AddRule($1.s, $2.regex, $3.ruleModifierList, $4.statementList);
    }
  ;

ruleId
  : T_LT T_WORD { $$.s = $2.s; }
  | /* empty */
  ;

/******************************************************************************
  INPUT
******************************************************************************/

input
  : T_GT inputPatternSeq { $$.regex = $2.regex; }
  ;

inputPatternSeq
  : inputPatternSeq inputPattern  { $$.regex = CombineSequence($1.regex, $2.regex); }
  | /* empty */                   { $$.regex = null; }
  ;

inputPattern
  : inputPattern T_PIPE inputPattern
      { $$.regex = new ChoiceWRegex($1.regex, $3.regex); }
  | inputPattern T_QUESTION
      { $$.regex =  new GroupWRegex(new RepetitionWRegex($1.regex, 0, 1)); }
  | T_LPAR inputPatternSeq T_RPAR
      { $$.regex = new GroupWRegex($2.regex); }
  | T_WORD
      { $$.regex = new WordWRegex($1.s); }
  | T_CWORD
      { $$.regex = new ConceptWRegex(KnowledgeBase, $1.s); }
  | T_STAR
      { $$.regex = new GroupWRegex(new RepetitionWRegex(new WildcardWRegex())); }
  | T_PLUS
      { $$.regex =  new GroupWRegex(new RepetitionWRegex(new WildcardWRegex(), 1, 9999)); }
  ;

/******************************************************************************
  MODIFIERS
******************************************************************************/

ruleModifierSeq
  : ruleModifierSeq ruleModifier  { $$.ruleModifierList.Add($2.ruleModifier); }
  | /* empty */                   { $$.ruleModifierList = new List<RuleModifier>(); }
  ;

ruleModifier
  : condition { $$ = $1; }
  | weight    { $$ = $1; }
  | schedule  { $$ = $1; }
  ;

condition
  : T_WHEN expr { $$.ruleModifier = new ConditionRuleModifier($2.expr); }
  ;

weight
  : T_WEIGHT T_NUMBER { $$.ruleModifier = new WeightRuleModifier($2.n); }
  ;

schedule
  : T_EVERY T_NUMBER { $$.ruleModifier = new ScheduleRuleModifier((int)$2.n); }
  ;

/******************************************************************************
  STATEMENT
******************************************************************************/

statementSeq
  : statementSeq statement  { $1.statementList.Add($2.statement); $$.statementList = $1.statementList; }
  | /* empty */             { $$.statementList = new List<Statement>(); }
  ;

statement
  : outputTemplate  { $$.statement = new OutputTemplateStatement($1.template); }
  | stmtCall        { $$.statement = new CallStatment($1.expr as FunctionCallExpr); }
  | stmtSet         { $$.statement = $1.statement;}
  | stmtAnswer      { $$.statement = $1.statement;}
  | stmtContinue    { $$.statement = $1.statement;}
  ;

outputTemplate
  : T_COLON  
      { ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT 
      { $$.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
  | T_LBRACE T_WORD T_RBRACE T_COLON 
      { ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT 
      { $$.template = new KeyValuePair<string,string>($2.s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
  ;

stmtCall
  : T_CALL exprReference T_LPAR exprSeq T_RPAR { $$.expr = new FunctionCallExpr($2.expr, $4.exprList); }
  ;

stmtSet
  : T_SET exprReference T_EQU expr { $$.statement = new SetStatement($2.expr, $4.expr); }
  ;

stmtAnswer
  : T_ANSWER T_LBRACE ruleSeq T_RBRACE { $$.statement = new AnswerStatement($3.ruleList); }
  ;

stmtContinue
  : T_CONTINUE               { $$.statement = new ContinueStatement(); }
  | T_CONTINUE_AT T_WORD     { $$.statement = new ContinueStatement($2.s, ContinueStatement.TargetEnum.Label); }
  | T_CONTINUE_WITH wordSeq  { $$.statement = new ContinueStatement($2.stringList, ContinueStatement.TargetEnum.Input); }
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
  : T_LPAR expr T_RPAR { $$.expr = $2.expr; }
  | T_EXCL expr        { $$.expr = new UnaryOperatorExpr($2.expr, UnaryOperatorExpr.OperatorType.Negation); }
  | exprIdentifier     { $$.expr = $1.expr; }
  | T_STRING           { $$.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
  | T_NUMBER           { $$.expr = new ConstantValueExpr($1.n); }
  ;

exprIdentifier
  : exprReference     { $$.expr = $1.expr; }
  | T_DOLLAR T_NUMBER { $$.expr = new IdentifierExpr("$"+$2.n); }
  | T_DOLLAR T_WORD   { $$.expr = new IdentifierExpr("$"+$2.s); }
  ;

exprReference
  : exprReference T_DOT T_WORD { $$.expr = new DotExpression($1.expr, $3.s); }
  | T_WORD                     { $$.expr = new DotExpression($1.s); }
  ;

/******************************************************************************
  OTHER
******************************************************************************/


wordSeq
  : wordSeq T_WORD  { $$.stringList = $1.stringList; $$.stringList.Add($2.s); }
  | T_WORD          { $$.stringList = new List<string>(new string[] { $1.s }); }
  ;

cwordSeq
  : cwordSeq cword { $$.stringList = $1.stringList; $$.stringList.Add($2.s); }
  | cword          { $$.stringList = new List<string>(new string[] { $1.s }); }
  ;

cword
  : T_WORD   { $$.s = $1.s; }
  | T_CWORD  { $$.s = $1.s; }
  ;

%%