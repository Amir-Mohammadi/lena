using System;
using System.Security.Cryptography;
using System.Text;

namespace lena.Services.Core.Common
{
  public class Crypto
  {
    public static string Sha1(string password)
    {
      var r = HashAlgorithm.Create("SHA1");
      if (r != null)
        return BitConverter.ToString(r.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
      return "";
    }
  }
}