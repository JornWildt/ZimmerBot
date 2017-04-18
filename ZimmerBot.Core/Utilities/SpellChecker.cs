﻿using System;
using System.Collections.Generic;
using System.IO;
using NHunspell;

namespace ZimmerBot.Core.Utilities
{
  public static class SpellChecker
  {
    private static bool IsInitialized { get; set; }

    private static Hunspell Speller { get; set; }

    private static object SpellerLocker = new object();


    public static void Initialize()
    {
      if (!IsInitialized)
      {
        if (AppSettings.EnableSpellingCorrections || AppSettings.EnableStemming)
        {
          string aff = Path.Combine(AppSettings.LanguageDirectory, AppSettings.Language + ".aff");
          string dic = Path.Combine(AppSettings.LanguageDirectory, AppSettings.Language + ".dic");
          Speller = new Hunspell(aff, dic);
          IsInitialized = true;
        }
      }
    }


    public static void Shutdown()
    {
      if (Speller != null)
        Speller.Dispose();
      Speller = null;
      IsInitialized = false;
    }


    public static string SpellCheck(string word)
    {
      if (!IsInitialized)
        throw new InvalidOperationException("Spell checker not initialized");

      if (Speller == null)
        return word;

      lock (SpellerLocker)
      {
        bool ok = Speller.Spell(word);
        if (!ok)
        {
          List<string> suggestions = Speller.Suggest(word);
          if (suggestions.Count > 0)
            return suggestions[0];
        }
      }

      return word;
    }


    public static string Stem(string word)
    {
      if (!IsInitialized)
        throw new InvalidOperationException("Spell checker not initialized");

      if (Speller == null)
        return word;

      lock (SpellerLocker)
      {
        List<string> stems = Speller.Stem(word);

        // Using stemming only if there is a stemming and it is unambiguous
        if (stems.Count == 1)
          return stems[0];
        else
          return word;
      }
    }
  }
}