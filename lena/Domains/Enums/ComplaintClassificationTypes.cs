using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public enum ComplaintClassificationTypes : short
  {
    None = 0,
    DoNotProvideServices = 1,
    DelayInProductDelivery = 2,
    DelaysInServiceDelivery = 4,
    DefectiveProduct = 8,
    ProductDoesNotMatchTheOrder = 16,
    Price = 32,
    ProblemsWithFinancialAccounts = 64,
    ContractualProblems = 128,
    Others = 256,
  }
}
