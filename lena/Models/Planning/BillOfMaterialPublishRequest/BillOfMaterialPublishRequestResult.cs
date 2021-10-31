using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class BillOfMaterialPublishRequestResult
  {
    public int Id { get; set; }
    public string StuffName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public StuffType StuffType { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public string EmployeeFullName { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public BillOfMaterialPublishRequestStatus Status { get; set; }
    public BillOfMaterialPublishRequestType Type { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}
