// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 20-03-2016 22:33:33
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 20-03-2016 22:33:32>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using ZimmerBot.Core.WordRegex;

namespace ZimmerBot.Core.ConfigParser
{
internal enum Token {error=2,EOF=3,T_GT=4,T_COLON=5,T_PIPE=6,
    T_LPAR=7,T_RPAR=8,T_STAR=9,T_OUTPUT=10,T_WORD=11,T_STRING=12};

internal partial struct ValueType
{ 
  public WRegex regex;
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
internal partial class ConfigParser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[23];
  private static State[] states = new State[21];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "ruleSeq", "rule", "input", "output", "inputSeq", "inputPattern", 
      "inputPatternSeq", "outputSeq", "outputPattern", "outputPatternSeq", "Anon@1", 
      };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{4,11,3,-2},new int[]{-4,4,-5,5});
    states[4] = new State(-3);
    states[5] = new State(new int[]{5,8},new int[]{-6,6,-11,7});
    states[6] = new State(-5);
    states[7] = new State(-18);
    states[8] = new State(-21,new int[]{-13,9});
    states[9] = new State(new int[]{10,10});
    states[10] = new State(-22);
    states[11] = new State(new int[]{7,16,11,19,9,20},new int[]{-8,12});
    states[12] = new State(new int[]{6,14,7,16,11,19,9,20,5,-8},new int[]{-8,13});
    states[13] = new State(new int[]{6,14,7,16,11,19,9,20,5,-11,8,-11},new int[]{-8,13});
    states[14] = new State(new int[]{7,16,11,19,9,20},new int[]{-8,15});
    states[15] = new State(new int[]{6,14,7,16,11,19,9,20,5,-12,8,-12},new int[]{-8,13});
    states[16] = new State(new int[]{7,16,11,19,9,20},new int[]{-8,17});
    states[17] = new State(new int[]{8,18,6,14,7,16,11,19,9,20},new int[]{-8,13});
    states[18] = new State(-13);
    states[19] = new State(-14);
    states[20] = new State(-15);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5,-6});
    rules[6] = new Rule(-7, new int[]{-7,-5});
    rules[7] = new Rule(-7, new int[]{});
    rules[8] = new Rule(-5, new int[]{4,-8});
    rules[9] = new Rule(-9, new int[]{-9,-8});
    rules[10] = new Rule(-9, new int[]{});
    rules[11] = new Rule(-8, new int[]{-8,-8});
    rules[12] = new Rule(-8, new int[]{-8,6,-8});
    rules[13] = new Rule(-8, new int[]{7,-8,8});
    rules[14] = new Rule(-8, new int[]{11});
    rules[15] = new Rule(-8, new int[]{9});
    rules[16] = new Rule(-10, new int[]{-10,-6});
    rules[17] = new Rule(-10, new int[]{});
    rules[18] = new Rule(-6, new int[]{-11});
    rules[19] = new Rule(-12, new int[]{-12,-11});
    rules[20] = new Rule(-12, new int[]{});
    rules[21] = new Rule(-13, new int[]{});
    rules[22] = new Rule(-11, new int[]{5,-13,10});
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
      case 5: // rule -> input, output
{ 
      Knowledge.Rule r = Domain.AddRule(ValueStack[ValueStack.Depth-2].regex);
      if (ValueStack[ValueStack.Depth-1].s != null)
        r.WithResponse(ValueStack[ValueStack.Depth-1].s);
      Console.WriteLine("RULE: " + ValueStack[ValueStack.Depth-1].s); 
    }
        break;
      case 8: // input -> T_GT, inputPattern
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 9: // inputPatternSeq -> inputPatternSeq, inputPattern
{ ((SequenceWRegex)ValueStack[ValueStack.Depth-2].regex).Add(ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 10: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = new SequenceWRegex(); }
        break;
      case 11: // inputPattern -> inputPattern, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 12: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); Console.WriteLine("|"); }
        break;
      case 13: // inputPattern -> T_LPAR, inputPattern, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 14: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new WordWRegex(ValueStack[ValueStack.Depth-1].s); Console.WriteLine("Word"); }
        break;
      case 15: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new RepetitionWRegex(new WildcardWRegex()); Console.WriteLine("*"); }
        break;
      case 18: // output -> outputPattern
{ Console.WriteLine("OUTPUT: " + ValueStack[ValueStack.Depth-1].s); CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 21: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).BEGIN(2); Console.WriteLine(": ..."); }
        break;
      case 22: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
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
