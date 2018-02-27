using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZimmerBot.Core.Utilities;

namespace Rejseplanen.ZimmerBot.AddOn
{
  public static class RejseplanenAppSettings
  {
    public static readonly AppSetting<string>  RejseplanenUrl = new AppSetting<string>("Rejseplanen.Url");
  }
}
