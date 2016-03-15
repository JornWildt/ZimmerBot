﻿using System;
using System.Collections;
using System.Reflection;
using Antlr4.StringTemplate;


namespace ZimmerBot.Core.Utilities
{
  public static class TextMerge
  {
    public static string MergeTemplate(string template, IDictionary source)
    {
      Template t = new Template(template);

      if (source != null)
      {
        foreach (string key in source.Keys)
        {
          t.Add(key, source[key]);
        }
      }

      return t.Render();
    }


    public static string MergeTemplate(string template, object source)
    {
      Template t = new Template(template);

      if (source != null)
      {
        Type type = source.GetType();
        foreach (PropertyInfo pi in type.GetProperties())
        {
          if (pi.CanRead)
          {
            object value = pi.GetValue(source);
            t.Add(pi.Name, value);
          }
        }
      }

      return t.Render();
    }
  }
}