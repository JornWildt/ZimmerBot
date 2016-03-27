%namespace ZimmerBot.Core.TemplateParser
%partial
%parsertype TemplateParser
%visibility internal
%tokentype Token

%union 
{ 
  public TemplateToken token;
  public SequenceTemplateToken tokenSequence;
  public string s;
}

%start main

%token T_TEXT
%token T_LTAG
%token T_RTAG

%%

main
  : tokenSeq  { Result = $1.tokenSequence; }
  ;

tokenSeq
  : tokenSeq token { $1.tokenSequence.Tokens.Add($2.token); $$.tokenSequence = $1.tokenSequence; }
  | /* empty */    { $$.tokenSequence = new SequenceTemplateToken(); }
  ;

token
  : T_TEXT                  { $$.token = new TextTemplateToken($1.s); }
  | T_LTAG tokenSeq T_RTAG  { $$.token = new RedirectTemplateToken($2.tokenSequence); }
  ;


%%