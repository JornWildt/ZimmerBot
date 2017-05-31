%namespace ZimmerBot.Core.ConfigParser
%using System.Linq
%using ZimmerBot.Core.WordRegex
%using ZimmerBot.Core.Expressions
%using ZimmerBot.Core.Statements
%using ZimmerBot.Core.Patterns
%using ZimmerBot.Core.Utilities

%partial
%parsertype ConfigParser
%visibility internal
%tokentype Token

%union { 
  public Statement statement;
  public List<Statement> statementList;
  public WRegexBase regex;
  public List<WRegexBase> regexList;
  public Expression expr;
  public List<Expression> exprList;
  public OutputTemplate template;
  public ZimmerBot.Core.Knowledge.RuleModifier ruleModifier;
  public List<ZimmerBot.Core.Knowledge.RuleModifier> ruleModifierList;
  public Knowledge.Rule rule;
  public List<Knowledge.Rule> ruleList;
  public List<string> stringList;
  public List<List<string>> stringListList;
  public List<OperatorKeyValueList> opKeyValueListList;
  public StringPairList keyValueList;
  public KeyValuePair<string,string> keyValue;
  public OperatorKeyValue opKeyValue;
  public OperatorKeyValueList opKeyValueList;
  public List<Pattern> patternList;
  public Pattern pattern;
  public List<PatternExpr> patternExprList;
  public PatternExpr patternExpr;
  public List<ZimmerBot.Core.Knowledge.WordDefinition> wordDefinitionList;
  public ZimmerBot.Core.Knowledge.WordDefinition wordDefinition;
  public List<RdfDefinition> rdfDefinitionList;
  public RdfDefinition rdfDefinition;
  public List<RdfValue> rdfValueList;
  public RdfValue rdfValue;
  public string s;
  public double n;
}

%start main

%token T_COLON, T_SEMICOLON

%token T_CONCEPT, T_CALL, T_SET, T_WEIGHT, T_EVERY, T_ANSWER, T_TOPIC, T_STARTTOPIC, T_REPEATABLE, T_NOTREPEATABLE
%token T_ENTITIES, T_PATTERN, T_DEFINE
%token T_RDF_IMPORT, T_RDF_PREFIX, T_RDF_ENTITIES
%token T_WHEN, T_CONTINUE, T_CONTINUE_AT, T_CONTINUE_WITH, T_ON, T_AT, T_STOPOUTPUT

%token T_TOPICRULE
%token T_GTGT
%token T_IMPLIES
%token T_COMMA
%token T_LPAR
%token T_RPAR
%token T_LBRACE
%token T_RBRACE
%token T_LBRACKET
%token T_RBRACKET
%token T_AMP
%token T_OUTPUT
%token T_WORD
%token T_CWORD
%token T_STRING
%token T_NUMBER

%left T_QUESTION
%left T_EQU, T_LT, T_GT
%left T_PLUS, T_MINUS
%left T_STAR
%left T_PIPE
%left T_EXCL, T_TILDE
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
      { RegisterConcept($2.s, $4.stringListList); }
