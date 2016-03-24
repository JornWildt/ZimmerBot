using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Knowledge
{
  public class ReactionSet : IEnumerable<Reaction>
  {
    protected List<Reaction> Reactions { get; set; }


    public ReactionSet()
    {
      Reactions = new List<Reaction>();
    }


    public void Add(Reaction r)
    {
      if (Reactions.Count > 0 && r.Score < Reactions[0].Score)
        return;

      if (Reactions.Count > 0  &&  r.Score > Reactions[0].Score)
        Reactions.Clear();

      Reactions.Add(r);
    }


    public int Count
    {
      get { return Reactions.Count; }
    }


    public Reaction this[int i]
    {
      get { return Reactions[i]; }
    }


    public IEnumerator<Reaction> GetEnumerator()
    {
      return Reactions.GetEnumerator();
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)Reactions).GetEnumerator();
    }
  }
}
