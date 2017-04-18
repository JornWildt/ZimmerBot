// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  JORN-PC
// DateTime: 18-04-2017 16:39:59
// UserName: Jorn
// Input file <ConfigParser\Config.Language.grammar.y - 18-04-2017 16:39:53>

// options: conflicts no-lines gplex conflicts

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using System.Linq;
using ZimmerBot.Core.WordRegex;
using ZimmerBot.Core.Expressions;
using ZimmerBot.Core.Statements;

namespace ZimmerBot.Core.ConfigParser
{
internal enum Token {error=2,EOF=3,T_COLON=4,T_CONCEPT=5,T_CALL=6,
    T_SET=7,T_WEIGHT=8,T_EVERY=9,T_ANSWER=10,T_TOPIC=11,T_STARTTOPIC=12,
    T_REPEATABLE=13,T_NOTREPEATABLE=14,T_ENTITIES=15,T_RDF_IMPORT=16,T_RDF_PREFIX=17,T_RDF_ENTITIES=18,
    T_WHEN=19,T_CONTINUE=20,T_CONTINUE_AT=21,T_CONTINUE_WITH=22,T_ON=23,T_AT=24,
    T_STOPOUTPUT=25,T_TOPICRULE=26,T_IMPLIES=27,T_COMMA=28,T_LPAR=29,T_RPAR=30,
    T_LBRACE=31,T_RBRACE=32,T_AMP=33,T_OUTPUT=34,T_WORD=35,T_CWORD=36,
    T_STRING=37,T_NUMBER=38,T_QUESTION=39,T_EQU=40,T_LT=41,T_GT=42,
    T_PLUS=43,T_STAR=44,T_PIPE=45,T_EXCL=46,T_DOT=47,T_DOLLAR=48};

internal partial struct ValueType
{ 
  public Statement statement;
  public List<Statement> statementList;
  public WRegexBase regex;
  public List<WRegexBase> regexList;
  public Expression expr;
  public List<Expression> exprList;
  public OutputTemplate template;
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
  private static Rule[] rules = new Rule[102];
  private static State[] states = new State[170];
  private static string[] nonTerms = new string[] {
      "main", "$accept", "itemSeq", "item", "configuration", "rule", "conceptPatternSeq", 
      "wordCommaSeq", "Anon@1", "ruleSeq", "Anon@2", "statementSeq", "stringSeq", 
      "cwordSeq", "ruleLabel", "inputSeq", "ruleModifierSeq", "topicOutput", 
      "topicStatementSeq", "input", "inputPatternSeq", "inputPattern", "ruleModifier", 
      "condition", "weight", "schedule", "expr", "statement", "internalStatement", 
      "outputTemplateSequence", "stmtCall", "stmtSet", "stmtAnswer", "stmtContinue", 
      "stmtStopOutput", "outputTemplate", "outputTemplateSequence2", "outputTemplateContent", 
      "Anon@3", "exprReference", "exprSeq", "wordSeq", "exprSeq2", "exprBinary", 
      "exprUnary", "exprIdentifier", "cword", };

