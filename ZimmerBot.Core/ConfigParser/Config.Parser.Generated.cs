// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 31-03-2016 06:20:50
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 31-03-2016 06:20:45>

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
    T_WEIGHT=7,T_CALL=8,T_EVERY=9,T_ANSWER=10,T_RDF_IMPORT=11,T_IMPLIES=12,
    T_COMMA=13,T_LPAR=14,T_RPAR=15,T_LBRACE=16,T_RBRACE=17,T_AMP=18,
    T_OUTPUT=19,T_WORD=20,T_STRING=21,T_NUMBER=22,T_QUESTION=23,T_EQU=24,
    T_PLUS=25,T_STAR=26,T_PIPE=27,T_DOT=28,T_DOLLAR=29};

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
  private static Rule[] rules = new Rule[57];
  private static State[] states = new State[84];
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
    states[3] = new State(new int[]{6,6,11,13,4,62,3,-2},new int[]{-4,4,-5,5,-6,15,-9,16});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{20,12},new int[]{-7,7});
    states[7] = new State(new int[]{12,8,13,10});
    states[8] = new State(new int[]{20,12},new int[]{-7,9});
    states[9] = new State(new int[]{13,10,6,-7,11,-7,4,-7,3,-7});
    states[10] = new State(new int[]{20,11});
    states[11] = new State(-55);
    states[12] = new State(-56);
    states[13] = new State(new int[]{21,14});
    states[14] = new State(-8);
    states[15] = new State(-6);
    states[16] = new State(-22,new int[]{-10,17});
    states[17] = new State(new int[]{18,76,7,79,9,82,5,-30,16,-30,8,-30,10,-30,6,-30,11,-30,4,-30,3,-30,17,-30},new int[]{-11,18,-14,74,-15,75,-16,78,-17,81});
    states[18] = new State(new int[]{5,21,16,24,8,31,10,57,6,-11,11,-11,4,-11,3,-11,17,-11},new int[]{-19,19,-20,20,-21,30,-22,56});
    states[19] = new State(-29);
    states[20] = new State(-31);
    states[21] = new State(-34,new int[]{-23,22});
    states[22] = new State(new int[]{19,23});
    states[23] = new State(-35);
    states[24] = new State(new int[]{20,25});
    states[25] = new State(new int[]{17,26});
    states[26] = new State(new int[]{5,27});
    states[27] = new State(-36,new int[]{-24,28});
    states[28] = new State(new int[]{19,29});
    states[29] = new State(-37);
    states[30] = new State(-32);
    states[31] = new State(new int[]{20,50},new int[]{-25,32});
    states[32] = new State(new int[]{14,33,28,48});
    states[33] = new State(new int[]{14,43,20,50,29,51,21,53,22,54,15,-41},new int[]{-26,34,-27,36,-18,55,-28,41,-29,42,-30,46,-25,47});
    states[34] = new State(new int[]{15,35});
    states[35] = new State(-38);
    states[36] = new State(new int[]{13,37,15,-40});
    states[37] = new State(new int[]{14,43,20,50,29,51,21,53,22,54},new int[]{-18,38,-28,41,-29,42,-30,46,-25,47});
    states[38] = new State(new int[]{24,39,13,-42,15,-42});
    states[39] = new State(new int[]{14,43,20,50,29,51,21,53,22,54},new int[]{-18,40,-28,41,-29,42,-30,46,-25,47});
    states[40] = new State(-46);
    states[41] = new State(-44);
    states[42] = new State(-45);
    states[43] = new State(new int[]{14,43,20,50,29,51,21,53,22,54},new int[]{-18,44,-28,41,-29,42,-30,46,-25,47});
    states[44] = new State(new int[]{15,45,24,39});
    states[45] = new State(-47);
    states[46] = new State(-48);
    states[47] = new State(new int[]{28,48,24,-51,13,-51,15,-51,18,-51,7,-51,9,-51,5,-51,16,-51,8,-51,10,-51,6,-51,11,-51,4,-51,3,-51,17,-51});
    states[48] = new State(new int[]{20,49});
    states[49] = new State(-53);
    states[50] = new State(-54);
    states[51] = new State(new int[]{22,52});
    states[52] = new State(-52);
    states[53] = new State(-49);
    states[54] = new State(-50);
    states[55] = new State(new int[]{24,39,13,-43,15,-43});
    states[56] = new State(-33);
    states[57] = new State(new int[]{16,58});
    states[58] = new State(-10,new int[]{-8,59});
    states[59] = new State(new int[]{17,60,4,62},new int[]{-6,61,-9,16});
    states[60] = new State(-39);
    states[61] = new State(-9);
    states[62] = new State(-14,new int[]{-12,63});
    states[63] = new State(new int[]{14,68,20,71,26,72,25,73,18,-12,7,-12,9,-12,5,-12,16,-12,8,-12,10,-12,6,-12,11,-12,4,-12,3,-12,17,-12},new int[]{-13,64});
    states[64] = new State(new int[]{27,65,23,67,14,-13,20,-13,26,-13,25,-13,18,-13,7,-13,9,-13,5,-13,16,-13,8,-13,10,-13,6,-13,11,-13,4,-13,3,-13,17,-13,15,-13});
    states[65] = new State(new int[]{14,68,20,71,26,72,25,73},new int[]{-13,66});
    states[66] = new State(-15);
    states[67] = new State(-16);
    states[68] = new State(-14,new int[]{-12,69});
    states[69] = new State(new int[]{15,70,14,68,20,71,26,72,25,73},new int[]{-13,64});
    states[70] = new State(-17);
    states[71] = new State(-18);
    states[72] = new State(-19);
    states[73] = new State(-20);
    states[74] = new State(-21);
    states[75] = new State(-23);
    states[76] = new State(new int[]{14,43,20,50,29,51,21,53,22,54},new int[]{-18,77,-28,41,-29,42,-30,46,-25,47});
    states[77] = new State(new int[]{24,39,18,-26,7,-26,9,-26,5,-26,16,-26,8,-26,10,-26,6,-26,11,-26,4,-26,3,-26,17,-26});
    states[78] = new State(-24);
    states[79] = new State(new int[]{22,80});
    states[80] = new State(-27);
    states[81] = new State(-25);
    states[82] = new State(new int[]{22,83});
    states[83] = new State(-28);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5});
    rules[6] = new Rule(-4, new int[]{-6});
    rules[7] = new Rule(-5, new int[]{6,-7,12,-7});
    rules[8] = new Rule(-5, new int[]{11,21});
    rules[9] = new Rule(-8, new int[]{-8,-6});
    rules[10] = new Rule(-8, new int[]{});
    rules[11] = new Rule(-6, new int[]{-9,-10,-11});
    rules[12] = new Rule(-9, new int[]{4,-12});
    rules[13] = new Rule(-12, new int[]{-12,-13});
    rules[14] = new Rule(-12, new int[]{});
    rules[15] = new Rule(-13, new int[]{-13,27,-13});
    rules[16] = new Rule(-13, new int[]{-13,23});
    rules[17] = new Rule(-13, new int[]{14,-12,15});
    rules[18] = new Rule(-13, new int[]{20});
    rules[19] = new Rule(-13, new int[]{26});
    rules[20] = new Rule(-13, new int[]{25});
    rules[21] = new Rule(-10, new int[]{-10,-14});
    rules[22] = new Rule(-10, new int[]{});
    rules[23] = new Rule(-14, new int[]{-15});
    rules[24] = new Rule(-14, new int[]{-16});
    rules[25] = new Rule(-14, new int[]{-17});
    rules[26] = new Rule(-15, new int[]{18,-18});
    rules[27] = new Rule(-16, new int[]{7,22});
    rules[28] = new Rule(-17, new int[]{9,22});
    rules[29] = new Rule(-11, new int[]{-11,-19});
    rules[30] = new Rule(-11, new int[]{});
    rules[31] = new Rule(-19, new int[]{-20});
    rules[32] = new Rule(-19, new int[]{-21});
    rules[33] = new Rule(-19, new int[]{-22});
    rules[34] = new Rule(-23, new int[]{});
    rules[35] = new Rule(-20, new int[]{5,-23,19});
    rules[36] = new Rule(-24, new int[]{});
    rules[37] = new Rule(-20, new int[]{16,20,17,5,-24,19});
    rules[38] = new Rule(-21, new int[]{8,-25,14,-26,15});
    rules[39] = new Rule(-22, new int[]{10,16,-8,17});
    rules[40] = new Rule(-26, new int[]{-27});
    rules[41] = new Rule(-26, new int[]{});
    rules[42] = new Rule(-27, new int[]{-27,13,-18});
    rules[43] = new Rule(-27, new int[]{-18});
    rules[44] = new Rule(-18, new int[]{-28});
    rules[45] = new Rule(-18, new int[]{-29});
    rules[46] = new Rule(-28, new int[]{-18,24,-18});
    rules[47] = new Rule(-29, new int[]{14,-18,15});
    rules[48] = new Rule(-29, new int[]{-30});
    rules[49] = new Rule(-29, new int[]{21});
    rules[50] = new Rule(-29, new int[]{22});
    rules[51] = new Rule(-30, new int[]{-25});
    rules[52] = new Rule(-30, new int[]{29,22});
    rules[53] = new Rule(-25, new int[]{-25,28,20});
    rules[54] = new Rule(-25, new int[]{20});
    rules[55] = new Rule(-7, new int[]{-7,13,20});
    rules[56] = new Rule(-7, new int[]{20});
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
      case 8: // configuration -> T_RDF_IMPORT, T_STRING
{ RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 9: // ruleSeq -> ruleSeq, rule
{ ValueStack[ValueStack.Depth-2].ruleGeneratorList.Add(ValueStack[ValueStack.Depth-1].ruleGenerator); CurrentSemanticValue.ruleGeneratorList = ValueStack[ValueStack.Depth-2].ruleGeneratorList; }
        break;
      case 10: // ruleSeq -> /* empty */
{ CurrentSemanticValue.ruleGeneratorList = new List<Func<Knowledge.Domain,Knowledge.Rule>>(); }
        break;
      case 11: // rule -> input, ruleModifierSeq, outputSeq
{ 
      CurrentSemanticValue.ruleGenerator = RuleGenerator(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-2].ruleModifierList, ValueStack[ValueStack.Depth-1].outputList);
    }
        break;
      case 12: // input -> T_GT, inputPatternSeq
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 13: // inputPatternSeq -> inputPatternSeq, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 14: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = null; }
        break;
      case 15: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 16: // inputPattern -> inputPattern, T_QUESTION
{ CurrentSemanticValue.regex =  new RepetitionWRegex(ValueStack[ValueStack.Depth-2].regex, 0, 1); }
        break;
      case 17: // inputPattern -> T_LPAR, inputPatternSeq, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 18: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new WordWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 19: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new RepetitionWRegex(new WildcardWRegex()); }
        break;
      case 20: // inputPattern -> T_PLUS
{ CurrentSemanticValue.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
        break;
      case 21: // ruleModifierSeq -> ruleModifierSeq, ruleModifier
{ CurrentSemanticValue.ruleModifierList.Add(ValueStack[ValueStack.Depth-1].ruleModifier); }
        break;
      case 22: // ruleModifierSeq -> /* empty */
{ CurrentSemanticValue.ruleModifierList = new List<RuleModifier>(); }
        break;
      case 23: // ruleModifier -> condition
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 24: // ruleModifier -> weight
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 25: // ruleModifier -> schedule
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 26: // condition -> T_AMP, expr
{ CurrentSemanticValue.ruleModifier = new ConditionRuleModifier(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 27: // weight -> T_WEIGHT, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new WeightRuleModifier(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 28: // schedule -> T_EVERY, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new ScheduleRuleModifier((int)ValueStack[ValueStack.Depth-1].n); }
        break;
      case 29: // outputSeq -> outputSeq, output
{ ValueStack[ValueStack.Depth-2].outputList.Add(ValueStack[ValueStack.Depth-1].output); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 30: // outputSeq -> /* empty */
{ CurrentSemanticValue.outputList = new List<OutputStatement>(); }
        break;
      case 31: // output -> outputPattern
{ CurrentSemanticValue.output = new TemplateOutputStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 32: // output -> call
{ CurrentSemanticValue.output = new CallOutputStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 33: // output -> answer
{ CurrentSemanticValue.output = ValueStack[ValueStack.Depth-1].output;}
        break;
      case 34: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 35: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 36: // Anon@2 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 37: // outputPattern -> T_LBRACE, T_WORD, T_RBRACE, T_COLON, Anon@2, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>(ValueStack[ValueStack.Depth-5].s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 38: // call -> T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].exprList); }
        break;
      case 39: // answer -> T_ANSWER, T_LBRACE, ruleSeq, T_RBRACE
{ CurrentSemanticValue.output = new AnswerOutputStatement(Domain, ValueStack[ValueStack.Depth-2].ruleGeneratorList); }
        break;
      case 40: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 41: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 42: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 43: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 44: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 45: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 46: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 47: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 48: // exprUnary -> exprIdentifier
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 49: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 50: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 51: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = new IdentifierExpr(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 52: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 53: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + "." + ValueStack[ValueStack.Depth-1].s; }
        break;
      case 54: // exprReference -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 55: // wordSeq -> wordSeq, T_COMMA, T_WORD
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 56: // wordSeq -> T_WORD
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
