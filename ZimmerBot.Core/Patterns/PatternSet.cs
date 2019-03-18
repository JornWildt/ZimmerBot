﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuttingEdge.Conditions;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Patterns
{
  public class PatternSet
  {
    public List<KeyValuePair<string,List<string>>> Identifiers { get; protected set; }

    public List<List<KeyValuePair<string, string>>> UnfoldedIdentifiers { get; protected set; }

    public List<Pattern> Patterns { get; protected set; }


    private double? _numberOfWords;
    public double NumberOfWords
    {
      get
      {
        if (_numberOfWords == null)
        {
          _numberOfWords = Patterns.Sum(p => p.NumberOfWords);
        }
        return _numberOfWords.Value;
      }
    }


    public PatternSet(List<KeyValuePair<string, List<string>>> identifiers, List<Pattern> patterns)
    {
      Condition.Requires(identifiers, nameof(identifiers)).IsNotNull();
      Condition.Requires(patterns, nameof(patterns)).IsNotNull();

      Identifiers = identifiers;
      Patterns = patterns;

      foreach (Pattern p in Patterns)
        p.RegisterParent(this);
    }


    public void SetupComplete(KnowledgeBase kb)
    {
      ExpandPatterns(kb);
      UnfoldIdentifiers();
    }


    public void ExpandPatterns(KnowledgeBase kb)
    {
      Pattern[] existingPatterns = Patterns.ToArray();
      Patterns.Clear();

      foreach (Pattern p in existingPatterns)
      {
        p.ExpandExpressions(kb, Patterns);
      }
    }


    public void UnfoldIdentifiers()
    {
      UnfoldedIdentifiers = new List<List<KeyValuePair<string, string>>>();

      UnfoldIdentifiers(0, new Stack<KeyValuePair<string,string>>(), UnfoldedIdentifiers);
    }


    private void UnfoldIdentifiers(
      int i,
      Stack<KeyValuePair<string, string>> set,
      List<List<KeyValuePair<string, string>>> result)
    {
      if (i < Identifiers.Count)
      {
        foreach (string id in Identifiers[i].Value)
        {
          set.Push(new KeyValuePair<string, string>(Identifiers[i].Key, id));
          UnfoldIdentifiers(i+1, set, result);
          set.Pop();
        }
      }
      else
      {
        result.Add(new List<KeyValuePair<string, string>>(set));
      }
    }


    public void UpdateStatistics(double totalNumberOfPatterns, double totalNumberOfWords)
    {
      foreach (var pattern in Patterns)
        pattern.UpdateStatistics(totalNumberOfPatterns, totalNumberOfWords);
    }


    public void ExtractWordsForSpellChecker()
    {
      foreach (var pattern in Patterns)
        pattern.ExtractWordsForSpellChecker();
    }
  }
}
