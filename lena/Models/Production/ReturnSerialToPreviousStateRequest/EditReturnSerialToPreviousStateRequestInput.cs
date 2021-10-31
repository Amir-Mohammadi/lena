using lena.Domains.Enums;
namespace lena.Models.Production.ReturnSerialToPreviousStateRequest
{
  public class EditReturnSerialToPreviousStateRequestInput
  {
    public int Id { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public int StuffCode { get; set; }
    public int UserId { get; set; }
    public int ConfirmerUserId { get; set; }
    public int WrongDoerUserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}