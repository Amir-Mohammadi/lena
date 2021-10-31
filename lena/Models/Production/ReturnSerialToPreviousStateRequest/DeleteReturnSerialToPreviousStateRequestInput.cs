using lena.Domains.Enums;
namespace lena.Models.Production.ReturnSerialToPreviousStateRequest
{
  public class DeleteReturnSerialToPreviousStateRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}