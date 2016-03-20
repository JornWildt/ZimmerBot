%namespace ZimmerBot.Core.ConfigParser
%using ZimmerBot.Core.WordRegex
%partial
%parsertype ConfigParser
%visibility internal
%tokentype Token

%union { 
  public WRegex regex;
  public string s;
}

%start main

%token T_GT
%token T_COLON
%token T_PIPE
%token T_LPAR
%token T_RPAR
%token T_STAR
%token T_OUTPUT
%token T_WORD
%token T_STRING

%%

main
  : ruleSeq
  ;

ruleSeq
  : ruleSeq rule
  | /* empty */
  ;

rule
  : input output 
    { 
      Knowledge.Rule r = Domain.AddRule($1.regex);
      if ($2.s != null)
        r.WithResponse($2.s);
      Console.WriteLine("RULE: " + $2.s); 
    }
  ;

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
      { $$.regex = new ChoiceWRegex($1.regex, $3.regex); Console.WriteLine("|"); }
  | T_LPAR inputPattern T_RPAR
      { $$.regex = new GroupWRegex($2.regex); }
  | T_WORD     
      { $$.regex = new WordWRegex($1.s); Console.WriteLine("Word"); }
  | T_STAR
      { $$.regex = new RepetitionWRegex(new WildcardWRegex()); Console.WriteLine("*"); }
  ;

outputSeq
  : outputSeq output
  | /* empty */
  ;

output
  : outputPattern { Console.WriteLine("OUTPUT: " + $1.s); $$.s = $1.s; }
  ;

outputPatternSeq
  : outputPatternSeq outputPattern
  | /* empty */
  ;

outputPattern
  : T_COLON  
      { ((ConfigScanner)Scanner).BEGIN(2); Console.WriteLine(": ..."); } 
    T_OUTPUT { $$.s = $3.s; }
  ;


%%