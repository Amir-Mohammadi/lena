using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class CargoItemLogResult
  {
    public int Id { get; set; }
    public int ModifierUserId { get; set; }
    public string ModifierUserFullName { get; set; }
    public int ModifierEmployeeId { get; set; }
    public string ModifierEmployeeFullName { get; set; }
    public DateTime ModifyDateTime { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public bool IsDelete { get; set; }
    public double NewCargoItemQty { get; set; }
    public double OldCargoItemQty { get; set; }
    public CargoItemLogStatus CargoItemLogStatus { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
