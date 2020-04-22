// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  CBRAIN-PC412
// DateTime: 22-04-2020 22:12:33
// UserName: jw
// Input file <Parser\Chat.Language.grammar.y - 06-02-2019 08:37:04>

// options: conflicts no-lines gplex conflicts

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using ZimmerBot.Core.Parser;

namespace ZimmerBot.Core.Parser
{
internal enum Token {error=2,EOF=3,T_NUMBER=4,T_WORD=5,T_STRING=6,
    T_DELIMITER=7,T_EMAIL=8,T_URL=9};

internal partial struct ValueType
{ 
  public ZStatementSequence stm;
  public ZTokenSequence ts;
  public ZToken t;
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
internal partial class ChatParser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[15];
  private static State[] states = new State[13];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "statementSeq", "statement", "itemSeq", "item", };

  static ChatParser() {
    states[0] = new State(new int[]{7,-5,4,-5,5,-5,6,-5,8,-5,9,-5,3,-3},new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{3,-2,7,-9,4,-9,5,-9,6,-9,8,-9,9,-9},new int[]{-4,4,-5,5});
    states[4] = new State(-4);
    states[5] = new State(new int[]{7,7,4,8,5,9,6,10,8,11,9,12,3,-6},new int[]{-6,6});
    states[6] = new State(-8);
    states[7] = new State(-7);
    states[8] = new State(-10);
    states[9] = new State(-11);
    states[10] = new State(-12);
    states[11] = new State(-13);
    states[12] = new State(-14);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-1, new int[]{});
    rules[4] = new Rule(-3, new int[]{-3,-4});
    rules[5] = new Rule(-3, new int[]{});
    rules[6] = new Rule(-4, new int[]{-5});
    rules[7] = new Rule(-4, new int[]{-5,7});
    rules[8] = new Rule(-5, new int[]{-5,-6});
    rules[9] = new Rule(-5, new int[]{});
    rules[10] = new Rule(-6, new int[]{4});
    rules[11] = new Rule(-6, new int[]{5});
    rules[12] = new Rule(-6, new int[]{6});
    rules[13] = new Rule(-6, new int[]{8});
    rules[14] = new Rule(-6, new int[]{9});
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
      case 2: // main -> statementSeq
{ Result = ValueStack[ValueStack.Depth-1].stm; CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 3: // main -> /* empty */
{ Result = new ZStatementSequence(); }
        break;
      case 4: // statementSeq -> statementSeq, statement
{ ValueStack[ValueStack.Depth-2].stm.Statements.Add(ValueStack[ValueStack.Depth-1].ts); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 5: // statementSeq -> /* empty */
{ CurrentSemanticValue.stm = new ZStatementSequence(); }
        break;
      case 6: // statement -> itemSeq
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 7: // statement -> itemSeq, T_DELIMITER
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 8: // itemSeq -> itemSeq, item
{ ValueStack[ValueStack.Depth-2].ts.Add(ValueStack[ValueStack.Depth-1].t); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 9: // itemSeq -> /* empty */
{ CurrentSemanticValue.ts = new ZTokenSequence(); }
        break;
      case 10: // item -> T_NUMBER
{ CurrentSemanticValue.t = ValueStack[ValueStack.Depth-1].t; }
        break;
      case 11: // item -> T_WORD
{ CurrentSemanticValue.t = ValueStack[ValueStack.Depth-1].t; }
        break;
      case 12: // item -> T_STRING
{ CurrentSemanticValue.t = ValueStack[ValueStack.Depth-1].t; }
        break;
      case 13: // item -> T_EMAIL
{ CurrentSemanticValue.t = ValueStack[ValueStack.Depth-1].t; }
        break;
      case 14: // item -> T_URL
{ CurrentSemanticValue.t = ValueStack[ValueStack.Depth-1].t; }
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
