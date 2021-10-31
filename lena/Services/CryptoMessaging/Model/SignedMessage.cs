using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using lena.Services.CryptoMessaging.Crypto;

namespace lena.Services.CryptoMessaging.Model
{
  public class SignedMessage
  {
    //public string Ticket { get; set; } = "";

    public string Ssid { get; set; }

    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpiresIn { get; set; }

    //public string Signature { get; set; } = "";

    //private readonly string _key;

    public Dictionary<string, string> Params;

    public SignedMessage()
    {
      Params = new Dictionary<string, string>();
    }
    public SignedMessage(string ssid)
    {
      Params = new Dictionary<string, string>();
      Ssid = ssid;
    }

    public SignedMessage(string token, string refreshToken, DateTime expiresIn)
    {
      Params = new Dictionary<string, string>();
      Token = token;
      RefreshToken = refreshToken;
      ExpiresIn = expiresIn;
    }

    //public SignedMessage(string key, string ticket, string ssid)
    //{
    //    Params = new Dictionary<string, string>();
    //    //_key = key;
    //    //Ticket = ticket;
    //    //Ssid = ssid;
    //}

    //public SignedMessage(string key, string ticket, string ssid, string signature)
    //{
    //    //_key = key;
    //    Params = new Dictionary<string, string>();

    //    Ticket = ticket;
    //    Ssid = ssid;
    //    Signature = signature;
    //}

    //public bool IsValid(string destKey)
    //{
    //    if (string.IsNullOrEmpty(Ticket) || string.IsNullOrEmpty(Ssid) || string.IsNullOrEmpty(Signature))
    //    {
    //        return false;
    //    }

    //    var validHash = Convert.ToBase64String(GetHash());
    //    var participantHash =
    //        Convert.ToBase64String(Xxtea.Decrypt(Convert.FromBase64String(Signature), destKey));
    //    return participantHash.Equals(validHash);
    //}

    //public void Sign()
    //{
    //    Signature = GetSignature(_key);
    //}

    //private byte[] GetHash()
    //{
    //    var digest = Ticket + Ssid + PayloadHashData();
    //    var sha1 = SHA1.Create();
    //    var sign = sha1.ComputeHash(Encoding.ASCII.GetBytes(digest));
    //    return sign;
    //}

    //private string GetSignature(string base64SharedKey)
    //{
    //    var sign = GetHash();
    //    return Convert.ToBase64String(Xxtea.Encrypt(sign, base64SharedKey));
    //}

    //private string PayloadHashData()
    //{
    //    var builder = new StringBuilder();
    //    foreach ( var key in Params.Keys)
    //    {
    //        builder.Append(key);
    //        builder.Append(Params[key]);
    //    }
    //    return builder.ToString();
    //}
  }
}