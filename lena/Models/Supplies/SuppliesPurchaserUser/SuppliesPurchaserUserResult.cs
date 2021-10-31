using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.SuppliesPurchaserUser
{
  public class SuppliesPurchaserUserResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffEnName { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
