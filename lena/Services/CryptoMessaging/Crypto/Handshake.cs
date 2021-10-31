using System;
using System.Security.Cryptography;
using System.Text;
namespace lena.Services.CryptoMessaging.Crypto
{
  public class Handshake
  {
    private readonly BigInteger _p;
    private readonly BigInteger _g;
    private BigInteger _publicKey;
    private BigInteger _secret;
    private BigInteger _sharedKey;
    private readonly RandomNumberGenerator _random;
    private const string DefaultP =
        "168294521999962130186481366746317932529486835835474438250419300005542062278891226921332165650972006021398772800281689816853901084848599221858183850409017080287848783739820087231353656731843042264088817536955443908290928737221030339896081613015073822320313739281637743653292503543919838375956129156593272174749";
    private const string DefaultG = "2";
    public Handshake() : this(DefaultP, DefaultG)
    {
    }
    public Handshake(long p, long q) : this(p.ToString(), q.ToString())
    {
    }
    public Handshake(string p, string q)
    {
      try
      {
        _p = new BigInteger(p, 10);
        _g = new BigInteger(q, 10);
      }
      catch (Exception)
      {
        throw new ArgumentException("p or q is not valid.");
      }
      _random = RandomNumberGenerator.Create();
      _publicKey = new BigInteger("0", 10);
      _secret = new BigInteger("0", 10);
      _sharedKey = new BigInteger("0", 10);
      Init();
    }
    private void Init()
    {
      GenerateRandomSecret();
      _publicKey = _g.modPow(_secret, _p);
    }
    private void GenerateRandomSecret()
    {
      var random = new byte[8];
      _random.GetBytes(random);
      _secret = new BigInteger(random);
    }
    public void ComputeSharedKey(string bobPublicKey)
    {
      var bPublicKey = new BigInteger(bobPublicKey, 16);
      _sharedKey = bPublicKey.modPow(_secret, _p);
    }
    private string To256Key()
    {
      var sha = SHA256.Create();
      var key = sha.ComputeHash(_sharedKey.getBytes());
      var base64Key = Convert.ToBase64String(key);
      return base64Key;
    }
    private byte[] To256KeyBytes()
    {
      var sha = SHA256.Create();
      var key = sha.ComputeHash(_sharedKey.getBytes());
      return key;
    }
    public string P => _p.ToString(16);
    public string G => _g.ToString(16);
    public string Secret => _secret.ToString(16);
    public string PublicKey => _publicKey.ToString(16);
    public string SharedKeyHex => _sharedKey.ToString(16);
    public byte[] SharedKeyBytes => _sharedKey.getBytes();
    public string SharedKeyBase64 => Convert.ToBase64String(_sharedKey.getBytes());
    public byte[] SharedKey256Bytes => To256KeyBytes();
    public string SharedKey256Base64 => To256Key();
    public override string ToString()
    {
      var content = new StringBuilder();
      content.AppendLine("-----START-PARSHAKE------");
      content.Append("G: ");
      content.Append(Format(G));
      content.AppendLine();
      content.Append("P: ");
      content.Append(Format(P));
      content.AppendLine();
      content.Append("SECRET: ");
      content.Append(Format(Secret));
      content.AppendLine();
      content.Append("PUBLIC_KEY: ");
      content.Append(Format(PublicKey));
      content.AppendLine();
      content.Append("SHARED_KEY: ");
      content.Append(Format(SharedKeyHex));
      content.AppendLine();
      content.AppendLine("-----END-PARSHAKE------");
      return content.ToString();
    }
    private string Format(string hexdump)
    {
      var output = new StringBuilder();
      var lw = 0;
      var hw = 0;
      if (hexdump.Length > 40) output.AppendLine();
      foreach (var hex in hexdump)
      {
        lw++;
        hw = lw % 40;
        if (lw == 0)
        {
          output.Append(hex);
          //                    output.Append('');
        }
        else if (hw != 0)
        {
          output.Append(hex);
        }
        else
        {
          output.AppendLine();
        }
      }
      output.AppendLine("");
      return output.ToString();
    }
  }
}