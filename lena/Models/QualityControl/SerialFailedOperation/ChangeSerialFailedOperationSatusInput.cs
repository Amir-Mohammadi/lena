using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperation
{
  public class ChangeSerialFailedOperationSatusInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
