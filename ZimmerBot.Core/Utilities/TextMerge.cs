using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Antlr4.StringTemplate;


namespace ZimmerBot.Core.Utilities
{
  public static class TextMerge
  {
    private static TemplateGroup Templates { get; set; }


    static TextMerge()
    {
      Templates = new TemplateGroup();
      Templates.RegisterRenderer(typeof(DateTime), new DateRenderer());
      Templates.RegisterRenderer(typeof(string), new ZimmerBot.Core.Utilities.StringRender());
    }


    public static string MergeTemplate(ChainedDictionary<string, string> templates, object source)
    {
      return MergeTemplate(templates, "default", source);
    }


    public static string MergeTemplate(ChainedDictionary<string,string> templates, string templateName, object source)
    {
      string template = templates[templateName];
      return MergeTemplate(template, source);
    }


    public static string MergeTemplate(string template, IDictionary source)
    {
      Template t = new Template(Templates,template);

      if (source != null)
      {
        foreach (string key in source.Keys)
        {
          // FIXME: not smart if those dotted values are supposed to be available ...
          if (!key.Contains("."))
            t.Add(key, source[key]);
        }
      }

      return t.Render();
    }


    public static string MergeTemplate(string template, object source)
    {
      if (source is IDictionary)
        return MergeTemplate(template, (IDictionary)source);

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
