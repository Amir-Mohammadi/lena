using lena.Domains.Enums;
namespace lena.Models
{
  public class ReturnSerialToPreviousStateRequestResult
  {
    public int Id { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public long StuffSerialCode { get; set; }
    public int UserId { get; set; }
    public int ConfirmerUserId { get; set; }
    public int WrongDoerUserId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}