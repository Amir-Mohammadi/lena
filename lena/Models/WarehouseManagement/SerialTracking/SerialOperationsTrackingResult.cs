using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SerialTracking
{
  public class SerialOperationsTrackingResult
  {
    public int? Order { get; set; }
    public string OperationCode { get; set; }
    public string OperationTitle { get; set; }
    public string TerminalDescription { get; set; }
    public DateTime? DateTime { get; set; }
    public long? OperationTime { get; set; }
    public double? Qty { get; set; }
    public byte[] RowVersion { get; set; }
    public List<string> Employees { get; set; }
    public bool IsDone { get; set; }
    public bool IsFaild { get; set; }
    public int? ProductionOperationEmployeeGroupId { get; set; }
    public string ProductionOrderCode { get; set; }
  }
}
