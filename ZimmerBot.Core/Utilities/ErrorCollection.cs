using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Utilities
{
  public class ErrorCollection : List<ErrorCollection.Error>
  {
    public class Error
    {
      public string Message { get; protected set; }
      public int LineNo { get; protected set; }
      public int Position { get; protected set; }

      public Error(string msg, int l, int p)
      {
        Message = msg;
        LineNo = l;
        Position = p;
      }
    }


    public void Add(string msg, int l, int p)
    {
      Add(new Error(msg, l, p));
    }
  }
}
