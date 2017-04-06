using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZimmerBot.Core.Utilities
{
  public static class CryptoHelper
  {
    private static SHA1Managed SHA1Generator { get; set; }


    public static void Initialize()
    {
      SHA1Generator = new SHA1Managed();
    }


    public static void Shutdown()
    {
      if (SHA1Generator != null)
        SHA1Generator.Dispose();
    }


    public static string CalculateChecksum(string s)
    {
      byte[] input = Encoding.UTF8.GetBytes(s);
      byte[] output = SHA1Generator.ComputeHash(input);

      string hex = BitConverter.ToString(output);
      hex = hex.Replace("-", "");

      return hex;
    }
  }
}
