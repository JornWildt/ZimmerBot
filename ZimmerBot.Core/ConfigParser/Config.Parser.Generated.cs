// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 26-03-2016 16:35:05
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 26-03-2016 16:35:02>

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
internal enum Token {error=2,EOF=3,T_GT=4,T_COLON=5,T_ABSTRACTION=6,
    T_WEIGHT=7,T_CALL=8,T_EVERY=9,T_ANSWER=10,T_IMPLIES=11,T_COMMA=12,
    T_LPAR=13,T_RPAR=14,T_LBRACE=15,T_RBRACE=16,T_AMP=17,T_OUTPUT=18,
    T_WORD=19,T_STRING=20,T_NUMBER=21,T_EQU=22,T_PLUS=23,T_STAR=24,
    T_PIPE=25,T_DOT=26,T_DOLLAR=27};

internal partial struct ValueType
{ 
  public OutputStatement output;
  public List<OutputStatement> outputList;
  public WRegex regex;
  public Expression expr;
  public KeyValuePair<string,string> template;
  public List<Expression> exprList;
  public RuleModifier ruleModifier;
  public List<RuleModifier> ruleModifierList;
  public Func<Knowledge.Domain,Knowledge.Rule> ruleGenerator;
  public List<Func<Knowledge.Domain,Knowledge.Rule>> ruleGeneratorList;
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
  private static Rule[] rules = new Rule[55];
  private static State[] states = new State[81];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "statementSeq", "statement", "configuration", "rule", 
      "wordSeq", "ruleSeq", "input", "ruleModifierSeq", "outputSeq", "inputPatternSeq", 
      "inputPattern", "ruleModifier", "condition", "weight", "schedule", "expr", 
      "output", "outputPattern", "call", "answer", "Anon@1", "Anon@2", "exprReference", 
      "exprSeq", "exprSeq2", "exprBinary", "exprUnary", "exprIdentifier", };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{6,6,4,60,3,-2},new int[]{-4,4,-5,5,-6,13,-9,14});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{19,12},new int[]{-7,7});
    states[7] = new State(new int[]{11,8,12,10});
    states[8] = new State(new int[]{19,12},new int[]{-7,9});
    states[9] = new State(new int[]{12,10,6,-7,4,-7,3,-7});
    states[10] = new State(new int[]{19,11});
    states[11] = new State(-53);
    states[12] = new State(-54);
    states[13] = new State(-6);
    states[14] = new State(-20,new int[]{-10,15});
    states[15] = new State(new int[]{17,73,7,76,9,79,5,-28,15,-28,8,-28,10,-28,6,-28,4,-28,3,-28,16,-28},new int[]{-11,16,-14,71,-15,72,-16,75,-17,78});
    states[16] = new State(new int[]{5,19,15,22,8,29,10,55,6,-10,4,-10,3,-10,16,-10},new int[]{-19,17,-20,18,-21,28,-22,54});
    states[17] = new State(-27);
    states[18] = new State(-29);
    states[19] = new State(-32,new int[]{-23,20});
    states[20] = new State(new int[]{18,21});
    states[21] = new State(-33);
    states[22] = new State(new int[]{19,23});
    states[23] = new State(new int[]{16,24});
    states[24] = new State(new int[]{5,25});
    states[25] = new State(-34,new int[]{-24,26});
    states[26] = new State(new int[]{18,27});
    states[27] = new State(-35);
    states[28] = new State(-30);
    states[29] = new State(new int[]{19,48},new int[]{-25,30});
    states[30] = new State(new int[]{13,31,26,46});
    states[31] = new State(new int[]{13,41,19,48,27,49,20,51,21,52,14,-39},new int[]{-26,32,-27,34,-18,53,-28,39,-29,40,-30,44,-25,45});
    states[32] = new State(new int[]{14,33});
    states[33] = new State(-36);
    states[34] = new State(new int[]{12,35,14,-38});
    states[35] = new State(new int[]{13,41,19,48,27,49,20,51,21,52},new int[]{-18,36,-28,39,-29,40,-30,44,-25,45});
    states[36] = new State(new int[]{22,37,12,-40,14,-40});
    states[37] = new State(new int[]{13,41,19,48,27,49,20,51,21,52},new int[]{-18,38,-28,39,-29,40,-30,44,-25,45});
    states[38] = new State(-44);
    states[39] = new State(-42);
    states[40] = new State(-43);
    states[41] = new State(new int[]{13,41,19,48,27,49,20,51,21,52},new int[]{-18,42,-28,39,-29,40,-30,44,-25,45});
    states[42] = new State(new int[]{14,43,22,37});
    states[43] = new State(-45);
    states[44] = new State(-46);
    states[45] = new State(new int[]{26,46,22,-49,12,-49,14,-49,17,-49,7,-49,9,-49,5,-49,15,-49,8,-49,10,-49,6,-49,4,-49,3,-49,16,-49});
    states[46] = new State(new int[]{19,47});
    states[47] = new State(-51);
    states[48] = new State(-52);
    states[49] = new State(new int[]{21,50});
    states[50] = new State(-50);
    states[51] = new State(-47);
    states[52] = new State(-48);
    states[53] = new State(new int[]{22,37,12,-41,14,-41});
    states[54] = new State(-31);
    states[55] = new State(new int[]{15,56});
    states[56] = new State(-9,new int[]{-8,57});
    states[57] = new State(new int[]{16,58,4,60},new int[]{-6,59,-9,14});
    states[58] = new State(-37);
    states[59] = new State(-8);
    states[60] = new State(-13,new int[]{-12,61});
    states[61] = new State(new int[]{13,65,19,68,24,69,23,70,17,-11,7,-11,9,-11,5,-11,15,-11,8,-11,10,-11,6,-11,4,-11,3,-11,16,-11},new int[]{-13,62});
    states[62] = new State(new int[]{25,63,13,-12,19,-12,24,-12,23,-12,17,-12,7,-12,9,-12,5,-12,15,-12,8,-12,10,-12,6,-12,4,-12,3,-12,16,-12,14,-12});
    states[63] = new State(new int[]{13,65,19,68,24,69,23,70},new int[]{-13,64});
    states[64] = new State(-14);
    states[65] = new State(-13,new int[]{-12,66});
    states[66] = new State(new int[]{14,67,13,65,19,68,24,69,23,70},new int[]{-13,62});
    states[67] = new State(-15);
    states[68] = new State(-16);
    states[69] = new State(-17);
    states[70] = new State(-18);
    states[71] = new State(-19);
    states[72] = new State(-21);
    states[73] = new State(new int[]{13,41,19,48,27,49,20,51,21,52},new int[]{-18,74,-28,39,-29,40,-30,44,-25,45});
    states[74] = new State(new int[]{22,37,17,-24,7,-24,9,-24,5,-24,15,-24,8,-24,10,-24,6,-24,4,-24,3,-24,16,-24});
    states[75] = new State(-22);
    states[76] = new State(new int[]{21,77});
    states[77] = new State(-25);
    states[78] = new State(-23);
    states[79] = new State(new int[]{21,80});
    states[80] = new State(-26);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5});
    rules[6] = new Rule(-4, new int[]{-6});
    rules[7] = new Rule(-5, new int[]{6,-7,11,-7});
    rules[8] = new Rule(-8, new int[]{-8,-6});
    rules[9] = new Rule(-8, new int[]{});
    rules[10] = new Rule(-6, new int[]{-9,-10,-11});
    rules[11] = new Rule(-9, new int[]{4,-12});
    rules[12] = new Rule(-12, new int[]{-12,-13});
    rules[13] = new Rule(-12, new int[]{});
    rules[14] = new Rule(-13, new int[]{-13,25,-13});
    rules[15] = new Rule(-13, new int[]{13,-12,14});
    rules[16] = new Rule(-13, new int[]{19});
    rules[17] = new Rule(-13, new int[]{24});
    rules[18] = new Rule(-13, new int[]{23});
    rules[19] = new Rule(-10, new int[]{-10,-14});
    rules[20] = new Rule(-10, new int[]{});
    rules[21] = new Rule(-14, new int[]{-15});
    rules[22] = new Rule(-14, new int[]{-16});
    rules[23] = new Rule(-14, new int[]{-17});
    rules[24] = new Rule(-15, new int[]{17,-18});
    rules[25] = new Rule(-16, new int[]{7,21});
    rules[26] = new Rule(-17, new int[]{9,21});
    rules[27] = new Rule(-11, new int[]{-11,-19});
    rules[28] = new Rule(-11, new int[]{});
    rules[29] = new Rule(-19, new int[]{-20});
    rules[30] = new Rule(-19, new int[]{-21});
    rules[31] = new Rule(-19, new int[]{-22});
    rules[32] = new Rule(-23, new int[]{});
    rules[33] = new Rule(-20, new int[]{5,-23,18});
    rules[34] = new Rule(-24, new int[]{});
    rules[35] = new Rule(-20, new int[]{15,19,16,5,-24,18});
    rules[36] = new Rule(-21, new int[]{8,-25,13,-26,14});
    rules[37] = new Rule(-22, new int[]{10,15,-8,16});
    rules[38] = new Rule(-26, new int[]{-27});
    rules[39] = new Rule(-26, new int[]{});
    rules[40] = new Rule(-27, new int[]{-27,12,-18});
    rules[41] = new Rule(-27, new int[]{-18});
    rules[42] = new Rule(-18, new int[]{-28});
    rules[43] = new Rule(-18, new int[]{-29});
    rules[44] = new Rule(-28, new int[]{-18,22,-18});
    rules[45] = new Rule(-29, new int[]{13,-18,14});
    rules[46] = new Rule(-29, new int[]{-30});
    rules[47] = new Rule(-29, new int[]{20});
    rules[48] = new Rule(-29, new int[]{21});
    rules[49] = new Rule(-30, new int[]{-25});
    rules[50] = new Rule(-30, new int[]{27,21});
    rules[51] = new Rule(-25, new int[]{-25,26,19});
    rules[52] = new Rule(-25, new int[]{19});
    rules[53] = new Rule(-7, new int[]{-7,12,19});
    rules[54] = new Rule(-7, new int[]{19});
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
      case 6: // statement -> rule
{ ValueStack[ValueStack.Depth-1].ruleGenerator(Domain); }
        break;
      case 7: // configuration -> T_ABSTRACTION, wordSeq, T_IMPLIES, wordSeq
{ RegisterAbstractions(ValueStack[ValueStack.Depth-3].stringList, ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 8: // ruleSeq -> ruleSeq, rule
{ ValueStack[ValueStack.Depth-2].ruleGeneratorList.Add(ValueStack[ValueStack.Depth-1].ruleGenerator); CurrentSemanticValue.ruleGeneratorList = ValueStack[ValueStack.Depth-2].ruleGeneratorList; }
        break;
      case 9: // ruleSeq -> /* empty */
{ CurrentSemanticValue.ruleGeneratorList = new List<Func<Knowledge.Domain,Knowledge.Rule>>(); }
        break;
      case 10: // rule -> input, ruleModifierSeq, outputSeq
{ 
      CurrentSemanticValue.ruleGenerator = RuleGenerator(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-2].ruleModifierList, ValueStack[ValueStack.Depth-1].outputList);
    }
        break;
      case 11: // input -> T_GT, inputPatternSeq
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 12: // inputPatternSeq -> inputPatternSeq, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 13: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = null; }
        break;
      case 14: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 15: // inputPattern -> T_LPAR, inputPatternSeq, T_RPAR
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
      case 19: // ruleModifierSeq -> ruleModifierSeq, ruleModifier
{ CurrentSemanticValue.ruleModifierList.Add(ValueStack[ValueStack.Depth-1].ruleModifier); }
        break;
      case 20: // ruleModifierSeq -> /* empty */
{ CurrentSemanticValue.ruleModifierList = new List<RuleModifier>(); }
        break;
      case 21: // ruleModifier -> condition
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 22: // ruleModifier -> weight
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 23: // ruleModifier -> schedule
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 24: // condition -> T_AMP, expr
{ CurrentSemanticValue.ruleModifier = new ConditionRuleModifier(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 25: // weight -> T_WEIGHT, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new WeightRuleModifier(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 26: // schedule -> T_EVERY, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new ScheduleRuleModifier((int)ValueStack[ValueStack.Depth-1].n); }
        break;
      case 27: // outputSeq -> outputSeq, output
{ ValueStack[ValueStack.Depth-2].outputList.Add(ValueStack[ValueStack.Depth-1].output); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 28: // outputSeq -> /* empty */
{ CurrentSemanticValue.outputList = new List<OutputStatement>(); }
        break;
      case 29: // output -> outputPattern
{ CurrentSemanticValue.output = new TemplateOutputStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 30: // output -> call
{ CurrentSemanticValue.output = new CallOutputStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 31: // output -> answer
{ CurrentSemanticValue.output = ValueStack[ValueStack.Depth-1].output;}
        break;
      case 32: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 33: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 34: // Anon@2 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 35: // outputPattern -> T_LBRACE, T_WORD, T_RBRACE, T_COLON, Anon@2, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>(ValueStack[ValueStack.Depth-5].s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 36: // call -> T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].exprList); }
        break;
      case 37: // answer -> T_ANSWER, T_LBRACE, ruleSeq, T_RBRACE
{ CurrentSemanticValue.output = new AnswerOutputStatement(Domain, ValueStack[ValueStack.Depth-2].ruleGeneratorList); }
        break;
      case 38: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 39: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 40: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 41: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 42: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 43: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 44: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 45: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 46: // exprUnary -> exprIdentifier
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 47: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 48: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 49: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = new IdentifierExpr(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 50: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 51: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + "." + ValueStack[ValueStack.Depth-1].s; }
        break;
      case 52: // exprReference -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 53: // wordSeq -> wordSeq, T_COMMA, T_WORD
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 54: // wordSeq -> T_WORD
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
