using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum QualityControlPaymentSuggestStatus : byte
  {
    FullPayement,
    HalfPayement,
    QuarterPayement,
    FullQualityControlConfirmationDependedPayement,
    NoPayement
  }
}