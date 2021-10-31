using System;
using System.Collections.Generic;
using lena.Domains.Enums;
using lena.Models.Stuff;
using lena.Models.WarehouseManagement.Warehouse;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockChecking
{
  public class StockCheckingResult
  {
    public int Id { get; set; }
    public int CreatorUserId { get; set; }
    public string CreatorName { get; set; }
    public DateTime CreateDate { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public StockCheckingStatus Status { get; set; }
    public IEnumerable<WarehouseComboResult> Warehouses { get; set; }
    public IEnumerable<EmployeeComboResult> RelatedPersons { get; set; }
    public IEnumerable<StuffComboResult> RelatedStuffs { get; set; }
    public int? ActiveTagTypeId { get; set; }
    public string ActiveTagTypeName { get; set; }
    public byte[] RowVersion { get; set; }
    public bool ShowInventory { get; set; }
  }
}
