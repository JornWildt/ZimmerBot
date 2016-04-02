// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 02-04-2016 21:58:55
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 02-04-2016 21:55:29>

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
internal enum Token {error=2,EOF=3,T_COLON=4,T_CONCEPT=5,T_WEIGHT=6,
    T_CALL=7,T_EVERY=8,T_ANSWER=9,T_RDF_IMPORT=10,T_RDF_PREFIX=11,T_IMPLIES=12,
    T_COMMA=13,T_LPAR=14,T_RPAR=15,T_LBRACE=16,T_RBRACE=17,T_AMP=18,
    T_OUTPUT=19,T_WORD=20,T_CWORD=21,T_STRING=22,T_NUMBER=23,T_QUESTION=24,
    T_EQU=25,T_LT=26,T_GT=27,T_PLUS=28,T_STAR=29,T_PIPE=30,
    T_EXCL=31,T_DOT=32,T_DOLLAR=33};

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
  public Func<Knowledge.KnowledgeBase,Knowledge.Rule> ruleGenerator;
  public List<Func<Knowledge.KnowledgeBase,Knowledge.Rule>> ruleGeneratorList;
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
  private static Rule[] rules = new Rule[62];
  private static State[] states = new State[92];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "statementSeq", "statement", "configuration", "rule", 
      "cwordSeq", "ruleSeq", "input", "ruleModifierSeq", "outputSeq", "inputPatternSeq", 
      "inputPattern", "ruleModifier", "condition", "weight", "schedule", "expr", 
      "output", "outputPattern", "call", "answer", "Anon@1", "Anon@2", "exprReference", 
      "exprSeq", "exprSeq2", "exprBinary", "exprUnary", "exprIdentifier", "cword", 
      };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{5,6,10,15,11,17,27,67,3,-2},new int[]{-4,4,-5,5,-6,20,-9,21});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{20,7});
    states[7] = new State(new int[]{25,8});
    states[8] = new State(new int[]{20,12,21,13},new int[]{-7,9,-31,14});
    states[9] = new State(new int[]{13,10,5,-7,10,-7,11,-7,27,-7,3,-7});
    states[10] = new State(new int[]{20,12,21,13},new int[]{-31,11});
    states[11] = new State(-58);
    states[12] = new State(-60);
    states[13] = new State(-61);
    states[14] = new State(-59);
    states[15] = new State(new int[]{22,16});
    states[16] = new State(-8);
    states[17] = new State(new int[]{20,18});
    states[18] = new State(new int[]{22,19});
    states[19] = new State(-9);
    states[20] = new State(-6);
    states[21] = new State(-25,new int[]{-10,22});
    states[22] = new State(new int[]{18,84,6,87,8,90,4,-33,16,-33,7,-33,9,-33,5,-33,10,-33,11,-33,27,-33,3,-33,17,-33},new int[]{-11,23,-14,82,-15,83,-16,86,-17,89});
    states[23] = new State(new int[]{4,26,16,29,7,36,9,62,5,-12,10,-12,11,-12,27,-12,3,-12,17,-12},new int[]{-19,24,-20,25,-21,35,-22,61});
    states[24] = new State(-32);
    states[25] = new State(-34);
    states[26] = new State(-37,new int[]{-23,27});
    states[27] = new State(new int[]{19,28});
    states[28] = new State(-38);
    states[29] = new State(new int[]{20,30});
    states[30] = new State(new int[]{17,31});
    states[31] = new State(new int[]{4,32});
    states[32] = new State(-39,new int[]{-24,33});
    states[33] = new State(new int[]{19,34});
    states[34] = new State(-40);
    states[35] = new State(-35);
    states[36] = new State(new int[]{20,55},new int[]{-25,37});
    states[37] = new State(new int[]{14,38,32,53});
    states[38] = new State(new int[]{14,48,20,55,33,56,22,58,23,59,15,-44},new int[]{-26,39,-27,41,-18,60,-28,46,-29,47,-30,51,-25,52});
    states[39] = new State(new int[]{15,40});
    states[40] = new State(-41);
    states[41] = new State(new int[]{13,42,15,-43});
    states[42] = new State(new int[]{14,48,20,55,33,56,22,58,23,59},new int[]{-18,43,-28,46,-29,47,-30,51,-25,52});
    states[43] = new State(new int[]{25,44,13,-45,15,-45});
    states[44] = new State(new int[]{14,48,20,55,33,56,22,58,23,59},new int[]{-18,45,-28,46,-29,47,-30,51,-25,52});
    states[45] = new State(-49);
    states[46] = new State(-47);
    states[47] = new State(-48);
    states[48] = new State(new int[]{14,48,20,55,33,56,22,58,23,59},new int[]{-18,49,-28,46,-29,47,-30,51,-25,52});
    states[49] = new State(new int[]{15,50,25,44});
    states[50] = new State(-50);
    states[51] = new State(-51);
    states[52] = new State(new int[]{32,53,25,-54,13,-54,15,-54,18,-54,6,-54,8,-54,4,-54,16,-54,7,-54,9,-54,5,-54,10,-54,11,-54,27,-54,3,-54,17,-54});
    states[53] = new State(new int[]{20,54});
    states[54] = new State(-56);
    states[55] = new State(-57);
    states[56] = new State(new int[]{23,57});
    states[57] = new State(-55);
    states[58] = new State(-52);
    states[59] = new State(-53);
    states[60] = new State(new int[]{25,44,13,-46,15,-46});
    states[61] = new State(-36);
    states[62] = new State(new int[]{16,63});
    states[63] = new State(-11,new int[]{-8,64});
    states[64] = new State(new int[]{17,65,27,67},new int[]{-6,66,-9,21});
    states[65] = new State(-42);
    states[66] = new State(-10);
    states[67] = new State(-15,new int[]{-12,68});
    states[68] = new State(new int[]{14,73,20,76,21,77,29,78,28,79,31,80,18,-13,6,-13,8,-13,4,-13,16,-13,7,-13,9,-13,5,-13,10,-13,11,-13,27,-13,3,-13,17,-13},new int[]{-13,69});
    states[69] = new State(new int[]{30,70,24,72,14,-14,20,-14,21,-14,29,-14,28,-14,31,-14,18,-14,6,-14,8,-14,4,-14,16,-14,7,-14,9,-14,5,-14,10,-14,11,-14,27,-14,3,-14,17,-14,15,-14});
    states[70] = new State(new int[]{14,73,20,76,21,77,29,78,28,79,31,80},new int[]{-13,71});
    states[71] = new State(-16);
    states[72] = new State(-17);
    states[73] = new State(-15,new int[]{-12,74});
    states[74] = new State(new int[]{15,75,14,73,20,76,21,77,29,78,28,79,31,80},new int[]{-13,69});
    states[75] = new State(-18);
    states[76] = new State(-19);
    states[77] = new State(-20);
    states[78] = new State(-21);
    states[79] = new State(-22);
    states[80] = new State(new int[]{14,73,20,76,21,77,29,78,28,79,31,80},new int[]{-13,81});
    states[81] = new State(-23);
    states[82] = new State(-24);
    states[83] = new State(-26);
    states[84] = new State(new int[]{14,48,20,55,33,56,22,58,23,59},new int[]{-18,85,-28,46,-29,47,-30,51,-25,52});
    states[85] = new State(new int[]{25,44,18,-29,6,-29,8,-29,4,-29,16,-29,7,-29,9,-29,5,-29,10,-29,11,-29,27,-29,3,-29,17,-29});
    states[86] = new State(-27);
    states[87] = new State(new int[]{23,88});
    states[88] = new State(-30);
    states[89] = new State(-28);
    states[90] = new State(new int[]{23,91});
    states[91] = new State(-31);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5});
    rules[6] = new Rule(-4, new int[]{-6});
    rules[7] = new Rule(-5, new int[]{5,20,25,-7});
    rules[8] = new Rule(-5, new int[]{10,22});
    rules[9] = new Rule(-5, new int[]{11,20,22});
    rules[10] = new Rule(-8, new int[]{-8,-6});
    rules[11] = new Rule(-8, new int[]{});
    rules[12] = new Rule(-6, new int[]{-9,-10,-11});
    rules[13] = new Rule(-9, new int[]{27,-12});
    rules[14] = new Rule(-12, new int[]{-12,-13});
    rules[15] = new Rule(-12, new int[]{});
    rules[16] = new Rule(-13, new int[]{-13,30,-13});
    rules[17] = new Rule(-13, new int[]{-13,24});
    rules[18] = new Rule(-13, new int[]{14,-12,15});
    rules[19] = new Rule(-13, new int[]{20});
    rules[20] = new Rule(-13, new int[]{21});
    rules[21] = new Rule(-13, new int[]{29});
    rules[22] = new Rule(-13, new int[]{28});
    rules[23] = new Rule(-13, new int[]{31,-13});
    rules[24] = new Rule(-10, new int[]{-10,-14});
    rules[25] = new Rule(-10, new int[]{});
    rules[26] = new Rule(-14, new int[]{-15});
    rules[27] = new Rule(-14, new int[]{-16});
    rules[28] = new Rule(-14, new int[]{-17});
    rules[29] = new Rule(-15, new int[]{18,-18});
    rules[30] = new Rule(-16, new int[]{6,23});
    rules[31] = new Rule(-17, new int[]{8,23});
    rules[32] = new Rule(-11, new int[]{-11,-19});
    rules[33] = new Rule(-11, new int[]{});
    rules[34] = new Rule(-19, new int[]{-20});
    rules[35] = new Rule(-19, new int[]{-21});
    rules[36] = new Rule(-19, new int[]{-22});
    rules[37] = new Rule(-23, new int[]{});
    rules[38] = new Rule(-20, new int[]{4,-23,19});
    rules[39] = new Rule(-24, new int[]{});
    rules[40] = new Rule(-20, new int[]{16,20,17,4,-24,19});
    rules[41] = new Rule(-21, new int[]{7,-25,14,-26,15});
    rules[42] = new Rule(-22, new int[]{9,16,-8,17});
    rules[43] = new Rule(-26, new int[]{-27});
    rules[44] = new Rule(-26, new int[]{});
    rules[45] = new Rule(-27, new int[]{-27,13,-18});
    rules[46] = new Rule(-27, new int[]{-18});
    rules[47] = new Rule(-18, new int[]{-28});
    rules[48] = new Rule(-18, new int[]{-29});
    rules[49] = new Rule(-28, new int[]{-18,25,-18});
    rules[50] = new Rule(-29, new int[]{14,-18,15});
    rules[51] = new Rule(-29, new int[]{-30});
    rules[52] = new Rule(-29, new int[]{22});
    rules[53] = new Rule(-29, new int[]{23});
    rules[54] = new Rule(-30, new int[]{-25});
    rules[55] = new Rule(-30, new int[]{33,23});
    rules[56] = new Rule(-25, new int[]{-25,32,20});
    rules[57] = new Rule(-25, new int[]{20});
    rules[58] = new Rule(-7, new int[]{-7,13,-31});
    rules[59] = new Rule(-7, new int[]{-31});
    rules[60] = new Rule(-31, new int[]{20});
    rules[61] = new Rule(-31, new int[]{21});
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
{ ValueStack[ValueStack.Depth-1].ruleGenerator(KnowledgeBase); }
        break;
      case 7: // configuration -> T_CONCEPT, T_WORD, T_EQU, cwordSeq
{ RegisterConcept(ValueStack[ValueStack.Depth-3].s, ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 8: // configuration -> T_RDF_IMPORT, T_STRING
{ RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 9: // configuration -> T_RDF_PREFIX, T_WORD, T_STRING
{ RDFPrefix(ValueStack[ValueStack.Depth-2].s, ((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 10: // ruleSeq -> ruleSeq, rule
{ ValueStack[ValueStack.Depth-2].ruleGeneratorList.Add(ValueStack[ValueStack.Depth-1].ruleGenerator); CurrentSemanticValue.ruleGeneratorList = ValueStack[ValueStack.Depth-2].ruleGeneratorList; }
        break;
      case 11: // ruleSeq -> /* empty */
{ CurrentSemanticValue.ruleGeneratorList = new List<Func<Knowledge.KnowledgeBase,Knowledge.Rule>>(); }
        break;
      case 12: // rule -> input, ruleModifierSeq, outputSeq
{ 
      CurrentSemanticValue.ruleGenerator = RuleGenerator(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-2].ruleModifierList, ValueStack[ValueStack.Depth-1].outputList);
    }
        break;
      case 13: // input -> T_GT, inputPatternSeq
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 14: // inputPatternSeq -> inputPatternSeq, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 15: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = null; }
        break;
      case 16: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 17: // inputPattern -> inputPattern, T_QUESTION
{ CurrentSemanticValue.regex =  new RepetitionWRegex(ValueStack[ValueStack.Depth-2].regex, 0, 1); }
        break;
      case 18: // inputPattern -> T_LPAR, inputPatternSeq, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 19: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new WordWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 20: // inputPattern -> T_CWORD
{ CurrentSemanticValue.regex = new ConceptWRegex(KnowledgeBase, ValueStack[ValueStack.Depth-1].s); }
        break;
      case 21: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new RepetitionWRegex(new WildcardWRegex()); }
        break;
      case 22: // inputPattern -> T_PLUS
{ CurrentSemanticValue.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
        break;
      case 23: // inputPattern -> T_EXCL, inputPattern
{ CurrentSemanticValue.regex =  new NegationWRegex(ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 24: // ruleModifierSeq -> ruleModifierSeq, ruleModifier
{ CurrentSemanticValue.ruleModifierList.Add(ValueStack[ValueStack.Depth-1].ruleModifier); }
        break;
      case 25: // ruleModifierSeq -> /* empty */
{ CurrentSemanticValue.ruleModifierList = new List<RuleModifier>(); }
        break;
      case 26: // ruleModifier -> condition
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 27: // ruleModifier -> weight
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 28: // ruleModifier -> schedule
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 29: // condition -> T_AMP, expr
{ CurrentSemanticValue.ruleModifier = new ConditionRuleModifier(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 30: // weight -> T_WEIGHT, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new WeightRuleModifier(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 31: // schedule -> T_EVERY, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new ScheduleRuleModifier((int)ValueStack[ValueStack.Depth-1].n); }
        break;
      case 32: // outputSeq -> outputSeq, output
{ ValueStack[ValueStack.Depth-2].outputList.Add(ValueStack[ValueStack.Depth-1].output); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 33: // outputSeq -> /* empty */
{ CurrentSemanticValue.outputList = new List<OutputStatement>(); }
        break;
      case 34: // output -> outputPattern
{ CurrentSemanticValue.output = new TemplateOutputStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 35: // output -> call
{ CurrentSemanticValue.output = new CallOutputStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 36: // output -> answer
{ CurrentSemanticValue.output = ValueStack[ValueStack.Depth-1].output;}
        break;
      case 37: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 38: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 39: // Anon@2 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 40: // outputPattern -> T_LBRACE, T_WORD, T_RBRACE, T_COLON, Anon@2, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>(ValueStack[ValueStack.Depth-5].s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 41: // call -> T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].exprList); }
        break;
      case 42: // answer -> T_ANSWER, T_LBRACE, ruleSeq, T_RBRACE
{ CurrentSemanticValue.output = new AnswerOutputStatement(KnowledgeBase, ValueStack[ValueStack.Depth-2].ruleGeneratorList); }
        break;
      case 43: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 44: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 45: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 46: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 47: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 48: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 49: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 50: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 51: // exprUnary -> exprIdentifier
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 52: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 53: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 54: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = new IdentifierExpr(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 55: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 56: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + "." + ValueStack[ValueStack.Depth-1].s; }
        break;
      case 57: // exprReference -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 58: // cwordSeq -> cwordSeq, T_COMMA, cword
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 59: // cwordSeq -> cword
{ CurrentSemanticValue.stringList = new List<string>(); CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 60: // cword -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 61: // cword -> T_CWORD
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
