using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Knowledge
{
  public class EntityClass
  {
    public string Name { get; protected set; }

    protected List<string[]> TokenizedNames;
  }
}
