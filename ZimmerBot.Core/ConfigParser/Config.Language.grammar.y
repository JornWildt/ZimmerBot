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
  public KeyValuePair<string,string> template;
  public RuleModifier ruleModifier;
  public List<RuleModifier> ruleModifierList;
  public Func<Knowledge.KnowledgeBase,Knowledge.Rule> ruleGenerator;
  public List<Func<Knowledge.KnowledgeBase,Knowledge.Rule>> ruleGeneratorList;
  public List<string> stringList;
  public List<List<string>> patternList;
  public string s;
  public double n;
}

%start main

%token T_COLON

%token T_CONCEPT, T_WEIGHT, T_CALL, T_EVERY, T_ANSWER, T_RDF_IMPORT, T_RDF_PREFIX

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

main
  : statementSeq
  ;

statementSeq
  : statementSeq statement
  | /* empty */
  ;

statement
  : configuration
  | rule          { $1.ruleGenerator(KnowledgeBase); } /* Instantiate rule */
  ;

configuration
  : T_CONCEPT T_WORD T_EQU conceptPatternSeq 
      { RegisterConcept($2.s, $4.patternList); }
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
  : ruleSeq rule  { $1.ruleGeneratorList.Add($2.ruleGenerator); $$.ruleGeneratorList = $1.ruleGeneratorList; }
  | /* empty */   { $$.ruleGeneratorList = new List<Func<Knowledge.KnowledgeBase,Knowledge.Rule>>(); }
  ;

rule
  : input ruleModifierSeq outputSeq
    { 
      $$.ruleGenerator = RuleGenerator($1.regex, $2.ruleModifierList, $3.outputList);
    }
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
      { $$.regex =  new RepetitionWRegex($1.regex, 0, 1); }
  | T_LPAR inputPatternSeq T_RPAR
      { $$.regex = new GroupWRegex($2.regex); }
  | T_WORD
      { $$.regex = new WordWRegex($1.s); }
  | T_CWORD
      { $$.regex = new ConceptWRegex(KnowledgeBase, $1.s); }
  | T_STAR
      { $$.regex = new RepetitionWRegex(new WildcardWRegex()); }
  | T_PLUS
      { $$.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
  | T_EXCL inputPattern
      { $$.regex =  new NegationWRegex($2.regex); }
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
  : T_AMP expr { $$.ruleModifier = new ConditionRuleModifier($2.expr); }
  ;

weight
  : T_WEIGHT T_NUMBER { $$.ruleModifier = new WeightRuleModifier($2.n); }
  ;

schedule
  : T_EVERY T_NUMBER { $$.ruleModifier = new ScheduleRuleModifier((int)$2.n); }
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
  | answer        { $$.output = $1.output;}
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
  : T_CALL exprReference T_LPAR exprSeq T_RPAR { $$.expr = new FunctionCallExpr($2.s, $4.exprList); }
  ;

answer
  : T_ANSWER T_LBRACE ruleSeq T_RBRACE { $$.output = new AnswerOutputStatement(KnowledgeBase, $3.ruleGeneratorList); }
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

cwordSeq
  : cwordSeq cword { $$.stringList.Add($2.s); }
  | cword          { $$.stringList = new List<string>(new string[] { $1.s }); }
  ;

cword
  : T_WORD   { $$.s = $1.s; }
  | T_CWORD  { $$.s = $1.s; }
  ;

%%