/*  | T_TOPIC T_WORD T_LPAR wordCommaSeq T_RPAR
      { StartTopic($2.s); } 
    T_LBRACE ruleSeq T_RBRACE 
      { FinalizeTopic($2.s); } */
  | T_TOPIC T_WORD
      { BeginTopic($2.s); } 
    T_LBRACE ruleSeq T_RBRACE 
      { FinalizeTopic($2.s); }
  | T_TOPIC T_WORD
    T_LBRACKET { BeginTopicStarters($2.s); } ruleSeq { EndTopicStarters(); } T_RBRACKET
      { BeginTopic($2.s); } 
    T_LBRACE ruleSeq T_RBRACE 
      { FinalizeTopic($2.s); }
  | T_ON T_LPAR T_WORD T_RPAR T_LBRACE statementSeq T_RBRACE
      { RegisterEventHandler($3.s, $6.statementList); }
  | T_ENTITIES T_LPAR T_WORD T_RPAR 
      { DoStripRegexLiterals = true; }
    T_LBRACE entityDefinition T_RBRACE
      { 
        DoStripRegexLiterals = false;
        RegisterEntities($3.s, $7.regexList); 
      }
  | T_DEFINE T_LPAR wordStringCommaSeq T_RPAR T_LBRACE definitionSeq T_RBRACE
      { RegisterDefinitions($3.stringList, $6.wordDefinitionList); }
  | T_PATTERN T_LPAR keyValueSeq T_RPAR
    T_LBRACE patternSeq T_RBRACE
      { RegisterPatternSet($3.keyValueList, $6.patternList); }
  | T_RDF_IMPORT T_STRING            
      { RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
  | T_RDF_PREFIX T_WORD T_STRING     
      { RDFPrefix($2.s, ((ConfigScanner)Scanner).StringInput.ToString()); }
  | T_RDF_ENTITIES T_LPAR T_STRING T_RPAR
      { RDFEntities($4.s); }
  ;

conceptPatternSeq
  : conceptPatternSeq T_COMMA cwordSeq  { $1.stringListList.Add($3.stringList); $$.stringListList = $1.stringListList; }
  | cwordSeq                            { $$.stringListList = new List<List<string>>(); $$.stringListList.Add($1.stringList); }
  ;

ruleSeq
  : ruleSeq rule  { $1.ruleList.Add($2.rule); $$.ruleList = $1.ruleList; }
  | /* empty */   { $$.ruleList = new List<Knowledge.Rule>(); }
  ;

rule
  : ruleLabel inputSeq ruleModifierSeq statementSeq
    { 
      $$.rule = AddRegexRule($1.s, $2.regexList, $3.ruleModifierList, $4.statementList);
    }
  | ruleLabel fuzzyTriggerSeq ruleModifierSeq statementSeq
    { 
      $$.rule = AddFuzzyRule($1.s, $2.opKeyValueListList, $3.ruleModifierList, $4.statementList);
    }
  | ruleLabel T_TOPICRULE topicOutput topicStatementSeq
    {
      $$.rule = AddTopicRule($1.s, $3.template, $4.statementList);
    }
  ;

fuzzyTriggerSeq
  : fuzzyTriggerSeq fuzzyTrigger { $$.opKeyValueListList = $1.opKeyValueListList; $$.opKeyValueListList.Add($2.opKeyValueList); }
  | fuzzyTrigger                 { $$.opKeyValueListList = new List<OperatorKeyValueList>(); $$.opKeyValueListList.Add($1.opKeyValueList); }
  ;

fuzzyTrigger
  : T_GTGT T_LBRACE opKeyValueSeq T_RBRACE { $$.opKeyValueList = $3.opKeyValueList; }
  | T_GTGT wordOrString
      { 
        $$.opKeyValueList = new OperatorKeyValueList(); 
        $$.opKeyValueList.Add(new OperatorKeyValue(AppSettings.IntentKey, "=", $2.s)); 
      }
  | T_GTGT wordOrString T_LPAR simpleOpKeyValueSeq T_RPAR
      { 
        $$.opKeyValueList = $4.opKeyValueList;
        $$.opKeyValueList.Insert(0, new OperatorKeyValue(AppSettings.IntentKey, "=", $2.s)); 
      }
  ;

ruleLabel
  : T_LT T_WORD { $$.s = $2.s; }
  | /* empty */
  ;

/******************************************************************************
  INPUT
******************************************************************************/

inputSeq
  : inputSeq input { $$.regexList = $1.regexList; $1.regexList.Add($2.regex); }
  | input          { $$.regexList = new List<WRegexBase>() { $1.regex }; }
  ;

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
      { $$.regex = BuildLiteralWRegex($1.s); }
  | T_CWORD
      { $$.regex = BuildConceptWRegex($1.s); }
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
  | /* empty */                   { $$.ruleModifierList = new List<ZimmerBot.Core.Knowledge.RuleModifier>(); }
  ;

ruleModifier
  : condition { $$ = $1; }
  | weight    { $$ = $1; }
  | schedule  { $$ = $1; }
  ;

condition
  : T_WHEN expr { $$.ruleModifier = new ZimmerBot.Core.Knowledge.ConditionRuleModifier($2.expr); }
  ;

weight
  : T_WEIGHT T_NUMBER { $$.ruleModifier = new ZimmerBot.Core.Knowledge.WeightRuleModifier($2.n); }
  ;

schedule
  : T_EVERY T_NUMBER { $$.ruleModifier = new ZimmerBot.Core.Knowledge.ScheduleRuleModifier((int)$2.n); }
  ;

/******************************************************************************
  STATEMENT
******************************************************************************/

/* Statement sequence cannot be empty. Otherwise we cannot have multi triggers like this:
   
   > Aaa
   > Bbb
   : Output

   The above could be either a trigger "Aaa" with empty statement list OR two triggers "Aaa" and "Bbb" for the same list.
*/

statementSeq
  : statementSeq statement  { $1.statementList.Add($2.statement); $$.statementList = $1.statementList; }
  | statement               { $$.statementList = new List<Statement>() { $1.statement }; }
  ;

topicStatementSeq
  : topicStatementSeq internalStatement  { $1.statementList.Add($2.statement); $$.statementList = $1.statementList; }
  | /* empty */                          { $$.statementList = new List<Statement>(); }
  ;

statement
  : outputTemplateSequence  { $$.statement = new OutputTemplateStatement($1.template); }
  | internalStatement       { $$.statement = $1.statement; }
  ;

internalStatement
  : stmtCall                { $$.statement = new CallStatment($1.expr as FunctionCallExpr); }
  | stmtSet                 { $$.statement = $1.statement; }
  | stmtAnswer              { $$.statement = $1.statement; }
  | stmtContinue            { $$.statement = $1.statement; }
  | stmtStopOutput          { $$.statement = $1.statement; }
  | T_STARTTOPIC T_WORD     { $$.statement = new StartTopicStatement($2.s); }
  | T_REPEATABLE            { $$.statement = new RepeatableStatement(true); }
  | T_NOTREPEATABLE         { $$.statement = new RepeatableStatement(false); }
  ;

outputTemplateSequence
  : outputTemplate outputTemplateSequence2                          {  $$.template = new OutputTemplate("default", $1.s, $2.stringList); }
  | T_LBRACE T_WORD T_RBRACE outputTemplate outputTemplateSequence2 {  $$.template = new OutputTemplate($2.s, $4.s, $5.stringList); }
  ;

outputTemplateSequence2
  : outputTemplateSequence2 T_PLUS outputTemplate  { $$.stringList.Add($3.s); }
  | /* empty */                                    { $$.stringList = new List<string>(); }
  ;

outputTemplate
  : T_COLON outputTemplateContent { $$.s = $2.s; }
  ;

outputTemplateContent
  :   { ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); } 
    T_OUTPUT 
      { $$.s = ((ConfigScanner)Scanner).StringInput.ToString().Trim(); }
  ;

