using System;
using System.Security.Cryptography;
using System.Text;

namespace lena.Services.CryptoMessaging.Nonce
{
  public class NonceGenerator
  {
    private DateTime _starTime;
    private RandomNumberGenerator _randomNumberGenerator;

    public NonceGenerator()
    {
      _starTime = DateTime.Now.ToUniversalTime();
      _randomNumberGenerator = RandomNumberGenerator.Create();
    }

    public string Next(string uuid = "")
    {
      var start = _starTime.Ticks.ToString();
      var now = DateTime.Now.ToUniversalTime().Ticks.ToString();
      var salt1 = new byte[32];
      var salt2 = new byte[32];
      _randomNumberGenerator.GetBytes(salt1);
      _randomNumberGenerator.GetBytes(salt2);
      var salt1B64 = Convert.ToBase64String(salt1);
      var salt2B64 = Convert.ToBase64String(salt1);
      var sha = SHA256.Create();
      var nonce = start + now + salt1B64 + now + salt2B64 + uuid;
      var nonceHash = sha.ComputeHash(Encoding.ASCII.GetBytes(nonce));
      return Convert.ToBase64String(nonceHash);
    }
  }


}