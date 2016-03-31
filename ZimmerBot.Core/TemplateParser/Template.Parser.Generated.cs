// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 31-03-2016 08:23:12
// UserName: Jorn
// Input file <TemplateParser\Template.Language.grammar.y - 31-03-2016 08:18:30>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace ZimmerBot.Core.TemplateParser
{
internal enum Token {error=2,EOF=3,T_TEXT=4,T_LTAG=5,T_RTAG=6};

internal partial struct ValueType
{ 
  public TemplateToken token;
  public SequenceTemplateToken tokenSequence;
  public string s;
}
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
internal abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
internal class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
internal partial class TemplateParser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[7];
  private static State[] states = new State[9];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "tokenSeq", "token", };

  static TemplateParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{4,5,5,6,3,-2},new int[]{-4,4});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(-4,new int[]{-3,7});
    states[7] = new State(new int[]{6,8,4,5,5,6},new int[]{-4,4});
    states[8] = new State(-6);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{4});
    rules[6] = new Rule(-4, new int[]{5,-3,6});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Token.error, (int)Token.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // main -> tokenSeq
{ Result = ValueStack[ValueStack.Depth-1].tokenSequence; }
        break;
      case 3: // tokenSeq -> tokenSeq, token
{ CurrentSemanticValue.tokenSequence = Combine(ValueStack[ValueStack.Depth-2].tokenSequence, ValueStack[ValueStack.Depth-1].token); }
        break;
      case 4: // tokenSeq -> /* empty */
{ CurrentSemanticValue.tokenSequence = new SequenceTemplateToken(); }
        break;
      case 5: // token -> T_TEXT
{ CurrentSemanticValue.token = new TextTemplateToken(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 6: // token -> T_LTAG, tokenSeq, T_RTAG
{ CurrentSemanticValue.token = new RedirectTemplateToken(ValueStack[ValueStack.Depth-2].tokenSequence); }
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Token)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Token)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

}
}
