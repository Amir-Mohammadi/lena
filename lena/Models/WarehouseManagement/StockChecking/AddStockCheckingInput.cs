using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StockChecking
{
  public class AddStockCheckingInput
  {
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int[] Users { get; set; }
    public short[] Warehouses { get; set; }
    public int[] Stuffs { get; set; }
    public bool ShowInventory { get; set; }
  }
}