topicOutput
  : outputTemplateContent outputTemplateSequence2 {  $$.template = new OutputTemplate("default", $1.s, $2.stringList); }
  ;

/******************************************************************************
  STATEMENTS
******************************************************************************/

stmtCall
  : T_CALL exprReference T_LPAR exprSeq T_RPAR { $$.expr = new FunctionCallExpr($2.expr, $4.exprList); }
  ;

stmtSet
  : T_SET exprReference T_EQU expr { $$.statement = new SetStatement($2.expr, $4.expr); }
  ;

stmtAnswer
  : T_ANSWER T_LBRACE ruleSeq T_RBRACE { $$.statement = new AnswerStatement($3.ruleList); }
  | T_ANSWER T_AT T_WORD               { $$.statement = new AnswerStatement($3.s); }
  ;

stmtContinue
  : T_CONTINUE               { $$.statement = new ContinueStatement(); }
  | T_CONTINUE_AT T_WORD     { $$.statement = new ContinueStatement(new ZimmerBot.Core.Knowledge.Continuation(ZimmerBot.Core.Knowledge.Continuation.ContinuationEnum.Label, $2.s)); }
  | T_CONTINUE_WITH wordSeq  { $$.statement = new ContinueStatement($2.stringList); }
  ;

stmtStopOutput
  : T_STOPOUTPUT { $$.statement = new StopOutputStatement(); }
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
  WORD DEFINITIONS
