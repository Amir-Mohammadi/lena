using lena.Domains.Enums;
namespace lena.Models.Guard.Transport
{
  public class DeleteTransportInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
