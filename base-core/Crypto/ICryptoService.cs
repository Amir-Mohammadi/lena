using core.Autofac;
namespace core.Crypto
{
  public interface ICryptoService : IScopedDependency
  {
    #region Hash
    string Hash(string values);
    bool Check(string value, string hashedValue);
    #endregion
    #region RSA
    string RSADecryption(string cypherText);
    string RSAEncryption(string plainText);
    #endregion
    #region AES        
    string AESDecryption(string cypherText);
    string AESEncryption(string plainText);
    #endregion
  }
}