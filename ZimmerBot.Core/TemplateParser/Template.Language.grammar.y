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
%token T_LPAR
%token T_RPAR
%token T_PIPE
%token T_LRTAG /* Left redirect */
%token T_RRTAG /* right redirect */
%token T_LVTAG /* Left variant */
%token T_RVTAG /* right variant */

%%

main
  : tokenSeq  { Result = $1.tokenSequence; }
  ;

tokenSeq
  : tokenSeq token { $$.tokenSequence = Combine($1.tokenSequence, $2.token); }
  | /* empty */    { $$.tokenSequence = new SequenceTemplateToken(); }
  ;

token
  : T_TEXT                    { $$.token = new TextTemplateToken($1.s); }
  | T_LRTAG tokenSeq T_RRTAG  { $$.token = new RedirectTemplateToken($2.tokenSequence); }
  | T_LVTAG  { ((TemplateScanner)Scanner).BEGIN(1); }
    variantSeq
    T_RVTAG   { $$.token = $3.token; ((TemplateScanner)Scanner).BEGIN(0); }
  ;

variantSeq
  : variantSeq T_PIPE variant { $$.token = ((ChooseTemplateToken)$1.token).Add($3.token); }
  | variant                   { $$.token = new ChooseTemplateToken($1.token); }
  ;

variant
  : T_TEXT                    { $$.token = new TextTemplateToken($1.s); }
  ;

%%