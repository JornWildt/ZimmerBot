using System;
using System.Collections.Generic;
using System.Text;
using CuttingEdge.Conditions;

namespace ZimmerBot.Core.TemplateParser
{
  public abstract class TemplateToken
  {
    public abstract string Instantiate(TemplateContext context);
  }


  public class SequenceTemplateToken : TemplateToken
  {
    public List<TemplateToken> Tokens { get; protected set; }

    public SequenceTemplateToken()
    {
      Tokens = new List<TemplateToken>();
    }


    public override string Instantiate(TemplateContext context)
    {
      StringBuilder s = new StringBuilder();
      foreach (TemplateToken t in Tokens)
        s.Append(t.Instantiate(context));
      return s.ToString();
    }
  }


  public class TextTemplateToken : TemplateToken
  {
    public string Text { get; protected set; }

    public TextTemplateToken(string text)
    {
      Condition.Requires(text, nameof(text)).IsNotNull();
      Text = text;
    }


    public void AppendText(string text)
    {
      Text = Text + text;
    }

    public override string Instantiate(TemplateContext context)
    {
      string s = context.Expander.ExpandPlaceholdes(Text);
      return s;
    }
  }


  public class RedirectTemplateToken : TemplateToken
  {
    public SequenceTemplateToken Tokens { get; protected set; }

    public RedirectTemplateToken(SequenceTemplateToken tokens)
    {
      Condition.Requires(tokens, nameof(tokens)).IsNotNull();
      Tokens = tokens;
    }


    public override string Instantiate(TemplateContext context)
    {
      string s = Tokens.Instantiate(context);
      s = context.Expander.Invoke(s);
      return s;
    }
  }


  public class ChooseTemplateToken : TemplateToken
  {
    static Random Randomizer = new Random();

    public List<TemplateToken> Variants { get; set; }

    public ChooseTemplateToken(TemplateToken initial)
    {
      Variants = new List<TemplateToken> { initial };
    }


    public ChooseTemplateToken Add(TemplateToken t)
    {
      Variants.Add(t);
      return this;
    }


    public override string Instantiate(TemplateContext context)
    {
      return Variants[Randomizer.Next(Variants.Count)].Instantiate(context);
    }
  }
}
