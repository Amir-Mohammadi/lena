using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ReturnOfSaleStatusLog
{
  public class ReturnOfSaleStatusLogResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }

    public string EmployeeCode { get; set; }

    public string EmployeeFullName { get; set; }

    public DateTime DateTime { get; set; }

    public ReturnOfSaleStatus Status { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
