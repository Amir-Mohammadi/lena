using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class UndoFinishedProductionOrderStatusInput
  {
    public int Id { get; set; }
    public byte[] RowVersion;
  }
}
