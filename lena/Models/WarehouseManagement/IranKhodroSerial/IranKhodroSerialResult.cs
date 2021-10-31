using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class IranKhodroSerialResult
  {
    public int Id { get; set; }
    public int ProductionYearId { get; set; }
    public string ProductionYearCode { get; set; }
    public DateTime ProductionYearYear { get; set; }
    public int CustomerStuffId { get; set; }
    public string CustomerStuffCode { get; set; }
    public string CustomerStuffName { get; set; }
    public CustomerStuffType CustomerStuffType { get; set; }
    public int CustomerStuffVersionId { get; set; }
    public string CustomerStuffVersionCode { get; set; }
    public string CustomerStuffVersionName { get; set; }
    public string ManufacturerCode { get; set; }
    public string TechnicalNumber { get; set; }
    public DateTime DateTime { get; set; }
    public int ProductionSerial { get; set; }
    public DateTime ProductionDateTime { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public LinkSerialType LinkSerialType { get; set; }
    public bool Print { get; set; }
    public int PrintQty { get; set; }
    public byte[] RowVersion { get; set; }

  }
}