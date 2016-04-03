// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 03-04-2016 21:11:25
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 03-04-2016 21:11:19>

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
  public List<Expression> exprList;
  public KeyValuePair<string,string> template;
  public RuleModifier ruleModifier;
  public List<RuleModifier> ruleModifierList;
  public Knowledge.Rule rule;
  public List<Knowledge.Rule> ruleList;
  public List<string> stringList;
  public List<List<string>> patternList;
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
  private static Rule[] rules = new Rule[64];
  private static State[] states = new State[94];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "statementSeq", "statement", "configuration", "rule", 
      "conceptPatternSeq", "cwordSeq", "ruleSeq", "input", "ruleModifierSeq", 
      "outputSeq", "inputPatternSeq", "inputPattern", "ruleModifier", "condition", 
      "weight", "schedule", "expr", "output", "outputPattern", "call", "answer", 
      "Anon@1", "Anon@2", "exprReference", "exprSeq", "exprSeq2", "exprBinary", 
      "exprUnary", "exprIdentifier", "cword", };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{5,6,10,17,11,19,27,69,3,-2},new int[]{-4,4,-5,5,-6,22,-10,23});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{20,7});
    states[7] = new State(new int[]{25,8});
    states[8] = new State(new int[]{20,13,21,14},new int[]{-7,9,-8,16,-32,15});
    states[9] = new State(new int[]{13,10,5,-7,10,-7,11,-7,27,-7,3,-7});
    states[10] = new State(new int[]{20,13,21,14},new int[]{-8,11,-32,15});
    states[11] = new State(new int[]{20,13,21,14,13,-10,5,-10,10,-10,11,-10,27,-10,3,-10},new int[]{-32,12});
    states[12] = new State(-60);
    states[13] = new State(-62);
    states[14] = new State(-63);
    states[15] = new State(-61);
    states[16] = new State(new int[]{20,13,21,14,13,-11,5,-11,10,-11,11,-11,27,-11,3,-11},new int[]{-32,12});
    states[17] = new State(new int[]{22,18});
    states[18] = new State(-8);
    states[19] = new State(new int[]{20,20});
    states[20] = new State(new int[]{22,21});
    states[21] = new State(-9);
    states[22] = new State(-6);
    states[23] = new State(-27,new int[]{-11,24});
    states[24] = new State(new int[]{18,86,6,89,8,92,4,-35,16,-35,7,-35,9,-35,5,-35,10,-35,11,-35,27,-35,3,-35,17,-35},new int[]{-12,25,-15,84,-16,85,-17,88,-18,91});
    states[25] = new State(new int[]{4,28,16,31,7,38,9,64,5,-14,10,-14,11,-14,27,-14,3,-14,17,-14},new int[]{-20,26,-21,27,-22,37,-23,63});
    states[26] = new State(-34);
    states[27] = new State(-36);
    states[28] = new State(-39,new int[]{-24,29});
    states[29] = new State(new int[]{19,30});
    states[30] = new State(-40);
    states[31] = new State(new int[]{20,32});
    states[32] = new State(new int[]{17,33});
    states[33] = new State(new int[]{4,34});
    states[34] = new State(-41,new int[]{-25,35});
    states[35] = new State(new int[]{19,36});
    states[36] = new State(-42);
    states[37] = new State(-37);
    states[38] = new State(new int[]{20,57},new int[]{-26,39});
    states[39] = new State(new int[]{14,40,32,55});
    states[40] = new State(new int[]{14,50,20,57,33,58,22,60,23,61,15,-46},new int[]{-27,41,-28,43,-19,62,-29,48,-30,49,-31,53,-26,54});
    states[41] = new State(new int[]{15,42});
    states[42] = new State(-43);
    states[43] = new State(new int[]{13,44,15,-45});
    states[44] = new State(new int[]{14,50,20,57,33,58,22,60,23,61},new int[]{-19,45,-29,48,-30,49,-31,53,-26,54});
    states[45] = new State(new int[]{25,46,13,-47,15,-47});
    states[46] = new State(new int[]{14,50,20,57,33,58,22,60,23,61},new int[]{-19,47,-29,48,-30,49,-31,53,-26,54});
    states[47] = new State(-51);
    states[48] = new State(-49);
    states[49] = new State(-50);
    states[50] = new State(new int[]{14,50,20,57,33,58,22,60,23,61},new int[]{-19,51,-29,48,-30,49,-31,53,-26,54});
    states[51] = new State(new int[]{15,52,25,46});
    states[52] = new State(-52);
    states[53] = new State(-53);
    states[54] = new State(new int[]{32,55,25,-56,13,-56,15,-56,18,-56,6,-56,8,-56,4,-56,16,-56,7,-56,9,-56,5,-56,10,-56,11,-56,27,-56,3,-56,17,-56});
    states[55] = new State(new int[]{20,56});
    states[56] = new State(-58);
    states[57] = new State(-59);
    states[58] = new State(new int[]{23,59});
    states[59] = new State(-57);
    states[60] = new State(-54);
    states[61] = new State(-55);
    states[62] = new State(new int[]{25,46,13,-48,15,-48});
    states[63] = new State(-38);
    states[64] = new State(new int[]{16,65});
    states[65] = new State(-13,new int[]{-9,66});
    states[66] = new State(new int[]{17,67,27,69},new int[]{-6,68,-10,23});
    states[67] = new State(-44);
    states[68] = new State(-12);
    states[69] = new State(-17,new int[]{-13,70});
    states[70] = new State(new int[]{14,75,20,78,21,79,29,80,28,81,31,82,18,-15,6,-15,8,-15,4,-15,16,-15,7,-15,9,-15,5,-15,10,-15,11,-15,27,-15,3,-15,17,-15},new int[]{-14,71});
    states[71] = new State(new int[]{30,72,24,74,14,-16,20,-16,21,-16,29,-16,28,-16,31,-16,18,-16,6,-16,8,-16,4,-16,16,-16,7,-16,9,-16,5,-16,10,-16,11,-16,27,-16,3,-16,17,-16,15,-16});
    states[72] = new State(new int[]{14,75,20,78,21,79,29,80,28,81,31,82},new int[]{-14,73});
    states[73] = new State(-18);
    states[74] = new State(-19);
    states[75] = new State(-17,new int[]{-13,76});
    states[76] = new State(new int[]{15,77,14,75,20,78,21,79,29,80,28,81,31,82},new int[]{-14,71});
    states[77] = new State(-20);
    states[78] = new State(-21);
    states[79] = new State(-22);
    states[80] = new State(-23);
    states[81] = new State(-24);
    states[82] = new State(new int[]{14,75,20,78,21,79,29,80,28,81,31,82},new int[]{-14,83});
    states[83] = new State(-25);
    states[84] = new State(-26);
    states[85] = new State(-28);
    states[86] = new State(new int[]{14,50,20,57,33,58,22,60,23,61},new int[]{-19,87,-29,48,-30,49,-31,53,-26,54});
    states[87] = new State(new int[]{25,46,18,-31,6,-31,8,-31,4,-31,16,-31,7,-31,9,-31,5,-31,10,-31,11,-31,27,-31,3,-31,17,-31});
    states[88] = new State(-29);
    states[89] = new State(new int[]{23,90});
    states[90] = new State(-32);
    states[91] = new State(-30);
    states[92] = new State(new int[]{23,93});
    states[93] = new State(-33);

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
    rules[10] = new Rule(-7, new int[]{-7,13,-8});
    rules[11] = new Rule(-7, new int[]{-8});
    rules[12] = new Rule(-9, new int[]{-9,-6});
    rules[13] = new Rule(-9, new int[]{});
    rules[14] = new Rule(-6, new int[]{-10,-11,-12});
    rules[15] = new Rule(-10, new int[]{27,-13});
    rules[16] = new Rule(-13, new int[]{-13,-14});
    rules[17] = new Rule(-13, new int[]{});
    rules[18] = new Rule(-14, new int[]{-14,30,-14});
    rules[19] = new Rule(-14, new int[]{-14,24});
    rules[20] = new Rule(-14, new int[]{14,-13,15});
    rules[21] = new Rule(-14, new int[]{20});
    rules[22] = new Rule(-14, new int[]{21});
    rules[23] = new Rule(-14, new int[]{29});
    rules[24] = new Rule(-14, new int[]{28});
    rules[25] = new Rule(-14, new int[]{31,-14});
    rules[26] = new Rule(-11, new int[]{-11,-15});
    rules[27] = new Rule(-11, new int[]{});
    rules[28] = new Rule(-15, new int[]{-16});
    rules[29] = new Rule(-15, new int[]{-17});
    rules[30] = new Rule(-15, new int[]{-18});
    rules[31] = new Rule(-16, new int[]{18,-19});
    rules[32] = new Rule(-17, new int[]{6,23});
    rules[33] = new Rule(-18, new int[]{8,23});
    rules[34] = new Rule(-12, new int[]{-12,-20});
    rules[35] = new Rule(-12, new int[]{});
    rules[36] = new Rule(-20, new int[]{-21});
    rules[37] = new Rule(-20, new int[]{-22});
    rules[38] = new Rule(-20, new int[]{-23});
    rules[39] = new Rule(-24, new int[]{});
    rules[40] = new Rule(-21, new int[]{4,-24,19});
    rules[41] = new Rule(-25, new int[]{});
    rules[42] = new Rule(-21, new int[]{16,20,17,4,-25,19});
    rules[43] = new Rule(-22, new int[]{7,-26,14,-27,15});
    rules[44] = new Rule(-23, new int[]{9,16,-9,17});
    rules[45] = new Rule(-27, new int[]{-28});
    rules[46] = new Rule(-27, new int[]{});
    rules[47] = new Rule(-28, new int[]{-28,13,-19});
    rules[48] = new Rule(-28, new int[]{-19});
    rules[49] = new Rule(-19, new int[]{-29});
    rules[50] = new Rule(-19, new int[]{-30});
    rules[51] = new Rule(-29, new int[]{-19,25,-19});
    rules[52] = new Rule(-30, new int[]{14,-19,15});
    rules[53] = new Rule(-30, new int[]{-31});
    rules[54] = new Rule(-30, new int[]{22});
    rules[55] = new Rule(-30, new int[]{23});
    rules[56] = new Rule(-31, new int[]{-26});
    rules[57] = new Rule(-31, new int[]{33,23});
    rules[58] = new Rule(-26, new int[]{-26,32,20});
    rules[59] = new Rule(-26, new int[]{20});
    rules[60] = new Rule(-8, new int[]{-8,-32});
    rules[61] = new Rule(-8, new int[]{-32});
    rules[62] = new Rule(-32, new int[]{20});
    rules[63] = new Rule(-32, new int[]{21});
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
      case 7: // configuration -> T_CONCEPT, T_WORD, T_EQU, conceptPatternSeq
{ RegisterConcept(ValueStack[ValueStack.Depth-3].s, ValueStack[ValueStack.Depth-1].patternList); }
        break;
      case 8: // configuration -> T_RDF_IMPORT, T_STRING
{ RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 9: // configuration -> T_RDF_PREFIX, T_WORD, T_STRING
{ RDFPrefix(ValueStack[ValueStack.Depth-2].s, ((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 10: // conceptPatternSeq -> conceptPatternSeq, T_COMMA, cwordSeq
{ ValueStack[ValueStack.Depth-3].patternList.Add(ValueStack[ValueStack.Depth-1].stringList); CurrentSemanticValue.patternList = ValueStack[ValueStack.Depth-3].patternList; }
        break;
      case 11: // conceptPatternSeq -> cwordSeq
{ CurrentSemanticValue.patternList = new List<List<string>>(); CurrentSemanticValue.patternList.Add(ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 12: // ruleSeq -> ruleSeq, rule
{ ValueStack[ValueStack.Depth-2].ruleList.Add(ValueStack[ValueStack.Depth-1].rule); CurrentSemanticValue.ruleList = ValueStack[ValueStack.Depth-2].ruleList; }
        break;
      case 13: // ruleSeq -> /* empty */
{ CurrentSemanticValue.ruleList = new List<Knowledge.Rule>(); }
        break;
      case 14: // rule -> input, ruleModifierSeq, outputSeq
{ 
      CurrentSemanticValue.rule = AddRule(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-2].ruleModifierList, ValueStack[ValueStack.Depth-1].outputList);
    }
        break;
      case 15: // input -> T_GT, inputPatternSeq
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 16: // inputPatternSeq -> inputPatternSeq, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 17: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = null; }
        break;
      case 18: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 19: // inputPattern -> inputPattern, T_QUESTION
{ CurrentSemanticValue.regex =  new RepetitionWRegex(ValueStack[ValueStack.Depth-2].regex, 0, 1); }
        break;
      case 20: // inputPattern -> T_LPAR, inputPatternSeq, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 21: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new WordWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 22: // inputPattern -> T_CWORD
{ CurrentSemanticValue.regex = new ConceptWRegex(KnowledgeBase, ValueStack[ValueStack.Depth-1].s); }
        break;
      case 23: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new RepetitionWRegex(new WildcardWRegex()); }
        break;
      case 24: // inputPattern -> T_PLUS
{ CurrentSemanticValue.regex =  new RepetitionWRegex(new WildcardWRegex(), 1, 9999); }
        break;
      case 25: // inputPattern -> T_EXCL, inputPattern
{ CurrentSemanticValue.regex =  new NegationWRegex(ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 26: // ruleModifierSeq -> ruleModifierSeq, ruleModifier
{ CurrentSemanticValue.ruleModifierList.Add(ValueStack[ValueStack.Depth-1].ruleModifier); }
        break;
      case 27: // ruleModifierSeq -> /* empty */
{ CurrentSemanticValue.ruleModifierList = new List<RuleModifier>(); }
        break;
      case 28: // ruleModifier -> condition
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 29: // ruleModifier -> weight
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 30: // ruleModifier -> schedule
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 31: // condition -> T_AMP, expr
{ CurrentSemanticValue.ruleModifier = new ConditionRuleModifier(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 32: // weight -> T_WEIGHT, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new WeightRuleModifier(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 33: // schedule -> T_EVERY, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new ScheduleRuleModifier((int)ValueStack[ValueStack.Depth-1].n); }
        break;
      case 34: // outputSeq -> outputSeq, output
{ ValueStack[ValueStack.Depth-2].outputList.Add(ValueStack[ValueStack.Depth-1].output); CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 35: // outputSeq -> /* empty */
{ CurrentSemanticValue.outputList = new List<OutputStatement>(); }
        break;
      case 36: // output -> outputPattern
{ CurrentSemanticValue.output = new TemplateOutputStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 37: // output -> call
{ CurrentSemanticValue.output = new CallOutputStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 38: // output -> answer
{ CurrentSemanticValue.output = ValueStack[ValueStack.Depth-1].output;}
        break;
      case 39: // Anon@1 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 40: // outputPattern -> T_COLON, Anon@1, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>("default", ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 41: // Anon@2 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 42: // outputPattern -> T_LBRACE, T_WORD, T_RBRACE, T_COLON, Anon@2, T_OUTPUT
{ CurrentSemanticValue.template = new KeyValuePair<string,string>(ValueStack[ValueStack.Depth-5].s, ((ConfigScanner)Scanner).StringInput.ToString().Trim()); }
        break;
      case 43: // call -> T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].exprList); }
        break;
      case 44: // answer -> T_ANSWER, T_LBRACE, ruleSeq, T_RBRACE
{ CurrentSemanticValue.output = new AnswerOutputStatement(KnowledgeBase, ValueStack[ValueStack.Depth-2].ruleList); }
        break;
      case 45: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 46: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 47: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 48: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 49: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 50: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 51: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 52: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-2]; }
        break;
      case 53: // exprUnary -> exprIdentifier
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 54: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 55: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 56: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = new IdentifierExpr(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 57: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 58: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-3].s + "." + ValueStack[ValueStack.Depth-1].s; }
        break;
      case 59: // exprReference -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 60: // cwordSeq -> cwordSeq, cword
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 61: // cwordSeq -> cword
{ CurrentSemanticValue.stringList = new List<string>(new string[] { ValueStack[ValueStack.Depth-1].s }); }
        break;
      case 62: // cword -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 63: // cword -> T_CWORD
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
