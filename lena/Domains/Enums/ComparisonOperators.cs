using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ComparisonOperators : byte
  {
    IsEqualTo,
    Contains,
    StartsWith,
    EndsWith,
    IsGreaterThan,
    IsGreaterThanOrEqualTo,
    IsLessThan,
    IsLessThanOrEqualTo,
    IsNotEqualTo,
    NotContains,
    IsNull,
    IsNotNull,
    IsNotContainedIn,
    IsContainedIn,
    IsEmpty,
    IsNotEmpty
  }
}
