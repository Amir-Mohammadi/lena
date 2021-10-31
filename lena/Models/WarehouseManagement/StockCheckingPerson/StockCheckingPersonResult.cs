using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.StockCheckingPerson
{
  public class StockCheckingPersonResult
  {
    public int StockCheckingId { get; set; }
    public int UserId { get; set; }
    public string StockCheckingTitle { get; set; }
    public StockCheckingStatus StockCheckingStatus { get; set; }

    public string UserName { get; set; }

    public string EmployeeName { get; set; }

    public string EmployeeCode { get; set; }

  }
}
