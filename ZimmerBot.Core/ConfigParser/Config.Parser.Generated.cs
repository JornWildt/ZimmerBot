// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 25-03-2016 07:43:00
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 25-03-2016 07:42:26>

// options: conflicts no-lines gplex conflicts

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using ZimmerBot.Core.WordRegex;
using ZimmerBot.Core.Expressions;

namespace ZimmerBot.Core.ConfigParser
{
internal enum Token {error=2,EOF=3,T_EXCL=4,T_GT=5,T_COLON=6,
    T_ABSTRACTION=7,T_CALL=8,T_IMPLIES=9,T_COMMA=10,T_LPAR=11,T_RPAR=12,
    T_LBRACE=13,T_RBRACE=14,T_AMP=15,T_OUTPUT=16,T_WORD=17,T_STRING=18,
    T_NUMBER=19,T_EOL=20,T_EQU=21,T_PLUS=22,T_STAR=23,T_PIPE=24,
    T_DOT=25,T_DOLLAR=26};

internal partial struct ValueType
{ 
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
  private static Rule[] rules = new Rule[51];
  private static State[] states = new State[82];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "statementSeq", "statement", "configuration", "rule", 
      "wordSeq", "input", "ruleModifier", "outputSeq", "inputPattern", "condition", 
      "weight", "expr", "output", "outputPattern", "call", "Anon@1", "Anon@2", 
      "exprReference", "exprSeq", "exprSeq2", "exprBinary", "exprUnary", "exprIdentifier", 
      };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{4,6,5,68,20,81,3,-2},new int[]{-4,4,-5,5,-6,15,-8,16});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{7,7});
    states[7] = new State(new int[]{17,14},new int[]{-7,8});
    states[8] = new State(new int[]{9,9,10,12});
    states[9] = new State(new int[]{17,14},new int[]{-7,10});
    states[10] = new State(new int[]{20,11,10,12});
    states[11] = new State(-8);
    states[12] = new State(new int[]{17,13});
    states[13] = new State(-49);
    states[14] = new State(-50);
    states[15] = new State(-6);
    states[16] = new State(new int[]{15,63,23,60,6,-22,13,-22,4,-22,5,-22,20,-22,3,-22},new int[]{-9,17,-12,58,-13,66});
    states[17] = new State(-26,new int[]{-10,18});
    states[18] = new State(new int[]{6,21,13,24,4,31,5,-9,20,-9,3,-9},new int[]{-15,19,-16,20,-17,30});
    states[19] = new State(-25);
    states[20] = new State(-27);
    states[21] = new State(-29,new int[]{-18,22});
    states[22] = new State(new int[]{16,23});
    states[23] = new State(-30);
    states[24] = new State(new int[]{17,25});
    states[25] = new State(new int[]{14,26});
    states[26] = new State(new int[]{6,27});
    states[27] = new State(-31,new int[]{-19,28});
    states[28] = new State(new int[]{16,29});
    states[29] = new State(-32);
    states[30] = new State(-28);
    states[31] = new State(new int[]{8,32});
    states[32] = new State(new int[]{17,52},new int[]{-20,33});
    states[33] = new State(new int[]{11,34,25,50});
    states[34] = new State(new int[]{11,45,17,52,26,53,18,55,19,56,12,-35},new int[]{-21,35,-22,38,-14,57,-23,43,-24,44,-25,48,-20,49});
    states[35] = new State(new int[]{12,36});
    states[36] = new State(new int[]{20,37});
    states[37] = new State(-33);
    states[38] = new State(new int[]{10,39,12,-34});
    states[39] = new State(new int[]{11,45,17,52,26,53,18,55,19,56},new int[]{-14,40,-23,43,-24,44,-25,48,-20,49});
    states[40] = new State(new int[]{21,41,10,-36,12,-36});
    states[41] = new State(new int[]{11,45,17,52,26,53,18,55,19,56},new int[]{-14,42,-23,43,-24,44,-25,48,-20,49});
    states[42] = new State(-40);
    states[43] = new State(-38);
    states[44] = new State(-39);
    states[45] = new State(new int[]{11,45,17,52,26,53,18,55,19,56},new int[]{-14,46,-23,43,-24,44,-25,48,-20,49});
    states[46] = new State(new int[]{12,47,21,41});
    states[47] = new State(-41);
    states[48] = new State(-42);
    states[49] = new State(new int[]{25,50,21,-45,10,-45,12,-45,20,-45});
    states[50] = new State(new int[]{17,51});
    states[51] = new State(-47);
    states[52] = new State(-48);
    states[53] = new State(new int[]{19,54});
    states[54] = new State(-46);
    states[55] = new State(-43);
    states[56] = new State(-44);
    states[57] = new State(new int[]{21,41,10,-37,12,-37});
    states[58] = new State(new int[]{23,60,6,-24,13,-24,4,-24,5,-24,20,-24,3,-24},new int[]{-13,59});
    states[59] = new State(-19);
    states[60] = new State(new int[]{19,61});
    states[61] = new State(new int[]{20,62});
    states[62] = new State(-23);
    states[63] = new State(new int[]{11,45,17,52,26,53,18,55,19,56},new int[]{-14,64,-23,43,-24,44,-25,48,-20,49});
    states[64] = new State(new int[]{20,65,21,41});
    states[65] = new State(-21);
    states[66] = new State(new int[]{15,63,6,-22,13,-22,4,-22,5,-22,20,-22,3,-22},new int[]{-12,67});
    states[67] = new State(-20);
    states[68] = new State(new int[]{20,80,11,74,17,77,23,78,22,79},new int[]{-11,69});
    states[69] = new State(new int[]{20,70,24,72,11,74,17,77,23,78,22,79},new int[]{-11,71});
    states[70] = new State(-10);
    states[71] = new State(new int[]{24,72,11,74,17,77,23,78,22,79,20,-13,12,-13},new int[]{-11,71});
    states[72] = new State(new int[]{11,74,17,77,23,78,22,79},new int[]{-11,73});
    states[73] = new State(new int[]{24,-14,11,74,17,77,23,-14,22,-14,20,-14,12,-14},new int[]{-11,71});
    states[74] = new State(new int[]{11,74,17,77,23,78,22,79},new int[]{-11,75});
    states[75] = new State(new int[]{12,76,24,72,11,74,17,77,23,78,22,79},new int[]{-11,71});
    states[76] = new State(-15);
    states[77] = new State(-16);
    states[78] = new State(-17);
    states[79] = new State(-18);
    states[80] = new State(-11);
    states[81] = new State(new int[]{15,-12,23,-12,6,-12,13,-12,4,-7,5,-7,20,-7,3,-7});

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5});
    rules[6] = new Rule(-4, new int[]{-6});
    rules[7] = new Rule(-4, new int[]{20});
    rules[8] = new Rule(-5, new int[]{4,7,-7,9,-7,20});
    rules[9] = new Rule(-6, new int[]{-8,-9,-10});
    rules[10] = new Rule(-8, new int[]{5,-11,20});
    rules[11] = new Rule(-8, new int[]{5,20});
    rules[12] = new Rule(-8, new int[]{20});
    rules[13] = new Rule(-11, new int[]{-11,-11});
    rules[14] = new Rule(-11, new int[]{-11,24,-11});
    rules[15] = new Rule(-11, new int[]{11,-11,12});
    rules[16] = new Rule(-11, new int[]{17});
    rules[17] = new Rule(-11, new int[]{23});
    rules[18] = new Rule(-11, new int[]{22});
    rules[19] = new Rule(-9, new int[]{-12,-13});
    rules[20] = new Rule(-9, new int[]{-13,-12});
    rules[21] = new Rule(-12, new int[]{15,-14,20});
    rules[22] = new Rule(-12, new int[]{});
    rules[23] = new Rule(-13, new int[]{23,19,20});
    rules[24] = new Rule(-13, new int[]{});
    rules[25] = new Rule(-10, new int[]{-10,-15});
    rules[26] = new Rule(-10, new int[]{});
    rules[27] = new Rule(-15, new int[]{-16});
    rules[28] = new Rule(-15, new int[]{-17});
    rules[29] = new Rule(-18, new int[]{});
    rules[30] = new Rule(-16, new int[]{6,-18,16});
    rules[31] = new Rule(-19, new int[]{});
    rules[32] = new Rule(-16, new int[]{13,17,14,6,-19,16});
    rules[33] = new Rule(-17, new int[]{4,8,-20,11,-21,12,20});
    rules[34] = new Rule(-21, new int[]{-22});
    rules[35] = new Rule(-21, new int[]{});
    rules[36] = new Rule(-22, new int[]{-22,10,-14});
    rules[37] = new Rule(-22, new int[]{-14});
    rules[38] = new Rule(-14, new int[]{-23});
    rules[39] = new Rule(-14, new int[]{-24});
    rules[40] = new Rule(-23, new int[]{-14,21,-14});
    rules[41] = new Rule(-24, new int[]{11,-14,12});
    rules[42] = new Rule(-24, new int[]{-25});
    rules[43] = new Rule(-24, new int[]{18});
    rules[44] = new Rule(-24, new int[]{19});
    rules[45] = new Rule(-25, new int[]{-20});
    rules[46] = new Rule(-25, new int[]{26,19});
    rules[47] = new Rule(-20, new int[]{-20,25,17});
    rules[48] = new Rule(-20, new int[]{17});
    rules[49] = new Rule(-7, new int[]{-7,10,17});
    rules[50] = new Rule(-7, new int[]{17});
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
      case 8: // configuration -> T_EXCL, T_ABSTRACTION, wordSeq, T_IMPLIES, wordSeq, T_EOL
{ RegisterAbstractions(ValueStack[ValueStack.Depth-4].stringList, ValueStack[ValueStack.Depth-2].stringList); }
        break;
      case 9: // rule -> input, ruleModifier, outputSeq
{ 
      Knowledge.Rule r = Domain.AddRule(ValueStack[ValueStack.Depth-3].regex);
      if (ValueStack[ValueStack.Depth-2].expr != null)
        r.WithCondition(ValueStack[ValueStack.Depth-2].expr);
      r.WithWeight(ValueStack[ValueStack.Depth-2].n);
      if (ValueStack[ValueStack.Depth-1].outputList != null)
        r.WithOutputStatements(ValueStack[ValueStack.Depth-1].outputList);
    }
        break;
      case 10: // input -> T_GT, inputPattern, T_EOL
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-2].regex; }
        break;
      case 11: // input -> T_GT, T_EOL
{ CurrentSemanticValue.regex = null; }
        break;
      case 12: // input -> T_EOL
{ CurrentSemanticValue.regex = null; }
        break;
      case 13: // inputPattern -> inputPattern, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 14: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 15: // inputPattern -> T_LPAR, inputPattern, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 16: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new WordWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 17: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new RepetitionWRegex(new WildcardWRegex()); }
        break;
      case 18: // inputPattern -> T_PLUS
{ CurrentSemanticValue.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
        break;
      case 19: // ruleModifier -> condition, weight
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-2].expr; CurrentSemanticValue.n = ValueStack[ValueStack.Depth-1].n; }
        break;
      case 20: // ruleModifier -> weight, condition
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-1].expr; CurrentSemanticValue.n = ValueStack[ValueStack.Depth-2].n; }
        break;
      case 21: // condition -> T_AMP, expr, T_EOL
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-2].expr; }
        break;
      case 22: // condition -> /* empty */
{ CurrentSemanticValue.expr = null; }
        break;
      case 23: // weight -> T_STAR, T_NUMBER, T_EOL
{ CurrentSemanticValue.n = ValueStack[ValueStack.Depth-2].n; }
        break;
      case 24: // weight -> /* empty */
{ CurrentSemanticValue.n = 1.0; }
        break;
      case 25: // outputSeq -> outputSeq, output
{ ValueStack[ValueStack.Depth-2].outputList.Add(ValueStack[ValueStack.Depth-1].output); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 26: // outputSeq -> /* empty */
{ CurrentSemanticValue.outputList = new List<OutputStatement>(); }
        break;
      case 27: // output -> outputPattern
{ CurrentSemanticValue.output = new TemplateOutputStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 28: // output -> call
{ CurrentSemanticValue.output = new CallOutputStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 29: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 30: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 31: // Anon@2 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 32: // outputPattern -> T_LBRACE, T_WORD, T_RBRACE, T_COLON, Anon@2, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>(ValueStack[ValueStack.Depth-5].s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 33: // call -> T_EXCL, T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR, T_EOL
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-5].s, ValueStack[ValueStack.Depth-3].exprList); }
        break;
      case 34: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 35: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 36: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 37: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 38: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 39: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 40: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 41: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 42: // exprUnary -> exprIdentifier
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 43: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 44: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 45: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = new IdentifierExpr(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 46: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 47: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + "." + ValueStack[ValueStack.Depth-1].s; }
        break;
      case 48: // exprReference -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 49: // wordSeq -> wordSeq, T_COMMA, T_WORD
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 50: // wordSeq -> T_WORD
{ CurrentSemanticValue.stringList = new List<string>(); CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
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
