using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PurchaseOrder
{
  public class SelectedOrder
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsArchived { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