  static ConfigParser() {
    states[0] = new State(-4,new int[]{-1,1,-3,3});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(new int[]{5,6,11,17,23,143,15,150,16,160,17,162,18,165,41,86,3,-2,26,-24,42,-24},new int[]{-4,4,-5,5,-6,169,-15,27});
    states[4] = new State(-3);
    states[5] = new State(-5);
    states[6] = new State(new int[]{35,7});
    states[7] = new State(new int[]{40,8});
    states[8] = new State(new int[]{35,13,36,14},new int[]{-7,9,-14,16,-47,15});
    states[9] = new State(new int[]{28,10,5,-7,11,-7,23,-7,15,-7,16,-7,17,-7,18,-7,41,-7,26,-7,42,-7,3,-7});
    states[10] = new State(new int[]{35,13,36,14},new int[]{-14,11,-47,15});
    states[11] = new State(new int[]{35,13,36,14,28,-17,5,-17,11,-17,23,-17,15,-17,16,-17,17,-17,18,-17,41,-17,26,-17,42,-17,3,-17},new int[]{-47,12});
    states[12] = new State(-96);
    states[13] = new State(-98);
    states[14] = new State(-99);
    states[15] = new State(-97);
    states[16] = new State(new int[]{35,13,36,14,28,-18,5,-18,11,-18,23,-18,15,-18,16,-18,17,-18,18,-18,41,-18,26,-18,42,-18,3,-18},new int[]{-47,12});
    states[17] = new State(new int[]{35,18});
    states[18] = new State(new int[]{29,19,31,-10},new int[]{-11,139});
    states[19] = new State(new int[]{35,138},new int[]{-8,20});
    states[20] = new State(new int[]{30,21,28,136});
    states[21] = new State(-8,new int[]{-9,22});
    states[22] = new State(new int[]{31,23});
    states[23] = new State(-20,new int[]{-10,24});
    states[24] = new State(new int[]{32,25,41,86,26,-24,42,-24},new int[]{-6,26,-15,27});
    states[25] = new State(-9);
    states[26] = new State(-19);
    states[27] = new State(new int[]{26,129,42,116},new int[]{-16,28,-20,135});
    states[28] = new State(new int[]{42,116,4,-38,31,-38,6,-38,7,-38,10,-38,20,-38,21,-38,22,-38,25,-38,12,-38,13,-38,14,-38,19,-38,8,-38,9,-38},new int[]{-17,29,-20,115});
    states[29] = new State(new int[]{4,37,31,41,6,48,7,77,10,82,20,91,21,92,22,94,25,99,12,100,13,102,14,103,19,107,8,110,9,113},new int[]{-12,30,-23,104,-28,105,-30,32,-36,33,-29,46,-31,47,-32,76,-33,81,-34,90,-35,98,-24,106,-25,109,-26,112});
    states[30] = new State(new int[]{4,37,31,41,6,48,7,77,10,82,20,91,21,92,22,94,25,99,12,100,13,102,14,103,5,-21,11,-21,23,-21,15,-21,16,-21,17,-21,18,-21,41,-21,26,-21,42,-21,3,-21,32,-21},new int[]{-28,31,-30,32,-36,33,-29,46,-31,47,-32,76,-33,81,-34,90,-35,98});
    states[31] = new State(-45);
    states[32] = new State(-49);
    states[33] = new State(-62,new int[]{-37,34});
    states[34] = new State(new int[]{43,35,4,-59,31,-59,6,-59,7,-59,10,-59,20,-59,21,-59,22,-59,25,-59,12,-59,13,-59,14,-59,5,-59,11,-59,23,-59,15,-59,16,-59,17,-59,18,-59,41,-59,26,-59,42,-59,3,-59,32,-59});
    states[35] = new State(new int[]{4,37},new int[]{-36,36});
    states[36] = new State(-61);
    states[37] = new State(-64,new int[]{-38,38,-39,39});
    states[38] = new State(-63);
    states[39] = new State(new int[]{34,40});
    states[40] = new State(-65);
    states[41] = new State(new int[]{35,42});
    states[42] = new State(new int[]{32,43});
    states[43] = new State(new int[]{4,37},new int[]{-36,44});
    states[44] = new State(-62,new int[]{-37,45});
    states[45] = new State(new int[]{43,35,4,-60,31,-60,6,-60,7,-60,10,-60,20,-60,21,-60,22,-60,25,-60,12,-60,13,-60,14,-60,5,-60,11,-60,23,-60,15,-60,16,-60,17,-60,18,-60,41,-60,26,-60,42,-60,3,-60,32,-60});
    states[46] = new State(-50);
    states[47] = new State(-51);
    states[48] = new State(new int[]{35,69},new int[]{-40,49});
    states[49] = new State(new int[]{29,50,47,67});
    states[50] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74,30,-76},new int[]{-41,51,-43,53,-27,75,-44,58,-45,59,-46,65,-40,66});
    states[51] = new State(new int[]{30,52});
    states[52] = new State(-67);
    states[53] = new State(new int[]{28,54,30,-75});
    states[54] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,55,-44,58,-45,59,-46,65,-40,66});
    states[55] = new State(new int[]{40,56,28,-77,30,-77});
    states[56] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,57,-44,58,-45,59,-46,65,-40,66});
    states[57] = new State(-81);
    states[58] = new State(-79);
    states[59] = new State(-80);
    states[60] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,61,-44,58,-45,59,-46,65,-40,66});
    states[61] = new State(new int[]{30,62,40,56});
    states[62] = new State(-82);
    states[63] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,64,-44,58,-45,59,-46,65,-40,66});
    states[64] = new State(-83);
    states[65] = new State(-84);
    states[66] = new State(new int[]{47,67,40,-87,28,-87,30,-87,4,-87,31,-87,6,-87,7,-87,10,-87,20,-87,21,-87,22,-87,25,-87,12,-87,13,-87,14,-87,5,-87,11,-87,23,-87,15,-87,16,-87,17,-87,18,-87,41,-87,26,-87,42,-87,3,-87,32,-87,19,-87,8,-87,9,-87});
    states[67] = new State(new int[]{35,68});
    states[68] = new State(-90);
    states[69] = new State(-91);
    states[70] = new State(new int[]{38,71,35,72});
    states[71] = new State(-88);
    states[72] = new State(-89);
    states[73] = new State(-85);
    states[74] = new State(-86);
    states[75] = new State(new int[]{40,56,28,-78,30,-78});
    states[76] = new State(-52);
    states[77] = new State(new int[]{35,69},new int[]{-40,78});
    states[78] = new State(new int[]{40,79,47,67});
    states[79] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,80,-44,58,-45,59,-46,65,-40,66});
    states[80] = new State(new int[]{40,56,4,-68,31,-68,6,-68,7,-68,10,-68,20,-68,21,-68,22,-68,25,-68,12,-68,13,-68,14,-68,5,-68,11,-68,23,-68,15,-68,16,-68,17,-68,18,-68,41,-68,26,-68,42,-68,3,-68,32,-68});
    states[81] = new State(-53);
    states[82] = new State(new int[]{31,83,24,88});
    states[83] = new State(-20,new int[]{-10,84});
    states[84] = new State(new int[]{32,85,41,86,26,-24,42,-24},new int[]{-6,26,-15,27});
    states[85] = new State(-69);
    states[86] = new State(new int[]{35,87});
    states[87] = new State(-23);
    states[88] = new State(new int[]{35,89});
    states[89] = new State(-70);
    states[90] = new State(-54);
    states[91] = new State(-71);
    states[92] = new State(new int[]{35,93});
    states[93] = new State(-72);
    states[94] = new State(new int[]{35,97},new int[]{-42,95});
    states[95] = new State(new int[]{35,96,4,-73,31,-73,6,-73,7,-73,10,-73,20,-73,21,-73,22,-73,25,-73,12,-73,13,-73,14,-73,5,-73,11,-73,23,-73,15,-73,16,-73,17,-73,18,-73,41,-73,26,-73,42,-73,3,-73,32,-73});
    states[96] = new State(-92);
    states[97] = new State(-93);
    states[98] = new State(-55);
    states[99] = new State(-74);
    states[100] = new State(new int[]{35,101});
    states[101] = new State(-56);
    states[102] = new State(-57);
    states[103] = new State(-58);
    states[104] = new State(-37);
    states[105] = new State(-46);
    states[106] = new State(-39);
    states[107] = new State(new int[]{29,60,46,63,35,69,48,70,37,73,38,74},new int[]{-27,108,-44,58,-45,59,-46,65,-40,66});
    states[108] = new State(new int[]{40,56,4,-42,31,-42,6,-42,7,-42,10,-42,20,-42,21,-42,22,-42,25,-42,12,-42,13,-42,14,-42,19,-42,8,-42,9,-42});
    states[109] = new State(-40);
    states[110] = new State(new int[]{38,111});
    states[111] = new State(-43);
    states[112] = new State(-41);
    states[113] = new State(new int[]{38,114});
    states[114] = new State(-44);
    states[115] = new State(-25);
    states[116] = new State(-29,new int[]{-21,117});
    states[117] = new State(new int[]{29,122,35,125,36,126,44,127,43,128,42,-27,4,-27,31,-27,6,-27,7,-27,10,-27,20,-27,21,-27,22,-27,25,-27,12,-27,13,-27,14,-27,19,-27,8,-27,9,-27},new int[]{-22,118});
    states[118] = new State(new int[]{45,119,39,121,29,-28,35,-28,36,-28,44,-28,43,-28,42,-28,4,-28,31,-28,6,-28,7,-28,10,-28,20,-28,21,-28,22,-28,25,-28,12,-28,13,-28,14,-28,19,-28,8,-28,9,-28,30,-28});
    states[119] = new State(new int[]{29,122,35,125,36,126,44,127,43,128},new int[]{-22,120});
    states[120] = new State(-30);
    states[121] = new State(-31);
    states[122] = new State(-29,new int[]{-21,123});
    states[123] = new State(new int[]{30,124,29,122,35,125,36,126,44,127,43,128},new int[]{-22,118});
    states[124] = new State(-32);
    states[125] = new State(-33);
    states[126] = new State(-34);
    states[127] = new State(-35);
    states[128] = new State(-36);
    states[129] = new State(-64,new int[]{-18,130,-38,133,-39,39});
    states[130] = new State(-48,new int[]{-19,131});
    states[131] = new State(new int[]{6,48,7,77,10,82,20,91,21,92,22,94,25,99,12,100,13,102,14,103,5,-22,11,-22,23,-22,15,-22,16,-22,17,-22,18,-22,41,-22,26,-22,42,-22,3,-22,32,-22},new int[]{-29,132,-31,47,-32,76,-33,81,-34,90,-35,98});
    states[132] = new State(-47);
    states[133] = new State(-62,new int[]{-37,134});
    states[134] = new State(new int[]{43,35,6,-66,7,-66,10,-66,20,-66,21,-66,22,-66,25,-66,12,-66,13,-66,14,-66,5,-66,11,-66,23,-66,15,-66,16,-66,17,-66,18,-66,41,-66,26,-66,42,-66,3,-66,32,-66});
    states[135] = new State(-26);
    states[136] = new State(new int[]{35,137});
    states[137] = new State(-94);
    states[138] = new State(-95);
    states[139] = new State(new int[]{31,140});
    states[140] = new State(-20,new int[]{-10,141});
    states[141] = new State(new int[]{32,142,41,86,26,-24,42,-24},new int[]{-6,26,-15,27});
    states[142] = new State(-11);
    states[143] = new State(new int[]{29,144});
    states[144] = new State(new int[]{35,145});
    states[145] = new State(new int[]{30,146});
    states[146] = new State(new int[]{31,147});
    states[147] = new State(new int[]{4,37,31,41,6,48,7,77,10,82,20,91,21,92,22,94,25,99,12,100,13,102,14,103},new int[]{-12,148,-28,105,-30,32,-36,33,-29,46,-31,47,-32,76,-33,81,-34,90,-35,98});
    states[148] = new State(new int[]{32,149,4,37,31,41,6,48,7,77,10,82,20,91,21,92,22,94,25,99,12,100,13,102,14,103},new int[]{-28,31,-30,32,-36,33,-29,46,-31,47,-32,76,-33,81,-34,90,-35,98});
    states[149] = new State(-12);
    states[150] = new State(new int[]{29,151});
    states[151] = new State(new int[]{35,152});
    states[152] = new State(new int[]{30,153});
    states[153] = new State(new int[]{31,154});
    states[154] = new State(new int[]{37,159},new int[]{-13,155});
    states[155] = new State(new int[]{32,156,28,157});
    states[156] = new State(-13);
    states[157] = new State(new int[]{37,158});
    states[158] = new State(-100);
    states[159] = new State(-101);
    states[160] = new State(new int[]{37,161});
    states[161] = new State(-14);
    states[162] = new State(new int[]{35,163});
    states[163] = new State(new int[]{37,164});
    states[164] = new State(-15);
    states[165] = new State(new int[]{29,166});
    states[166] = new State(new int[]{37,167});
    states[167] = new State(new int[]{30,168});
    states[168] = new State(-16);
    states[169] = new State(-6);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-2, new int[]{-1,3});
    rules[2] = new Rule(-1, new int[]{-3});
    rules[3] = new Rule(-3, new int[]{-3,-4});
    rules[4] = new Rule(-3, new int[]{});
    rules[5] = new Rule(-4, new int[]{-5});
    rules[6] = new Rule(-4, new int[]{-6});
    rules[7] = new Rule(-5, new int[]{5,35,40,-7});
    rules[8] = new Rule(-9, new int[]{});
    rules[9] = new Rule(-5, new int[]{11,35,29,-8,30,-9,31,-10,32});
    rules[10] = new Rule(-11, new int[]{});
    rules[11] = new Rule(-5, new int[]{11,35,-11,31,-10,32});
    rules[12] = new Rule(-5, new int[]{23,29,35,30,31,-12,32});
    rules[13] = new Rule(-5, new int[]{15,29,35,30,31,-13,32});
    rules[14] = new Rule(-5, new int[]{16,37});
    rules[15] = new Rule(-5, new int[]{17,35,37});
    rules[16] = new Rule(-5, new int[]{18,29,37,30});
    rules[17] = new Rule(-7, new int[]{-7,28,-14});
    rules[18] = new Rule(-7, new int[]{-14});
    rules[19] = new Rule(-10, new int[]{-10,-6});
    rules[20] = new Rule(-10, new int[]{});
    rules[21] = new Rule(-6, new int[]{-15,-16,-17,-12});
    rules[22] = new Rule(-6, new int[]{-15,26,-18,-19});
    rules[23] = new Rule(-15, new int[]{41,35});
    rules[24] = new Rule(-15, new int[]{});
    rules[25] = new Rule(-16, new int[]{-16,-20});
    rules[26] = new Rule(-16, new int[]{-20});
    rules[27] = new Rule(-20, new int[]{42,-21});
    rules[28] = new Rule(-21, new int[]{-21,-22});
    rules[29] = new Rule(-21, new int[]{});
    rules[30] = new Rule(-22, new int[]{-22,45,-22});
    rules[31] = new Rule(-22, new int[]{-22,39});
    rules[32] = new Rule(-22, new int[]{29,-21,30});
    rules[33] = new Rule(-22, new int[]{35});
    rules[34] = new Rule(-22, new int[]{36});
    rules[35] = new Rule(-22, new int[]{44});
    rules[36] = new Rule(-22, new int[]{43});
    rules[37] = new Rule(-17, new int[]{-17,-23});
    rules[38] = new Rule(-17, new int[]{});
    rules[39] = new Rule(-23, new int[]{-24});
    rules[40] = new Rule(-23, new int[]{-25});
    rules[41] = new Rule(-23, new int[]{-26});
    rules[42] = new Rule(-24, new int[]{19,-27});
    rules[43] = new Rule(-25, new int[]{8,38});
    rules[44] = new Rule(-26, new int[]{9,38});
    rules[45] = new Rule(-12, new int[]{-12,-28});
    rules[46] = new Rule(-12, new int[]{-28});
    rules[47] = new Rule(-19, new int[]{-19,-29});
    rules[48] = new Rule(-19, new int[]{});
    rules[49] = new Rule(-28, new int[]{-30});
    rules[50] = new Rule(-28, new int[]{-29});
    rules[51] = new Rule(-29, new int[]{-31});
    rules[52] = new Rule(-29, new int[]{-32});
    rules[53] = new Rule(-29, new int[]{-33});
    rules[54] = new Rule(-29, new int[]{-34});
    rules[55] = new Rule(-29, new int[]{-35});
    rules[56] = new Rule(-29, new int[]{12,35});
    rules[57] = new Rule(-29, new int[]{13});
    rules[58] = new Rule(-29, new int[]{14});
    rules[59] = new Rule(-30, new int[]{-36,-37});
    rules[60] = new Rule(-30, new int[]{31,35,32,-36,-37});
    rules[61] = new Rule(-37, new int[]{-37,43,-36});
    rules[62] = new Rule(-37, new int[]{});
    rules[63] = new Rule(-36, new int[]{4,-38});
    rules[64] = new Rule(-39, new int[]{});
    rules[65] = new Rule(-38, new int[]{-39,34});
    rules[66] = new Rule(-18, new int[]{-38,-37});
    rules[67] = new Rule(-31, new int[]{6,-40,29,-41,30});
    rules[68] = new Rule(-32, new int[]{7,-40,40,-27});
    rules[69] = new Rule(-33, new int[]{10,31,-10,32});
    rules[70] = new Rule(-33, new int[]{10,24,35});
    rules[71] = new Rule(-34, new int[]{20});
    rules[72] = new Rule(-34, new int[]{21,35});
    rules[73] = new Rule(-34, new int[]{22,-42});
    rules[74] = new Rule(-35, new int[]{25});
    rules[75] = new Rule(-41, new int[]{-43});
    rules[76] = new Rule(-41, new int[]{});
    rules[77] = new Rule(-43, new int[]{-43,28,-27});
    rules[78] = new Rule(-43, new int[]{-27});
    rules[79] = new Rule(-27, new int[]{-44});
    rules[80] = new Rule(-27, new int[]{-45});
    rules[81] = new Rule(-44, new int[]{-27,40,-27});
    rules[82] = new Rule(-45, new int[]{29,-27,30});
    rules[83] = new Rule(-45, new int[]{46,-27});
    rules[84] = new Rule(-45, new int[]{-46});
    rules[85] = new Rule(-45, new int[]{37});
    rules[86] = new Rule(-45, new int[]{38});
    rules[87] = new Rule(-46, new int[]{-40});
    rules[88] = new Rule(-46, new int[]{48,38});
    rules[89] = new Rule(-46, new int[]{48,35});
    rules[90] = new Rule(-40, new int[]{-40,47,35});
    rules[91] = new Rule(-40, new int[]{35});
    rules[92] = new Rule(-42, new int[]{-42,35});
    rules[93] = new Rule(-42, new int[]{35});
    rules[94] = new Rule(-8, new int[]{-8,28,35});
    rules[95] = new Rule(-8, new int[]{35});
    rules[96] = new Rule(-14, new int[]{-14,-47});
    rules[97] = new Rule(-14, new int[]{-47});
    rules[98] = new Rule(-47, new int[]{35});
    rules[99] = new Rule(-47, new int[]{36});
    rules[100] = new Rule(-13, new int[]{-13,28,37});
    rules[101] = new Rule(-13, new int[]{37});
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
      case 8: // Anon@1 -> /* empty */
{ StartTopic(ValueStack[ValueStack.Depth-4].s); }
        break;
      case 9: // configuration -> T_TOPIC, T_WORD, T_LPAR, wordCommaSeq, T_RPAR, Anon@1, 
              //                  T_LBRACE, ruleSeq, T_RBRACE
{ FinalizeTopic(ValueStack[ValueStack.Depth-8].s); }
        break;
      case 10: // Anon@2 -> /* empty */
{ StartTopic(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 11: // configuration -> T_TOPIC, T_WORD, Anon@2, T_LBRACE, ruleSeq, T_RBRACE
{ FinalizeTopic(ValueStack[ValueStack.Depth-5].s); }
        break;
      case 12: // configuration -> T_ON, T_LPAR, T_WORD, T_RPAR, T_LBRACE, statementSeq, T_RBRACE
{ RegisterEventHandler(ValueStack[ValueStack.Depth-5].s, ValueStack[ValueStack.Depth-2].statementList); }
        break;
      case 13: // configuration -> T_ENTITIES, T_LPAR, T_WORD, T_RPAR, T_LBRACE, stringSeq, 
               //                  T_RBRACE
{ RegisterEntities(ValueStack[ValueStack.Depth-5].s, ValueStack[ValueStack.Depth-2].stringList); }
        break;
      case 14: // configuration -> T_RDF_IMPORT, T_STRING
{ RDFImport(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 15: // configuration -> T_RDF_PREFIX, T_WORD, T_STRING
{ RDFPrefix(ValueStack[ValueStack.Depth-2].s, ((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 16: // configuration -> T_RDF_ENTITIES, T_LPAR, T_STRING, T_RPAR
{ RDFEntities(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 17: // conceptPatternSeq -> conceptPatternSeq, T_COMMA, cwordSeq
{ ValueStack[ValueStack.Depth-3].patternList.Add(ValueStack[ValueStack.Depth-1].stringList); CurrentSemanticValue.patternList = ValueStack[ValueStack.Depth-3].patternList; }
        break;
      case 18: // conceptPatternSeq -> cwordSeq
{ CurrentSemanticValue.patternList = new List<List<string>>(); CurrentSemanticValue.patternList.Add(ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 19: // ruleSeq -> ruleSeq, rule
{ ValueStack[ValueStack.Depth-2].ruleList.Add(ValueStack[ValueStack.Depth-1].rule); CurrentSemanticValue.ruleList = ValueStack[ValueStack.Depth-2].ruleList; }
        break;
      case 20: // ruleSeq -> /* empty */
{ CurrentSemanticValue.ruleList = new List<Knowledge.Rule>(); }
        break;
      case 21: // rule -> ruleLabel, inputSeq, ruleModifierSeq, statementSeq
{ 
      CurrentSemanticValue.rule = AddRule(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-3].regexList, ValueStack[ValueStack.Depth-2].ruleModifierList, ValueStack[ValueStack.Depth-1].statementList);
    }
        break;
      case 22: // rule -> ruleLabel, T_TOPICRULE, topicOutput, topicStatementSeq
{
      CurrentSemanticValue.rule = AddTopicRule(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].template, ValueStack[ValueStack.Depth-1].statementList);
    }
        break;
      case 23: // ruleLabel -> T_LT, T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 25: // inputSeq -> inputSeq, input
{ CurrentSemanticValue.regexList = ValueStack[ValueStack.Depth-2].regexList; ValueStack[ValueStack.Depth-2].regexList.Add(ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 26: // inputSeq -> input
{ CurrentSemanticValue.regexList = new List<WRegexBase>() { ValueStack[ValueStack.Depth-1].regex }; }
        break;
      case 27: // input -> T_GT, inputPatternSeq
{ CurrentSemanticValue.regex = ValueStack[ValueStack.Depth-1].regex; }
        break;
      case 28: // inputPatternSeq -> inputPatternSeq, inputPattern
{ CurrentSemanticValue.regex = CombineSequence(ValueStack[ValueStack.Depth-2].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 29: // inputPatternSeq -> /* empty */
{ CurrentSemanticValue.regex = null; }
        break;
      case 30: // inputPattern -> inputPattern, T_PIPE, inputPattern
{ CurrentSemanticValue.regex = new ChoiceWRegex(ValueStack[ValueStack.Depth-3].regex, ValueStack[ValueStack.Depth-1].regex); }
        break;
      case 31: // inputPattern -> inputPattern, T_QUESTION
{ CurrentSemanticValue.regex =  new GroupWRegex(new RepetitionWRegex(ValueStack[ValueStack.Depth-2].regex, 0, 1)); }
        break;
      case 32: // inputPattern -> T_LPAR, inputPatternSeq, T_RPAR
{ CurrentSemanticValue.regex = new GroupWRegex(ValueStack[ValueStack.Depth-2].regex); }
        break;
      case 33: // inputPattern -> T_WORD
{ CurrentSemanticValue.regex = new LiteralWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 34: // inputPattern -> T_CWORD
{ CurrentSemanticValue.regex = BuildConceptWRegex(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 35: // inputPattern -> T_STAR
{ CurrentSemanticValue.regex = new GroupWRegex(new RepetitionWRegex(new WildcardWRegex())); }
        break;
      case 36: // inputPattern -> T_PLUS
{ CurrentSemanticValue.regex =  new GroupWRegex(new RepetitionWRegex(new WildcardWRegex(), 1, 9999)); }
        break;
      case 37: // ruleModifierSeq -> ruleModifierSeq, ruleModifier
{ CurrentSemanticValue.ruleModifierList.Add(ValueStack[ValueStack.Depth-1].ruleModifier); }
        break;
      case 38: // ruleModifierSeq -> /* empty */
{ CurrentSemanticValue.ruleModifierList = new List<RuleModifier>(); }
        break;
      case 39: // ruleModifier -> condition
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 40: // ruleModifier -> weight
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 41: // ruleModifier -> schedule
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 42: // condition -> T_WHEN, expr
{ CurrentSemanticValue.ruleModifier = new ConditionRuleModifier(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 43: // weight -> T_WEIGHT, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new WeightRuleModifier(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 44: // schedule -> T_EVERY, T_NUMBER
{ CurrentSemanticValue.ruleModifier = new ScheduleRuleModifier((int)ValueStack[ValueStack.Depth-1].n); }
        break;
      case 45: // statementSeq -> statementSeq, statement
{ ValueStack[ValueStack.Depth-2].statementList.Add(ValueStack[ValueStack.Depth-1].statement); CurrentSemanticValue.statementList = ValueStack[ValueStack.Depth-2].statementList; }
        break;
      case 46: // statementSeq -> statement
{ CurrentSemanticValue.statementList = new List<Statement>() { ValueStack[ValueStack.Depth-1].statement }; }
        break;
      case 47: // topicStatementSeq -> topicStatementSeq, internalStatement
{ ValueStack[ValueStack.Depth-2].statementList.Add(ValueStack[ValueStack.Depth-1].statement); CurrentSemanticValue.statementList = ValueStack[ValueStack.Depth-2].statementList; }
        break;
      case 48: // topicStatementSeq -> /* empty */
{ CurrentSemanticValue.statementList = new List<Statement>(); }
        break;
      case 49: // statement -> outputTemplateSequence
{ CurrentSemanticValue.statement = new OutputTemplateStatement(ValueStack[ValueStack.Depth-1].template); }
        break;
      case 50: // statement -> internalStatement
{ CurrentSemanticValue.statement = ValueStack[ValueStack.Depth-1].statement; }
        break;
      case 51: // internalStatement -> stmtCall
{ CurrentSemanticValue.statement = new CallStatment(ValueStack[ValueStack.Depth-1].expr as FunctionCallExpr); }
        break;
      case 52: // internalStatement -> stmtSet
{ CurrentSemanticValue.statement = ValueStack[ValueStack.Depth-1].statement; }
        break;
      case 53: // internalStatement -> stmtAnswer
{ CurrentSemanticValue.statement = ValueStack[ValueStack.Depth-1].statement; }
        break;
      case 54: // internalStatement -> stmtContinue
{ CurrentSemanticValue.statement = ValueStack[ValueStack.Depth-1].statement; }
        break;
      case 55: // internalStatement -> stmtStopOutput
{ CurrentSemanticValue.statement = ValueStack[ValueStack.Depth-1].statement; }
        break;
      case 56: // internalStatement -> T_STARTTOPIC, T_WORD
{ CurrentSemanticValue.statement = new StartTopicStatement(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 57: // internalStatement -> T_REPEATABLE
{ CurrentSemanticValue.statement = new RepeatableStatement(true); }
        break;
      case 58: // internalStatement -> T_NOTREPEATABLE
{ CurrentSemanticValue.statement = new RepeatableStatement(false); }
        break;
      case 59: // outputTemplateSequence -> outputTemplate, outputTemplateSequence2
{  CurrentSemanticValue.template = new OutputTemplate("default", ValueStack[ValueStack.Depth-2].s, ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 60: // outputTemplateSequence -> T_LBRACE, T_WORD, T_RBRACE, outputTemplate, 
               //                           outputTemplateSequence2
{  CurrentSemanticValue.template = new OutputTemplate(ValueStack[ValueStack.Depth-4].s, ValueStack[ValueStack.Depth-2].s, ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 61: // outputTemplateSequence2 -> outputTemplateSequence2, T_PLUS, outputTemplate
{ CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 62: // outputTemplateSequence2 -> /* empty */
{ CurrentSemanticValue.stringList = new List<string>(); }
        break;
      case 63: // outputTemplate -> T_COLON, outputTemplateContent
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 64: // Anon@3 -> /* empty */
{ ((ConfigScanner)Scanner).StringInput = new StringBuilder(); ((ConfigScanner)Scanner).BEGIN(2); }
        break;
      case 65: // outputTemplateContent -> Anon@3, T_OUTPUT
{ CurrentSemanticValue.s = ((ConfigScanner)Scanner).StringInput.ToString().Trim(); }
        break;
      case 66: // topicOutput -> outputTemplateContent, outputTemplateSequence2
{  CurrentSemanticValue.template = new OutputTemplate("default", ValueStack[ValueStack.Depth-2].s, ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 67: // stmtCall -> T_CALL, exprReference, T_LPAR, exprSeq, T_RPAR
{ CurrentSemanticValue.expr = new FunctionCallExpr(ValueStack[ValueStack.Depth-4].expr, ValueStack[ValueStack.Depth-2].exprList); }
        break;
      case 68: // stmtSet -> T_SET, exprReference, T_EQU, expr
{ CurrentSemanticValue.statement = new SetStatement(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 69: // stmtAnswer -> T_ANSWER, T_LBRACE, ruleSeq, T_RBRACE
{ CurrentSemanticValue.statement = new AnswerStatement(ValueStack[ValueStack.Depth-2].ruleList); }
        break;
      case 70: // stmtAnswer -> T_ANSWER, T_AT, T_WORD
{ CurrentSemanticValue.statement = new AnswerStatement(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 71: // stmtContinue -> T_CONTINUE
{ CurrentSemanticValue.statement = new ContinueStatement(); }
        break;
      case 72: // stmtContinue -> T_CONTINUE_AT, T_WORD
{ CurrentSemanticValue.statement = new ContinueStatement(new ZimmerBot.Core.Knowledge.Continuation(ZimmerBot.Core.Knowledge.Continuation.ContinuationEnum.Label, ValueStack[ValueStack.Depth-1].s)); }
        break;
      case 73: // stmtContinue -> T_CONTINUE_WITH, wordSeq
{ CurrentSemanticValue.statement = new ContinueStatement(ValueStack[ValueStack.Depth-1].stringList); }
        break;
      case 74: // stmtStopOutput -> T_STOPOUTPUT
{ CurrentSemanticValue.statement = new StopOutputStatement(); }
        break;
      case 75: // exprSeq -> exprSeq2
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 76: // exprSeq -> /* empty */
{ CurrentSemanticValue.exprList = new List<Expression>(); }
        break;
      case 77: // exprSeq2 -> exprSeq2, T_COMMA, expr
{ ValueStack[ValueStack.Depth-3].exprList.Add(ValueStack[ValueStack.Depth-1].expr); CurrentSemanticValue = ValueStack[ValueStack.Depth-3]; }
        break;
      case 78: // exprSeq2 -> expr
{ CurrentSemanticValue.exprList = new List<Expression>(); CurrentSemanticValue.exprList.Add(ValueStack[ValueStack.Depth-1].expr); }
        break;
      case 79: // expr -> exprBinary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 80: // expr -> exprUnary
{ CurrentSemanticValue = ValueStack[ValueStack.Depth-1]; }
        break;
      case 81: // exprBinary -> expr, T_EQU, expr
{ CurrentSemanticValue.expr = new BinaryOperatorExpr(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].expr, BinaryOperatorExpr.OperatorType.Equals); }
        break;
      case 82: // exprUnary -> T_LPAR, expr, T_RPAR
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-2].expr; }
        break;
      case 83: // exprUnary -> T_EXCL, expr
{ CurrentSemanticValue.expr = new UnaryOperatorExpr(ValueStack[ValueStack.Depth-1].expr, UnaryOperatorExpr.OperatorType.Negation); }
        break;
      case 84: // exprUnary -> exprIdentifier
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-1].expr; }
        break;
      case 85: // exprUnary -> T_STRING
{ CurrentSemanticValue.expr = new ConstantValueExpr(((ConfigScanner)Scanner).StringInput.ToString()); }
        break;
      case 86: // exprUnary -> T_NUMBER
{ CurrentSemanticValue.expr = new ConstantValueExpr(ValueStack[ValueStack.Depth-1].n); }
        break;
      case 87: // exprIdentifier -> exprReference
{ CurrentSemanticValue.expr = ValueStack[ValueStack.Depth-1].expr; }
        break;
      case 88: // exprIdentifier -> T_DOLLAR, T_NUMBER
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].n); }
        break;
      case 89: // exprIdentifier -> T_DOLLAR, T_WORD
{ CurrentSemanticValue.expr = new IdentifierExpr("$"+ValueStack[ValueStack.Depth-1].s); }
        break;
      case 90: // exprReference -> exprReference, T_DOT, T_WORD
{ CurrentSemanticValue.expr = new DotExpression(ValueStack[ValueStack.Depth-3].expr, ValueStack[ValueStack.Depth-1].s); }
        break;
      case 91: // exprReference -> T_WORD
{ CurrentSemanticValue.expr = new DotExpression(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 92: // wordSeq -> wordSeq, T_WORD
{ CurrentSemanticValue.stringList = ValueStack[ValueStack.Depth-2].stringList; CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 93: // wordSeq -> T_WORD
{ CurrentSemanticValue.stringList = new List<string>(new string[] { ValueStack[ValueStack.Depth-1].s }); }
        break;
      case 94: // wordCommaSeq -> wordCommaSeq, T_COMMA, T_WORD
{ CurrentSemanticValue.stringList = ValueStack[ValueStack.Depth-3].stringList; CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 95: // wordCommaSeq -> T_WORD
{ CurrentSemanticValue.stringList = new List<string>(); CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 96: // cwordSeq -> cwordSeq, cword
{ CurrentSemanticValue.stringList = ValueStack[ValueStack.Depth-2].stringList; CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 97: // cwordSeq -> cword
{ CurrentSemanticValue.stringList = new List<string>(new string[] { ValueStack[ValueStack.Depth-1].s }); }
        break;
      case 98: // cword -> T_WORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 99: // cword -> T_CWORD
{ CurrentSemanticValue.s = ValueStack[ValueStack.Depth-1].s; }
        break;
      case 100: // stringSeq -> stringSeq, T_COMMA, T_STRING
{ CurrentSemanticValue.stringList = ValueStack[ValueStack.Depth-3].stringList; CurrentSemanticValue.stringList.Add(ValueStack[ValueStack.Depth-1].s); }
        break;
      case 101: // stringSeq -> T_STRING
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
