﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using NHunspell;
using ZimmerBot.Core.Knowledge;

namespace ZimmerBot.Core.Utilities
{
  public static class SpellChecker
  {
    public static bool IsInitialized { get; private set; }

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


    public static void AddWord(string word)
    {
      if (!IsInitialized)
        throw new InvalidOperationException("Spell checker not initialized");

      if (Speller == null)
        return;

      foreach (var w in word.Split(new char[] { ' '  }, StringSplitOptions.RemoveEmptyEntries))
        Speller.Add(w);
    }


    static TextInfo _textInfo;

    static TextInfo CurrentTextInfo
    {
      get
      {
        if (_textInfo == null)
          _textInfo = new CultureInfo(AppSettings.Language.Value.Replace("_", "-"), false).TextInfo;
        return _textInfo;
      }
    }


    public static string SpellCheck(string word)
    {
      if (!IsInitialized)
        throw new InvalidOperationException("Spell checker not initialized");

      if (Speller == null)
        return word;

      lock (SpellerLocker)
      {
        // Try various casings, as NHunspell seems to be case-sensitive!
        bool ok = 
          Speller.Spell(word) 
          || Speller.Spell(CurrentTextInfo.ToTitleCase(word))
          || word.Any(c => char.IsDigit(c));

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
