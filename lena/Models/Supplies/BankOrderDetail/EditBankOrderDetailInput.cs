using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderDetail
{
  public class EditBankOrderDetailInput
  {
    public int Id { get; set; }
    public int Index { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public int StuffHSGroupId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public double Fee { get; set; }
    public double GrossWeight { get; set; }
  }
}
