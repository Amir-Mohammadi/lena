using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ConditionalQualityControl
{
  public class ConditionalQualityControlResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string QualityControlAccepterTitle { get; set; }
    public string QualityControlAccepterFirstName { get; set; }
    public string QualityControlAccepterLastName { get; set; }
    public int QualityControlConfirmationId { get; set; }
    public string QualityControlConfirmationCode { get; set; }
    public int QualityControlId { get; set; }
    public string QualityControlCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public QualityControlType QualityControlType { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public QualityControlStatus QualityControlStatus { get; set; }
    public ConditionalQualityControlStatus Status { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double? AcceptedQty { get; set; }
    public double? FailedQty { get; set; }
    public double? ConditionalRequestQty { get; set; }
    public double? ConditionalQty { get; set; }
    public double? ReturnedQty { get; set; }
    public double? ConsumedQty { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public int? QualityControlAccepterUserGroupId { get; set; }
    public string QualityControlAccepterGroupName { get; set; }
    public string QualityControlConfirmationDescription { get; set; }
    public string QualityControlDescription { get; set; }
    public string ConditionalQualityControlRequestDescription { get; set; }
    public string ConditionalQualityControlConfirmationDescription { get; set; }
    public int? ResponseConditionalConfirmationUserId { get; set; }
    public string ResponseConditionalConfirmationUserName { get; set; }
    public DateTime? ResponseConditionalConfirmationDate { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
