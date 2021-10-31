using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum CollateralType : byte
  {
    None = 0,
    Cheque = 1,//چک
    Bill = 2,//سفته
    BlockInAccount = 4//بلوکه در حساب
  }
}
