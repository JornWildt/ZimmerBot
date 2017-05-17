using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Utilities;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityClass
  {
    public string ClassName { get; protected set; }

    public int LongestWordCount
    {
      get
      {
        return TokenizedEntityNames.Count;
      }
    }

    public double TotalNumberOfWords { get; protected set; }

    protected List<List<string>> TokenizedEntityNames;

    protected List<int> WordByPositionCount { get; set; }

    protected double[] PositionInCategoryProbability { get; set; }

    protected Dictionary<string, double[]> WordByPositionInCategoryProbability { get; set; }

    static Regex LabelReducer = new Regex("[^\\w ]");

    public EntityClass(string className)
    {
      Condition.Requires(className, nameof(className)).IsNotNullOrWhiteSpace();
      ClassName = className;
      TokenizedEntityNames = new List<List<string>>();
      WordByPositionCount = new List<int>();
    }


    public void AddEntity(string name)
    {
      // Remove punctuation and other ignored characters
      name = LabelReducer.Replace(name, " ");

      // Ignore repeated entities
      //if (TokenizedEntityNames.ContainsKey(name))
        //return;

      string[] tokens = name.Split(' ').ToArray();
      //TokenizedEntityNames[name] = tokens;

      for (int pos = 0; pos < tokens.Length; ++pos)
      {
        RegisterWordToken(tokens[pos], pos);
      }
    }


    public void AddEntityWord(string word, int pos)
    {
      RegisterWordToken(word, pos);
    }


    protected void RegisterWordToken(string word, int pos)
    {
      // Register tokens
      while (pos >= TokenizedEntityNames.Count)
        TokenizedEntityNames.Add(new List<string>());
      TokenizedEntityNames[pos].Add(word);

      // Update statistics
      UpdateStatistics(pos);

      // Add word to spell checker
      if (SpellChecker.IsInitialized)
        SpellChecker.AddWord(word);
    }

    protected void UpdateStatistics(int pos)
    {
      while (pos >= WordByPositionCount.Count)
        WordByPositionCount.Add(0);
      WordByPositionCount[pos] += 1;
      TotalNumberOfWords += 1;
    }


    public void UpdateStatistics(double totalNumberOfWords)
    {
      WordByPositionInCategoryProbability = new Dictionary<string, double[]>(StringComparer.OrdinalIgnoreCase);

      // Count occurences
      for (int pos = 0; pos < TokenizedEntityNames.Count; ++pos)
      {
        foreach (string word in TokenizedEntityNames[pos])
        {
          if (!WordByPositionInCategoryProbability.ContainsKey(word))
            WordByPositionInCategoryProbability[word] = new double[LongestWordCount];
          WordByPositionInCategoryProbability[word][pos] += 1.0;

          if (SpellChecker.IsInitialized)
            SpellChecker.AddWord(word);
        }
      }

      // Calculate total probability of each position in this category
      // (relative to the total number of words in all categories)
      PositionInCategoryProbability = new double[LongestWordCount];
      for (int pos = 0; pos < LongestWordCount; ++pos)
        PositionInCategoryProbability[pos] = WordByPositionCount[pos] / totalNumberOfWords;

      // Normalize
      foreach (string word in WordByPositionInCategoryProbability.Keys)
      {
        for (int pos = 0; pos < LongestWordCount; ++pos)
        {
          WordByPositionInCategoryProbability[word][pos] =
            PositionInCategoryProbability[pos] * WordByPositionInCategoryProbability[word][pos] / WordByPositionCount[pos];
        }
      }
    }

    public double ProbabilityFor(string word, int pos)
    {
      if (WordByPositionInCategoryProbability.ContainsKey(word))
        return WordByPositionInCategoryProbability[word][pos];
      else
        return 0.0;
    }

  }
}
