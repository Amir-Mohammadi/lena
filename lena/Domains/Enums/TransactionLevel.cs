using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum TransactionLevel : byte
  {
    Available = 1,
    Blocked = 2,
    QualityControl = 3,
    Plan = 4,
    Waste = 5
  }
}
