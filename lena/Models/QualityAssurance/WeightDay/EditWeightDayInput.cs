using lena.Domains.Enums;
namespace lena.Models
{
  public class EditWeightDayInput
  {
    public int Id { get; set; }
    public int Day { get; set; }
    public double Amount { get; set; }
    public byte[] RowVersion { get; set; }
  }
}