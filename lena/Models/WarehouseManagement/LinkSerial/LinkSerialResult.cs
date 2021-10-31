using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class LinkSerialResult
  {
    public string LinkedSerial { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public Nullable<int> UserLinkerId { get; set; }
    public string LinkerEmployeeFullName { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public DateTime DateTime { get; set; }
    public Nullable<DateTime> LinkDateTime { get; set; }
    public Nullable<DateTime> ProductionDateTime { get; set; }
    public int? StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public long? StuffSerialCode { get; set; }
    public string Serial { get; set; }
    public int? SerialProfileCode { get; set; }
    public LinkSerialStatus Status { get; set; }
    public LinkSerialType Type { get; set; }

    #region IranKhodroSerial
    public bool? Print { get; set; }
    public int? PrintQty { get; set; }
    public string CustomerStuffCode { get; set; }
    public string CustomerStuffName { get; set; }
    public string TechnicalNumber { get; set; }
    #endregion
    public byte[] RowVersion { get; set; }

  }
}