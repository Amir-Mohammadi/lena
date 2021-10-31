using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderLog
{
  public class BankOrderLogResult
  {
    public int Id { get; set; }

    public int BankOrderStatusTypeId { get; set; }

    public string BankOrderStatusTypeCode { get; set; }

    public string BankOrderStatusTypeName { get; set; }

    public string OrderNumber { get; set; }

    public DateTime DateTime { get; set; }

    public int UserId { get; set; }

    public string EmployeeFullName { get; set; }

    public string Description { get; set; }

    public byte[] RowVersion { get; set; }
  }
}