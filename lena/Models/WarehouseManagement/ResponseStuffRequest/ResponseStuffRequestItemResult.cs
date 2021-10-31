using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ResponseStuffRequest
{
  public class ResponseStuffRequestItemResult
  {
    private string _toRecipientName;
    private string _toWarehouseName;
    private string _toDepartmentName;
    private string _toEmployeeFullName;

    public int Id { get; set; }
    public string Code { get; set; }
    public string StuffCode { get; set; }
    public int? StuffId { get; set; }
    public string StuffName { get; set; }
    public double RequestQty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public string RequestDescription { get; set; }
    public double Qty { get; set; }
    public StuffRequestType StuffRequestType { get; set; }
    public StuffRequestItemStatusType Status { get; set; }
    public StuffRequestItemStatusType RequestStatus { get; set; }
    public string Description { get; set; }
    public int StuffRequestId { get; set; }
    public DateTime DateTime { get; set; }
    public int? ScrumEntityId { get; set; }
    public string ScrumEntityCode { get; set; }
    public string ScrumEntityName { get; set; }
    public int? ToDepartmentId { get; set; }
    public string ToDepartmentName
    {
      get
      {
        return _toDepartmentName;
      }
      set
      {
        _toDepartmentName = value;
        UpdateToRecipientName();
      }
    }
    public int? ToEmployeeId { get; set; }
    public string ToEmployeeFullName
    {
      get
      {
        return _toEmployeeFullName;
      }
      set
      {
        _toEmployeeFullName = value;
        UpdateToRecipientName();
      }
    }
    public int FromWarehouseId { get; set; }
    public string FromWarehouseName { get; set; }
    public int? ToWarehouseId { get; set; }
    public string ToWarehouseName
    {
      get
      {
        return _toWarehouseName;
      }
      set
      {
        _toWarehouseName = value;
        UpdateToRecipientName();

      }
    }
    public string ToRecipientName
    {
      get
      {
        return _toRecipientName;
      }
    }
    public int RequestUserId { get; set; }
    public string RequestUserEmployeeName { get; set; }
    public string StuffRequestCode { get; set; }
    public int StuffRequestItemId { get; set; }
    public string StuffRequestItemCode { get; set; }
    public byte[] RowVersion { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string RequestStuffCode { get; set; }
    public int? RequestStuffId { get; set; }
    public string RequestStuffName { get; set; }
    public int? RequestBillOfMaterialVersion { get; set; }
    //sdfsdfdsf
    private void UpdateToRecipientName()
    {
      if (!string.IsNullOrEmpty(_toWarehouseName))
        _toRecipientName = _toWarehouseName;
      if (!string.IsNullOrEmpty(_toDepartmentName))
        _toRecipientName = _toDepartmentName;
      if (!string.IsNullOrWhiteSpace(_toEmployeeFullName))
      {
        if (!string.IsNullOrEmpty(_toDepartmentName))
          _toRecipientName = $"{_toDepartmentName} ( {_toEmployeeFullName} )";
        else
          _toRecipientName = _toEmployeeFullName;
      }
    }
  }
}
