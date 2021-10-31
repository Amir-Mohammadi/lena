using System;
using System.Security.Cryptography;
using System.Text;
using core.Setting;
namespace core.Crypto
{
  public class CryptoService : ICryptoService
  {
    #region Fields        
    private readonly SiteSetting siteSetting;
    #endregion
    #region Constractor
    public CryptoService(ISiteSettingProvider siteSettingProvider)
    {
      this.siteSetting = siteSettingProvider.SiteSetting;
    }
    #endregion
    #region Hash
    public bool Check(string value, string hashedValue)
    {
      return Hash(value) == hashedValue;
    }
    public string Hash(string value)
    {
      var sha256 = SHA256.Create();
      var byteValue = Encoding.UTF8.GetBytes(value);
      var byteHash = sha256.ComputeHash(byteValue);
      return Convert.ToBase64String(byteHash);
    }
    #endregion
    #region RSA
    private RSACryptoServiceProvider GetRSACryptoServiceProvider()
    {
      RSACryptoServiceProvider rsa = new();
      var privateKey = Convert.FromBase64String(siteSetting.RSAKey);
      rsa.ImportRSAPrivateKey(privateKey, out _);
      return rsa;
    }
    public string RSAEncryption(string plainText)
    {
      var rsa = GetRSACryptoServiceProvider();
      var bytesPlainText = Encoding.Unicode.GetBytes(plainText);
      var bytesCypherText = rsa.Encrypt(bytesPlainText, false);
      return Convert.ToBase64String(bytesCypherText);
    }
    public string RSADecryption(string cypherText)
    {
      var rsa = GetRSACryptoServiceProvider();
      var bytesCypherText = Convert.FromBase64String(cypherText);
      var bytesPlainText = rsa.Decrypt(bytesCypherText, false);
      var plainText = Encoding.Unicode.GetString(bytesPlainText);
      return plainText;
    }
    #endregion
    #region AES
    //TODO complate this
    // private SymmetricAlgorithm GetAESCryptoServiceProvider()
    // {
    //   var aes = Aes.Create();
    //   return aes;
    // }
    public string AESDecryption(string cypherText)
    {
      return cypherText;
    }
    public string AESEncryption(string plainText)
    {
      return plainText;
    }
    #endregion
  }
}