******************************************************************************/

definitionSeq
  : definition T_DOT definitionSeq  { $$.wordDefinitionList = $3.wordDefinitionList; $$.wordDefinitionList.Add($1.wordDefinition); }
  | /* empty */                     { $$.wordDefinitionList = new List<ZimmerBot.Core.Knowledge.WordDefinition>(); }
  ;

definition
  : definitionWord definitionAlternatives T_COLON definitionDataSeq
      { 
        $$.wordDefinition = new ZimmerBot.Core.Knowledge.WordDefinition($1.s, $2.stringList, $4.rdfDefinitionList);
      }
  ;

definitionWord
  : T_WORD    { $$.s = $1.s; }
  | T_STRING  { $$.s = $1.s; }
  ;

definitionAlternatives
  : T_LPAR emptyWordStringCommaSeq T_RPAR { $$.stringList = $2.stringList; }
  | /* empty */                           { $$.stringList = new List<string>(); }
  ;

definitionDataSeq
  : definitionData T_SEMICOLON definitionDataSeq  { $$.rdfDefinitionList = $3.rdfDefinitionList; $$.rdfDefinitionList.Add($1.rdfDefinition); }
  | definitionData                                { $$.rdfDefinitionList = new List<RdfDefinition>(); $$.rdfDefinitionList.Add($1.rdfDefinition); }
  | /* empty */                                   { $$.rdfDefinitionList = new List<RdfDefinition>(); }
  ;

definitionData
  : wordOrString T_COLON definitionDataValueSeq { $$.rdfDefinition = new RdfDefinition($1.s, $3.rdfValueList); }
  ;

definitionDataValueSeq
  : definitionDataValue T_COMMA definitionDataValueSeq  { $$.rdfValueList = $3.rdfValueList; $$.rdfValueList.Add($1.rdfValue); }
  | definitionDataValue                                 { $$.rdfValueList = new List<RdfValue>(); $$.rdfValueList.Add($1.rdfValue); }
  ;

definitionDataValue
  : T_STRING          { $$.rdfValue = new RdfStringValue($1.s); }
  | T_WORD            { $$.rdfValue = new RdfStringValue($1.s); }
  | T_NUMBER          { $$.rdfValue = new RdfNumberValue($1.n); }
  | T_LT wordSeq T_GT { $$.rdfValue = new RdfInternalUriValue($2.stringList); }
  ;

/******************************************************************************
  OTHER
******************************************************************************/

entityDefinition
  : stringSeq    { $$.regexList = new List<WRegexBase>(); $$.regexList.AddRange($1.stringList.Select(s => WRegex.BuildFromSpaceSeparatedString(s, true))); }
  | inputSeq     { $$.regexList = $1.regexList; }
  ;

wordSeq
  : wordSeq T_WORD  { $$.stringList = $1.stringList; $$.stringList.Add($2.s); }
  | T_WORD          { $$.stringList = new List<string>(new string[] { $1.s }); }
  ;

wordCommaSeq
  : wordCommaSeq T_COMMA T_WORD  { $$.stringList = $1.stringList; $$.stringList.Add($3.s); }
  | T_WORD                       { $$.stringList = new List<string>(); $$.stringList.Add($1.s); }
  ;

emptyWordStringCommaSeq
  : wordStringCommaSeq    { $$.stringList = $1.stringList; }
  | /* empty */           { $$.stringList = new List<string>(); }
  ;

wordStringCommaSeq
  : wordStringCommaSeq T_COMMA wordOrString  { $$.stringList = $1.stringList; $$.stringList.Add($3.s); }
  | wordOrString                             { $$.stringList = new List<string>(); $$.stringList.Add($1.s); }
  ;

wordOrString
  : T_WORD   { $$.s = $1.s; }
  | T_STRING { $$.s = $1.s; }
  ;

cwordSeq
  : cwordSeq cword { $$.stringList = $1.stringList; $$.stringList.Add($2.s); }
  | cword          { $$.stringList = new List<string>(new string[] { $1.s }); }
  ;

cword
  : T_WORD   { $$.s = $1.s; }
  | T_CWORD  { $$.s = $1.s; }
  | T_STRING { $$.s = $1.s; }
  ;

stringSeq
  : stringSeq T_COMMA T_STRING { $$.stringList = $1.stringList; $$.stringList.Add($3.s); }
  | T_STRING                   { $$.stringList = new List<string>(); $$.stringList.Add($1.s); }
  ;

simpleOpKeyValueSeq
  : simpleOpKeyValueSeq T_COMMA simpleOpKeyValue { $$.opKeyValueList = $1.opKeyValueList; $$.opKeyValueList.Add($3.opKeyValue); }
  | simpleOpKeyValue                             { $$.opKeyValueList = new OperatorKeyValueList(); $$.opKeyValueList.Add($1.opKeyValue); }
  ;

simpleOpKeyValue
  : T_WORD               { $$.opKeyValue = new OperatorKeyValue($1.s, "=", Constants.StarValue); }
  | T_WORD T_EQU value   { $$.opKeyValue = new OperatorKeyValue($1.s, "=", $3.s); }
  | T_WORD T_COLON value { $$.opKeyValue = new OperatorKeyValue($1.s, ":", $3.s); }
  ;

keyValueSeq
  : keyValueSeq T_COMMA keyValue { $$.keyValueList = $1.keyValueList; $$.keyValueList.Add($3.keyValue); }
  | keyValue                     { $$.keyValueList = new StringPairList(); $$.keyValueList.Add($1.keyValue); }
  ;

keyValue
  : T_WORD T_EQU value { $$.keyValue = new KeyValuePair<string,string>($1.s, $3.s); }
  ;

opKeyValueSeq
  : opKeyValueSeq T_COMMA opKeyValue { $$.opKeyValueList = $1.opKeyValueList; $$.opKeyValueList.Add($3.opKeyValue); }
  | opKeyValue                       { $$.opKeyValueList = new OperatorKeyValueList(); $$.opKeyValueList.Add($1.opKeyValue); }
  ;

opKeyValue
  : T_WORD T_EQU value   { $$.opKeyValue = new OperatorKeyValue($1.s, "=", $3.s); }
  | T_WORD T_COLON value { $$.opKeyValue = new OperatorKeyValue($1.s, ":", $3.s); }
  ;

value
  : T_WORD   { $$.s = $1.s; }
  | T_STRING { $$.s = $1.s; }
  | T_STAR   { $$.s = Constants.StarValue; }
  ;

patternSeq
  : patternSeq pattern  { $$.patternList = $1.patternList; $$.patternList.Add($2.pattern); }
  | pattern             { $$.patternList = new List<Pattern>(); $$.patternList.Add($1.pattern); }
  ;

pattern
  : T_GT patternExprSeq { $$.pattern = new Pattern($2.patternExprList); }
  ;

patternExprSeq
  : patternExprSeq patternExpr { $$.patternExprList = $1.patternExprList; $$.patternExprList.Add($2.patternExpr); }
  | patternExpr                { $$.patternExprList = new List<PatternExpr>(); $$.patternExprList.Add($1.patternExpr); }
  ;

patternExpr
  : entityPatternExpr   { $$.patternExpr = $1.patternExpr; }
  | T_WORD              { $$.patternExpr = new WordPatternExpr($1.s); }
  | T_CWORD             { $$.patternExpr = new ConceptPatternExpr($1.s); }
  | T_STRING            { $$.patternExpr = new WordPatternExpr($1.s); }
  | T_TILDE patternExpr { $$.patternExpr = new NegationPatternExpr($2.patternExpr); }
  ;

entityPatternExpr
  : T_LBRACE T_WORD T_COLON T_WORD T_RBRACE { $$.patternExpr = new EntityPatternExpr($2.s, $4.s); }
  | T_LBRACE T_WORD T_RBRACE { $$.patternExpr = new EntityPatternExpr($2.s, null); }
  ;

%%